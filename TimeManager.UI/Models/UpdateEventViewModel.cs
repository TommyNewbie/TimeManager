﻿using System;
using System.ComponentModel.DataAnnotations;
using TimeManager.UI.Infrastructure.ValidationAttributes;

namespace TimeManager.UI.Models
{
    public class UpdateEventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [MaxLength(50, ErrorMessage = "The {0} length must be lower then {1}.")]
        public string Name { get; set; }

        [Display(Name = "Begin time")]
        public DateTime BeginTime { get; set; }

        [EndDateValidation("Begin time", "BeginTime", ErrorMessage = "The {0} must be bigger then the {1}.")]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}