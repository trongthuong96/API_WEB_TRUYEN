using API.Models.Models;
using API.Models.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class StoryCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public AuthorCreateDto Author { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public ICollection<CategoryStoryDto> categoryStoryDtos { get; set; }
    }
}
