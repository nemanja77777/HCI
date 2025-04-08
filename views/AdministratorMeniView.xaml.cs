
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.creates;
using ProjekatA.views;
using ProjekatA.viewModels;
using System.Windows.Controls;
using System.Windows.Media;
using PdfSharp.Drawing;

namespace ProjekatA.views
{
    
    public partial class AdministratorMeniView : Window
    {
       
        private int themeIndex = 0;
        public AdministratorMeniView()
        {
            InitializeComponent();
            LoginViewModel.ApplyThemeAndLanguage(Properties.Settings.Default.Style, Properties.Settings.Default.Language);
            Opcija1MenuItem.IsSubmenuOpen = true;
        }
        
        // Kada kliknemo na "Vozilo"
        
        private void Vozilo_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new VoziloView()); // Učitaj Vozilo.xaml
        }

        // Kada kliknemo na "Nalog"
        private void NalogKlijent_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new NalogView()); // Učitaj Nalog.xaml
        }

        // Kada kliknemo na "Zaposleni"
        private void Zaposleni_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new ZaposleniView()); // Učitaj Zaposleni.xaml
        }

        // Kada kliknemo na "Administrator"
        private void Administrator_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White; ;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new AdministratorView()); // Učitaj Administrator.xaml
        }

        // Kada kliknemo na "Tehnički Pregled"
        private void TehnickiPregled_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new TehnickiPregledView()); // Učitaj TehnickiPregled.xaml
        }

        // Kada kliknemo na "Lokacija"
        private void Lokacija_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White; ;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new LokacijaView()); // Učitaj Lokacija.xaml
        }

        // Kada kliknemo na "Izvještaj"
        private void Izvjestaj_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new IzvjestajView()); // Učitaj Izvjestaj.xaml
        }

        // Kada kliknemo na "Klijent"
        private void Klijent_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White; 
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new KlijentView()); // Učitaj Klijent.xaml
        }
        
        private void Kalendar_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            MainFrame.Navigate(new KalendarView());
        }

        private void PDF_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }
            new PDFCreate().ShowDialog();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((sender as MenuItem).Parent as Menu).Items)
            {
                if (item is MenuItem menuItem)
                {
                    menuItem.Foreground = Brushes.White;
                }
            }

            // Postavi plavu boju samo na kliknutu opciju
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0078D4");
                SolidColorBrush brush = new SolidColorBrush(color);
                clickedItem.Foreground = brush;
            }

            new MainWindow().Show();
            Close();
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

        private void TopPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove(); // Omogućava pomjeranje prozora
            }
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VoziloView());
        }
    }
}
