using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using SistemaStock.Models;

namespace SistemaStock.Views
{
   
    public partial class Productos : Window
    {
        private List<Producto> productos = new List<Producto>();

        public Productos()
        {
            InitializeComponent();

            productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Categoria = "Categoría A", Precio = 10.99m, Stock = 100 });
            productos.Add(new Producto {Id = 2, Nombre = "Producto 2", Categoria = "Categoría B", Precio = 15.49m, Stock = 50 });   
            productos.Add(new Producto { Id = 3, Nombre = "Producto 3", Categoria = "Categoría A", Precio = 7.99m, Stock = 200 });

            TablaProductos.ItemsSource = productos;
        }

        private void AgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            AgregarProducto ventana = new AgregarProducto();

            if (ventana.ShowDialog() == true)
            {
                Producto nuevo = ventana.nuevoProducto;

                nuevo.Id = productos.Count + 1;

                productos.Add(nuevo);

                TablaProductos.Items.Refresh();
            }
        }
    }

    
}
