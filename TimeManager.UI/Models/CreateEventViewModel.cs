﻿using System;
using System.ComponentModel.DataAnnotations;
using TimeManager.UI.Infrastructure.ValidationAttributes;

namespace TimeManager.UI.Models
{
    public class CreateEventViewModel
    {
        [Required()]
        [MaxLength(50)]
        public string Name { get; set; }

        [StartsAfterToday(ErrorMessage = "{0} can't be lower then today")]
        [Display(Name = "Begin time")]
        public DateTime BeginTime { get; set; }

        [EndDateValidation("Begin time", "BeginTime", ErrorMessage = "{0} must be bigger then {1}")]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}