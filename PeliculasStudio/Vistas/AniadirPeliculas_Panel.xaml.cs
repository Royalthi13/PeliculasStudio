using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para AniadirPeliculas_Panel.xaml
    /// </summary>
    public partial class AniadirPeliculas_Panel : Window
    {
        private TipoRol _rolAdminActual;
        public AniadirPeliculas_Panel(TipoRol rol)
        {

            InitializeComponent();
            _rolAdminActual = rol;

            // Cargamos los géneros del Enum automáticamente
            cbGenero.ItemsSource = Enum.GetValues(typeof(GeneroPelicula));
            cbGenero.SelectedIndex = 0;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
      
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtResumen.Text))
            {
                MessageBox.Show("El título y el resumen son obligatorios.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtAnio.Text, out int anioValidado) || anioValidado < 1888 || anioValidado > 2030)
            {
                MessageBox.Show("Por favor, introduce un año válido (entre 1888 y 2030).", "Año incorrecto", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

     
            GeneroPelicula generoSeleccionado = (GeneroPelicula)cbGenero.SelectedItem;

         
            string respuesta = DatabaseServicie.AniadirPelicula(
                _rolAdminActual,
                txtTitulo.Text.Trim(),
                anioValidado,
                generoSeleccionado,
                txtResumen.Text.Trim(),
                txtTrailer.Text.Trim(),
                txtPortada.Text.Trim()
            );

            // 5. Gestión del resultado
            if (respuesta.StartsWith("ÉXITO"))
            {
                MessageBox.Show(respuesta, "Operación Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);

            
                this.DialogResult = true;
                this.Close();
            }
            else
            {
          
                MessageBox.Show(respuesta, "Error al guardar", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
