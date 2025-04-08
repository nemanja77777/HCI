using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.models;
using ProjekatA.creates;
using ProjekatA.updates;
using System.ComponentModel;
using ProjekatA.views;
using ProjekatA.messages;

namespace ProjekatA.viewModels
{
    public class LokacijaViewModel : INotifyPropertyChanged
    {
        public LokacijaView LokacijaView { get; }

        private ObservableCollection<LokacijaModel> _listaLokacija;

        public LokacijaModel SelektovanaLokacija { get; set; }
        public ObservableCollection<LokacijaModel> ListaLokacija

        {
            get { return _listaLokacija; }
            set
            {
                if (_listaLokacija != value)
                {
                    _listaLokacija = value;
                    OnPropertyChanged(nameof(ListaLokacija));
                }
            }
        }

        public ICommand KreirajLokacijuCommand { get; }
        public ICommand IzmjeniLokacijuCommand { get; }
        public ICommand ObrisiLokacijuCommand { get; }
        

        public LokacijaViewModel(LokacijaView lokacijaView)
        {
            LokacijaModel lokacija = new LokacijaModel();
            ListaLokacija = new ObservableCollection<LokacijaModel>(lokacija.ReadAll().Cast<LokacijaModel>());

            KreirajLokacijuCommand = new RelayCommand(KreirajLokaciju);
            IzmjeniLokacijuCommand = new RelayCommand(IzmjeniLokaciju, CanExecuteLokacija);
            ObrisiLokacijuCommand = new RelayCommand(ObrisiLokaciju, CanExecuteLokacija);

            LokacijaView = lokacijaView;
        }

        private void KreirajLokaciju()
        {
            LokacijaCreate lokacijaCreate = new LokacijaCreate(this);
            lokacijaCreate.ShowDialog();
        }

        private void IzmjeniLokaciju()
        {
            if (SelektovanaLokacija != null)
            {
                LokacijaUpdate izmenaProzor = new LokacijaUpdate(SelektovanaLokacija, this);
                izmenaProzor.ShowDialog();
            }
            
        }

        private void ObrisiLokaciju()
        {
            if (SelektovanaLokacija != null)
            {
                bool InAdministrator = false;
                bool inTehnickiPregled = false;

                foreach (var admin in new AdministratorModel().ReadAll())
                {
                    if (((AdministratorModel)admin).LokacijaId == SelektovanaLokacija.Id)
                    {
                        InAdministrator = true;
                        break;
                    }
                }

                foreach (var tp in new TehnickiPregledModel().ReadAll())
                {
                    if (((TehnickiPregledModel)tp).LokacijaId == SelektovanaLokacija.Id)
                    {
                        inTehnickiPregled = true;
                        break;
                    }
                }

                if (InAdministrator || inTehnickiPregled)
                {
                    new MyIntegrityWarning().ShowDialog();
                    return;
                }
                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();
                if (myDelete.Potvrda)
                {

                    new LokacijaModel().Delete(SelektovanaLokacija.Id);
                    ListaLokacija.Remove(SelektovanaLokacija);
                    new MyDeleteSuccess().ShowDialog();
                }
            }
            else
            {
                new MyDeleteWarning().ShowDialog();
            }
        }

        private bool CanExecuteLokacija() => SelektovanaLokacija != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateListaLokacija(LokacijaModel izmenjenaLokacija)
        {
            var lokacijaZaIzmenu = ListaLokacija.FirstOrDefault(l => l.Id == izmenjenaLokacija.Id);
            if (lokacijaZaIzmenu != null)
            {
                ListaLokacija[ListaLokacija.IndexOf(lokacijaZaIzmenu)] = izmenjenaLokacija;
            }
        }
    }
}
