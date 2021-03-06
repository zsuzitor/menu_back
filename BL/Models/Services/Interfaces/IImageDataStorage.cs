﻿
using System.IO;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IImageDataStorage
    {
        Task Init();
        Task<bool> Delete(string path);
        Task<string> Create(Stream readStream, string fileName);
        Task<string> CreateUpload(Stream readStream, string fileName);
    }
}
