using System;
using System.ComponentModel.DataAnnotations;
using TimeManager.UI.Infrastructure.ValidationAttributes;

namespace TimeManager.UI.Models
{
    public class EditEventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Begin time")]
        public DateTime BeginTime { get; set; }

        [EndDateValidation("Begin time", "BeginTime", ErrorMessage = "{0} must be bigger then {1}")]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}