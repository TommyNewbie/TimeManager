using System;
using System.ComponentModel.DataAnnotations;

namespace TimeManager.UI.Models
{
    public class CreateEventViewModel
    {
        [Required()]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Begin time")]
        public DateTime BeginTime { get; set; }

        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}