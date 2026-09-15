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
            Datos.CargarProductos();
            CargarMovimientos();

            ActualizarDashboard();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            Productos ventana = new Productos();
            ventana.ShowDialog();
           ;
            ActualizarDashboard();
            
        }

        private void btnMovimientos_Click(object sender, RoutedEventArgs e)
        {
              Movimientos ventana = new Movimientos();
              ventana.ShowDialog();
              Datos.CargarProductos();
              CargarMovimientos();

            ActualizarDashboard();
            
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            Configuracion ventana = new Configuracion();
            ventana.ShowDialog();
            ActualizarDashboard();
        }

        private void ActualizarDashboard()
        {
            Datos.CargarProductos();

            txtTotalProductos.Text = Datos.Productos.Count.ToString();

            txtStockTotal.Text = Datos.Productos.Sum(p => p.Stock).ToString();

            int stockMinimo = Properties.Settings.Default.StockMinimoPredeterminado;

            txtStockBajo.Text = Datos.Productos
                .Count(p => p.Stock < stockMinimo)
                .ToString();

            TablaUltimosMovimientos.ItemsSource = Datos.Movimientos
                .OrderByDescending(m => m.Fecha)
                .Take(5)
                .ToList();
        }

       private void CargarMovimientos()
        {
            Datos.Movimientos.Clear();
            using (var conexion = Conexion.ObtenerConexion())
            {
                conexion.Open();

                string sql = @"SELECT m.Id, m.ProductoId, p.Nombre, m.Tipo, m.Cantidad, m.Fecha
                       FROM Movimientos m
                       INNER JOIN Productos p ON p.Id = m.ProductoId
                       ORDER BY m.Id DESC";

                using (var comando = new Microsoft.Data.SqlClient.SqlCommand(sql, conexion))
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Datos.Movimientos.Add(new Movimiento
                        {
                            Id = lector.GetInt32(0),
                            ProductoId = lector.GetInt32(1),
                            Producto = lector.GetString(2),
                            Tipo = lector.GetString(3),
                            Cantidad = lector.GetInt32(4),
                            Fecha = lector.GetString(5)
                        });
                    }
                }
            }
        }
    }
}
