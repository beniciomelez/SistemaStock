using SistemaStock.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SistemaStock.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarMovimiento.xaml
    /// </summary>
    public partial class AgregarMovimiento : Window
    {
        public AgregarMovimiento()
        {
            InitializeComponent();

            CargarProductos();

            cmbTipoMovimiento.SelectedIndex = 0;
        }
        private void GuardarMovimiento_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProducto.SelectedItem is Producto productoSeleccionado &&
        cmbTipoMovimiento.SelectedItem is ComboBoxItem tipoSeleccionado)
            {
                string tipo = tipoSeleccionado.Content.ToString();

                int cantidad;

                if (!int.TryParse(txtCantidad.Text, out cantidad))
                {
                    MessageBox.Show("Ingrese una cantidad válida.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (tipo == "Salida" && cantidad > productoSeleccionado.Stock)
                {
                    MessageBox.Show("No hay suficiente stock para realizar esta salida.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Calcular nuevo stock
                int nuevoStock = productoSeleccionado.Stock;
                if (tipo == "Entrada")
                    nuevoStock += cantidad;
                else if (tipo == "Salida")
                    nuevoStock -= cantidad;

                // Guardar movimiento y actualizar stock en SQL Server (en una sola transacción)
                using (var conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    using (var transaccion = conexion.BeginTransaction())
                    {
                        string sqlInsert = @"
                                           INSERT INTO Movimientos (ProductoId, Tipo, Cantidad, Fecha)
                                           VALUES (@ProductoId, @Tipo, @Cantidad, @Fecha)";

                        using (var comandoInsert = new Microsoft.Data.SqlClient.SqlCommand(sqlInsert, conexion, transaccion))
                        {
                            comandoInsert.Parameters.AddWithValue("@ProductoId", productoSeleccionado.Id);
                            comandoInsert.Parameters.AddWithValue("@Tipo", tipo);
                            comandoInsert.Parameters.AddWithValue("@Cantidad", cantidad);
                            comandoInsert.Parameters.AddWithValue("@Fecha", DateTime.Now.ToString("yyyy-MM-dd"));
                            comandoInsert.ExecuteNonQuery();
                        }

                        string sqlUpdateStock = @"UPDATE Productos SET Stock = @Stock WHERE Id = @Id";

                        using (var comandoStock = new Microsoft.Data.SqlClient.SqlCommand(sqlUpdateStock, conexion, transaccion))
                        {
                            comandoStock.Parameters.AddWithValue("@Stock", nuevoStock);
                            comandoStock.Parameters.AddWithValue("@Id", productoSeleccionado.Id);
                            comandoStock.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                }

                // Reflejar el nuevo stock en el objeto en memoria
                productoSeleccionado.Stock = nuevoStock;

                // Crear movimiento para mostrarlo inmediatamente
                Movimiento nuevoMovimiento = new Movimiento
                {
                    Id = Datos.Movimientos.Count + 1,
                    ProductoId = productoSeleccionado.Id,
                    Tipo = tipo,
                    Cantidad = cantidad,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd")
                };

                Datos.Movimientos.Add(nuevoMovimiento);

                MessageBox.Show("Movimiento agregado correctamente.", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Seleccione un producto y un tipo de movimiento.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CargarProductos()
        {
            Datos.Productos.Clear();

            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Id, Nombre, Categoria, Precio, Stock, StockMinimo, Activo FROM Productos WHERE Activo = 1";
                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Producto producto = new Producto
                            {
                                Id = lector.GetInt32(0),
                                Nombre = lector.GetString(1),
                                Categoria = lector.GetString(2),
                                Precio = lector.GetDecimal(3),
                                Stock = lector.GetInt32(4),
                                StockMinimo = lector.GetInt32(5),
                                Activo = lector.GetBoolean(6)
                            };
                            Datos.Productos.Add(producto);
                        }
                    }
                }
            }
            cmbProducto.ItemsSource = Datos.Productos;
            cmbProducto.DisplayMemberPath = "Nombre";
            cmbProducto.SelectedValuePath = "Id";
        }
    }
}
