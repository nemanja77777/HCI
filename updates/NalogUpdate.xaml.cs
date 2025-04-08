using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ProjekatA.models;
using Microsoft.AspNetCore.Identity;
using ProjekatA.viewModels; // Ensure correct namespace
using ProjekatA.views;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using ProjekatA.messages;

namespace ProjekatA.updates
{
    public partial class NalogUpdate : Window
    {
        private NalogModel SelektovaniNalog { get; set; }
        private PasswordHasher<NalogModel> passwordHasher = new PasswordHasher<NalogModel>();

        
        public NalogUpdate(NalogModel nalog, NalogViewModel viewModel)
        {
            InitializeComponent();
            SelektovaniNalog = nalog;
            this.DataContext = viewModel;  
           
            KorisnickoImeTextBox.Text = SelektovaniNalog.KorisnickoIme;
        }

        
        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(KorisnickoImeTextBox.Text) || string.IsNullOrWhiteSpace(SifraPasswordBox.Password))
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }
           
            var viewModel = (NalogViewModel)this.DataContext; 

            if (SelektovaniNalog.KorisnickoIme != KorisnickoImeTextBox.Text)
            {
                var postojeciNalog = viewModel.ListaNaloga
                    .FirstOrDefault(nalog => nalog.KorisnickoIme.Equals(KorisnickoImeTextBox.Text, StringComparison.OrdinalIgnoreCase));

                if (postojeciNalog != null)
                {
                    new MyUpdateWarning().ShowDialog();
                    return;
                }
            }

            SelektovaniNalog.KorisnickoIme = KorisnickoImeTextBox.Text;
            
            string hashiranaSifra = passwordHasher.HashPassword(SelektovaniNalog, SifraPasswordBox.Password);

            SelektovaniNalog.Sifra = hashiranaSifra;
            SelektovaniNalog.Id = viewModel.SelektovaniNalog.Id;
            SelektovaniNalog.Update(); 

            
            var viewModel1 = (NalogViewModel)this.DataContext;
            viewModel1.UpdateListaNaloga(SelektovaniNalog);

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.nalogView.dgNalozi.ItemsSource) as IEditableCollectionView;
            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();  
                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();   

                viewModel.nalogView.dgNalozi.Items.Refresh(); 
            }

            new MyUpdateSuccess().ShowDialog();
            this.Close();
        }
        private void TopPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Ne zelim da nesto radi
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
