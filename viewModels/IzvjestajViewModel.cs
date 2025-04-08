using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ProjekatA.models;
using GalaSoft.MvvmLight.CommandWpf;
using ProjekatA.creates;
using ProjekatA.updates;
using System.ComponentModel;
using System.IO; // Dodaj ovo za INotifyPropertyChanged
using System.Windows;
using Microsoft.Win32;
using ProjekatA.views;
using ProjekatA.messages;

namespace ProjekatA.viewModels
{
    public class IzvjestajViewModel : INotifyPropertyChanged  // Implementacija INotifyPropertyChanged
    {
        public IzvjestajView IzvjestajView;
        private ObservableCollection<ZaposleniTehnickiPregledModel> _listaIzvjestaja;
        public ObservableCollection<ZaposleniTehnickiPregledModel> ListaIzvjestaja
        {
            get { return _listaIzvjestaja; }
            set
            {
                if (_listaIzvjestaja != value)
                {
                    _listaIzvjestaja = value;
                    OnPropertyChanged(nameof(ListaIzvjestaja));  // Obavestiti UI o promenama
                }
            }
        }

        public ICommand KreirajIzvjestajCommand { get; }
        public ICommand IzmjeniIzvjestajCommand { get; }
        public ICommand ObrisiIzvjestajCommand { get; }
        public ICommand ProcitajSadrzajCommand { get; }
        public ZaposleniTehnickiPregledModel SelektovaniIzvjestaj { get; set; } // Za selektovani Izvjestaj

