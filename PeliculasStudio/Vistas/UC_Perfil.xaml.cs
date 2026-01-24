using PeliculasStudio.BaseDatos;
using PeliculasStudio.Modelos;
using PeliculasStudio.Utilidades;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PeliculasStudio.Vistas
{
    public partial class UC_Perfil : UserControl
    {
        private Usuario _usuarioActual;

        public UC_Perfil(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;

            // Sincronizar botón de tema con el estado actual de la App
            btnTema.IsChecked = App.IsDarkMode;

            CargarDatos();
        }

        private void CargarDatos()
        {
            txtUsuario.Text = _usuarioActual.Nombreusuario;
            txtEmail.Text = _usuarioActual.Gmail; 
        }

        private void btnTema_Click(object sender, RoutedEventArgs e)
        {
            App.IsDarkMode = btnTema.IsChecked ?? false;
            GestordeTemas.AplicarTema(App.IsDarkMode);
        }

        
        // CAMBIO DE NOMBRE
        
        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblMensajeNombre.Text = "";
        }

        private void BtnGuardarNombre_Click(object sender, RoutedEventArgs e)
        {
            string nuevoNombre = txtUsuario.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MostrarMensaje(lblMensajeNombre, "El nombre no puede estar vacío.", false);
                return;
            }

            if (nuevoNombre.Length < 4 || !Regex.IsMatch(nuevoNombre, @"^[a-zA-Z0-9_]+$"))
            {
                MostrarMensaje(lblMensajeNombre, "Mínimo 4 caracteres y sin símbolos.", false);
                return;
            }

            if (DatabaseServicie.ExisteUsuario(nuevoNombre, _usuarioActual.Id))
            {
                MostrarMensaje(lblMensajeNombre, "Este usuario ya existe.", false);
                return;
            }

            // Guardar
            _usuarioActual.Nombreusuario = nuevoNombre;
            string resultado = DatabaseServicie.ActualizarUsuario(_usuarioActual);

            MostrarMensaje(lblMensajeNombre, resultado == "ÉXITO" ? "Nombre actualizado." : "Error al guardar.", resultado == "ÉXITO");
        }


        
        // CAMBIO DE EMAIL (NUEVO)
        
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblMensajeEmail.Text = "";
        }

        private void BtnGuardarEmail_Click(object sender, RoutedEventArgs e)
        {
            string nuevoEmail = txtEmail.Text.Trim().ToLower();

            //Validar que no esté vacío
            if (string.IsNullOrWhiteSpace(nuevoEmail))
            {
                MostrarMensaje(lblMensajeEmail, "El correo es obligatorio.", false);
                return;
            }

            //Validar formato (usando Regex simple pero efectiva)
            if (!Regex.IsMatch(nuevoEmail, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                MostrarMensaje(lblMensajeEmail, "Formato de correo no válido.", false);
                return;
            }

            //Comprobar si ya existe en otro usuario
            if (DatabaseServicie.ExisteCorreo(nuevoEmail, _usuarioActual.Id))
            {
                MostrarMensaje(lblMensajeEmail, "Este correo ya está registrado.", false);
                return;
            }

            //Guardar
            _usuarioActual.Gmail = nuevoEmail;
            string resultado = DatabaseServicie.ActualizarUsuario(_usuarioActual);

            MostrarMensaje(lblMensajeEmail, resultado == "ÉXITO" ? "Email actualizado correctamente." : "Error: " + resultado, resultado == "ÉXITO");
        }


        // Genérico para mensajes de estado
        private void MostrarMensaje(TextBlock label, string msg, bool exito)
        {
            label.Text = msg;
            label.Foreground = exito ?
                (Brush)FindResource("BrushMensajeExito") :
                (Brush)FindResource("BrushMensajeError");
        }


        
        // CAMBIO DE CONTRASEÑA
        
        private void txtPassActual_PasswordChanged(object sender, RoutedEventArgs e)
        {
            bool esCorrecta = Cifrado.VerifyPassword(txtPassActual.Password, _usuarioActual.Contrasenia);
            iconPassActual.Visibility = esCorrecta ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtPassNueva_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string pass = txtPassNueva.Password;
            var brushError = (Brush)FindResource("BrushMensajeError");
            var brushExito = (Brush)FindResource("BrushMensajeExito");

            ActualizarRequisito(pass.Length >= 8, iconLongitud, lblLongitud, brushError, brushExito);
            ActualizarRequisito(pass.Any(char.IsUpper), iconMayuscula, lblMayuscula, brushError, brushExito);
            ActualizarRequisito(pass.Any(char.IsDigit), iconNumero, lblNumero, brushError, brushExito);
            ActualizarRequisito(pass.Any(ch => !char.IsLetterOrDigit(ch)), iconEspecial, lblEspecial, brushError, brushExito);

            txtPassNuevaRepetir_PasswordChanged(null, null);
        }

        private void txtPassNuevaRepetir_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var brushError = (Brush)FindResource("BrushMensajeError");
            var brushExito = (Brush)FindResource("BrushMensajeExito");

            bool coinciden = !string.IsNullOrEmpty(txtPassNueva.Password) &&
                             txtPassNueva.Password == txtPassNuevaRepetir.Password;

            ActualizarRequisito(coinciden, iconCoincide, lblCoincide, brushError, brushExito);
            lblCoincide.Text = coinciden ? "Las contraseñas coinciden" : "Las contraseñas no coinciden";
        }

        private void ActualizarRequisito(bool cumple, TextBlock icono, TextBlock texto, Brush rojo, Brush verde)
        {
            icono.Text = cumple ? "✔" : "✖";
            icono.Foreground = cumple ? verde : rojo;
            texto.Foreground = cumple ? verde : rojo;
        }

        private void BtnActualizarPass_Click(object sender, RoutedEventArgs e)
        {
            if (!Cifrado.VerifyPassword(txtPassActual.Password, _usuarioActual.Contrasenia))
            {
                MessageBox.Show("La contraseña actual es incorrecta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtPassNueva.Password != txtPassNuevaRepetir.Password)
            {
                MessageBox.Show("Las nuevas contraseñas no coinciden.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtPassNueva.Password.Length < 8)
            {
                MessageBox.Show("La nueva contraseña no cumple los requisitos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _usuarioActual.Contrasenia = Cifrado.HashPassword(txtPassNueva.Password);
            string resultado = DatabaseServicie.ActualizarUsuario(_usuarioActual);

            if (resultado == "ÉXITO")
            {
                MessageBox.Show("Contraseña actualizada con éxito.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                txtPassActual.Password = "";
                txtPassNueva.Password = "";
                txtPassNuevaRepetir.Password = "";
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        //ELIMINAR CUENTA Y NAVEGACIÓN
        
        private void BtnEliminarCuenta_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new VentanaPassword("PELIGRO: Estás a punto de borrar tu cuenta permanentemente. Introduce tu contraseña para confirmar:");

            if (ventana.ShowDialog() == true)
            {
                if (Cifrado.VerifyPassword(ventana.Password, _usuarioActual.Contrasenia))
                {
                    DatabaseServicie.BorrarUsuario(_usuarioActual.Id);
                    MessageBox.Show("Tu cuenta ha sido eliminada.", "Adiós", MessageBoxButton.OK, MessageBoxImage.Information);

                    var main = Window.GetWindow(this) as MainWindow;
                    main?.Navegar(new UC_Login());
                }
                else
                {
                    MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            var main = Window.GetWindow(this) as MainWindow;
            main?.Navegar(new UC_Inicio(_usuarioActual));
        }
    }
}
