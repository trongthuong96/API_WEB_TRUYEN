using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API.Models.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ApartmentNumber { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int WardId { get; set; }
        [ForeignKey("WardId")]
        public Ward Ward { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
