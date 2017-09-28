﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStoreUsingCore.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
