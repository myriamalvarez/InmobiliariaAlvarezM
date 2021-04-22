using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class LoginView
    {
        //[Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       // [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
    }
}
