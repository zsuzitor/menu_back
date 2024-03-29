﻿

using PlanitPoker.Models.Enums;
using PlanitPoker.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PlanitPoker.Models.Returns
{
    public sealed class StoredRoomReturn
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("image_path")]
        public string ImagePath { get; set; }


        [JsonPropertyName("die_date")]
        public DateTime DieDate { get; set; }
        [JsonPropertyName("users")]
        public List<PlanitUserReturn> Users { get; set; }
        [JsonPropertyName("status")]
        public RoomSatus Status { get; set; }


        [JsonPropertyName("actual_stories")]
        public List<StoryReturn> Stories { get; set; }
        [JsonPropertyName("total_stories_count")]
        public long TotalNotActualStoriesCount { get; set; }

        [JsonPropertyName("current_story_id")]
        public string CurrentStoryId { get; set; }

        [JsonPropertyName("cards")]
        public List<string> Cards { get; set; }

        public StoredRoomReturn()
        {
            Users = new List<PlanitUserReturn>();
            Stories = new List<StoryReturn>();
            CurrentStoryId = "";
            Cards = new List<string>();
        }

        public StoredRoomReturn(StoredRoom obj) : this()
        {
            if (obj == null)
            {
                return;
            }

            Name = obj.Name;
            ImagePath = obj.ImagePath;
            DieDate = obj.DieDate;
            Status = obj.Status;
            CurrentStoryId = obj.CurrentStoryId;
            TotalNotActualStoriesCount = obj.TotalNotActualStoriesCount;

            if (obj.Users != null)
            {
                Users = obj.Users.Select(x => new PlanitUserReturn(x)).ToList();

            }

            if (obj.Stories != null)
            {
                Stories = obj.Stories.Select(x => new StoryReturn(x)).ToList();
            }

            Cards = obj.Cards.ToList();
        }
    }
}
