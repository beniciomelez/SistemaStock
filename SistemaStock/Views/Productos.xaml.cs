using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using SistemaStock.Models;
using System.Linq;

namespace SistemaStock.Views
{

    public partial class Productos : Window
    {
        

        public Productos()
        {
            InitializeComponent();

            CargarProductos();
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ventana = new AgregarProducto();

            if (ventana.ShowDialog() == true)
            {
                Producto nuevo = ventana.nuevoProducto;

                using (var conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    string sql = @"INSERT INTO Productos
                           (Nombre, Categoria, Precio, Stock, StockMinimo)
                           VALUES
                           (@Nombre, @Categoria, @Precio, @Stock, @StockMinimo)";

                    using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
                        comando.Parameters.AddWithValue("@Categoria", nuevo.Categoria);
                        comando.Parameters.AddWithValue("@Precio", nuevo.Precio);
                        comando.Parameters.AddWithValue("@Stock", nuevo.Stock);
                        comando.Parameters.AddWithValue("@StockMinimo", nuevo.StockMinimo);

                        comando.ExecuteNonQuery();
                    }
                }

                Datos.Productos.Add(nuevo);

                TablaProductos.Items.Refresh();

                MessageBox.Show("Producto guardado correctamente.");
            }
        }

        private void BuscarProducto_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string texto = txtBuscar.Text.ToLower();

            List<Producto> resultados = Datos.Productos
                .Where(p => p.Nombre.ToLower().Contains(texto))
                .ToList();

            TablaProductos.ItemsSource = resultados;
        }

        private void CargarProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string sql = @"SELECT Id, Nombre, Categoria, Precio, Stock, StockMinimo
                       FROM Productos";

                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Categoria = reader.GetString(2),
                            Precio = reader.GetDecimal(3),
                            Stock = reader.GetInt32(4),
                            StockMinimo = reader.GetInt32(5)
                        });
                    }
                }
            }

            Datos.Productos = productos;
            TablaProductos.ItemsSource = Datos.Productos;
        }

    }
}
