using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.models;
using ProjekatA.views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using System.Globalization;
using ProjekatA.messages;



namespace ProjekatA.viewModels
{

    public class LoginViewModel : INotifyPropertyChanged
    {
        public static string CurrentUsername;
        public static bool IsAdministrator = false;
        public static string CurrentStyle { get; set; }
        public static string CurrentLanguage { get; set; }
        private string _username;
        private string _password;
        private Window _mainWindow;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            LoginCommand = new RelayCommand(Login);
        }



        private void Login()
        {
            bool provjera = false;
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
            {
                new MyLoggingWarning().ShowDialog();
                return;
            }

            var passwordHasher = new PasswordHasher<object>();
            var adminNalozi = new AdministratorModel().ReadAll();
            var zaposleniNalozi = new ZaposleniModel().ReadAll();

            
            var validUser = adminNalozi.FirstOrDefault(nalog =>
            (((NalogModel)(new NalogModel().Read((((AdministratorModel)nalog).NalogId)))).KorisnickoIme == Username));

            if (validUser != null)
            {
               

                var result = passwordHasher.VerifyHashedPassword(null, ((NalogModel)(new NalogModel().Read((((AdministratorModel)(validUser)).NalogId)))).Sifra, Password);

                if (result == PasswordVerificationResult.Success)
                {

                    if (!string.IsNullOrEmpty(CurrentStyle) || !string.IsNullOrEmpty(CurrentLanguage))
                    {

                        SaveUserPreferences((((NalogModel)new NalogModel().Read((((AdministratorModel)validUser).NalogId)))).KorisnickoIme, CurrentStyle, CurrentLanguage);
                        
                    }

                    CurrentUsername = (((NalogModel)new NalogModel().Read((((AdministratorModel)validUser).NalogId))))
                        .KorisnickoIme;
                    IsAdministrator = true;
                     new AdministratorMeniView().Show();
                    provjera = true;
                    _mainWindow.Close();
                }
                else
                {
                    new MyLoggingWarning().ShowDialog();
                }

            }
            

            if (provjera == false)
            {
                var validUser2 = zaposleniNalozi.FirstOrDefault(nalog =>
                (((NalogModel)(new NalogModel().Read((((ZaposleniModel)nalog).NalogId)))).KorisnickoIme == Username));

                if (validUser2 != null)
                {
                    var result2 = passwordHasher.VerifyHashedPassword(null, ((NalogModel)(new NalogModel().Read((((ZaposleniModel)(validUser2)).NalogId)))).Sifra, Password);

                    if (result2 == PasswordVerificationResult.Success)
                    {

                        if (!string.IsNullOrEmpty(CurrentStyle) || !string.IsNullOrEmpty(CurrentLanguage))
                        {
                            SaveUserPreferences(((ZaposleniModel)validUser2).KorisnickoIme, CurrentStyle, CurrentLanguage);
                        }
                        var administratorMeni = new AdministratorMeniView();



                        CurrentUsername = ((NalogModel)(new NalogModel().Read(((ZaposleniModel)validUser2).NalogId))).KorisnickoIme;
                        new ZaposleniMeniView().Show();
                        IsAdministrator = false;
                        _mainWindow.Close();

                    }
                    else
                    {
                        new MyLoggingWarning().ShowDialog();
                    }

                }
                

            }
            

        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static void SaveUserPreferences(string username, string style, string language)
        {
            
            if (style!=null && style.StartsWith("themes/") && style.EndsWith("Theme.xaml"))
            {
                style = style.Substring(7, style.Length - 17);
            }

            Properties.Settings.Default.Username = username;
            if (!string.IsNullOrEmpty(style) && style != Properties.Settings.Default.Style)
            {
                Properties.Settings.Default.Style = style;
                new MyStyleChanged().ShowDialog();
                Thread.Sleep(500);
            }

            if (!string.IsNullOrEmpty(language) && language != Properties.Settings.Default.Language)
            {
                Properties.Settings.Default.Language = language;
                new MyLanguageChanged().ShowDialog();
            }

            Properties.Settings.Default.Save();
        }

        public static class UserPreferencesHelper
        {
        }
        public static void ApplyThemeAndLanguage(string themeFileName, string languageCode)
        {
            // Primena teme
            try
            {
                // Pretpostavljamo da se teme nalaze u folderu "Themes"
                string themePath = $"/Themes/{themeFileName}Theme.xaml";
                var newTheme = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

                // Ukloni sve prethodno učitane teme iz foldera Themes
                var dictionaries = Application.Current.Resources.MergedDictionaries;
                for (int i = dictionaries.Count - 1; i >= 0; i--)
                {
                    var dict = dictionaries[i];
                    if (dict.Source != null && dict.Source.OriginalString.StartsWith("/Themes/", StringComparison.OrdinalIgnoreCase))
                    {
                        dictionaries.RemoveAt(i);
                    }
                }
                // Dodaj novu temu
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Greška pri postavljanju teme: " + ex.Message);
            }

            // Primena jezika
            try
            {
                // Odredi putanju do resource fajla na osnovu jezičkog koda
                string languageFile = string.Empty;
                if (languageCode.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    languageFile = "/resources/en.xaml";
                }
                else if (languageCode.StartsWith("sr", StringComparison.OrdinalIgnoreCase))
                {
                    languageFile = "/resources/sr.xaml";
                }
                else
                {
                    // Ako je nepoznat jezik, možeš postaviti podrazumevani jezik, npr. engleski
                    languageFile = "/resources/en.xaml";
                }

                var newLanguageDictionary = new ResourceDictionary { Source = new Uri(languageFile, UriKind.Relative) };

                // Ukloni prethodne resource dictionary-je za jezik iz foldera resources
                var mergedDicts = Application.Current.Resources.MergedDictionaries;
                for (int i = mergedDicts.Count - 1; i >= 0; i--)
                {
                    var dict = mergedDicts[i];
                    if (dict.Source != null && dict.Source.OriginalString.StartsWith("/resources/", StringComparison.OrdinalIgnoreCase))
                    {
                        mergedDicts.RemoveAt(i);
                    }
                }
                // Dodaj novi jezički dictionary
                Application.Current.Resources.MergedDictionaries.Add(newLanguageDictionary);

                // Postavi kulturu za trenutno izvođenje aplikacije
                var culture = new CultureInfo(languageCode);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Greška pri postavljanju jezika: " + ex.Message);
            }
        }
    }
}
