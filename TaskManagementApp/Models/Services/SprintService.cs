using DAL.Migrations;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    internal class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;    
        public SprintService(ISprintRepository sprintRepository) {
            _sprintRepository = sprintRepository;
        }
    }
}
