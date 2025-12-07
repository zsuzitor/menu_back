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
        Task<List<Preset>> GetAllAsync(long projectId, UserInfo userInfo);
        Task<Preset> DeleteAsync(long presetId, UserInfo userInfo);
        Task<Preset> CreateAsync(string name, UserInfo userInfo);
        Task<Preset> EditAsync(Preset preset, UserInfo userInfo);
    }
}
