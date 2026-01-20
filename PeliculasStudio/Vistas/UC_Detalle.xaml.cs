using LibVLCSharp.Shared;
using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PeliculasStudio.Vistas
{
    public enum OrigenNavegacion
    {
        Inicio,
        Catalogo
    }
    public partial class UC_Detalle : UserControl, IDisposable
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _mediaPlayer;
        private readonly Pelicula _pelicula;
        private readonly Usuario _usuarioAnterior;
        private readonly OrigenNavegacion _origen;
        private bool _isDraggingSlider = false;

        public UC_Detalle(Pelicula pelicula, Usuario usuario, OrigenNavegacion origen)
        {
            InitializeComponent();

            _pelicula = pelicula;
            _usuarioAnterior = usuario;
            _origen = origen;

            Core.Initialize();
            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);
            VlcPlayer.MediaPlayer = _mediaPlayer;

          
            _mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            _mediaPlayer.EndReached += (s, e) => Dispatcher.Invoke(CerrarVideo);

            CargarDatosVisuales();
        }

        private void CargarDatosVisuales()
        {
            if (_pelicula == null) return;

            txtTitulo.Text = _pelicula.Titulo.ToUpper();
            txtAnio.Text = _pelicula.Anio.ToString();
            txtGenero.Text = _pelicula.Genero.ToString();
            txtResumen.Text = _pelicula.Resumen;
            txtVistas.Text = $"👁 {_pelicula.CantVisualizaciones} Vistas";

            if (!string.IsNullOrEmpty(_pelicula.RutaImagenCompleta) && File.Exists(_pelicula.RutaImagenCompleta))
            {
                var imagen = new BitmapImage(new Uri(_pelicula.RutaImagenCompleta));
                imgPortada.Source = imagen;
                imgFondo.Source = imagen; 
            }

            if (_usuarioAnterior.Rol == TipoRol.Admin)
            {
                btnEliminar.Visibility = Visibility.Visible;
            }
        }

        private void BtnReproducir_Click(object sender, RoutedEventArgs e)
        {
            string rutaVideo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Videos", _pelicula.TrailerPath);

            if (!File.Exists(rutaVideo))
            {
                MessageBox.Show($"No se encuentra el video en: {rutaVideo}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pelicula.CantVisualizaciones++;
            DatabaseServicie.GetConexion()?.Update(_pelicula);
            txtVistas.Text = $"👁 {_pelicula.CantVisualizaciones} Vistas";

            GridInfo.Visibility = Visibility.Collapsed;
            GridPlayer.Visibility = Visibility.Visible;

            using (var media = new Media(_libVlc, new Uri(rutaVideo)))
            {
                _mediaPlayer.Play(media);
            }
            long tiempoGuardado = DatabaseServicie.ObtenerTiempoVisto(_usuarioAnterior.Id, _pelicula.Id);
            if (tiempoGuardado > 0)
            {
              
                _mediaPlayer.Time = tiempoGuardado / 10000;
            }
            btnPlayPause.Content = "⏸";
        }

        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
                btnPlayPause.Content = "▶";
            }
            else
            {
                _mediaPlayer.Play();
                btnPlayPause.Content = "⏸";
            }
        }

        private void BtnRetroceder_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Time = Math.Max(0, _mediaPlayer.Time - 10000);
        }

        private void BtnAvanzar_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Time = Math.Min(_mediaPlayer.Length, _mediaPlayer.Time + 10000);
        }

        private void BtnCerrarVideo_Click(object sender, RoutedEventArgs e)
        {
            CerrarVideo();
        }

        private void CerrarVideo()
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause(); 

                
                DatabaseServicie.GuardarProgreso(
                    _usuarioAnterior.Id,
                    _pelicula.Id,
                    _mediaPlayer.Time * 10000, 
                    _mediaPlayer.Length * 10000
                );

                _mediaPlayer.Stop();
            }
            GridPlayer.Visibility = Visibility.Collapsed;
            GridInfo.Visibility = Visibility.Visible;
        }

       
        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!_isDraggingSlider)
                {
                    sliderTime.Value = e.Position * 100;
                }
            });
        }

        private void SliderTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDraggingSlider = true;
        }

        private void SliderTime_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mediaPlayer.Position = (float)(sliderTime.Value / 100);
            _isDraggingSlider = false;
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"¿Eliminar '{_pelicula.Titulo}'?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DatabaseServicie.BorrarPelicula(_usuarioAnterior.Rol, _pelicula.Id);
                BtnVolver_Click(null, null);
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Dispose();
            var main = Window.GetWindow(this) as MainWindow;
            if (main != null)
            {
                if (_origen == OrigenNavegacion.Catalogo)
                {
                   
                    main.Navegar(new UC_Catalogo(_usuarioAnterior));
                }
                else
                {
                    
                    main.Navegar(new UC_Inicio(_usuarioAnterior));
                }
            }
        }

        public void Dispose()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            _libVlc?.Dispose();
        }
    }
}