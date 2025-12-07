using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IPresetRepository : IGeneralRepository<Preset, long>
    {
        Task<List<Preset>> GetAllAsync(long projectId);
        Task<Preset> GetWithLabelsAsync(long projectId);
        Task<List<WorkTaskLabelPresetRelation>> DeleteAsync(List<WorkTaskLabelPresetRelation> list);
    }
}
