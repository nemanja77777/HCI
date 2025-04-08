using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProjekatA.models;
using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.creates;
using ProjekatA.updates;
using System.ComponentModel;
using System.Windows;
using ProjekatA.views;
using ProjekatA.messages;


namespace ProjekatA.viewModels
{
    public class VoziloViewModel : INotifyPropertyChanged
    {
        
        public VoziloView voziloView;
        private ObservableCollection<VoziloModel> _listaVozila;
        public ObservableCollection<VoziloModel> ListaVozila
        {
            get { return _listaVozila; }
            set
            {
                if (_listaVozila != value)
                {
                    _listaVozila = value;
                    OnPropertyChanged(nameof(ListaVozila));
                }
            }
        }

        public string DatumString;
        public ICommand KreirajVoziloCommand { get; }
        public ICommand IzmjeniVoziloCommand { get; }
        public ICommand ObrisiVoziloCommand { get; }
        public VoziloModel SelektovanoVozilo { get; set; }

        public VoziloViewModel(VoziloView voziloView)
        {
            VoziloModel vozilo = new VoziloModel();
            ListaVozila = new ObservableCollection<VoziloModel>(
                vozilo.ReadAll().Select(v => new VoziloModel
                {
                    Id = ((VoziloModel)v).Id,
                    VrstaRegistracije = ((VoziloModel)v).VrstaRegistracije,
                    Model = ((VoziloModel)v).Model,
                    DatumProizvodnje = ((VoziloModel)v).DatumProizvodnje,
                    KlijentNalogId = ((VoziloModel)v).KlijentNalogId,
                    DatumProizvodnjeString = ((VoziloModel)v).DatumProizvodnje.ToString("MM-yyyy"),
                    KorisnickoIme = ((NalogModel)(new NalogModel().Read(((VoziloModel)v).KlijentNalogId))).KorisnickoIme
                })
            );
            
            KreirajVoziloCommand = new RelayCommand(KreirajVozilo);
            IzmjeniVoziloCommand = new RelayCommand(IzmjeniVozilo, CanExecuteVozilo);
            ObrisiVoziloCommand = new RelayCommand(ObrisiVozilo, CanExecuteVozilo);
            this.voziloView = voziloView;
        }



        private void KreirajVozilo()
        {
            VoziloCreate voziloCreate = new VoziloCreate(this);
            voziloCreate.ShowDialog();
        }

        // Komanda za izmjenu selektovanog vozila
        private void IzmjeniVozilo()
        {
            if (SelektovanoVozilo != null)
            {
                VoziloUpdate izmenaProzor = new VoziloUpdate(SelektovanoVozilo, this);
                izmenaProzor.ShowDialog();
                OnPropertyChanged(nameof(ListaVozila));
            }
            else
            {
                new MyUpdateWarning().ShowDialog();
            }
        }

        private void ObrisiVozilo()
        {
            if (SelektovanoVozilo != null)
            {
                MyDeleteQuestion myDeleteQuestion =  new MyDeleteQuestion();
                myDeleteQuestion.ShowDialog();
                

                if (myDeleteQuestion.Potvrda)
                {
                    
                    VoziloModel voziloModel = new VoziloModel();
                    voziloModel.Delete(SelektovanoVozilo.Id); 
                    ListaVozila.Remove(SelektovanoVozilo);
                    new MyDeleteSuccess().ShowDialog();
                    OnPropertyChanged(nameof(ListaVozila));
                }
            }
        }

        private bool CanExecuteVozilo() => SelektovanoVozilo != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateListaVozila(VoziloModel izmenjenoVozilo)
        {
            var voziloZaIzmenu = ListaVozila.FirstOrDefault(v => v.Id == izmenjenoVozilo.Id);
            if (voziloZaIzmenu != null)
            {
                ListaVozila[ListaVozila.IndexOf(voziloZaIzmenu)] = izmenjenoVozilo;
            }
        }
    }
}
