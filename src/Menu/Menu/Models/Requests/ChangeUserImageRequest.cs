using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Menu.Host.Models.Requests
{
    public class ChangeUserImageRequest
    {
        [FromForm]
        public IFormFile Image { get; set; }
    }
}
