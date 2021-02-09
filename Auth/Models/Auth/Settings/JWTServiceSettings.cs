

using jwtLib.JWTAuth.Interfaces;

namespace Auth.Models.Auth.Settings
{
    public class JWTServiceSettings : IJWTServiceSettings, IJWTSettings
    {
        public int LifetimeAccessToken { get; set; }
        public int LifetimeRefreshToken { get; set; }
        public string KeyForAccessToken { get; set; }
        public string KeyForRefreshToken { get; set; }
        public string AuthenticationType { get; set; }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenName { get; set; }

        public JWTServiceSettings()
        {

        }
        public JWTServiceSettings(int lifetimeAccessToken, int lifetimeRefreshToken, string keyForAccessToken, string keyForRefreshToken, string authenticationType)
        {
            LifetimeAccessToken = lifetimeAccessToken;
            LifetimeRefreshToken = lifetimeRefreshToken;
            KeyForAccessToken = keyForAccessToken;
            KeyForRefreshToken = keyForRefreshToken;
            AuthenticationType = authenticationType;
        }
    }
}
