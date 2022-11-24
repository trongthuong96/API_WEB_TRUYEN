using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CreateDate { get; set; }
    }
}
