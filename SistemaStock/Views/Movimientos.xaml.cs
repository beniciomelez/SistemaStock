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
using System.Linq;



namespace SistemaStock.Views
{
    /// <summary>
    /// Lógica de interacción para Movimientos.xaml
    /// </summary>
    public partial class Movimientos : Window
    {
        private List<Models.Movimiento> movimientos = new List<Models.Movimiento>();

        public Movimientos()
        {
            InitializeComponent();

            movimientos.Add(new Models.Movimiento { Id = 1, Producto = "Producto 1", Tipo = "Entrada", Cantidad = 10, Fecha = "2024-06-01" });
            movimientos.Add(new Models.Movimiento { Id = 2, Producto = "Producto 2", Tipo = "Salida", Cantidad = 5, Fecha = "2024-06-02" });
            movimientos.Add(new Models.Movimiento { Id = 3, Producto = "Producto 3", Tipo = "Entrada", Cantidad = 20, Fecha = "2024-06-03" });

            cmbTipo.SelectedIndex = 0;
            TablaMovimientos.ItemsSource = movimientos;
        }
        private void FiltrarMovimientos_Click(object sender, RoutedEventArgs e)
        {
            string tipoSeleccionado = (cmbTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
            List<Models.Movimiento> resultados;
            if (tipoSeleccionado == "Todos")
            {
                resultados = movimientos;
            }
            else
            {
                resultados = movimientos.FindAll(m => m.Tipo == tipoSeleccionado);
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

            var resultados = movimientos
                .Where(m => m.Producto.ToLower().Contains(texto))
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

    }
}
