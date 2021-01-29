﻿

namespace Auth.Models.Auth.Poco
{
    public class UserInfo
    {
        public long UserId { get; private set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserInfo(long userId)
        {
            UserId = userId;
        }
    }
}
