
using System;

namespace TaskManagementApp.Models
{
    public static class Consts
    {
        public const string ProjectPrefix = "task_management_";
        public static class ErrorConsts
        {
            
            public const string BadWorkTaskStatus = $"{ProjectPrefix}bad_work_task_status";


            public const string ProjectNotFound = $"{ProjectPrefix}project_not_found";
            public const string ProjectNotFoundOrNotAccesible = $"{ProjectPrefix}project_not_found_or_not_accessible";
            public const string EmptyProjectName = $"{ProjectPrefix}empty_project_name";
            

            public const string EmptyUserName = $"{ProjectPrefix}empty_user_name";
            public const string UserAlreadyAdded = $"{ProjectPrefix}user_already_added";

            
            public const string ProjectUserNotFound = $"{ProjectPrefix}project_user_not_founded";
            public const string HaveNoAccessToEditProject = $"{ProjectPrefix}have_no_access_to_edit_project";
            public const string UserNotFound = $"{ProjectPrefix}user_not_found";
            public const string TaskNotFound = $"{ProjectPrefix}task_not_found";
            public const string TaskLogTimeNotFound = $"{ProjectPrefix}task_log_time_not_found";
            public const string ProjectHaveNoAccess = $"{ProjectPrefix}project_have_no_access";
            public const string TaskHaveNoAccess = $"{ProjectPrefix}task_have_no_access";
            public const string CommentNotFoundOrNotAccess = $"{ProjectPrefix}comment_not_found_or_have_no_access";
            public const string CommentNotFound = $"{ProjectPrefix}comment_not_found";
            public const string UserInMainAppNotFound = $"{ProjectPrefix}user_main_app_not_found";
            public const string EmptyTaskName = $"{ProjectPrefix}emptyTaskName";
            public const string TaskWithStatusExists = $"{ProjectPrefix}task_with_status_exist";
            public const string WorkTaskEmptyStatusName = $"{ProjectPrefix}empty_status_name";
            public const string WorkTaskStatusNotExists = $"{ProjectPrefix}work_task_status_not_exist";
            public const string WorkTaskTimeLogValidationError = $"{ProjectPrefix}work_task_time_log_validation_error";
            public const string WorkTaskTimeLogIntervalValidationError = $"{ProjectPrefix}work_task_time_log_interval_validation_error";
            public const string SprintNotFound = $"{ProjectPrefix}sprint_not_found";
            public const string LabelNotFound = $"{ProjectPrefix}label_not_found";
            public const string LabelExists = $"{ProjectPrefix}label_exists";
            public const string BadRelationType = $"{ProjectPrefix}bad_relation";
            public const string RelationNotFound = $"{ProjectPrefix}relation_not_found";
            public const string PresetNotFound = $"{ProjectPrefix}preset_not_found";
            public const string PresetNotValide = $"{ProjectPrefix}preset_not_valide";
            public const string RelationError = $"{ProjectPrefix}relation_error";



        }

        public class EmailConfigurationsCode
        {
            public const string AddedNewCommentInTask = "AddedNewCommentInTask";
            public const string NewExecutorInTask = "NewExecutorInTask";
            public const string StatusInTaskWasChanged = "StatusInTaskWasChanged";
            public const string TaskWasChanged = "TaskWasChanged";

        }

        public class CacheKeys
        {
            public static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(5);
            public const string Project = $"{ProjectPrefix}Project";
            public const string Sprints = $"{ProjectPrefix}Sprints_";
            public const string Users = $"{ProjectPrefix}Users_";
            public const string TaskStatuses = $"{ProjectPrefix}TaskStatuses_";
            public const string Presets = $"{ProjectPrefix}Presets_";


            //.Include(x => x.Sprints).Include(x => x.Users).Include(x => x.TaskStatuses)
            //.Include(x => x.Presets)
        }
    }
}
