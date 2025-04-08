using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;
using ProjekatA.models;
using ProjekatA.messages;
using ProjekatA.viewModels;
using System.Windows.Input;

namespace ProjekatA.updates
{
    public partial class LokacijaUpdate : Window
    {
        private LokacijaModel SelektovanaLokacija { get; set; }

        // Constructor that receives the selected Lokacija and the ViewModel
        public LokacijaUpdate(LokacijaModel lokacija, LokacijaViewModel viewModel)
        {
            InitializeComponent();
            SelektovanaLokacija = lokacija;
            this.DataContext = viewModel;  // Bind ViewModel to DataContext

            // Populate the controls with current Lokacija data
            AdresaTextBox.Text = SelektovanaLokacija.Adresa;
            GradTextBox.Text = SelektovanaLokacija.Grad;
            DrzavaTextBox.Text = SelektovanaLokacija.Drzava;
        }

        // Logic for updating the location
        private void IzmjeniButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(AdresaTextBox.Text) ||
                string.IsNullOrWhiteSpace(GradTextBox.Text) ||
                string.IsNullOrWhiteSpace(DrzavaTextBox.Text))
            {
                new MyUpdateWarning().ShowDialog();
                return;
            }

            var viewModel = (LokacijaViewModel)this.DataContext;  

            if (SelektovanaLokacija.Adresa != AdresaTextBox.Text)
            {
                var postojecaLokacija = viewModel.ListaLokacija
                    .FirstOrDefault(lok => string.Equals(lok.Adresa.Replace(" ", ""), AdresaTextBox.Text.Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

                if (postojecaLokacija != null)
                {
                    new MyUpdateWarning().ShowDialog();
                    return;
                }
            }

            
            SelektovanaLokacija.Adresa = AdresaTextBox.Text;
            SelektovanaLokacija.Grad = GradTextBox.Text;
            SelektovanaLokacija.Drzava = DrzavaTextBox.Text;
            SelektovanaLokacija.Id = viewModel.SelektovanaLokacija.Id;
            
            SelektovanaLokacija.Update();  
            
            viewModel.UpdateListaLokacija(SelektovanaLokacija);

            var collectionView = CollectionViewSource.GetDefaultView(viewModel.LokacijaView.dgLokacije.ItemsSource) as IEditableCollectionView;

            if (collectionView != null)
            {
                if (collectionView.IsEditingItem)
                    collectionView.CommitEdit();  

                if (collectionView.IsAddingNew)
                    collectionView.CommitNew();   

                viewModel.LokacijaView.dgLokacije.Items.Refresh(); 
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
