using System;
using System.Windows;
using System.Diagnostics;

namespace PeliculasStudio.Utilidades
{
    public static class GestordeTemas
    {
        public static void AplicarTema(bool modoOscuro)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            ResourceDictionary nuevoTema = new ResourceDictionary();


            string ruta = modoOscuro ? "Temas/Tema.Oscuro.xaml" : "Temas/Tema.Claro.xaml";

            try
            {
                nuevoTema.Source = new Uri(ruta, UriKind.Relative);

                Application.Current.Resources.MergedDictionaries.Add(nuevoTema);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GestorDeTemas] Error crítico al cargar {ruta}: {ex.Message}");
            }
        } 

    }
}
