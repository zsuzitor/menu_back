

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
            public const string DontHaveAccess = "dont_have_access";
            public const string CantVote = "cant_vote";
            public const string StoryNotFound = "story_not_found";
            public const string StoryBadStatus = "story_bad_status";
        }

        public class PlanitPokerHubEndpoints
        {
            public const string ConnectedToRoomError = "ConnectedToRoomError";
            public const string NewRoomAlive = "NewRoomAlive";

            public const string
                NotifyFromServer = "PlaningNotifyFromServer"; //todo будет принимать объект result с ошибками как в апи

            public const string EnteredInRoom = "EnteredInRoom";
            public const string NewUserInRoom = "NewUserInRoom";
            public const string UserLeaved = "UserLeaved";
            public const string VoteStart = "VoteStart"; //голосование начато, оценки почищены

            public const string VoteEnd = "VoteEnd";

            //private const string VoteSuccess = "VoteSuccess";
            public const string VoteChanged = "VoteChanged";
            public const string RoomNotCreated = "RoomNotCreated";
            public const string UserNameChanged = "UserNameChanged";
            public const string UserRoleChanged = "UserRoleChanged";
            public const string AddedNewStory = "AddedNewStory";
            public const string CurrentStoryChanged = "CurrentStoryChanged";
            public const string NewCurrentStory = "NewCurrentStory";
            public const string DeletedStory = "DeletedStory";
            public const string MovedStoryToComplete = "MovedStoryToComplete";
            public const string NeedRefreshTokens = "NeedRefreshTokens";
        }
    }
}
