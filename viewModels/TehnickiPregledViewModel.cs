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
    public class TehnickiPregledViewModel : INotifyPropertyChanged  // Implementacija INotifyPropertyChanged
    {
        public TehnickiPregledView TehnickiPregledView;

        private ObservableCollection<TehnickiPregledModel> _listaTehnickiPregleda;
        public ObservableCollection<TehnickiPregledModel> ListaTehnickiPregleda
        {
            get { return _listaTehnickiPregleda; }
            set
            {
                if (_listaTehnickiPregleda != value)
                {
                    _listaTehnickiPregleda = value;
                    OnPropertyChanged(nameof(ListaTehnickiPregleda));  // Obavestiti UI o promenama
                }
            }
        }

        public ICommand KreirajTehnickiPregledCommand { get; }
        public ICommand IzmjeniTehnickiPregledCommand { get; }
        public ICommand ObrisiTehnickiPregledCommand { get; }
        private TehnickiPregledModel _selektovaniTehnickiPregled;
        public TehnickiPregledModel SelektovaniTehnickiPregled
        {
            get { return _selektovaniTehnickiPregled; }
            set
            {
                if (_selektovaniTehnickiPregled != value)
                {
                    _selektovaniTehnickiPregled = value;
                    OnPropertyChanged(nameof(SelektovaniTehnickiPregled));  // Obavesti UI o promeni
                }
            }
        }


        public TehnickiPregledViewModel(TehnickiPregledView TehnickiPregledView)
        {
            TehnickiPregledModel tehnickiPregledi = new TehnickiPregledModel();
            ListaTehnickiPregleda = new ObservableCollection<TehnickiPregledModel>(
                tehnickiPregledi.ReadAll().Select(v => new TehnickiPregledModel()
                {
                    Id = ((TehnickiPregledModel)v).Id,
                    KorisnickoIme = ((NalogModel)(new NalogModel().Read((((TehnickiPregledModel)v).KlijentNalogId)))).KorisnickoIme,
                    Lokacija = (LokacijaModel)new LokacijaModel().Read(((TehnickiPregledModel)v).LokacijaId),                        
                    Datum = ((TehnickiPregledModel)v).Datum,
                    Adresa = ((LokacijaModel)new LokacijaModel().Read(((TehnickiPregledModel)v).LokacijaId)).Adresa,
                    Grad = ((LokacijaModel)new LokacijaModel().Read(((TehnickiPregledModel)v).LokacijaId)).Grad,
                    Vrsta = ((TehnickiPregledModel)v).Vrsta,
                    DatumString = ((TehnickiPregledModel)v).Datum.ToString("dd-MM-yyyy"),
                    SatiString = ((TehnickiPregledModel)v).Datum.ToString("HH"),
                    MinutiString = ((TehnickiPregledModel)v).Datum.ToString("mm")



                }
                )
            );
            KreirajTehnickiPregledCommand = new RelayCommand(KreirajTehnickiPregled);
            IzmjeniTehnickiPregledCommand = new RelayCommand(IzmjeniTehnickiPregled, CanExecuteTehnickiPregled);
            ObrisiTehnickiPregledCommand = new RelayCommand(ObrisiTehnickiPregled, CanExecuteTehnickiPregled);
            this.TehnickiPregledView = TehnickiPregledView;
        }

        private void KreirajTehnickiPregled()
        {
            TehnickiPregledCreate tehnickiPregledCreate = new TehnickiPregledCreate(this);
            tehnickiPregledCreate.ShowDialog();
        }

        private void IzmjeniTehnickiPregled()
        {
            if (SelektovaniTehnickiPregled != null)
            {
                
                TehnickiPregledUpdate izmenaProzor = new TehnickiPregledUpdate(SelektovaniTehnickiPregled, this);
                izmenaProzor.ShowDialog();  
                  
            }
            
        }

        private void ObrisiTehnickiPregled()
        {
            if (SelektovaniTehnickiPregled != null)
            {
                bool InZTP = false;
                

                foreach (var ztp in new ZaposleniTehnickiPregledModel().ReadAll())
                {
                    if (((ZaposleniTehnickiPregledModel)ztp).TehnickiPregledId == SelektovaniTehnickiPregled.Id)
                    {
                        InZTP = true;
                        break;
                    }
                }

                if (InZTP)
                {
                    new MyIntegrityWarning().ShowDialog();
                    return;
                }

                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();

                if (myDelete.Potvrda)
                {
                    
                    new TehnickiPregledModel().Delete(SelektovaniTehnickiPregled.Id);
                    ListaTehnickiPregleda.Remove(SelektovaniTehnickiPregled);
                    new MyDeleteSuccess().ShowDialog();
                }
            }
            else
            {
                new MyDeleteWarning().ShowDialog();
            }
        }



        
        private bool CanExecuteTehnickiPregled() => SelektovaniTehnickiPregled != null;

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void UpdateListaTehnickiPregleda(TehnickiPregledModel TP)
        {
            var x = ListaTehnickiPregleda.FirstOrDefault(v => v.Id == TP.Id);
            if (x != null)
            {
                x.Datum = TP.Datum;
                x.DatumString = TP.Datum.ToString("dd-MM-yyyy");
                // Dodaj i ostala polja koja su možda promijenjena
            }
        }


    }
}
