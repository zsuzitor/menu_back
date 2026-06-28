using BL.Models.Services.Interfaces;
using jwtLib.JWTAuth.Interfaces;

namespace BL.Models.Services
{
    public class Hasher : IHasher
    {
        private readonly IJWTHasher _hasher;

        public Hasher(IJWTHasher hasher)
        {
            _hasher = hasher;
        }

        public string GetHash(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            return _hasher.GetHash(str);
        }
    }
}
