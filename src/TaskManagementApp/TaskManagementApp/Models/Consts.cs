
namespace TaskManagementApp.Models
{
    public static class Consts
    {
        public static class ErrorConsts
        {
            
            public const string BadWorkTaskStatus = "task_management_bad_work_task_status";


            public const string ProjectNotFound = "task_management_project_not_found";
            public const string ProjectNotFoundOrNotAccesible = "task_management_project_not_found_or_not_accessible";
            public const string EmptyProjectName = "task_management_empty_project_name";
            

            public const string EmptyUserName = "task_management_empty_user_name";
            public const string UserAlreadyAdded = "task_management_user_already_added";

            
            public const string ProjectUserNotFound = "task_management_project_user_not_founded";
            public const string HaveNoAccessToEditProject = "task_management_have_no_access_to_edit_project";
            public const string UserNotFound = "task_management_user_not_found";
            public const string TaskNotFound = "task_management_task_not_found";
            public const string TaskLogTimeNotFound = "task_management_task_log_time_not_found";
            public const string ProjectHaveNoAccess = "task_management_project_have_no_access";
            public const string TaskHaveNoAccess = "task_management_task_have_no_access";
            public const string CommentNotFoundOrNotAccess = "task_management_comment_not_found_or_have_no_access";
            public const string CommentNotFound = "task_management_comment_not_found";
            public const string UserInMainAppNotFound = "task_management_user_main_app_not_found";
            public const string EmptyTaskName = "task_management_emptyTaskName";
            public const string TaskWithStatusExists = "task_management_task_with_status_exist";
            public const string WorkTaskEmptyStatusName = "task_management_empty_status_name";
            public const string WorkTaskStatusNotExists = "task_management_work_task_status_not_exist";
            public const string WorkTaskTimeLogValidationError = "task_management_work_task_time_log_validation_error";
            public const string WorkTaskTimeLogIntervalValidationError = "task_management_work_task_time_log_interval_validation_error";
            public const string SprintNotFound = "task_management_sprint_not_found";
            public const string LabelNotFound = "task_management_label_not_found";
            public const string LabelExists = "task_management_label_exists";
            public const string BadRelationType = "task_management_bad_relation";
            public const string RelationNotFound = "task_management_relation_not_found";
            public const string RelationError = "task_management_relation_error";



        }

        public class EmailConfigurationsCode
        {
            public const string AddedNewCommentInTask = "AddedNewCommentInTask";
            public const string NewExecutorInTask = "NewExecutorInTask";
            public const string StatusInTaskWasChanged = "StatusInTaskWasChanged";
            public const string TaskWasChanged = "TaskWasChanged";

        }
    }
}
