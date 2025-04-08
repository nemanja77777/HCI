using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.models;
using ProjekatA.creates;
using ProjekatA.updates;
using System.Windows;
using ProjekatA.views;
using ProjekatA.messages;
using System.Diagnostics;

namespace ProjekatA.viewModels
{
    public class KlijentViewModel : INotifyPropertyChanged
    {
        public KlijentView KlijentView;
        public ObservableCollection<KlijentModel> ListaKlijenata { get; set; }
        public ICommand KreirajKlijentCommand { get; }
        public ICommand IzmjeniKlijentCommand { get; }
        public ICommand ObrisiKlijentCommand { get; }

        private KlijentModel _selektovaniKlijent;
        public KlijentModel SelektovaniKlijent
        {
            get => _selektovaniKlijent;
            set
            {
                _selektovaniKlijent = value;
                OnPropertyChanged(nameof(SelektovaniKlijent));
            }
        }


        public KlijentViewModel(KlijentView klijentView)
        {
            KlijentModel klijentModel = new KlijentModel();
            ListaKlijenata = new ObservableCollection<KlijentModel>(
                klijentModel.ReadAll().Select(v => new KlijentModel
                {
                    NalogId = ((KlijentModel)v).NalogId,
                    KorisnickoIme = ((NalogModel)(new NalogModel().Read(((KlijentModel)v).NalogId))).KorisnickoIme,
                    OpisKlijenta = ((KlijentModel)v).OpisKlijenta
                })
            );
            KreirajKlijentCommand = new RelayCommand(KreirajKlijent);
            IzmjeniKlijentCommand = new RelayCommand(IzmjeniKlijent, CanExecuteKlijent);
            ObrisiKlijentCommand = new RelayCommand(ObrisiKlijent, CanExecuteKlijent);
            this.KlijentView = klijentView;
        }

        private void KreirajKlijent()
        {
            KlijentCreate klijentCreate = new KlijentCreate(this);
            klijentCreate.ShowDialog();
        }

        private void IzmjeniKlijent()
        {
            if (SelektovaniKlijent != null)
            {
                KlijentUpdate klijentUpdate = new KlijentUpdate(SelektovaniKlijent, this);
                klijentUpdate.ShowDialog();
            }
            else
            {
                new MyUpdateWarning().ShowDialog();
            }
        }

        private void ObrisiKlijent()
        {

            if (SelektovaniKlijent != null)
            {
                bool InVozilo = false;
                bool inTehnickiPregled = false;

                foreach ( var vozilo in new VoziloModel().ReadAll())
                {
                    if (((VoziloModel)vozilo).KlijentNalogId == SelektovaniKlijent.NalogId)
                    {
                        InVozilo = true;
                        break;
                    }
                }

                foreach (var tehnickiPregeld in new TehnickiPregledModel().ReadAll())
                {
                    if (((TehnickiPregledModel)tehnickiPregeld).KlijentNalogId == SelektovaniKlijent.NalogId)
                    {
                        inTehnickiPregled = true;
                        break;
                    }
                }
                

                if (InVozilo || inTehnickiPregled)
                {
                    new MyIntegrityWarning().ShowDialog();
                    return;
                }
                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();
                if (myDelete.Potvrda)
                {
                    KlijentModel km = new KlijentModel();
                    km.Delete(SelektovaniKlijent.NalogId);
                    ListaKlijenata.Remove(SelektovaniKlijent);
                    new MyDeleteSuccess().ShowDialog();

                }
            }
            
        }
        public void UpdateListaKlijenata(KlijentModel izmenjeniKlijent)
        {
            var voziloZaIzmenu = ListaKlijenata.FirstOrDefault(v => v.NalogId == izmenjeniKlijent.NalogId);
            if (voziloZaIzmenu != null)
            {
                ListaKlijenata[ListaKlijenata.IndexOf(voziloZaIzmenu)] = izmenjeniKlijent;
            }
        }
        private bool CanExecuteKlijent() => SelektovaniKlijent != null;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}