using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ProjekatA.creates
{
    public partial class ZaposleniCreate : Window
    {
        private ZaposleniViewModel _viewModel;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public ZaposleniCreate(ZaposleniViewModel viewModel)
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

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.zaposleniView.dgZaposleni.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.zaposleniView.dgZaposleni.Items.Refresh();
            }
            DataContext = this;
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string ZvanjeZaposlenog = ZvanjeZaposlenogTextBox.Text;
            string nalogKorisnickoIme = NalogIdComboBox.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(ZvanjeZaposlenog) ||
                string.IsNullOrWhiteSpace(nalogKorisnickoIme))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }
            ZaposleniModel zaposleni  = new ZaposleniModel()
            {
                Zvanje = ZvanjeZaposlenogTextBox.Text,
                NalogId = new NalogModel().VratiId(nalogKorisnickoIme),


            };
            zaposleni.KorisnickoIme = nalogKorisnickoIme;
            zaposleni.Create();


            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaZaposlenih.Add(zaposleni);
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
