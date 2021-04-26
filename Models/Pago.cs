using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Código")]
        public int IdPago { get; set; }

        [Required]
        [Display(Name = "Importe($)")]
        public int Importe { get; set; }
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.Date)]
        public DateTime FechaDePago { get; set; }
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }
        [ForeignKey("IdContrato")]
        public Contrato Contrato { get; set; }
    }
}
