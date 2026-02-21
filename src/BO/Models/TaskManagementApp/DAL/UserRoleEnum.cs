using System;

namespace BO.Models.TaskManagementApp.DAL
{
    [Flags]
    public enum UserRoleEnum
    {
        Viewer = 1,
        Editor = 2,
        Admin = 3,
        Deactivated = 4,
    }
}
