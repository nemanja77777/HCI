using ProjekatA.models;
using ProjekatA.viewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ProjekatA.messages;

namespace ProjekatA.creates
{

    public partial class VoziloCreate : Window
    {
        public List<string> KorisnickaImena { get; set; }
        public string IzabranoKorisnickoIme { get; set; }

        private VoziloViewModel _viewModel; 

        public VoziloCreate(VoziloViewModel viewModel)
        {
            InitializeComponent();
            KorisnickaImena = new List<string>(
                new KlijentModel().ReadAll()
                    .Cast<KlijentModel>() 
                    .Select(n => ((NalogModel)(new NalogModel().Read(n.NalogId))).KorisnickoIme) 
            );

            DataContext = this;
            _viewModel = viewModel; 
        }

       
        private void KreirajButton_Click(object sender, RoutedEventArgs e)
        {
            string vrstaRegistracije = VrstaRegistracijeTextBox.Text;
            string model = ModelTextBox.Text;
            DateTime? datumProizvodnje = DatumProizvodnjeDatePicker.SelectedDate;
            string klijentNalogIDText = KlijentNalogIDComboBox.SelectedItem as string;


            
            if (string.IsNullOrWhiteSpace(vrstaRegistracije) ||
                string.IsNullOrWhiteSpace(model) ||
                datumProizvodnje == null ||
                string.IsNullOrWhiteSpace(klijentNalogIDText))
            {
                new MyCreateWarning().ShowDialog();
                return;
            }

            VoziloModel vozilo = new VoziloModel
            {
                VrstaRegistracije = vrstaRegistracije,
                Model = model,
                DatumProizvodnje = datumProizvodnje.Value,
                KlijentNalogId = new NalogModel().VratiId(klijentNalogIDText),
                KorisnickoIme = klijentNalogIDText
                
            };

            VoziloModel voziloModel = (VoziloModel)(new VoziloModel().Read(vozilo.Create()));
            voziloModel.KorisnickoIme = klijentNalogIDText;
            
            new MyCreateSuccess().ShowDialog();
            _viewModel.ListaVozila.Add(voziloModel);
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
