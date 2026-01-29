using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using PeliculasStudio.Modelos;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PeliculasStudio.Utilidades
{
    public static class ServicioInformes
    {
        public static void GenerarInformeMensualidad(Usuario usuario)
        {
            try
            {
                // Crear el documento y la página 
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Informe de Gasto - Peliculas Studio";
                PdfPage page = document.AddPage();

                // Objeto de gráficos para dibujar 
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Definir Fuentes y Pinceles 
                XFont fuenteTitulo = new XFont("Verdana", 20, XFontStyleEx.Bold);
                XFont fuenteCabecera = new XFont("Verdana", 14, XFontStyleEx.Bold);
                XFont fuenteTexto = new XFont("Verdana", 12, XFontStyleEx.Regular);
                XFont fuenteNegrita = new XFont("Verdana", 12, XFontStyleEx.Bold);

                XBrush brushNegro = XBrushes.Black;
                XBrush brushAzul = XBrushes.DarkBlue;
                XPen penLinea = new XPen(XColors.Gray, 1);

                // Cálculos de la Suscripción
                // 5€/mes. Mes empezado = Mes pagado entero.
                DateTime fechaHoy = DateTime.Now;
                DateTime fechaReg = usuario.FechaRegistro;

                // Cálculo de meses transcurridos
                int meses = ((fechaHoy.Year - fechaReg.Year) * 12) + fechaHoy.Month - fechaReg.Month;

                // Sumamos 1 para cobrar el mes actual en curso
                int mesesFacturables = meses + 1;

                // Precio fijo
                double precioMensual = 5.00;
                double totalPagar = mesesFacturables * precioMensual;


                //Dibujar el Informe (Coordenadas X, Y)
                double x = 40; // Margen izquierdo
                double y = 50; // Posición vertical inicial

                // Cabecera
                gfx.DrawString("PELICULAS STUDIO", fuenteTitulo, brushAzul, x, y);
                y += 30;
                gfx.DrawString("Informe de Suscripción", fuenteCabecera, brushNegro, x, y);
                y += 15;
                gfx.DrawLine(penLinea, x, y, page.Width - x, y);
                y += 40;

                // Datos del Cliente
                gfx.DrawString($"Cliente: {usuario.Nombreusuario}", fuenteTexto, brushNegro, x, y);
                y += 20;
                gfx.DrawString($"Email: {usuario.Gmail}", fuenteTexto, brushNegro, x, y);
                y += 20;
                gfx.DrawString($"Fecha Registro: {usuario.FechaRegistro:dd/MM/yyyy}", fuenteTexto, brushNegro, x, y);
                y += 20;
                gfx.DrawString($"Fecha Informe: {fechaHoy:dd/MM/yyyy}", fuenteTexto, brushNegro, x, y);
                y += 50;

                // Detalle Económico 
                gfx.DrawString("DETALLE DE FACTURACIÓN", fuenteCabecera, brushAzul, x, y);
                y += 30;

                // Fila 1: Precio Mensual
                gfx.DrawString("Precio Mensual Estándar:", fuenteTexto, brushNegro, x, y);
                // Alineamos el precio a la derecha (posición 350 aprox)
                gfx.DrawString($"{precioMensual:F2} €", fuenteTexto, brushNegro, 350, y);
                y += 25;

                // Fila 2: Meses
                gfx.DrawString("Meses Activos (incluye actual):", fuenteTexto, brushNegro, x, y);
                gfx.DrawString($"{mesesFacturables}", fuenteTexto, brushNegro, 350, y);
                y += 25;

                // Línea de total
                gfx.DrawLine(penLinea, x, y, 450, y);
                y += 20;

                // Total
                gfx.DrawString("TOTAL ACUMULADO:", fuenteNegrita, brushNegro, x, y);
                // El total en negrita y azul
                gfx.DrawString($"{totalPagar:F2} €", new XFont("Verdana", 16, XFontStyleEx.Bold), brushAzul, 350, y);


                //Guardar el PDF
                string nombreFichero = $"Informe_Gasto_{usuario.Nombreusuario}_{DateTime.Now.Ticks}.pdf";
                string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreFichero);

                document.Save(rutaCompleta);

                // Abrir automáticamente (usando ShellExecute)
                Process.Start(new ProcessStartInfo(rutaCompleta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el informe: " + ex.Message);
            }
        }
    }
}
