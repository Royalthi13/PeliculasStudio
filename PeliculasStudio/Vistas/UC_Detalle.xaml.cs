using LibVLCSharp.Shared;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO; 

namespace PeliculasStudio.Vistas
{
    public partial class UC_Detalle : UserControl
    {
        private LibVLC libVlc;
       
        private LibVLCSharp.Shared.MediaPlayer mediaPlayer;

        public UC_Detalle()
        {
            InitializeComponent();
            Core.Initialize();

            libVlc = new LibVLC();

           
            mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(libVlc);

            VlcPlayer.MediaPlayer = mediaPlayer;
        }

        public void Reproducir(string nombreArchivo)
        {
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;

         
            string rutaVideo = System.IO.Path.Combine(rutaBase, "Assets", "Videos", nombreArchivo);

            if (File.Exists(rutaVideo))
            {
                using (var media = new Media(libVlc, new Uri(rutaVideo)))
                {
                    mediaPlayer.Play(media);
                }
            }
            else
            {
                MessageBox.Show($"Error: No encuentro el video en {rutaVideo}");
            }
        }

        
    }
}