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
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            Productos ventana = new Productos();
            ventana.Show();
        }

        private void btnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            Movimientos ventana = new Movimientos();
            ventana.Show();
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            Configuracion ventana = new Configuracion();
            ventana.Show();
        }
    }
}