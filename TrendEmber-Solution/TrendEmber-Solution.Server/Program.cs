using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrendEmber.Core.Authentication;
using TrendEmber.Core.Identity;
using TrendEmber.Data;
using TrendEmber.Service;
using TrendEmber_Solution.Server.Policy;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add identity context
builder.Services.AddDbContext<IdentityContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

//add jwt 
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettingsSection["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");
var issuer = jwtSettingsSection["Issuer"] ?? throw new InvalidOperationException("JwtSettings:Issuer is not configured.");
var audience = jwtSettingsSection["Audience"] ?? throw new InvalidOperationException("JwtSettings:Audience is not configured.");


var jwtSettings = new JwtSettings(secretKey, issuer, audience);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.Admin.ToString(), policy => policy.RequireRole(Roles.Admin.ToString()));
    options.AddPolicy(Policy.Analyst.ToString(), policy => policy.RequireRole(Roles.Analyst.ToString()));
});

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.MapPost("/token", async (LoginRequest loginRequest, TokenService tokenService, UserManager<ApplicationUser> userManager) =>
{
    var user = await userManager.FindByNameAsync(loginRequest.Email);
    if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
    {
        return Results.Unauthorized();
    }
    var token = tokenService.GenerateJwtToken(user);

    return Results.Ok(new { Token = token });
});

app.Run();
