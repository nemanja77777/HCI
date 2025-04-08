using Microsoft.AspNetCore.Identity;
using ProjekatA.models;
using ProjekatA.viewModels;  
using System.Windows;
using System.Windows.Input;
using ProjekatA.messages;

namespace ProjekatA.creates
{
    public partial class NalogCreate : Window
    {
        private NalogViewModel _viewModel; 

        public NalogCreate(NalogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel; 
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string korisnickoIme = KorisnickoImeTextBox.Text;
            string sifra = SifraPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(sifra))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            if (_viewModel.ListaNaloga.Any(nalog => nalog.KorisnickoIme.Equals(korisnickoIme, StringComparison.OrdinalIgnoreCase)))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, sifra);

            NalogModel nalog = new NalogModel
            {
                KorisnickoIme = korisnickoIme,
                Sifra = hashedPassword
            };

            _viewModel.ListaNaloga.Add((NalogModel)(new NalogModel().Read(nalog.Create())));
            new MyCreateSuccess().ShowDialog();
            this.Close();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //Ne zelim da nesto radi
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TopPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
