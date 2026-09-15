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

        public AgregarProducto(Producto productoAEditar) : this()
        {
            txtTitulo.Text = "Editar producto";
            Title = "Editar producto";
            btnAgregar.Content = "Guardar";

            txtNombre.Text = productoAEditar.Nombre;
            txtCategoria.Text = productoAEditar.Categoria;
            txtPrecio.Text = productoAEditar.Precio.ToString("0.##");
            txtStock.Text = productoAEditar.Stock.ToString();

            nuevoProducto = productoAEditar;
        }

        private void SoloTexto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void SoloNumeros_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void Precio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;

            foreach (char c in e.Text)
            {
                if (char.IsDigit(c))
                {
                    continue;
                }

                if (c == '.' || c == ',')
                {
                    string textoResultante = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength)
                                                         .Insert(textBox.SelectionStart, e.Text);
                    if (textoResultante.Count(ch => ch == '.' || ch == ',') > 1)
                    {
                        e.Handled = true;
                        return;
                    }
                    continue;
                }

                e.Handled = true;
                return;
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre no puede estar vacío.", "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("La categoría no puede estar vacía.", "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El precio no puede estar vacío.", "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("El stock no puede estar vacío.", "Campo requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string precioTexto = txtPrecio.Text.Replace(',', '.');

            if (!decimal.TryParse(precioTexto, System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Ingresá un precio válido (solo números y un punto o coma para decimales).", "Precio inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingresá un stock válido (solo números enteros positivos).", "Stock inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (nuevoProducto == null)
            {
                nuevoProducto = new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Categoria = txtCategoria.Text.Trim(),
                    Precio = precio,
                    Stock = stock,
                    StockMinimo = Properties.Settings.Default.StockMinimoPredeterminado
                };
            }
            else
            {
                nuevoProducto.Nombre = txtNombre.Text.Trim();
                nuevoProducto.Categoria = txtCategoria.Text.Trim();
                nuevoProducto.Precio = precio;
                nuevoProducto.Stock = stock;
            }

            DialogResult = true;
        }
    }
}
