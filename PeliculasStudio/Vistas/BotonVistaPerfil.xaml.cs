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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para BotonVistaPerfil.xaml
    /// </summary>
    public partial class BotonVistaPerfil : UserControl
    {
        public event RoutedEventHandler Click;

        public static readonly DependencyProperty TextoProperty =
            DependencyProperty.Register(
                "Texto",                             // Nombre de la propiedad
                typeof(string),                      // Tipo de dato (string)
                typeof(BotonVistaPerfil),                // Dueño (esta clase)
                new PropertyMetadata(string.Empty)); // Valor por defecto

        
        public string Texto
        {
            get { return (string)GetValue(TextoProperty); }
            set { SetValue(TextoProperty, value); }
        }

 
        public BotonVistaPerfil()
        {
            InitializeComponent();
        }
        private void BtnInterno_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
