using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Host.Models.WordsCardsApp.Requests
{
    public class CreateFromFileRequest
    {
        [FromForm]
        public IFormFile File { get; set; }
    }
}
