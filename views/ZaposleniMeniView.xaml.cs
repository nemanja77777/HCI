using ProjekatA.creates;
using ProjekatA.viewModels;
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

namespace ProjekatA.views
{
    /// <summary>
    /// Interaction logic for ZaposleniMeniView.xaml
    /// </summary>
    public partial class ZaposleniMeniView : Window
    {
        public ZaposleniMeniView()
        {
            InitializeComponent();
            LoginViewModel.ApplyThemeAndLanguage(Properties.Settings.Default.Style, Properties.Settings.Default.Language);
            Opcija1MenuItem.IsSubmenuOpen = true;
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
        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new IzvjestajView());
        }
    }
}
