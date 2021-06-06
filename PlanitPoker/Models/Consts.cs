

namespace PlanitPoker.Models
{
    public class Consts
    {
        public static class Roles
        {
            public const string User = "User";
            public const string Creator = "Creator";
            public const string Admin = "Admin";
            public const string Observer = "Observer";

            public static bool IsValideRole(string roleName)
            {
                return roleName == User || roleName == Creator|| roleName == Admin|| roleName == Observer;
            }
        }
    }
}