        public IzvjestajViewModel(IzvjestajView IzvjestajView)
        {
            //List<string> Names = new AdministratorModel().ReadAll().Cast<AdministratorModel>()
                //.Select(a => ((NalogModel)(new NalogModel().Read(a.NalogId))).KorisnickoIme).ToList();

            ZaposleniTehnickiPregledModel zaposleni = new ZaposleniTehnickiPregledModel();
            if (LoginViewModel.IsAdministrator)
            {
               
                ListaIzvjestaja = new ObservableCollection<ZaposleniTehnickiPregledModel>(
                    zaposleni.ReadAll().Select(v => new ZaposleniTehnickiPregledModel
                    {
                        Lokacija = (LokacijaModel)(new LokacijaModel().Read(
                            ((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)),
                        TehnickiPregledId = ((ZaposleniTehnickiPregledModel)v).TehnickiPregledId,
                        ZaposleniNalogId = ((ZaposleniTehnickiPregledModel)v).ZaposleniNalogId,
                        KorisnickoIme =
                            ((NalogModel)(new NalogModel().Read(((ZaposleniTehnickiPregledModel)v).ZaposleniNalogId)))
                            .KorisnickoIme,
                        Grad = ((LokacijaModel)(new LokacijaModel().Read(
                            (((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)))).Grad,
                        Adresa = ((LokacijaModel)(new LokacijaModel().Read(
                            (((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)))).Adresa,
                        IzvjestajNaslov = ((ZaposleniTehnickiPregledModel)v).IzvjestajNaslov,
                        TehnickiPregled = (TehnickiPregledModel)(new TehnickiPregledModel().Read((((ZaposleniTehnickiPregledModel)v).TehnickiPregledId)))
                        
                    })
                );
            }
            else
            {
                ListaIzvjestaja = new ObservableCollection<ZaposleniTehnickiPregledModel>(
                    zaposleni.ReadAll().Where(z=>((ZaposleniTehnickiPregledModel)z).ZaposleniNalogId == new NalogModel().VratiId(LoginViewModel.CurrentUsername)).Select(v => new ZaposleniTehnickiPregledModel
                    {
                        Lokacija = (LokacijaModel)(new LokacijaModel().Read(
                            ((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)),
                        TehnickiPregledId = ((ZaposleniTehnickiPregledModel)v).TehnickiPregledId,
                        ZaposleniNalogId = ((ZaposleniTehnickiPregledModel)v).ZaposleniNalogId,
                        KorisnickoIme =
                            ((NalogModel)(new NalogModel().Read(((ZaposleniTehnickiPregledModel)v).ZaposleniNalogId)))
                            .KorisnickoIme,
                        Grad = ((LokacijaModel)(new LokacijaModel().Read(
                            (((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)))).Grad,
                        Adresa = ((LokacijaModel)(new LokacijaModel().Read(
                            (((TehnickiPregledModel)(new TehnickiPregledModel().Read(((ZaposleniTehnickiPregledModel)v)
                                .TehnickiPregledId))).LokacijaId)))).Adresa,
                        IzvjestajNaslov = ((ZaposleniTehnickiPregledModel)v).IzvjestajNaslov,
                        TehnickiPregled = (TehnickiPregledModel)(new TehnickiPregledModel().Read((((ZaposleniTehnickiPregledModel)v).TehnickiPregledId)))
                    })
                );
            }

            KreirajIzvjestajCommand = new RelayCommand(KreirajIzvjestaj);
            IzmjeniIzvjestajCommand = new RelayCommand(IzmjeniIzvjestaj, CanExecuteIzvjestaj);
            ObrisiIzvjestajCommand = new RelayCommand(ObrisiIzvjestaj, CanExecuteIzvjestaj);
            ProcitajSadrzajCommand = new RelayCommand(ProcitajSadrzaj,CanExecuteIzvjestaj);
            this.IzvjestajView = IzvjestajView;
        }

        // Komanda za kreiranje novog Izvjestaja
        private void KreirajIzvjestaj()
        {
            IzvjestajCreate IzvjestajCreate = new IzvjestajCreate(this);
            IzvjestajCreate.ShowDialog();
        }

        // Komanda za izmene selektovanog Izvjestaja
        private void IzmjeniIzvjestaj()
        {
            if (SelektovaniIzvjestaj != null)
            {
              
                IzvjestajUpdate izmenaProzor = new IzvjestajUpdate(SelektovaniIzvjestaj, this);
                izmenaProzor.ShowDialog();
            }
            
        }

        // Komanda za brisanje selektovanog Izvjestaja
        private void ObrisiIzvjestaj()
        {
            if (SelektovaniIzvjestaj != null)
            {
                
                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();
                if (myDelete.Potvrda)
                {

                    new ZaposleniTehnickiPregledModel().Delete(SelektovaniIzvjestaj.TehnickiPregledId, SelektovaniIzvjestaj.ZaposleniNalogId);
                    ListaIzvjestaja.Remove(SelektovaniIzvjestaj);
                    new MyDeleteSuccess().ShowDialog();
                }
            }
            else
            {
                new MyDeleteWarning().ShowDialog();
            }

        }



        // Proverava da li je selektovani Izvjestaj validan za izmenu ili brisanje
        private bool CanExecuteIzvjestaj() => SelektovaniIzvjestaj != null;

        // Implementacija INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda za ažuriranje liste Izvjestaja u ViewModel-u
        public void UpdateListaIzvjestaja(ZaposleniTehnickiPregledModel izmenjeniIzvjestaj)
        {
           // var IzvjestajZaIzmenu = ListaIzvjestaja.FirstOrDefault(Izvjestaj => Izvjestaj.KorisnickoIme == izmenjeniIzvjestaj.KorisnickoIme);
           // if (IzvjestajZaIzmenu != null)
           // {
           //     ListaIzvjestaja[ListaIzvjestaja.IndexOf(IzvjestajZaIzmenu)] = izmenjeniIzvjestaj;
           // }
        }
        private void ProcitajSadrzaj()
        {
            if (SelektovaniIzvjestaj == null)
            {
                new MyCreateWarning().ShowDialog();
            }

            try
            {
                // Poziv metode koja vraća byte[] sa sadržajem fajla iz baze
                byte[] fileContent =
                    new ZaposleniTehnickiPregledModel().ReadFileContent(SelektovaniIzvjestaj.TehnickiPregledId,
                        SelektovaniIzvjestaj.ZaposleniNalogId);

                if (fileContent == null || fileContent.Length == 0)
                {
                    new MyFileCreateWarning();
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = SelektovaniIzvjestaj.IzvjestajNaslov, // Pretpostavka da naslov sadrži ekstenziju
                    Filter = "Dokumenti (*.pdf, *.docx, *.doc)|*.pdf;*.docx;*.doc|Svi fajlovi|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, fileContent);
                    new MyFileCreateSuccess().ShowDialog();
                }
            }
            catch (Exception ex)
            {
                new MyFileCreateWarning().ShowDialog();
            }
        }


}
}
