using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;

namespace ProjekatA.creates
{
    public partial class TehnickiPregledCreate : Window
    {
        private TehnickiPregledViewModel _viewModel;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public List<LokacijaModel> Lokacije { get; set; }
        public LokacijaModel IzabranaLokacija { get; set; }

        private DateTime? OdabraniDatum;
        private int OdabraniSati;
        private int OdabraniMinuti;


        public TehnickiPregledCreate(TehnickiPregledViewModel viewModel)
        {
            InitializeComponent();
            KorisnickaImena = new KlijentModel().ReadAll().Cast<KlijentModel>().Select(km => ((NalogModel)(new NalogModel().Read(km.NalogId))).KorisnickoIme).ToList();
            Lokacije = new LokacijaModel().ReadAll().Cast<LokacijaModel>().ToList();
            var collectionView = CollectionViewSource.GetDefaultView(viewModel.TehnickiPregledView.dgTehnickiPregledi.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                viewModel.TehnickiPregledView.dgTehnickiPregledi.Items.Refresh();
            }

            DataContext = this;
            _viewModel = viewModel;
        }

        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string KorisnickoIme = ImenaComboBox.SelectedItem as string;
            LokacijaModel Lokacija = LokacijeComboBox.SelectedItem as LokacijaModel;
            string vrsta = VrstaTextBox.Text;
           // DateTime? datumProizvodnje = DatumDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(KorisnickoIme) ||
                Lokacija == null || 
                string.IsNullOrWhiteSpace(vrsta)
                )
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            DateTime konacanDatumVreme = new DateTime(
                OdabraniDatum.Value.Year,
                OdabraniDatum.Value.Month,
                OdabraniDatum.Value.Day,
                OdabraniSati,
                OdabraniMinuti,
                0
            );

            TehnickiPregledModel temp = new TehnickiPregledModel()
            {
                LokacijaId = Lokacija.Id,
                KlijentNalogId = new NalogModel().VratiId(KorisnickoIme),
                Vrsta = VrstaTextBox.Text,
                Datum = konacanDatumVreme,

            };

            TehnickiPregledModel tehnickiPregledModel =
                (TehnickiPregledModel)(new TehnickiPregledModel().Read(temp.Create()));
            tehnickiPregledModel.KorisnickoIme = KorisnickoIme;
            tehnickiPregledModel.Adresa = Lokacija.Adresa;
            tehnickiPregledModel.Grad = Lokacija.Grad;
            tehnickiPregledModel.Lokacija = Lokacija;
            tehnickiPregledModel.DatumString = konacanDatumVreme.ToString("dd-MM-yyyy");
            tehnickiPregledModel.SatiString = konacanDatumVreme.ToString("HH");
            tehnickiPregledModel.MinutiString = konacanDatumVreme.ToString("mm");

            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaTehnickiPregleda.Add(tehnickiPregledModel);
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

        private void DatumDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatumDatePicker.SelectedDate.HasValue)
            {
                OdabraniDatum = DatumDatePicker.SelectedDate.Value;
            }
        }
        private void cbSati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSati.SelectedItem is ComboBoxItem selectedItem)
            {
                OdabraniSati = int.Parse(selectedItem.Content.ToString());
            }
        }

        private void cbMinuti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMinuti.SelectedItem is ComboBoxItem selectedItem)
            {
                OdabraniMinuti = int.Parse(selectedItem.Content.ToString());
            }
        }
    }
}
