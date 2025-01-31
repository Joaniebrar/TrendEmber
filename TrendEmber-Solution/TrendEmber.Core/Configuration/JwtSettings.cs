namespace TrendEmber.Core.Authentication
{
    public class JwtSettings
    {
        public JwtSettings(string secretKey, string issuer, string audience) { 
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
        }

        public string SecretKey { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
    }
}
