using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Globalization;
using System.Threading;
using System.Windows.Resources;
using System.Windows.Input;
using ProjekatA.viewModels;
using ProjekatA.views;
using ProjekatA.viewModels;

namespace ProjekatA
{
    public partial class MainWindow : Window
    {
        private int themeIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
            LoginViewModel.ApplyThemeAndLanguage(Properties.Settings.Default.Style, Properties.Settings.Default.Language);
            this.DataContext = new LoginViewModel(this);
        }

        private void LanguageChanged(object sender, RoutedEventArgs e)
        {
            if (EnglishRadio.IsChecked == true)
            {
                LoadLanguage("en");
            }
            else if (SerbianRadio.IsChecked == true)
            {
                LoadLanguage("sr");
            }
        }

        private void LoadLanguage(string langCode)
        {
            LoginViewModel.CurrentLanguage = langCode;
            string langFile = $"resources/{langCode}.xaml";
            Uri langUri = new Uri(langFile, UriKind.Relative);

            try
            {
                // Preuzimanje trenutnih stilova pre nego što promenimo jezik
                var currentDictionaries = new List<ResourceDictionary>(Application.Current.Resources.MergedDictionaries);

                // Učitavanje jezickog resursa
                ResourceDictionary langDict = (ResourceDictionary)Application.LoadComponent(langUri);

                // Brisanje svih prethodnih resursa
                Application.Current.Resources.MergedDictionaries.Clear();

                // Ponovno dodavanje svih prethodnih stilova
                foreach (var dict in currentDictionaries)
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }

                // Dodajemo novi jezički resurs
                Application.Current.Resources.MergedDictionaries.Add(langDict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading language file: {ex.Message}");
            }
        }



        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove(); // Omogućava pomjeranje prozora
            }
        }
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            string themePath = themeIndex switch
            {
                0 => "themes/LightTheme.xaml",
                1 => "themes/DarkTheme.xaml",
                2 => "themes/HighContrastTheme.xaml",
                _ => "themes/LightTheme.xaml"
            };
            LoginViewModel.CurrentStyle = themePath;

            // Ukloni prethodnu temu, ali zadrži osnovne stilove
            var themeDictionary = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.ToString().Contains("Theme"));

            if (themeDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(themeDictionary);
            }

            // Dodaj novu temu
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri(themePath, UriKind.Relative)
            });

            // Postavi odgovarajući tekst na dugme
            

            // Rotiraj brojač tema
            themeIndex = (themeIndex + 1) % 3;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Ažuriraj Password u ViewModel-u kada se lozinka promeni
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }


        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                PasswordBox.Focus(); // Fokusiraj PasswordBox kada se pritisne strelica dole
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                UsernameTextBox.Focus();
            }
        }
    }
}
