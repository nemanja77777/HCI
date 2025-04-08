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
using ProjekatA.messages;

namespace ProjekatA.viewModels
{
    public class NalogViewModel : INotifyPropertyChanged  // Implementacija INotifyPropertyChanged
    {
        public NalogView nalogView;
        private ObservableCollection<NalogModel> _listaNaloga;
        public ObservableCollection<NalogModel> ListaNaloga
        {
            get { return _listaNaloga; }
            set
            {
                if (_listaNaloga != value)
                {
                    _listaNaloga = value;
                    OnPropertyChanged(nameof(ListaNaloga));  // Obavestiti UI o promenama
                }
            }
        }

        public ICommand KreirajNalogCommand { get; }
        public ICommand IzmjeniNalogCommand { get; }
        public ICommand ObrisiNalogCommand { get; }
        public NalogModel SelektovaniNalog { get; set; } // Za selektovani nalog

        public NalogViewModel(NalogView nalogView)
        {
            NalogModel nalog = new NalogModel();
            ListaNaloga = new ObservableCollection<NalogModel>(
                nalog.ReadAll().Select(v => new NalogModel
                {
                    Id = ((NalogModel)v).Id,
                    KorisnickoIme = ((NalogModel)v).KorisnickoIme,
                    Sifra = ((NalogModel)v).Sifra

                })
            );
            KreirajNalogCommand = new RelayCommand(KreirajNalog);
            IzmjeniNalogCommand = new RelayCommand(IzmjeniNalog, CanExecuteNalog);
            ObrisiNalogCommand = new RelayCommand(ObrisiNalog, CanExecuteNalog);
            this.nalogView = nalogView;
        }

        private void KreirajNalog()
        {
            NalogCreate nalogCreate = new NalogCreate(this);
            nalogCreate.ShowDialog();
        }

        private void IzmjeniNalog()
        {
            if (SelektovaniNalog != null)
            {
                NalogUpdate izmjenaProzor = new NalogUpdate(SelektovaniNalog, this);
                izmjenaProzor.ShowDialog();  

                OnPropertyChanged(nameof(ListaNaloga));  
            }
            else
            {
                new MyUpdateWarning().ShowDialog();
            }
        }

        
        private void ObrisiNalog()
        {
            if (SelektovaniNalog != null)
            {
                
                bool postojiUKlijentima = new KlijentModel().Read(SelektovaniNalog.Id) != null;
                bool postojiUZaposlenima = new ZaposleniModel().Read(SelektovaniNalog.Id) != null;
                bool postojiUAdministratorima = new AdministratorModel().Read(SelektovaniNalog.Id) != null;

                if (postojiUKlijentima || postojiUZaposlenima || postojiUAdministratorima)
                {
                    new MyIntegrityWarning().ShowDialog();
                    return;
                }

                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();

                if (myDelete.Potvrda)
                {
                    
                    NalogModel nalogModel = new NalogModel();
                    nalogModel.Delete(SelektovaniNalog.Id);
                    new MyDeleteSuccess().ShowDialog();
                    OnPropertyChanged(nameof(ListaNaloga)); 
                    ListaNaloga.Remove(SelektovaniNalog);
                }
            }
        }
        private bool CanExecuteNalog() => SelektovaniNalog != null;

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public void UpdateListaNaloga(NalogModel izmenjeniNalog)
        {
            var nalogZaIzmenu = ListaNaloga.FirstOrDefault(nalog => nalog.KorisnickoIme == izmenjeniNalog.KorisnickoIme);
            if (nalogZaIzmenu != null)
            {
                ListaNaloga[ListaNaloga.IndexOf(nalogZaIzmenu)] = izmenjeniNalog;
            }
        }

    }
}
