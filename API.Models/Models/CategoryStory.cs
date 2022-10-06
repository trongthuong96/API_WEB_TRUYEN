using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API.Models.Models
{
    public class CategoryStory
    {
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public Guid StoryId { get; set; }
        [ForeignKey("StoryId")]
        public Story Story { get; set; }
    }
}
