using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaStock.Models
{
    internal class Movimiento
    {
       
        
            public int Id { get; set; }
            public int ProductoId { get; set; }
            public string Tipo { get; set; }
            public int Cantidad { get; set; }
            public string Fecha { get; set; }
        
    }
}
