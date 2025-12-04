using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class PresetRepository : GeneralRepository<Preset, long>, IPresetRepository
    {
        public PresetRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }
    }
}
