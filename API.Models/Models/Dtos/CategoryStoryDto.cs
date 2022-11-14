using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class CategoryStoryDto
    {
        [Required]
        public Guid CategoryId { get; set; }
        public Guid StoryId { get; set; }
    }
}
