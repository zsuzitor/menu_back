using System;
using System.Collections.Generic;
using System.Text;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class PresetService : IPresetService
    {
        private readonly IPresetRepository _presetrepo;

        public PresetService(IPresetRepository presetRepo)
        {
            _presetrepo = presetRepo;
        }


    }
}
