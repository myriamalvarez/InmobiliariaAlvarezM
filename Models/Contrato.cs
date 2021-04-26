using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }
        [Required]
        public int Importe { get; set; }
        [Required]
        [DisplayName("Fecha de inicio"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [DisplayName("Fecha Final"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [Display(Name = "Direccion")]
        public int IdInmueble { get; set; }
        public Inquilino Inquilino { get; set; }
        public Inmueble Inmueble { get; set; }

    }
}
