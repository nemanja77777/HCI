using ProjekatA.creates;
using ProjekatA.models;  // Dodajemo using za modele
using ProjekatA.updates;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.CommandWpf;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Mysqlx.Connection;
using ProjekatA.messages;
using ProjekatA.views;

namespace ProjekatA.viewModels
{
    public class AdministratorViewModel : INotifyPropertyChanged
    {
        public AdministratorView AdministratorView;
        public ObservableCollection<AdministratorModel> ListaAdministratora;
        public AdministratorModel SelektovaniAdministrator;
        public ICommand KreirajAdministratorCommand { get; }
        public ICommand IzmjeniAdministratorCommand { get; }
        public ICommand ObrisiAdministratorCommand { get; }

        public ObservableCollection<AdministratorModel> Administratori
        {
            get { return ListaAdministratora; }
            set
            {
                ListaAdministratora = value;
                OnPropertyChanged(nameof(Administratori));
            }
        }

        public AdministratorModel SelectedAdministrator
        {
            get { return SelektovaniAdministrator; }
            set
            {
                SelektovaniAdministrator = value;
                OnPropertyChanged(nameof(SelectedAdministrator));
            }
        }

        public AdministratorViewModel(AdministratorView administratorView)
        {
            Administratori = new ObservableCollection<AdministratorModel>(
                ((List<object>)new AdministratorModel().ReadAll())
                .Cast<AdministratorModel>()
                .Select(a =>
                {
                    var nalog = new NalogModel().Read(a.NalogId);
                    var lokacija = new LokacijaModel().Read(a.LokacijaId);

                    return new AdministratorModel
                    {
                        Lokacija = (LokacijaModel)(new LokacijaModel().Read(a.LokacijaId)),
                        NalogId = a.NalogId,
                        LokacijaId = a.LokacijaId,
                        KorisnickoIme = ((NalogModel)nalog).KorisnickoIme,
                        Adresa = ((LokacijaModel)lokacija).Adresa
                    };
                })
            );
            KreirajAdministratorCommand = new RelayCommand(KreirajAdministrator);
            IzmjeniAdministratorCommand = new RelayCommand(IzmjeniAdministrator, CanExecuteAdministrator);
            ObrisiAdministratorCommand = new RelayCommand(ObrisiAdministrator, CanExecuteAdministrator);
            this.AdministratorView = administratorView;
        }

        private void KreirajAdministrator()
        {
            AdministratorCreate administratorCreate = new AdministratorCreate(this);
            administratorCreate.ShowDialog();
            

        }

        private void IzmjeniAdministrator()
        {
            if (SelektovaniAdministrator != null)
            {
                AdministratorUpdate klijentUpdate = new AdministratorUpdate(SelektovaniAdministrator, this);
                klijentUpdate.ShowDialog();
            }
        }

        private void ObrisiAdministrator()
        {
            if (SelektovaniAdministrator != null)
            {
                MyDeleteQuestion myDelete = new MyDeleteQuestion();
                myDelete.ShowDialog();
                if (myDelete.Potvrda)
                {

                    new AdministratorModel().Delete(SelektovaniAdministrator.NalogId);
                    ListaAdministratora.Remove(SelektovaniAdministrator);
                }
            }
            else
            {
                new MyDeleteWarning().ShowDialog();
            }
        }

        private bool CanExecuteAdministrator() => SelektovaniAdministrator != null;
        public event PropertyChangedEventHandler PropertyChanged;

        
        public void UpdateListaAdminisratora(AdministratorModel izmjenjeniAdministrator)
        {
            var voziloZaIzmenu = ListaAdministratora.FirstOrDefault(v => v.NalogId == izmjenjeniAdministrator.NalogId);
            if (voziloZaIzmenu != null)
            {
                ListaAdministratora[ListaAdministratora.IndexOf(voziloZaIzmenu)] = izmjenjeniAdministrator;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
