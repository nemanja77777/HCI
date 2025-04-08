using System;
using System.Linq;
using System.Windows;
using ProjekatA.models;
using ProjekatA.viewModels;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using iText.Layout.Properties;
using ProjekatA.messages;
using System.Windows.Controls;

namespace ProjekatA.updates
{
    public partial class VoziloUpdate : Window
    {
        
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }
        private VoziloModel SelektovanoVozilo { get; set; }
        private VoziloViewModel _viewModel { get; set; }
         
        
        public VoziloUpdate(VoziloModel vozilo, VoziloViewModel viewModel)
        {
            InitializeComponent();
            KorisnickaImena = new List<string>(
                new KlijentModel().ReadAll()
                    .Cast<KlijentModel>()
                    .Select(n => ((NalogModel)(new NalogModel().Read(n.NalogId))).KorisnickoIme)
            );

            DataContext = this;
            SelektovanoVozilo = vozilo;
            VrstaRegistracijeTextBox.Text = SelektovanoVozilo.VrstaRegistracije;
            ModelTextBox.Text = SelektovanoVozilo.Model;
            DatumProizvodnjeDatePicker.SelectedDate = SelektovanoVozilo.DatumProizvodnje;
            _viewModel = viewModel;
            IzabranoKorisnickoIme = SelektovanoVozilo.KorisnickoIme;
        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(VrstaRegistracijeTextBox.Text) ||
                string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                DatumProizvodnjeDatePicker.SelectedDate == null ||
                MYCOMBOBOX.SelectedItem == null)
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }



            SelektovanoVozilo.Id = _viewModel.SelektovanoVozilo.Id;
            SelektovanoVozilo.VrstaRegistracije = VrstaRegistracijeTextBox.Text;
            SelektovanoVozilo.Model = ModelTextBox.Text;
            SelektovanoVozilo.DatumProizvodnje = DatumProizvodnjeDatePicker.SelectedDate.Value;
            SelektovanoVozilo.KlijentNalogId = new NalogModel().VratiId(MYCOMBOBOX.SelectedItem as string);
            SelektovanoVozilo.KorisnickoIme = MYCOMBOBOX.SelectedItem as string;

            SelektovanoVozilo.Update();
            new MyUpdateSuccess().ShowDialog();


            VoziloViewModel viewModel = _viewModel;
            viewModel.UpdateListaVozila(SelektovanoVozilo);

            
            var collectionView = CollectionViewSource.GetDefaultView(viewModel.voziloView.dgVozila.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.voziloView.dgVozila.Items.Refresh();
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
