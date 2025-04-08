using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;

namespace ProjekatA.creates
{
   
    public partial class IzvjestajCreate : Window
    {
        private byte[] _uploadedFileBytes;
        private string fileName;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public List<TehnickiPregledModel> TehnickiPregledi { get; set; }
        public TehnickiPregledModel IzabraniTehnickiPregled { get; set; }

        private IzvjestajViewModel _viewModel;
        public IzvjestajCreate(IzvjestajViewModel viewModel)
        {
            InitializeComponent();
            if (LoginViewModel.IsAdministrator)
            {
                KorisnickaImena = new ZaposleniModel().ReadAll().Cast<ZaposleniModel>()
                    .Select(z => ((NalogModel)(new NalogModel().Read(z.NalogId))).KorisnickoIme).ToList();
            }
            else
            {
                KorisnickaImena = new List<string>();
                KorisnickaImena.Add(LoginViewModel.CurrentUsername);
                IzabranoKorisnickoIme = LoginViewModel.CurrentUsername;
            }

            TehnickiPregledi = new TehnickiPregledModel().ReadAll().Cast<TehnickiPregledModel>().ToList();
            
            var collectionView = CollectionViewSource.GetDefaultView(viewModel.IzvjestajView.dgIzvjestaji.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.IzvjestajView.dgIzvjestaji.Items.Refresh();
            }


            DataContext = this;
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string KorisnickoIme = ZaposleniComboBox.SelectedItem as string;
            TehnickiPregledModel tehnickiPregledModel = TPComboBox.SelectedItem as TehnickiPregledModel;

            if (string.IsNullOrWhiteSpace(KorisnickoIme) || 
                tehnickiPregledModel == null ||
                _uploadedFileBytes == null 
                || string.IsNullOrWhiteSpace(fileName))
            {
                
                new MyCreateWarning().ShowDialog();
                return;
            }

            if (PrimaryKeyConstraint(tehnickiPregledModel.Id,new NalogModel().VratiId(KorisnickoIme)))
            {
                new MyPrimaryKeyWarning().ShowDialog();
                return;
            }

            ZaposleniTehnickiPregledModel zaposleniTehnickiPregledModel = new ZaposleniTehnickiPregledModel()
            {
                TehnickiPregledId = tehnickiPregledModel.Id,
                TehnickiPregled = (TehnickiPregledModel)new TehnickiPregledModel().Read(tehnickiPregledModel.Id),
                ZaposleniNalogId = new NalogModel().VratiId(KorisnickoIme),
                IzvjestajSadrzaj = _uploadedFileBytes,
                IzvjestajNaslov = fileName

            };
           
            zaposleniTehnickiPregledModel.KorisnickoIme = KorisnickoIme;
            zaposleniTehnickiPregledModel.Adresa = (((LokacijaModel)(new LokacijaModel().Read(((TehnickiPregledModel)(new TehnickiPregledModel().Read(tehnickiPregledModel.Id))).LokacijaId)))).Adresa;
            zaposleniTehnickiPregledModel.Grad = (((LokacijaModel)(new LokacijaModel().Read(((TehnickiPregledModel)(new TehnickiPregledModel().Read(tehnickiPregledModel.Id))).LokacijaId)))).Grad;
            zaposleniTehnickiPregledModel.Lokacija =
                (LokacijaModel)(new LokacijaModel().Read(tehnickiPregledModel.LokacijaId));
           

            zaposleniTehnickiPregledModel.Create();
           
            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaIzvjestaja.Add(zaposleniTehnickiPregledModel);
            
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

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Dokumenta (*.doc;*.docx;*.pdf)|*.doc;*.docx;*.pdf",
                Title = "Izaberite fajl"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                SelectedFileText.Text = $"Odabrani fajl: {Path.GetFileName(filePath)}";

                fileName = Path.GetFileName(filePath);
                _uploadedFileBytes = File.ReadAllBytes(filePath);
                new MyFileCreateSuccess().ShowDialog();
            }
            else
            {
                new MyFileCreateWarning().ShowDialog();
            }
        }
        public bool PrimaryKeyConstraint(int tehnickiPregledId, int zaposleniNalogId)
        {
            var sviZapisi = new ZaposleniTehnickiPregledModel().ReadAll();

            return sviZapisi
                .Cast<ZaposleniTehnickiPregledModel>()
                .Any(z => z.TehnickiPregledId == tehnickiPregledId && z.ZaposleniNalogId == zaposleniNalogId);
        }


    }
}
