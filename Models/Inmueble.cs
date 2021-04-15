using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Inmueble
    {
        [Display(Name = "Codigo")]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Uso { get; set; }
        public string Tipo { get; set; }
        public int Ambientes { get; set; }
        public int Precio { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Dueño")]
        public int IdPropietario { get; set; }
        [ForeignKey(nameof(IdPropietario))]
        public Propietario Propietario { get; set; }
    }
}

