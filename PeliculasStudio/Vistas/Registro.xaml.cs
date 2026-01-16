using PeliculasStudio.BaseDatos;
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

    public partial class Registro : Window
    {
        public Registro()
        {
            InitializeComponent();

           
            if (App.IsDarkMode)
            {
                btnTema.IsChecked = true;
                AplicarTemaOscuro();
            }
            else
            {
                btnTema.IsChecked = false;
                AplicarTemaClaro();
            }
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            // 1. RECOGIDA DE DATOS
            string nombre = txtUsuario.Text.Trim();
            string correo = txtGmail.Text.Trim().ToLower();
            string pass = txtPassword.Password;
            string passRepeat = txtRepeatPassword.Password;

            // 2. COLORES SEGÚN EL TEMA
            SolidColorBrush colorError = Brushes.Red;
            SolidColorBrush colorNormal = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));

            // 3. VALIDACIÓN DE CAMPOS VACÍOS (Pintar bordes)
            bool hayCamposVacios = false;

            // Usuario
            if (string.IsNullOrWhiteSpace(nombre))
            {
                txtUsuario.BorderBrush = colorError;
                txtUsuario.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtUsuario.BorderBrush = colorNormal;
                txtUsuario.BorderThickness = new Thickness(1);
            }

            // Gmail
            if (string.IsNullOrWhiteSpace(correo))
            {
                txtGmail.BorderBrush = colorError;
                txtGmail.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtGmail.BorderBrush = colorNormal;
                txtGmail.BorderThickness = new Thickness(1);
            }

            // Password
            if (string.IsNullOrWhiteSpace(pass))
            {
                txtPassword.BorderBrush = colorError;
                txtPassword.BorderThickness = new Thickness(2);
                hayCamposVacios = true;
            }
            else
            {
                txtPassword.BorderBrush = colorNormal;
                txtPassword.BorderThickness = new Thickness(1);
            }

            // Si falta algo, paramos aquí
            if (hayCamposVacios)
            {
                MessageBox.Show("Por favor, rellena los campos en rojo.");
                return;
            }

            // 4. VALIDACIÓN DE SEGURIDAD (La función que hicimos antes)
            if (!ValidarSeguridadPassword(pass))
            {
                MessageBox.Show("La contraseña no es segura.");
                return;
            }

            // 5. VALIDACIÓN DE COINCIDENCIA
            if (pass != passRepeat)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }

            // 6. LLAMADA A LA BBDD (Solo llegamos aquí si TODO está bien)
            string resultado = DatabaseServicie.CrearUsuario(nombre, correo, pass);

            if (resultado.StartsWith("ERROR"))
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                btnVolver_Click(null, null);
            }
        }

        private bool ValidarSeguridadPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            bool tieneLongitud = password.Length >= 8;
            bool tieneMayuscula = password.Any(char.IsUpper);
            bool tieneNumero = password.Any(char.IsDigit);
            bool tieneEspecial = password.Any(ch => !char.IsLetterOrDigit(ch));

           
            return tieneLongitud && tieneMayuscula && tieneNumero && tieneEspecial;
        }


        //-------------------------------------------------------------------------------

        private void LimpiarBorde_TextChanged(object sender, TextChangedEventArgs e)
        {
            var control = sender as Control;
            if (control != null)
            {
                // Al escribir, devolvemos el borde al color normal del tema
                control.BorderBrush = App.IsDarkMode ?
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));
                control.BorderThickness = new Thickness(1);
            }
        }
        private void txtRepeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = txtPassword.Password;
            string repeatPass = txtRepeatPassword.Password;

            SolidColorBrush verde = Brushes.Green;
            SolidColorBrush rojo = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4C4C")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F"));

            // Caso: Está vacío
            if (string.IsNullOrEmpty(repeatPass))
            {
                txtRepeatPassword.BorderBrush = App.IsDarkMode ?
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ABAdB3"));

                txtRepeatPassword.BorderThickness = new Thickness(1);
                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden"; // Texto por defecto
                iconCoincide.Foreground = rojo;
                lblCoincide.Foreground = rojo;
                return;
            }

            // Comprobación de coincidencia
            if (pass == repeatPass)
            {
                // --- COINCIDEN ---
                txtRepeatPassword.BorderBrush = verde;
                txtRepeatPassword.BorderThickness = new Thickness(2.5);

                iconCoincide.Text = "✔";
                lblCoincide.Text = "Las contraseñas coinciden"; // <--- CAMBIO AQUÍ
                iconCoincide.Foreground = verde;
                lblCoincide.Foreground = verde;
            }
            else
            {
                // --- NO COINCIDEN ---
                txtRepeatPassword.BorderBrush = Brushes.Red;
                txtRepeatPassword.BorderThickness = new Thickness(2.5);

                iconCoincide.Text = "✖";
                lblCoincide.Text = "Las contraseñas no coinciden"; // <--- CAMBIO AQUÍ
                iconCoincide.Foreground = rojo;
                lblCoincide.Foreground = rojo;
            }
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = txtPassword.Password;

            // Definimos los colores dinámicos según el tema actual
            // Si es modo oscuro usamos un rojo más brillante (#FF4C4C), si es claro el que ya tenías (#D32F2F)
            SolidColorBrush colorRojo = App.IsDarkMode ?
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4C4C")) :
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F"));

            SolidColorBrush colorVerde = Brushes.Green;

            // 1. Validar Longitud (Mínimo 8)
            ActualizarEstadoRequisito(pass.Length >= 8, iconLongitud, lblLongitud, colorRojo, colorVerde);

            // 2. Validar Mayúscula
            ActualizarEstadoRequisito(pass.Any(char.IsUpper), iconMayuscula, lblMayuscula, colorRojo, colorVerde);

            // 3. Validar Número
            ActualizarEstadoRequisito(pass.Any(char.IsDigit), iconNumero, lblNumero, colorRojo, colorVerde);

            // 4. Validar Carácter Especial
            bool tieneEspecial = pass.Any(ch => !char.IsLetterOrDigit(ch));
            ActualizarEstadoRequisito(tieneEspecial, iconEspecial, lblEspecial, colorRojo, colorVerde);
            txtRepeatPassword_PasswordChanged(null, null);
        }

        // Método auxiliar para evitar repetir código
        private void ActualizarEstadoRequisito(bool cumple, TextBlock icono, TextBlock texto, Brush rojo, Brush verde)
        {
            if (cumple)
            {
                icono.Text = "✔";
                icono.Foreground = verde;
                texto.Foreground = verde;
            }
            else
            {
                icono.Text = "✖";
                icono.Foreground = rojo;
                texto.Foreground = rojo;
            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
        
            MainWindow principal = new MainWindow();
            principal.Show();

            this.Close();
        }

        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            // 3. ACTUALIZAMOS LA VARIABLE GLOBAL
            App.IsDarkMode = btnTema.IsChecked ?? false;

            if (App.IsDarkMode) AplicarTemaOscuro();
            else AplicarTemaClaro();
        }

        private void AplicarTemaOscuro()
        {
            GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1E1E"));
            BorderFormulario.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D2D"));
            BorderRequisitos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));

            // Ponemos todos los textos en Blanco Puro
            txtTitulo.Foreground = Brushes.White;
            lblUsuario.Foreground = Brushes.White;
            lblGmail.Foreground = Brushes.White;
            lblPassword.Foreground = Brushes.White;
            lblSeguridadTitulo.Foreground = Brushes.White;
            lblInstruccion.Foreground = Brushes.LightGray;
            lblNota.Foreground = Brushes.LightGray;
          
        }

        private void AplicarTemaClaro()
        {
            GridPrincipal.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0"));
            BorderFormulario.Background = Brushes.White;
            BorderRequisitos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9F9F9"));

            // Volvemos a los grises oscuros
            txtTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            lblUsuario.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblGmail.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblPassword.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            lblSeguridadTitulo.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            lblInstruccion.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#777777"));
            lblNota.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
          
        }
    }
}

