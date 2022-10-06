using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Models
{
    public class Like
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public Guid StoryId { get; set; }
        [ForeignKey("StoryId")]
        public Story Story { get; set; }
        public int count { get; set; }
    }
}
