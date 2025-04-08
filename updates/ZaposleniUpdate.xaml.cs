using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;

namespace ProjekatA.updates
{
    public partial class ZaposleniUpdate : Window
    {
        private ZaposleniModel SelektovaniZaposleni;
        private ZaposleniViewModel _viewModel;

        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public ZaposleniUpdate(ZaposleniModel zaposleni, ZaposleniViewModel viewModel)
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
           


            DataContext = this;
            SelektovaniZaposleni = zaposleni;
            _viewModel = viewModel;
            KorisnickaImena.Add(SelektovaniZaposleni.KorisnickoIme);
            ZvanjeZaposlenogTextBox.Text = SelektovaniZaposleni.Zvanje;
            IzabranoKorisnickoIme = SelektovaniZaposleni.KorisnickoIme;
            NalogKorisnickoImeComboBox.SelectedItem = SelektovaniZaposleni.KorisnickoIme;
            
        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ZvanjeZaposlenogTextBox.Text) ||
                NalogKorisnickoImeComboBox.SelectedItem == null)
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }

            List<int> zaposleniIDs = new ZaposleniTehnickiPregledModel().ReadAll().Cast<ZaposleniTehnickiPregledModel>()
                .Select(z => z.ZaposleniNalogId).ToList();
            if (zaposleniIDs.Contains(SelektovaniZaposleni.NalogId))
            {
                new MyIntegrityWarning().ShowDialog();
                return;
            }

            SelektovaniZaposleni.Delete(SelektovaniZaposleni.NalogId);

            SelektovaniZaposleni.NalogId = new NalogModel().VratiId(NalogKorisnickoImeComboBox.Text);
            SelektovaniZaposleni.Zvanje = ZvanjeZaposlenogTextBox.Text;
            SelektovaniZaposleni.KorisnickoIme = NalogKorisnickoImeComboBox.SelectedItem as string;

            SelektovaniZaposleni.Create();
            new MyUpdateSuccess().ShowDialog();

            ZaposleniViewModel viewModel = _viewModel;
            viewModel.UpdateListaZaposlenih(SelektovaniZaposleni);

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.zaposleniView.dgZaposleni.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.zaposleniView.dgZaposleni.Items.Refresh();
            }
            this.Close();
        }

        private void TopPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Ne zelim da nesto radi
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}