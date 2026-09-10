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

            Datos.InicializarDatos();

            cmbProducto.ItemsSource = Datos.Productos;
            cmbProducto.DisplayMemberPath = "Nombre";

            cmbTipoMovimiento.SelectedIndex = 0;
        }
        private void GuardarMovimiento_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProducto.SelectedItem is Producto productoSeleccionado &&
        cmbTipoMovimiento.SelectedItem is ComboBoxItem tipoSeleccionado)
            {
                string tipo = tipoSeleccionado.Content.ToString();

                int cantidad;

                if (int.TryParse(txtCantidad.Text, out cantidad))
                {
                    // Validar que la cantidad sea mayor a 0
                    if (cantidad <= 0)
                    {
                        MessageBox.Show("La cantidad debe ser mayor a 0.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Validar que haya suficiente stock para una salida
                    if (tipo == "Salida" && cantidad > productoSeleccionado.Stock)
                    {
                        MessageBox.Show("No hay suficiente stock para realizar esta salida.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Modificar el stock
                    if (tipo == "Entrada")
                    {
                        productoSeleccionado.Stock += cantidad;
                    }
                    else if (tipo == "Salida")
                    {
                        productoSeleccionado.Stock -= cantidad;
                    }

                    // Crear el movimiento
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
                    MessageBox.Show("Ingrese una cantidad válida.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto y un tipo de movimiento.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
