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
using System.Diagnostics;

namespace ProjekatA.viewModels
{
    public class ZaposleniViewModel : INotifyPropertyChanged
    {
        public ZaposleniView zaposleniView;
        private ObservableCollection<ZaposleniModel> _listaZaposlenih;
        public ObservableCollection<ZaposleniModel> ListaZaposlenih
        {
            get { return _listaZaposlenih; }
            set
            {
                if (_listaZaposlenih != value)
                {
                    _listaZaposlenih = value;
                   

                    OnPropertyChanged(nameof(ListaZaposlenih));
                }
            }
        }

        public ICommand KreirajZaposleniCommand { get; }
        public ICommand IzmjeniZaposleniCommand { get; }
        public ICommand ObrisiZaposleniCommand { get; }
        private ZaposleniModel _selektovaniZaposleni;
        public ZaposleniModel SelektovaniZaposleni
        {
            get => _selektovaniZaposleni;
            set
            {
                _selektovaniZaposleni = value;
                OnPropertyChanged(nameof(SelektovaniZaposleni));
            }
        }

        public ZaposleniViewModel(ZaposleniView zaposleniView)
        {
            ZaposleniModel zaposleni = new ZaposleniModel();
            ListaZaposlenih = new ObservableCollection<ZaposleniModel>(
                zaposleni.ReadAll().Select(v => new ZaposleniModel
                {
                    NalogId = ((ZaposleniModel)v).NalogId,
                    Zvanje = ((ZaposleniModel)v).Zvanje,
                    
                    KorisnickoIme = ((NalogModel)(new NalogModel().Read(((ZaposleniModel)v).NalogId))).KorisnickoIme
                })
            );

            KreirajZaposleniCommand = new RelayCommand(KreirajZaposleni);
            IzmjeniZaposleniCommand = new RelayCommand(IzmjeniZaposleni, CanExecuteZaposleni);
            ObrisiZaposleniCommand = new RelayCommand(ObrisiZaposleni, CanExecuteZaposleni);
            this.zaposleniView = zaposleniView;
        }
        private void KreirajZaposleni()
        {
            ZaposleniCreate zaposleniCreate = new ZaposleniCreate(this);
            zaposleniCreate.ShowDialog();
        }

        
        private void IzmjeniZaposleni()
        {
            if (SelektovaniZaposleni != null)
            {
                ZaposleniUpdate klijentUpdate = new ZaposleniUpdate(SelektovaniZaposleni, this);
                klijentUpdate.ShowDialog();
            }
            else
            {
                new MyUpdateWarning().ShowDialog();
            }
        }

        private void ObrisiZaposleni()
        {
            if (SelektovaniZaposleni != null)
            {
                bool InZaposleniTehnickiPregled = false;
                
                foreach (var ztp in new ZaposleniTehnickiPregledModel().ReadAll())
                {
                    if (((ZaposleniTehnickiPregledModel)ztp).ZaposleniNalogId == SelektovaniZaposleni.NalogId)
                    {
                        InZaposleniTehnickiPregled = true;
                        break;
                    }
                }

                if (InZaposleniTehnickiPregled)
                {
                    new MyIntegrityWarning().ShowDialog();
                    return;
                }

                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();

                if (myDelete.Potvrda)
                {

                    new ZaposleniModel().Delete(SelektovaniZaposleni.NalogId);
                    new MyDeleteSuccess().ShowDialog();
                    ListaZaposlenih.Remove(SelektovaniZaposleni);
                    OnPropertyChanged(nameof(ListaZaposlenih));
                }
            }
            else
            {
                new MyDeleteWarning().ShowDialog();
            }
        }

        private bool CanExecuteZaposleni() => SelektovaniZaposleni != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateListaZaposlenih(ZaposleniModel izmenjeniZaposleni)
        {
            var zaposleniZaIzmenu = ListaZaposlenih.FirstOrDefault(z => z.NalogId == izmenjeniZaposleni.NalogId);
            if (zaposleniZaIzmenu != null)
            {
                ListaZaposlenih[ListaZaposlenih.IndexOf(zaposleniZaIzmenu)] = izmenjeniZaposleni;
            }
        }
    }
}
