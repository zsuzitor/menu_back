using Azure.Storage.Blobs;
using BL.Models.Services.Interfaces;
using BO.Models.Config;
using System.IO;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class ImageDataBlobStorage : IImageDataStorage
    {
        private readonly string _connectionString;
        private readonly string _containerImagesName;
        private readonly string _containerUploadedImagesName;

        public ImageDataBlobStorage(ImageConfig settings)
        {
            _connectionString = settings.ConnectionString;//TODO остальное тоже в конфиги?
            string dbGuid = "8e188595-ae55-42f6-bca2-a88814e5f2b8";
            _containerImagesName = "menuimages-";
            _containerUploadedImagesName = _containerImagesName + "uploads-" + dbGuid;
            _containerImagesName += dbGuid;
        }

        public async Task Init()
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);


            // Create the container and return a container client object
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(_containerImagesName);
        }

        public async Task<string> Create(Stream readStream, string fileName)
        {
            return await Create(readStream, _containerImagesName, fileName);
        }

        public async Task<string> CreateUpload(Stream readStream, string fileName)
        {
            return await Create(readStream, _containerUploadedImagesName, fileName);
        }

        public async Task<bool> Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerImagesName);
            return await containerClient.DeleteBlobIfExistsAsync(path, Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        }

        private async Task<string> Create(Stream readStream,string path, string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(path);
            //string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;//TODO так нельзя,+ формирование имени мб вынести в 1 место
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            _=await blobClient.UploadAsync(readStream, true);
            return blobClient.Uri.ToString();
        }

        
    }
}
