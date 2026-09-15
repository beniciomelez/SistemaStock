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

                    // Buscar si ya existe un producto inactivo con el mismo nombre para reactivarlo
                    Producto existente = null;

                    string sqlBuscar = @"SELECT Id, StockMinimo FROM Productos
                                 WHERE Activo = 0 AND LOWER(Nombre) = LOWER(@Nombre)";

                    using (var comandoBuscar = new Microsoft.Data.SqlClient.SqlCommand(sqlBuscar, conexion))
                    {
                        comandoBuscar.Parameters.AddWithValue("@Nombre", nuevo.Nombre);

                        using (var lector = comandoBuscar.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                existente = new Producto
                                {
                                    Id = lector.GetInt32(0),
                                    StockMinimo = lector.GetInt32(1)
                                };
                            }
                        }
                    }

                    if (existente != null)
                    {
                        string sqlReactivar = @"UPDATE Productos
                                SET Activo = 1,
                                    Categoria = @Categoria,
                                    Precio = @Precio,
                                    Stock = @Stock
                                WHERE Id = @Id";

                        using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sqlReactivar, conexion))
                        {
                            comando.Parameters.AddWithValue("@Categoria", nuevo.Categoria);
                            comando.Parameters.AddWithValue("@Precio", nuevo.Precio);
                            comando.Parameters.AddWithValue("@Stock", nuevo.Stock);
                            comando.Parameters.AddWithValue("@Id", existente.Id);

                            comando.ExecuteNonQuery();
                        }

                        nuevo.Id = existente.Id;
                        nuevo.StockMinimo = existente.StockMinimo;
                        nuevo.Activo = true;

                        MessageBox.Show("El producto ya existía y fue reactivado correctamente.");
                    }
                    else
                    {
                        string sql = @"INSERT INTO Productos
                               (Nombre, Categoria, Precio, Stock, StockMinimo, Activo)
                               OUTPUT INSERTED.Id
                               VALUES
                               (@Nombre, @Categoria, @Precio, @Stock, @StockMinimo, 1)";

                        using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                        {
                            comando.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
                            comando.Parameters.AddWithValue("@Categoria", nuevo.Categoria);
                            comando.Parameters.AddWithValue("@Precio", nuevo.Precio);
                            comando.Parameters.AddWithValue("@Stock", nuevo.Stock);
                            comando.Parameters.AddWithValue("@StockMinimo", nuevo.StockMinimo);

                            nuevo.Id = Convert.ToInt32(comando.ExecuteScalar());
                        }

                        nuevo.Activo = true;

                        MessageBox.Show("Producto guardado correctamente.");
                    }
                }

                Datos.Productos.Add(nuevo);

                TablaProductos.Items.Refresh();
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

        private void EditarProducto_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Producto producto)
            {
                return;
            }

            AgregarProducto ventana = new AgregarProducto(producto);

            if (ventana.ShowDialog() == true)
            {
                Producto editado = ventana.nuevoProducto;

                using (var conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    string sql = @"UPDATE Productos
                           SET Nombre = @Nombre,
                               Categoria = @Categoria,
                               Precio = @Precio,
                               Stock = @Stock
                           WHERE Id = @Id";

                    using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", editado.Nombre);
                        comando.Parameters.AddWithValue("@Categoria", editado.Categoria);
                        comando.Parameters.AddWithValue("@Precio", editado.Precio);
                        comando.Parameters.AddWithValue("@Stock", editado.Stock);
                        comando.Parameters.AddWithValue("@Id", editado.Id);

                        comando.ExecuteNonQuery();
                    }
                }

                TablaProductos.Items.Refresh();

                MessageBox.Show("Producto actualizado correctamente.");
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Producto producto)
            {
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Seguro que querés eliminar \"{producto.Nombre}\"? Los movimientos históricos se conservarán.",
                "Eliminar producto",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
            {
                return;
            }

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string sql = "UPDATE Productos SET Activo = 0 WHERE Id = @Id";

                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", producto.Id);
                    comando.ExecuteNonQuery();
                }
            }

            Datos.Productos.Remove(producto);

            string texto = txtBuscar.Text.ToLower();
            TablaProductos.ItemsSource = Datos.Productos
                .Where(p => p.Nombre.ToLower().Contains(texto))
                .ToList();

            MessageBox.Show("Producto eliminado correctamente.");
        }

        private void CargarProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string sql = @"SELECT Id, Nombre, Categoria, Precio, Stock, StockMinimo, Activo
                       FROM Productos
                       WHERE Activo = 1";

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
                            StockMinimo = reader.GetInt32(5),
                            Activo = reader.GetBoolean(6)
                        });
                    }
                }
            }

            Datos.Productos = productos;
            TablaProductos.ItemsSource = Datos.Productos;
        }

    }
}
