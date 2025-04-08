using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;

namespace ProjekatA.creates
{
    public partial class KlijentCreate : Window
    {
        private KlijentViewModel _viewModel;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public KlijentCreate(KlijentViewModel viewModel)
        {
            InitializeComponent();
            var sviNalozi = new NalogModel().ReadAll()
                .Cast<NalogModel>()
                .ToList();

            var zauzetiNalogIdjevi = new HashSet<int>(
                new KlijentModel().ReadAll().Cast<KlijentModel>().Select(k => k.NalogId)
                    .Union(new ZaposleniModel().ReadAll().Cast<ZaposleniModel>().Select(z => z.NalogId))
                    .Union(new AdministratorModel().ReadAll().Cast<AdministratorModel>().Select(a => a.NalogId))
            );

             KorisnickaImena = sviNalozi
                .Where(n => !zauzetiNalogIdjevi.Contains(n.Id))
                .Select(n => n.KorisnickoIme)
                .ToList();

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.KlijentView.dgKlijenti.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.KlijentView.dgKlijenti.Items.Refresh();
            }
            DataContext = this;
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string OpisKlijenta = OpisKlijentaTextBox.Text;
            string nalogKorisnickoIme = NalogIdComboBox.SelectedItem as string;



            if (string.IsNullOrWhiteSpace(OpisKlijenta) ||
                string.IsNullOrWhiteSpace(nalogKorisnickoIme))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            KlijentModel klijent = new KlijentModel()
            {
                OpisKlijenta = OpisKlijenta,
                NalogId = new NalogModel().VratiId(nalogKorisnickoIme),
                

            };
            klijent.KorisnickoIme = nalogKorisnickoIme;
            klijent.Create();
            

            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaKlijenata.Add(klijent);
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