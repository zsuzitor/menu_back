using Microsoft.AspNetCore.Mvc;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class ChangeUserRequest
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public bool Deactivated { get; set; }
        public bool IsAdmin { get; set; }
    }
}
