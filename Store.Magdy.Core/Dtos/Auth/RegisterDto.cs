using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Email is Required !")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Display Name is Required !")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Password is Required !")]
        public string Password { get; set; }

    }
}
