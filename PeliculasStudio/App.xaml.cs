using LibVLCSharp.Shared;
using PeliculasStudio.BaseDatos;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Esto nicializa el motor de video (VLC)
            Core.Initialize();

            // y esto inicializa la base de datos y crea los datos iniciales
            DatabaseServicie.Inicializar();
        }
    }

}
