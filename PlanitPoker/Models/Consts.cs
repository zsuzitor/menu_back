

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

        public class PlanitPokerErrorConsts
        {
            public const string RoomNameIsEmpty = "room_name_is_empty";
            public const string PlanitUserNotFound = "planit_user_not_found";
            public const string RoomAlreadyExist = "room_already_exist";
            public const string RoomNotFound = "room_not_found";
            public const string SomeErrorWithRoomCreating = "some_error_with_room_creating";
        }
    }
}
