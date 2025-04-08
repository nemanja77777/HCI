using System;
using System.Linq;
using System.Windows;
using ProjekatA.models;
using ProjekatA.viewModels; 
using ProjekatA.views;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using ProjekatA.messages;

namespace ProjekatA.updates
{
    public partial class IzvjestajUpdate : Window
    {
        private byte[] _uploadedFileBytes;
        private string fileName;

        private ZaposleniTehnickiPregledModel SelektovaniTPZ;
        private IzvjestajViewModel _viewModel;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public List<TehnickiPregledModel> TehnickiPregledi { get; set; }
        public TehnickiPregledModel IzabraniTehnickiPregled { get; set; }

        public IzvjestajUpdate(ZaposleniTehnickiPregledModel izvjestaj, IzvjestajViewModel viewModel)
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

            DataContext = this;
            SelektovaniTPZ = izvjestaj;
            _viewModel = viewModel;

            IzabranoKorisnickoIme = SelektovaniTPZ.KorisnickoIme;
            IzabraniTehnickiPregled = TehnickiPregledi.SingleOrDefault(tp => tp.Id == SelektovaniTPZ.TehnickiPregledId);
            SelectedFileText.Text = SelektovaniTPZ.IzvjestajNaslov;
            _uploadedFileBytes = new ZaposleniTehnickiPregledModel().ReadFileContent(SelektovaniTPZ.TehnickiPregledId,
                SelektovaniTPZ.ZaposleniNalogId);
            fileName = SelektovaniTPZ.IzvjestajNaslov;
        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (ZaposleniComboBox.SelectedItem == null ||
                TPComboBox.SelectedItem == null ||
                _uploadedFileBytes == null ||
                string.IsNullOrWhiteSpace(fileName))
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }
            if (PrimaryKeyConstraint((TPComboBox.SelectedItem as TehnickiPregledModel).Id, new NalogModel().VratiId(ZaposleniComboBox.SelectedItem as string)))
            {
                new MyPrimaryKeyWarning().ShowDialog();
                return;
            }

            SelektovaniTPZ.Delete(SelektovaniTPZ.TehnickiPregledId, SelektovaniTPZ.ZaposleniNalogId);

            SelektovaniTPZ.IzvjestajNaslov = fileName;
            SelektovaniTPZ.TehnickiPregledId = (TPComboBox.SelectedItem as TehnickiPregledModel).Id;
            SelektovaniTPZ.ZaposleniNalogId = new NalogModel().VratiId(ZaposleniComboBox.SelectedItem as string);
            SelektovaniTPZ.IzvjestajSadrzaj = _uploadedFileBytes;
            SelektovaniTPZ.KorisnickoIme = ZaposleniComboBox.SelectedItem as string;
            SelektovaniTPZ.Grad = (TPComboBox.SelectedItem as TehnickiPregledModel).Grad;
            SelektovaniTPZ.Adresa = (TPComboBox.SelectedItem as TehnickiPregledModel).Adresa;
            SelektovaniTPZ.TehnickiPregled = TPComboBox.SelectedItem as TehnickiPregledModel;
            
            
            SelektovaniTPZ.Create();
            new MyUpdateSuccess().ShowDialog();

            IzvjestajViewModel viewModel = _viewModel;
            viewModel.UpdateListaIzvjestaja(SelektovaniTPZ);

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.IzvjestajView.dgIzvjestaji.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.IzvjestajView.dgIzvjestaji.Items.Refresh();
            }
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
