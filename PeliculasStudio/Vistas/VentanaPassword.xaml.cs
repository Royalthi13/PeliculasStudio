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
using System.Windows.Shapes;

namespace PeliculasStudio.Vistas
{
    /// <summary>
    /// Lógica de interacción para VentanaPassword.xaml
    /// </summary>
    public partial class VentanaPassword : Window
    {
        public string Password { get; private set; }

        // Este es el constructor que necesitamos
        public VentanaPassword(string mensaje)
        {
            InitializeComponent();
            
            txtMensaje.Text = mensaje;
            txtPassword.Focus();
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.Password;
            this.DialogResult = true;
        }
    }
}
