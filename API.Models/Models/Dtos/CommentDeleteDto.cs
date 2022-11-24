using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class CommentDeleteDto
    {
        public string UserId { get; set; }
        public Guid StoryId { get; set; }
    }
}
