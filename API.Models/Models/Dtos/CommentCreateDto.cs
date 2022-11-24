using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class CommentCreateDto
    {
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public string UserId { get; set; }
    }
}
