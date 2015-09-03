using System;
using System.ComponentModel.DataAnnotations;
using TimeManager.UI.Infrastructure;
using TimeManager.UI.Infrastructure.ValidationAttributes;

namespace TimeManager.UI.Models
{
    public class CreateEventViewModel
    {
        [Required()]
        [MaxLength(50)]
        public string Name { get; set; }

        [StartsAfterToday]
        [Display(Name = "Begin time")]
        public DateTime BeginTime { get; set; }

        [EndDateValidation("Begin time", "BeginTime")]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}