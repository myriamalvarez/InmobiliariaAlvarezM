using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public class Inquilino
    {
        public int IdInquilino { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Dni { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string DireccionLaboral { get; set; }

        public string NombreGarante { get; set; }

        public string ApellidoGarante { get; set; }

        public string DniGarante { get; set; }
    }
}
