

using BO.Models.DAL.Domain;
using Menu.Models.Returns.Interfaces;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types
{
    public class ShortUserReturnFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is User objTyped)
            {
                return new ShortUserReturn(objTyped);
            }

            return obj;
        }


        public class ShortUserReturn
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("main_image_path")]
            public string ImagePath { get; set; }


            public ShortUserReturn(User obj)
            {
                Id = obj.Id;
                Name = obj.Name;
                Email = obj.Email;
                ImagePath = obj.ImagePath;
            }
        }

    }
}
