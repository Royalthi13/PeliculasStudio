using PeliculasStudio.Modelos;
using PeliculasStudio.Vistas;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeliculasStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Usuario _usuarioActual;
        public MainWindow()
        {
            InitializeComponent();
            Navegar(new UC_Login());
        }

        public void Navegar(UserControl nuevaVista)
        {
            ContenedorPrincipal.Content = nuevaVista;
        
        }
       


    }
}