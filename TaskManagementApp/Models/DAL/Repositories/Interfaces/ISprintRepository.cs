using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface ISprintRepository : IGeneralRepository<WorkTaskSprint, long>
    {
    }
}
