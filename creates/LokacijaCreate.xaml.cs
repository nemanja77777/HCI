using ProjekatA.models;
using ProjekatA.viewModels;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ProjekatA.messages;

namespace ProjekatA.creates
{
    public partial class LokacijaCreate : Window
    {
        private LokacijaViewModel _viewModel;

        public LokacijaCreate(LokacijaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string adresa = AdresaTextBox.Text;
            string grad = GradTextBox.Text;
            string drzava = DrzavaTextBox.Text;

            if (string.IsNullOrWhiteSpace(adresa) || string.IsNullOrWhiteSpace(grad) || string.IsNullOrWhiteSpace(drzava))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }
            
            if (_viewModel.ListaLokacija.Any(l => string.Equals(l.Adresa.Replace(" ", ""), adresa.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            LokacijaModel lokacija = new LokacijaModel
            {
                Adresa = adresa,
                Grad = grad,
                Drzava = drzava
            };

            _viewModel.ListaLokacija.Add((LokacijaModel)(new LokacijaModel().Read(lokacija.Create())));
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
