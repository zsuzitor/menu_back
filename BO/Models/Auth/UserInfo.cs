

namespace BO.Models.Auth
{
    public sealed class UserInfo
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
