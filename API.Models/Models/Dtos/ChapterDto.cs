using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class ChapterDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int NumberChapter { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Views { get; set; }

        [Required]
        public Guid StoryId { get; set; }
        public String StoryName { get; set; }
    }
}
