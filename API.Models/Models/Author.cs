using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models
{
    [Index(nameof(pseudonym), IsUnique = true)]
    public class Author
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string pseudonym { get; set; }
    }
}
