using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Contrato
    {
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }
        public int Importe { get; set; }
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha de termino")]
        public DateTime FechaFin { get; set; }
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [Display(Name = "Direccion")]
        public int IdInmueble { get; set; }
        public Inquilino Inquilino { get; set; }
        public Inmueble Inmueble { get; set; }

    }
}
