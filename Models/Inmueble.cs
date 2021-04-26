using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public enum enEstado
    {
        Disponible = 1,
        No_disponible = 2,
        En_Refaccion = 3,
    }
    public class Inmueble
    {
        [Display(Name = "Codigo")]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public int Precio { get; set; }
        [Required]
        public int Estado { get; set; }
        [Required]
        [Display(Name = "Dueño")]
        public int IdPropietario { get; set; }
        [ForeignKey(nameof(IdPropietario))]
        public Propietario Propietario { get; set; }

        [Display(Name = "Estado")]
        public string EstadoNombre => Estado > 0 ? ((enEstado)Estado).ToString() : "";

        public static IDictionary<int, string> ObtenerEstado()
        {
            SortedDictionary<int, string> estados = new SortedDictionary<int, string>();
            Type tipoEnumEstado = typeof(enEstado);
            foreach (var valor in Enum.GetValues(tipoEnumEstado))
            {
                estados.Add((int)valor, Enum.GetName(tipoEnumEstado, valor));
            }
            return estados;
        }
    }
}

