using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class ChapterCreateDto
    {
        [Required]
        public int NumberChapter { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid StoryId { get; set; }
    }
}
