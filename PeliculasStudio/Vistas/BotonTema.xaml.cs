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
        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = !App.IsDarkMode;
            GestordeTemas.AplicarTema(App.IsDarkMode);
        }
    }
}
