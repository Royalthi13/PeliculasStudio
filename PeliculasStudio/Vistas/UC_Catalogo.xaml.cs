using PeliculasStudio.Modelos;
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
    /// Lógica de interacción para UC_Catalogo.xaml
    /// </summary>
    public partial class UC_Catalogo : UserControl
    {
        private Usuario _usuarioActual;
        public UC_Catalogo(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
        }
    }
}
