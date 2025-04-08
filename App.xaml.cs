using System.Linq;
using System.Windows;

namespace ProjekatA
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //ChangeTheme("pack://application:,,,/themes/LightTheme.xaml");
        }

        public void ChangeTheme(string themePath)
        {
            var themeDictionary = Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.ToString().Contains("Theme"));

            if (themeDictionary != null)
            {
                // Ukloni prethodnu temu ako postoji
                Current.Resources.MergedDictionaries.Remove(themeDictionary);
            }

            // Dodaj novu temu, koristeći apsolutnu putanju (UriKind.Absolute)
            //Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            //{
            //    Source = new Uri(themePath, UriKind.Absolute) // Apsolutna putanja za "pack" URI
            //});
        }

    }
}