using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CategoryCreateDto

    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
