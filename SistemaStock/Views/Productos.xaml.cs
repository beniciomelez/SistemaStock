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

            Datos.InicializarDatos();
            TablaProductos.ItemsSource = Datos.Productos;
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ventana = new AgregarProducto();

            if (ventana.ShowDialog() == true)
            {
                Producto nuevo = ventana.nuevoProducto;

                nuevo.Id = Datos.Productos.Count + 1;

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


    }
}
