using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaStock.Models
{
    internal class Datos
    {

        public static List<Producto> Productos { get; set; } = new List<Producto>();

        public static List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

        public static void InicializarDatos()
        {
            if (Productos.Count == 0)
            {
                Productos.Add(new Producto
                {
                    Id = 1,
                    Nombre = "Producto 1",
                    Categoria = "Categoría A",
                    Precio = 10.99m,
                    Stock = 100,
                    StockMinimo = 20
                });

                Productos.Add(new Producto
                {
                    Id = 2,
                    Nombre = "Producto 2",
                    Categoria = "Categoría B",
                    Precio = 15.49m,
                    Stock = 50,
                    StockMinimo = 10
                });

                Productos.Add(new Producto
                {
                    Id = 3,
                    Nombre = "Producto 3",
                    Categoria = "Categoría A",
                    Precio = 7.99m,
                    Stock = 200,
                    StockMinimo = 30
                });
            }

            if (Movimientos.Count == 0)
            {
                Movimientos.Add(new Movimiento
                {
                    Id = 1,
                    ProductoId = 1 ,
                    Tipo = "Entrada",
                    Cantidad = 10,
                    Fecha = "2024-06-01"
                });

                Movimientos.Add(new Movimiento
                {
                    Id = 2,
                    ProductoId = 2,
                    Tipo = "Salida",
                    Cantidad = 5,
                    Fecha = "2024-06-02"
                });

                Movimientos.Add(new Movimiento
                {
                    Id = 3,
                    ProductoId = 3,
                    Tipo = "Entrada",
                    Cantidad = 20,
                    Fecha = "2024-06-03"
                });
            }
        }
    }
}