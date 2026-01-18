using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para UC_Inicio.xaml
    /// </summary>
    public partial class UC_Inicio : UserControl
    {
        private readonly Usuario _usuarioActual;

       
        private static List<Pelicula> _peliculasTendenciaCache;
        private static Pelicula _peliculaDestacadaCache;

        public UC_Inicio(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            btnTema.IsChecked = App.IsDarkMode;

            CargarPeliculas();
        }

        private void CargarPeliculas()
        {

            if (_peliculasTendenciaCache != null && _peliculaDestacadaCache != null)
            {

                listaDestacados.ItemsSource = _peliculasTendenciaCache;
                CargarBanner(_peliculaDestacadaCache);
                return;

            }

            var db = DatabaseServicie.GetConexion();
            if (db == null) return;

            var todasLasPelis = db.Table<Pelicula>().ToList();
            if (todasLasPelis.Count == 0) return;

            var pelisRandom = todasLasPelis.OrderBy(x => Guid.NewGuid()).ToList();

          
            _peliculaDestacadaCache = pelisRandom.First();
            _peliculasTendenciaCache = pelisRandom.Skip(1).Take(4).ToList();

          
            listaDestacados.ItemsSource = _peliculasTendenciaCache;
            CargarBanner(_peliculaDestacadaCache);
        }

        private void CargarBanner(Pelicula peli)
        {
            if (peli != null && !string.IsNullOrEmpty(peli.RutaImagenCompleta))
            {
                try
                {
                    imgBanner.Source = new BitmapImage(new Uri(peli.RutaImagenCompleta, UriKind.Absolute));
                    txtBannerTitulo.Text = peli.Titulo.ToUpper();
                }
                catch { }
            }
        }

     

        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void MenuPerfil_Click(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow as MainWindow;
            main?.Navegar(new Perfil(_usuarioActual));
        }

        private void MenuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
         
            _peliculasTendenciaCache = null;
            _peliculaDestacadaCache = null;

            var main = Application.Current.MainWindow as MainWindow;
            main?.Navegar(new UC_Login());
        }

        private void BtnIrCatalogo_Click(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow as MainWindow;
            main?.Navegar(new UC_Catalogo(_usuarioActual));
        }

        private void BtnBanner_Click(object sender, RoutedEventArgs e)
        {
           
            if (_peliculaDestacadaCache != null)
            {
                var main = Application.Current.MainWindow as MainWindow;
                main?.Navegar(new UC_Detalle(_peliculaDestacadaCache, _usuarioActual));
            }
        }

        private void Card_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Pelicula peli)
            {
                var main = Application.Current.MainWindow as MainWindow;
                main?.Navegar(new UC_Detalle(peli, _usuarioActual));
            }
        }

        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = btnTema.IsChecked ?? false;
            GestordeTemas.AplicarTema(App.IsDarkMode);
        }

    }
}
