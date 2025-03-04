using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddDbContext<TrendsDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("TrendsConnection")));

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse(); // Prevents the default redirect behavior
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
                },
                OnForbidden = context =>
                {
                    // If a user is not authorized (e.g., no valid roles), return 403 instead of redirect
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"error\":\"Forbidden\"}");
                }
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.Admin.ToString(), policy => policy.RequireRole(Roles.Admin.ToString()));
    options.AddPolicy(Policy.Analyst.ToString(), policy => policy.RequireRole(Roles.Analyst.ToString()));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITradeService, TradeService>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    dbContext.Database.Migrate();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //seed initial super user
    Task.Run(async () => {
        var initialUsers = builder.Configuration.GetSection("InitialUsers").GetChildren();

        foreach (var initialUserConfig in initialUsers)
        {
            var username = initialUserConfig.GetValue<string>("Username") ?? throw new ArgumentNullException("Username cannot be null or empty.");
            var email = initialUserConfig.GetValue<string>("Email") ?? throw new ArgumentNullException("Email cannot be null or empty.");
            var fullName = initialUserConfig.GetValue<string>("FullName") ?? throw new ArgumentNullException("FullName cannot be null or empty.");
            var password = initialUserConfig.GetValue<string>("Password") ?? throw new ArgumentNullException("Password cannot be null or empty.");

            var rolesString = initialUserConfig.GetValue<string>("Roles") ?? throw new ArgumentNullException("Roles cannot be null or empty.");
            var roles = rolesString.Split('|').Select(role => role.Trim()).ToArray();

            var user = userManager.FindByEmailAsync(email).Result;
            if (user == null)
            {
                var newUser = new ApplicationUser(fullName) { UserName = username, Email = email };
                var result = userManager.CreateAsync(newUser, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRolesAsync(newUser, roles).Wait();
                }
            }
        }
    }).Wait();


    //tradeService.CalculateMeanAndStandardDeviation();
    //tradeService.CalculatePriceHistoryShapeZScore();
    Task.Run(async () =>
    {
        var trendsDbContext = scope.ServiceProvider.GetRequiredService<TrendsDbContext>();
        trendsDbContext.Database.Migrate();
        var tradeService = scope.ServiceProvider.GetRequiredService<ITradeService>();
        //await tradeService.FindPeaksAndTroughsForWatchListAsync();
        //await tradeService.DetectGapsAsync();
    }).Wait();

}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");


app.Run();
