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
    public partial class KlijentUpdate : Window
    {
        private KlijentModel SelektovaniKlijent;
        private KlijentViewModel _viewModel;

        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }
        public KlijentUpdate(KlijentModel klijent, KlijentViewModel viewModel)
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
            SelektovaniKlijent = klijent;
            _viewModel = viewModel;

            KorisnickaImena.Add(SelektovaniKlijent.KorisnickoIme);
            OpisKlijentaTextBox.Text = SelektovaniKlijent.OpisKlijenta;
            IzabranoKorisnickoIme = SelektovaniKlijent.KorisnickoIme;

        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OpisKlijentaTextBox.Text) ||
                NalogKorisnickoImeComboBox.SelectedItem == null)
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }

            
            List<int> vs = new VoziloModel().ReadAll().Cast<VoziloModel>().Select(V => V.KlijentNalogId).ToList();
            if (vs.Contains(SelektovaniKlijent.NalogId))
            {
                new MyIntegrityWarning().ShowDialog();
                return;
            }

            List<int> tps = new TehnickiPregledModel().ReadAll().Cast<TehnickiPregledModel>()
                .Select(tp => tp.KlijentNalogId).ToList();
            if (tps.Contains(SelektovaniKlijent.NalogId))
            {
                new MyIntegrityWarning().ShowDialog();
                return;
            }
            SelektovaniKlijent.Delete(SelektovaniKlijent.NalogId);

            SelektovaniKlijent.NalogId = new NalogModel().VratiId(NalogKorisnickoImeComboBox.SelectedItem as string);
            SelektovaniKlijent.OpisKlijenta = OpisKlijentaTextBox.Text;
            SelektovaniKlijent.KorisnickoIme = NalogKorisnickoImeComboBox.SelectedItem as string;


            SelektovaniKlijent.Create();
            new MyUpdateSuccess().ShowDialog();


            KlijentViewModel viewModel = _viewModel;
            viewModel.UpdateListaKlijenata(SelektovaniKlijent);


            var collectionView = CollectionViewSource.GetDefaultView(viewModel.KlijentView.dgKlijenti.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.KlijentView.dgKlijenti.Items.Refresh();
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
