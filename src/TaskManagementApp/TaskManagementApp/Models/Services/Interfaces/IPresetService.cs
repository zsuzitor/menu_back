using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IPresetService
    {
        Task<List<Preset>> GetAllAsync(long projectId, long userId);
        Task<List<Preset>> GetAllAsync(long projectId);
        Task<List<Preset>> GetAllWithLabelsAsync(long projectId);
        Task<Preset> DeleteAsync(long presetId, long userId);
        Task<Preset> CreateAsync(long projectId, string name, long userId);
        Task<Preset> EditAsync(Preset preset, long userId);
    }
}
