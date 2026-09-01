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
                    Stock = 100
                });

                Productos.Add(new Producto
                {
                    Id = 2,
                    Nombre = "Producto 2",
                    Categoria = "Categoría B",
                    Precio = 15.49m,
                    Stock = 50
                });

                Productos.Add(new Producto
                {
                    Id = 3,
                    Nombre = "Producto 3",
                    Categoria = "Categoría A",
                    Precio = 7.99m,
                    Stock = 200
                });
            }
        }
    }
}