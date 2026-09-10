using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SistemaStock.Views;
using System.Linq;
using SistemaStock.Models;

namespace SistemaStock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Datos.InicializarDatos();
            ActualizarDashboard();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            Productos ventana = new Productos();
            ventana.ShowDialog();
            ActualizarDashboard();
        }

        private void btnMovimientos_Click(object sender, RoutedEventArgs e)
        {
              Movimientos ventana = new Movimientos();
              ventana.ShowDialog();
              ActualizarDashboard();
            
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            Configuracion ventana = new Configuracion();
            ventana.Show();
        }

        private void ActualizarDashboard()
        {
            txtTotalProductos.Text = Datos.Productos.Count.ToString();

            txtStockTotal.Text = Datos.Productos.Sum(p => p.Stock).ToString();

            txtStockBajo.Text = Datos.Productos
                .Count(p => p.Stock < p.StockMinimo)
                .ToString();

            TablaUltimosMovimientos.ItemsSource = Datos.Movimientos
                .OrderByDescending(m => m.Fecha)
                .Take(5)
                .ToList();
        }
    }
}