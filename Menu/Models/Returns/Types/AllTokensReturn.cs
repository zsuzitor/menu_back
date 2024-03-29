﻿using jwtLib.JWTAuth.Models.Poco;
using System.Text.Json.Serialization;
using Common.Models;

namespace Menu.Models.Returns.Types
{
    public sealed class TokensReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is AllTokens objTyped)
            {
                return new AllTokensReturn(objTyped);
            }

            return obj;
        }
    }

    public class AllTokensReturn
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        public AllTokensReturn(AllTokens obj)
        {
            AccessToken = obj.AccessToken;
            RefreshToken = obj.RefreshToken;
        }

    }
}
