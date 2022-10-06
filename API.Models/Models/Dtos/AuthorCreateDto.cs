using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API.Models
{
    public class AuthorCreateDto
    {
        [Required]
        public string pseudonym { get; set; }
    }
}
