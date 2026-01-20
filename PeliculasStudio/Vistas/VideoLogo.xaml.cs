using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para VideoLogo.xaml
    /// </summary>
    public partial class VideoLogo : UserControl
    {
        public VideoLogo()
        {
            InitializeComponent();
        }
        private void introPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            string rutaVideo = Path.Combine(rutaBase, "Assets", "Videos", "Animación_de_Imagen_para_Fondo_de_Login.mp4");

            if (File.Exists(rutaVideo))
            {
                introPlayer.Source = new Uri(rutaVideo);
                introPlayer.Play(); 
            }
            else
            {
                
                MessageBox.Show($"No se encuentra el video en: {rutaVideo}");
                IrAlLogin();
            }
        }

        private void introPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            IrAlLogin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                introPlayer.Stop(); 
            }
            catch { } 

            IrAlLogin();
        }

        private void IrAlLogin()
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Login());
        }
    }
}
