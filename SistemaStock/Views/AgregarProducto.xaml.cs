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
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : Window
    {

        public Producto nuevoProducto { get; private set; }
        public AgregarProducto()
        {
            InitializeComponent();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
               string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("Completá todos los campos.");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Precio o stock inválido.");
                return;
            }

            nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text,
                Categoria = txtCategoria.Text,
                Precio = precio,
                Stock = stock
            };

            DialogResult = true;
        }
    }
}
