using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.Returns.Interfaces;

namespace Menu.Models.Returns
{
    public class TokensFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj is AllTokens objTyped)
            {
                return new AllTokensReturn(objTyped);
            }

            return obj;
        }
    }

    public class AllTokensReturn
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }

        public AllTokensReturn(AllTokens obj)
        {
            access_token = obj.AccessToken;
            refresh_token = obj.RefreshToken;
        }

    }
}
