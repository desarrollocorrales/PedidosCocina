using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbastecedoraPreparadosPedidos.Modelos
{
    public class Venta
    {
        public string FolioTicket { set; get; }
        public string Fecha { set; get; }
        public string Hora { set; get; }
        public string Clave { set; get; }
        public string Articulo { set; get; }
        public ushort Cantidad { set; get; }
    }
}
