using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlvarezM.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> BuscarPorContrato(int id);
    }
}
