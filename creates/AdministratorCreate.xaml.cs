using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ProjekatA.creates
{
    public partial class AdministratorCreate : Window
    {
        private AdministratorViewModel _viewModel;

        // public AdministratorViewModel AdministratorViewModel { get; set; }
        public List<string> KorisnickaImena { get; set; }

        public string IzabranoKorisnickoIme { get; set; }

        public List<LokacijaModel> Lokacije { get; set; }
        public LokacijaModel IzabranaLokacija { get; set; }

        public AdministratorCreate(AdministratorViewModel viewModel)
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

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.AdministratorView.dgAdministratori.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.AdministratorView.dgAdministratori.Items.Refresh();
            }

            DataContext = this;
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string KorisnickoIme = ImenaComboBox.SelectedItem as string;
            LokacijaModel Lokacija = LokacijeComboBox.SelectedItem as LokacijaModel;

            if (string.IsNullOrWhiteSpace(KorisnickoIme) ||
                Lokacija == null)
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            AdministratorModel administrator = new AdministratorModel()
            {
                NalogId = new NalogModel().VratiId(KorisnickoIme),
                LokacijaId = Lokacija.Id,
                Lokacija = Lokacija
            };

            administrator.KorisnickoIme = KorisnickoIme;
            administrator.Adresa = Lokacija.Adresa;
            administrator.Create();

            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaAdministratora.Add(administrator);
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