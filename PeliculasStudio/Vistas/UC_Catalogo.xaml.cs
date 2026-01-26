using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para UC_Catalogo.xaml
    /// </summary>
    public partial class UC_Catalogo : UserControl
    {
        private Usuario _usuarioActual;
        private List<Pelicula> _todasLasPeliculas;
        public UC_Catalogo(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;

            CargarDatosIniciales();
        }
        private void CargarDatosIniciales()
        {
            var db = DatabaseServicie.GetConexion();
            if (db != null)
            {
                _todasLasPeliculas = db.Table<Pelicula>().ToList();

             
                var generos = Enum.GetValues(typeof(GeneroPelicula)).Cast<GeneroPelicula>().ToList();
                CmbGenero.Items.Clear();
                CmbGenero.Items.Add("Todos");
                foreach (var g in generos) CmbGenero.Items.Add(g.ToString());
                CmbGenero.SelectedIndex = 0;

                AplicarFiltros();
            }
        }

        private void AplicarFiltros()
        {
            if (_todasLasPeliculas == null) return;

            var lista = _todasLasPeliculas.AsEnumerable();

            
            string textoBuscar = txtBuscador.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                lista = lista.Where(p => p.Titulo.ToLower().Contains(textoBuscar));
            }

          
            if (CmbGenero.SelectedIndex > 0)
            {
                string genero = CmbGenero.SelectedItem.ToString();
                lista = lista.Where(p => p.Genero.ToString() == genero);
            }

        
            if (CmbOrden.SelectedIndex == 0) lista = lista.OrderByDescending(p => p.Anio);
            else if (CmbOrden.SelectedIndex == 1) lista = lista.OrderBy(p => p.Titulo);
            else if (CmbOrden.SelectedIndex == 2) lista = lista.OrderByDescending(p => p.Titulo);

            ListaCatalogo.ItemsSource = lista.ToList();
        }

        private void TxtBuscador_TextChanged(object sender, TextChangedEventArgs e) => AplicarFiltros();
        private void CmbGenero_SelectionChanged(object sender, SelectionChangedEventArgs e) => AplicarFiltros();
        private void CmbOrden_SelectionChanged(object sender, SelectionChangedEventArgs e) => AplicarFiltros();

      
        private void BtnIrInicio_Click(object sender, RoutedEventArgs e)
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Inicio(_usuarioActual));
        }

        private void Card_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Pelicula peli)
            {
                var main = Window.GetWindow(this) as MainWindow;
                main?.Navegar(new UC_Detalle(peli, _usuarioActual, OrigenNavegacion.Catalogo));
            }
        }



        private void MenuPerfil_Click(object sender, RoutedEventArgs e) { /* Ir a perfil */ }

        private void MenuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Login());
        }

        
    }
}
