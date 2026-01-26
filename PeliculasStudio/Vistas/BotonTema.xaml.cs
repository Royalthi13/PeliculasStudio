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
    /// Lógica de interacción para BotonTema.xaml
    /// </summary>
    public partial class BotonTema : UserControl
    {
        public BotonTema()
        {
            InitializeComponent();
            btnTema.IsChecked = App.IsDarkMode;
        }

        /**
         * Metodo CambiarInterfazTema:
         * Gestiona la apariencia global de la ventana de Registro mediante la conmutación de diccionarios de recursos.
         * 1. Desvincula el tema visual anterior del árbol de recursos de la aplicación.
         * 2. Carga dinámicamente el diccionario XAML correspondiente (Claro u Oscuro).
         * 3. Notifica al motor de WPF para actualizar los controles vinculados mediante 'DynamicResource'.
         * @param modoOscuro: Determina si se carga 'Tema.Oscuro.xaml' (true) o 'Tema.Claro.xaml' (false).
         **/
        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = !App.IsDarkMode;
            GestordeTemas.AplicarTema(App.IsDarkMode);
        }
    }
}
