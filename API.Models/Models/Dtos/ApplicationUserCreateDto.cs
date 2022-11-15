using System;
using System.Collections.Generic;
using System.Text;

namespace API.Models.Models.Dtos
{
    public class ApplicationUserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
