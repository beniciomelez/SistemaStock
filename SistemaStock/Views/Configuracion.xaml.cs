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
    /// Lógica de interacción para Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        public Configuracion()
        {
            InitializeComponent();
            txtStockMinimo.Text = Properties.Settings.Default.StockMinimoPredeterminado.ToString();
        }

        private void BtnGuardarStockMinimo_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtStockMinimo.Text, out int stockMinimo) && stockMinimo >= 0)
            {
                Properties.Settings.Default.StockMinimoPredeterminado = stockMinimo;
                Properties.Settings.Default.Save();

                MessageBox.Show("Stock mínimo guardado correctamente.");
            }
            else
            {
                MessageBox.Show("Ingresá un número válido.");
            }
        }

    }

}