using Microsoft.AspNetCore.Mvc;

namespace Menu.Models.TaskManagementApp.Requests
{
    public class ChangeUserRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Deactivated { get; set; }
        public bool IsAdmin { get; set; }
    }
}
