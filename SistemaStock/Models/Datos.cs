using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaStock.Models
{
    internal class Datos
    {

        public static List<Producto> Productos { get; set; } = new List<Producto>();

        public static List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

        public static void CargarProductos()
        {
            Productos.Clear();

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string sql = @"SELECT Id, Nombre, Categoria, Precio, Stock, StockMinimo
                       FROM Productos";

                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Productos.Add(new Producto
                        {
                            Id = lector.GetInt32(0),
                            Nombre = lector.GetString(1),
                            Categoria = lector.GetString(2),
                            Precio = lector.GetDecimal(3),
                            Stock = lector.GetInt32(4),
                            StockMinimo = lector.GetInt32(5)
                        });
                    }
                }
            }
        }
    }
}