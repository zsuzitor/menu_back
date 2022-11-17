using PlanitPoker.Models.Entity;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types.PlanitPoker
{
    public sealed class RoomShort
    {
        [JsonPropertyName("roomname")]
        public string Roomname { get; set; }
        [JsonPropertyName("image_path")]
        public string ImgPath { get; set; }

        public RoomShort(RoomShortInfo room)
        {
            Roomname = room.Name;
            ImgPath = room.ImagePath;
        }
    }
}
