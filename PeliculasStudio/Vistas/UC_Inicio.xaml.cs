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
        private static List<Pelicula> _peliculasRecomendadas;
        private static List<Pelicula> _peliculasSeguirViendo; 
        private static Pelicula _peliculaBanner;

        public UC_Inicio(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;

        
            listaSeguirViendoScrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            listaDestacadosScrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

            CargarDatos();
        }

        private void CargarDatos()
        {
            var db = DatabaseServicie.GetConexion();
            if (db == null) return;

            var todas = db.Table<Pelicula>().ToList();
            if (todas.Count == 0) return;

           
            if (_peliculasRecomendadas == null || _peliculaBanner == null)
            {
                var random = new Random();
                var mezcladas = todas.OrderBy(x => random.Next()).ToList();
                _peliculaBanner = mezcladas.First();
          
                _peliculasRecomendadas = mezcladas.Skip(1).Take(10).ToList();
            }

            _peliculasSeguirViendo = DatabaseServicie.ObtenerPeliculasSeguirViendo(_usuarioActual.Id);

         
            CargarBanner(_peliculaBanner);

          
            listaDestacados.ItemsSource = _peliculasRecomendadas;

            
            if (_peliculasSeguirViendo != null && _peliculasSeguirViendo.Count > 0)
            {
                txtSeguirViendo.Visibility = Visibility.Visible;
                listaSeguirViendoScrollViewer.Visibility = Visibility.Visible;
                listaSeguirViendo.ItemsSource = _peliculasSeguirViendo;
            }
            else
            {
                txtSeguirViendo.Visibility = Visibility.Collapsed;
                listaSeguirViendoScrollViewer.Visibility = Visibility.Collapsed;
            }
        }

        private void CargarBanner(Pelicula peli)
        {
            if (peli != null && !string.IsNullOrEmpty(peli.RutaImagenCompleta))
            {
                try
                {
                    var bitmap = new BitmapImage(new Uri(peli.RutaImagenCompleta));
                    imgBanner.Source = bitmap;
                   
                    txtBannerTitulo.Text = peli.Titulo.ToUpper();
                }
                catch { }
            }
        }

       
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                
                scv.LineLeft();
                scv.LineLeft(); 
                scv.LineLeft();
            }
            else
            {
               
                scv.LineRight();
                scv.LineRight();
                scv.LineRight();
            }
            e.Handled = true; 
        }

       

        private void NavegarADetalle(Pelicula peli)
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Detalle(peli, _usuarioActual, OrigenNavegacion.Inicio));
        }

        private void Card_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Pelicula peli)
            {
                NavegarADetalle(peli);
            }
        }

        private void BtnBanner_Click(object sender, RoutedEventArgs e)
        {
            if (_peliculaBanner != null) NavegarADetalle(_peliculaBanner);
        }

        private void BtnIrCatalogo_Click(object sender, RoutedEventArgs e)
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Catalogo(_usuarioActual));
        }

        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = !App.IsDarkMode;
            GestordeTemas.AplicarTema(App.IsDarkMode);
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
        private void MenuPerfil_Click(object sender, RoutedEventArgs e) { /* fdalta ir a perfil */ }

        private void MenuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            _peliculasRecomendadas = null;
            _peliculaBanner = null;
            _peliculasSeguirViendo = null;
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Login());
        }

    }
}
