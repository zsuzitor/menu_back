using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Host.Models.PlanitPoker.Requests
{
    public class ChangeRoomImageRequest
    {
        [FromForm]
        public string RoomName { get; set; }

        [FromForm]
        public IFormFile Image { get; set; }
    }
}
