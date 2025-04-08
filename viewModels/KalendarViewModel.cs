using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProjekatA.models;
using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.creates;
using ProjekatA.updates;
using System.ComponentModel;  // Dodaj ovo za INotifyPropertyChanged
using System.Windows;
using ProjekatA.views;
using System.Windows.Controls;
using System.Globalization;
using ProjekatA.messages;


namespace ProjekatA.viewModels
{
    public class KalendarViewModel : INotifyPropertyChanged  // Implementacija INotifyPropertyChanged
    {
        public KalendarView KalendarView;
        private ObservableCollection<KalendarModel> _listaKalendara;
        public ObservableCollection<KalendarModel> FiltriraniPodaci { get; set; } = new ObservableCollection<KalendarModel>();
        public ObservableCollection<KalendarModel> ListaKalendara
        {
            get { return _listaKalendara; }
            set
            {
                if (_listaKalendara != value)
                {
                    _listaKalendara = value;
                    OnPropertyChanged(nameof(ListaKalendara));  // Obavestiti UI o promenama
                }
            }
        }

        public ICommand KreirajKalendarCommand { get; }
        public ICommand IzmjeniKalendarCommand { get; }
        public ICommand ObrisiKalendarCommand { get; }
        public KalendarModel SelektovaniKalendar { get; set; } // Za selektovani Kalendar

        public KalendarViewModel(KalendarView KalendarView)
        { 
            KalendarModel kalendar = new KalendarModel();
            if (LoginViewModel.IsAdministrator)
            {
                ListaKalendara = new ObservableCollection<KalendarModel>(
                    kalendar.ReadAll().Select(v => new KalendarModel
                    {
                        KorisnickoIme = v.KorisnickoIme,
                        Grad = v.Grad,
                        Adresa = v.Adresa,
                        Vrsta = v.Vrsta,
                        Vrijeme = v.Vrijeme,
                        Lokacija = (LokacijaModel)(new LokacijaModel().Read((new LokacijaModel().VratiId(v.Adresa))))
                    })
                );
            }
            else
            {
                ListaKalendara = new ObservableCollection<KalendarModel>(
                    kalendar.ReadAll().Where(kal=>kal.KorisnickoIme == LoginViewModel.CurrentUsername).Select(v => new KalendarModel
                    {
                        KorisnickoIme = v.KorisnickoIme,
                        Grad = v.Grad,
                        Adresa = v.Adresa,
                        Vrsta = v.Vrsta,
                        Vrijeme = v.Vrijeme,
                        Lokacija = (LokacijaModel)(new LokacijaModel().Read((new LokacijaModel().VratiId(v.Adresa))))
                    })
                );

            }

            KreirajKalendarCommand = new RelayCommand(KreirajKalendar);
            IzmjeniKalendarCommand = new RelayCommand(IzmjeniKalendar, CanExecuteKalendar);
            ObrisiKalendarCommand = new RelayCommand(ObrisiKalendar, CanExecuteKalendar);
            this.KalendarView = KalendarView;
            FiltriraniPodaci = new ObservableCollection<KalendarModel>(ListaKalendara);
            KalendarView.dgKelendari.ItemsSource = FiltriraniPodaci;
        }

        // Komanda za kreiranje novog Kalendara
        private void KreirajKalendar()
        {
           // KalendarCreate KalendarCreate = new KalendarCreate(this);
           // KalendarCreate.ShowDialog();
        }

        // Komanda za izmene selektovanog Kalendara
        private void IzmjeniKalendar()
        {
            if (SelektovaniKalendar != null)
            {
                // Otvori novi prozor za izmenu
               // KalendarUpdate izmenaProzor = new KalendarUpdate(SelektovaniKalendar, this);
               // izmenaProzor.ShowDialog();  // ShowDialog je blokirajući poziv, znači da će čekati da korisnik završi sa izmenama pre nego što se vrati u glavni prozor

                // Osveži DataGrid nakon što se prozor zatvori
                OnPropertyChanged(nameof(ListaKalendara));  // Ove promene će automatski osvežiti DataGrid
            }
            else
            {
                MessageBox.Show("Molimo odaberite Kalendar koji želite izmeniti.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Komanda za brisanje selektovanog Kalendara
        private void ObrisiKalendar()
        {
            if (SelektovaniKalendar != null)
            {


                // Prikazivanje MessageBox-a za potvrdu brisanja
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj Kalendar?",
                    "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Ako korisnik potvrdi brisanje, nastavljamo sa brisanjem
                if (rezultat == MessageBoxResult.Yes)
                {

                    KalendarModel ZaposleniKalendarModell = new KalendarModel();
                    //ZaposleniKalendarModel.Delete(SelektovaniKalendar.Id);
                    OnPropertyChanged(nameof(ListaKalendara)); // Osvežava listu nakon brisanja
                    ListaKalendara.Remove(SelektovaniKalendar);
                }
            }
        }



        // Proverava da li je selektovani Kalendar validan za izmenu ili brisanje
        private bool CanExecuteKalendar() => SelektovaniKalendar != null;

        // Implementacija INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda za ažuriranje liste Kalendara u ViewModel-u
        public void UpdateListaKalendara(KalendarModel izmenjeniKalendar)
        {
            // var KalendarZaIzmenu = ListaKalendara.FirstOrDefault(Kalendar => Kalendar.KorisnickoIme == izmenjeniKalendar.KorisnickoIme);
            // if (KalendarZaIzmenu != null)
            // {
            //     ListaKalendara[ListaKalendara.IndexOf(KalendarZaIzmenu)] = izmenjeniKalendar;
            // }
        }
        public void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.Calendar calendar && calendar.SelectedDate.HasValue)
            {

                string odabraniDatum = calendar.SelectedDate.Value.ToString("dd/MM"); // Format koji odgovara zapisu u stringu
                FiltriraniPodaci.Clear();
                foreach (var item in ListaKalendara.Where(x => x.Vrijeme.StartsWith(odabraniDatum)))
                {
                    FiltriraniPodaci.Add(item);
                }
               
            }
        }
        


    }
}
