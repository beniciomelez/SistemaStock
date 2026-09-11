using SistemaStock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para Movimientos.xaml
    /// </summary>
    public partial class Movimientos : Window
    {
        

        public Movimientos()
        {
            InitializeComponent();

            cmbTipo.SelectedIndex = 0;
            CargarMovimientos();
        }
        private void FiltrarMovimientos_Click(object sender, RoutedEventArgs e)
        {
            string tipoSeleccionado = (cmbTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
            List<Models.Movimiento> resultados;
            if (tipoSeleccionado == "Todos")
            {
                resultados = Datos.Movimientos;
            }
            else
            {
                resultados = Datos.Movimientos.FindAll(m => m.Tipo == tipoSeleccionado);
            }
            TablaMovimientos.ItemsSource = resultados;

        }

        private void Filtrar()
        {
            string texto = txtBuscarMovimiento.Text.ToLower();

            string tipo = "";

            if (cmbTipo.SelectedItem is ComboBoxItem item)
            {
                tipo = item.Content.ToString();
            }

            var resultados = Datos.Movimientos
    .Where(m =>
    {
        var producto = Datos.Productos
            .FirstOrDefault(p => p.Id == m.ProductoId);

        return producto != null &&
               producto.Nombre.ToLower().Contains(texto);
    })
    .Where(m => tipo == "Todos" || m.Tipo == tipo)
    .ToList();

            TablaMovimientos.ItemsSource = resultados;
        }
        private void txtBuscarMovimiento_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtrar();
        }

        private void cmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtrar();
        }

        private void AgregarMovimiento_Click(object sender, RoutedEventArgs e)
        {
            AgregarMovimiento ventana = new AgregarMovimiento();

            if (ventana.ShowDialog() == true)
            {
                TablaMovimientos.ItemsSource = null;
                TablaMovimientos.ItemsSource = Datos.Movimientos;
            }
        }

        private void CargarMovimientos()
        {
            List<Models.Movimiento> movimientos = new List<Models.Movimiento>();
            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();
                string sql = @"
                            SELECT M.Id, M.ProductoId, P.Nombre, M.Tipo, M.Cantidad, M.Fecha
                            FROM Movimientos M
                            INNER JOIN Productos P ON M.ProductoId = P.Id
                            ORDER BY M.Id DESC";
                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                {
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Models.Movimiento movimiento = new Models.Movimiento
                            {
                                Id = lector.GetInt32(0),
                                ProductoId = lector.GetInt32(1),
                                Tipo = lector.GetString(3),
                                Cantidad = lector.GetInt32(4),
                                Fecha = lector.GetString(5)
                            };
                            movimientos.Add(movimiento);
                        }
                    }
                }
            }
            Datos.Movimientos = movimientos;
            TablaMovimientos.ItemsSource = Datos.Movimientos;
        }
    }
}
