using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.messages;
using ProjekatA.models;
using ProjekatA.viewModels;

namespace ProjekatA.updates
{
    public partial class TehnickiPregledUpdate : Window
    {
        private TehnickiPregledModel SelektovaniTehnickiPregeld;
        private TehnickiPregledViewModel _viewModel;
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        public List<LokacijaModel> Lokacije { get; set; }
        public LokacijaModel IzabranaLokacija { get; set; }

        private DateTime? OdabraniDatum;
        private int OdabraniSati;
        private int OdabraniMinuti;

        private string StringOdabraniSati;
        private string StringOdabraniMinuti;

        public TehnickiPregledUpdate(TehnickiPregledModel pregled, TehnickiPregledViewModel viewModel)
        {
            InitializeComponent();
            KorisnickaImena = new KlijentModel().ReadAll().Cast<KlijentModel>().Select(nm => ((NalogModel)(new NalogModel().Read(nm.NalogId))).KorisnickoIme).ToList();
            Lokacije = new LokacijaModel().ReadAll().Cast<LokacijaModel>().ToList();

            DataContext = this;
            SelektovaniTehnickiPregeld = pregled;
            _viewModel = viewModel;

            IzabranoKorisnickoIme = SelektovaniTehnickiPregeld.KorisnickoIme;
            IzabranaLokacija = SelektovaniTehnickiPregeld.Lokacija;
            Lokacije.Add(SelektovaniTehnickiPregeld.Lokacija);
            LokacijeComboBox.SelectedItem = SelektovaniTehnickiPregeld.Lokacija;
            VrstaTextBox.Text = SelektovaniTehnickiPregeld.Vrsta;
            DatumDatePicker.SelectedDate = SelektovaniTehnickiPregeld.Datum;

            int OdabraniMinuti = int.Parse(SelektovaniTehnickiPregeld.Datum.ToString("mm"));
            int OdabraniSati = int.Parse(SelektovaniTehnickiPregeld.Datum.ToString("HH"));

            Dispatcher.InvokeAsync(() =>
            {
                string minutiString = OdabraniMinuti.ToString("D2");
                string satiString = OdabraniSati.ToString("D2");

                StringOdabraniMinuti = minutiString;
                StringOdabraniSati = satiString;

                foreach (ComboBoxItem item in cbMinuti.Items)
                {
                    if (item.Content?.ToString() == minutiString)
                    {
                        cbMinuti.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cbSati.Items)
                {
                    if (item.Content?.ToString() == satiString)
                    {
                        cbSati.SelectedItem = item;
                        break;
                    }
                }
            });




        }

        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(VrstaTextBox.Text)    ||
                DatumDatePicker.SelectedDate == null            ||  
                cbSati.SelectedItem == null                     ||
                cbMinuti.SelectedItem == null                   ||
                ImenaComboBox.SelectedItem == null              ||
                LokacijeComboBox.SelectedItem == null
                )
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }

            
            SelektovaniTehnickiPregeld.Id = _viewModel.SelektovaniTehnickiPregled.Id;
            SelektovaniTehnickiPregeld.KorisnickoIme = ImenaComboBox.SelectedItem as string;
            SelektovaniTehnickiPregeld.Adresa = (LokacijeComboBox.SelectedItem as LokacijaModel).Adresa;
            SelektovaniTehnickiPregeld.Vrsta = VrstaTextBox.Text;
            SelektovaniTehnickiPregeld.LokacijaId =(LokacijeComboBox.SelectedItem as LokacijaModel).Id;
            SelektovaniTehnickiPregeld.KlijentNalogId = new NalogModel().VratiId(ImenaComboBox.SelectedItem as string);
            SelektovaniTehnickiPregeld.Grad =
                ((LokacijaModel)(new LokacijaModel().Read(SelektovaniTehnickiPregeld.LokacijaId))).Grad;
            SelektovaniTehnickiPregeld.Lokacija= LokacijeComboBox.SelectedItem as LokacijaModel;

            SelektovaniTehnickiPregeld.Datum = new DateTime(
                DatumDatePicker.SelectedDate.Value.Year,
                DatumDatePicker.SelectedDate.Value.Month,
                DatumDatePicker.SelectedDate.Value.Day,
                int.Parse(((ComboBoxItem)cbSati.SelectedItem).Content.ToString()),
                int.Parse(((ComboBoxItem)cbMinuti.SelectedItem).Content.ToString()),
                0
                );
            SelektovaniTehnickiPregeld.DatumString = SelektovaniTehnickiPregeld.Datum.ToString("dd-MM-yyyy");
            SelektovaniTehnickiPregeld.SatiString = SelektovaniTehnickiPregeld.Datum.ToString("HH");
            SelektovaniTehnickiPregeld.MinutiString = SelektovaniTehnickiPregeld.Datum.ToString("mm");

            SelektovaniTehnickiPregeld.Update();
            new MyUpdateSuccess().ShowDialog();

            TehnickiPregledViewModel tehnickiPregledViewModel = _viewModel;
            tehnickiPregledViewModel.UpdateListaTehnickiPregleda(SelektovaniTehnickiPregeld);

            var collectionView = CollectionViewSource.GetDefaultView(tehnickiPregledViewModel.TehnickiPregledView.dgTehnickiPregledi.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();
                tehnickiPregledViewModel.TehnickiPregledView.dgTehnickiPregledi.Items.Refresh();
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
