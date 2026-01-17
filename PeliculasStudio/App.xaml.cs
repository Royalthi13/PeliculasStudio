using LibVLCSharp.Shared;
using PeliculasStudio.BaseDatos;
using PeliculasStudio.Utilidades;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PeliculasStudio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Esta variable guardará el estado del tema para toda la app
        public static bool IsDarkMode { get; set; } = false;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Esto nicializa el motor de video (VLC)
            Core.Initialize();

            // y esto inicializa la base de datos y crea los datos iniciales
            DatabaseServicie.Inicializar();


            GestordeTemas.AplicarTema(IsDarkMode);


        }
      
    }

}
