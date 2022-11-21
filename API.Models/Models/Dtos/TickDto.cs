using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class TickDto
    {
        public TickDto()
        {

        }

        public TickDto(string userId, Guid storyId)
        {
            this.UserId = userId;
            this.StoryId = storyId;
        }

        public string UserId { get; set; }
        public Guid StoryId { get; set; }
    }
}
