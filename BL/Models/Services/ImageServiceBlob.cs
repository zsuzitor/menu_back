//using Azure.Storage.Blobs;
//using BO.Models.DAL.Domain;
//using DAL.Models.DAL.Repositories.Interfaces;
//using Menu.Models.Services;
//using Menu.Models.Services.Interfaces;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL.Models.Services
//{
//    public class ImageServiceBlob : ImageService//IImageService
//    {

//        private readonly IImageRepository _imageRepository;
//        private readonly string _connectionString;
//        private readonly string _containerImagesName;
//        private readonly string _containerUploadedImagesName;


//        public ImageServiceBlob(IImageRepository imgRep):base(imgRep, null)
//        {
//            _connectionString = "";
//            string dbGuid = "8e188595-ae55-42f6-bca2-a88814e5f2b8";
//            _containerImagesName = "menu_images_";
//            _containerUploadedImagesName = _containerImagesName+"uploads_"+ dbGuid;
//            _containerImagesName += dbGuid;
//        }

//        public async Task Init()
//        {
//            // Create a BlobServiceClient object which will be used to create a container client
//            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);


//            // Create the container and return a container client object
//            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(_containerImagesName);
//        }


//        public override async Task<string> CreateWithOutDbRecord(IFormFile image)
//        {
//            return await CreateWithOutDbRecord(image, _containerImagesName);
//        }

//        public async Task<string> CreateUploadFileWithOutDbRecord(IFormFile image)
//        {
//            return await CreateWithOutDbRecord(image, _containerUploadedImagesName);
//        }

//        public async Task<CustomImage> DeleteById(long idImage)
//        {
//            var imgFromDb = await _imageRepository.Get(idImage);
//            if (imgFromDb == null)
//            {
//                return null;
//            }
//            return (await DeleteFull(new List<CustomImage>() { imgFromDb })).FirstOrDefault();
//        }

//        public async Task<List<CustomImage>> DeleteById(List<long> idImages)
//        {
//            if (idImages == null || idImages.Count == 0)
//            {
//                return new List<CustomImage>();
//            }

//            var imgFromDb = await _imageRepository.Get(idImages);
//            return await DeleteFull(imgFromDb);
//        }

//        public async Task<List<CustomImage>> DeleteFull(List<CustomImage> images)
//        {
//            if (images == null || images?.Count == 0)
//            {
//                return new List<CustomImage>();
//            }

//            await _imageRepository.Delete(images);
//            foreach (var img in images)
//            {
//                await DeleteFileWithOutDbRecord(img.Path);
//            }

//            return images;
//        }

//        public async Task<bool> DeleteFileWithOutDbRecord(string path)
//        {
//            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
//            var containerClient = blobServiceClient.GetBlobContainerClient(_containerImagesName);
//            return await containerClient.DeleteBlobIfExistsAsync(path, Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);

//        }

//        public async Task<List<CustomImage>> GetCreatableUploadObjects(List<IFormFile> images, long articleId)
//        {
//            if (images == null || images.Count == 0)
//            {
//                return null;
//            }

//            List<CustomImage> imagesForAdd = new List<CustomImage>();
//            foreach (var uploadedImg in images)
//            {
//                var physImg = await CreateUploadFileWithOutDbRecord(uploadedImg);

//                var img = new CustomImage()
//                {
//                    ArticleId = articleId,
//                    Path = physImg,
//                };
//                imagesForAdd.Add(img);
//            }

//            //_db.Images.AddRange(imagesForAdd);
//            //await _db.SaveChangesAsync();
//            return imagesForAdd;
//        }

//        public async Task<List<long>> GetIdsByArticleId(long idArticle)
//        {
//            throw new NotImplementedException();
//        }

//        public string GetRelativePath(string subPath)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<CustomImage> Upload(IFormFile image, long articleId)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<List<CustomImage>> Upload(List<IFormFile> images, long articleId)
//        {
//            throw new NotImplementedException();
//        }


//        private async Task<string> CreateWithOutDbRecord(IFormFile image, string path)
//        {
//            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
//            var containerClient = blobServiceClient.GetBlobContainerClient(path);
//            string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;//TODO так нельзя,+ формирование имени мб вынести в 1 место
//            BlobClient blobClient = containerClient.GetBlobClient(uniqueFileName);
//            await blobClient.UploadAsync(image.OpenReadStream(), true);
//            return uniqueFileName;
//        }
//    }
//}
