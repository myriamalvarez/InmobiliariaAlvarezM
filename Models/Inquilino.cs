using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name = "Codigo")]
        public int IdInquilino { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Dni { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string DireccionLaboral { get; set; }
        [Required]
        public string NombreGarante { get; set; }
        [Required]
        public string ApellidoGarante { get; set; }
        [Required]
        public string DniGarante { get; set; }
    }
}
