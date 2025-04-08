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
    public partial class AdministratorUpdate : Window
    {

        private AdministratorModel SelektovaniAdministrator;
        private AdministratorViewModel _viewModel;

        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public List<LokacijaModel> Lokacije { get; set; }
        public LokacijaModel IzabranaLokacija { get; set; }
        public AdministratorUpdate(AdministratorModel Administrator, AdministratorViewModel viewModel)
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

            Lokacije = new LokacijaModel().ReadAll().Cast<LokacijaModel>().ToList();

            DataContext = this;
            SelektovaniAdministrator = Administrator;
            _viewModel = viewModel;

            KorisnickaImena.Add(SelektovaniAdministrator.KorisnickoIme);
            Lokacije.Add(SelektovaniAdministrator.Lokacija);
            IzabranoKorisnickoIme = SelektovaniAdministrator.KorisnickoIme;
            IzabranaLokacija = SelektovaniAdministrator.Lokacija;
            LokacijeComboBox.SelectedItem = SelektovaniAdministrator.Lokacija;
        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (LokacijeComboBox.SelectedItem == null ||
                ImenaComboBox.SelectedItem == null

                )
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }
            SelektovaniAdministrator.Delete(SelektovaniAdministrator.NalogId);

            SelektovaniAdministrator.NalogId = new NalogModel().VratiId(ImenaComboBox.SelectionBoxItem as string);
            SelektovaniAdministrator.LokacijaId = (LokacijeComboBox.SelectionBoxItem as LokacijaModel).Id;
            SelektovaniAdministrator.Lokacija =LokacijeComboBox.SelectedItem as LokacijaModel; 
            SelektovaniAdministrator.KorisnickoIme = ImenaComboBox.SelectedItem as string;
            SelektovaniAdministrator.Adresa = LokacijeComboBox.SelectedItem as string;

            

            SelektovaniAdministrator.Create();
            new MyUpdateSuccess().ShowDialog();

            AdministratorViewModel viewModel = _viewModel;
            viewModel.UpdateListaAdminisratora(SelektovaniAdministrator);

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.AdministratorView.dgAdministratori.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.AdministratorView.dgAdministratori.Items.Refresh();
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