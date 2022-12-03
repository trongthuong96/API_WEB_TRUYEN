using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class CommentCreateDto
    {
        [Required]
        public string Content { get; set; }
        public Guid StoryId { get; set; }
        public string UserId { get; set; }
    }
}
