using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenC.Web.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [MinLength(10)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8)]
        public string Senha { get; set; }
    }
}