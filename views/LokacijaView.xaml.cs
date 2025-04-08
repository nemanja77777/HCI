using System.Windows.Controls;
using ProjekatA.viewModels; // Pretpostavljam da je LokacijaViewModel u ovom namespace-u

namespace ProjekatA.views
{
    /// <summary>
    /// Interaction logic for LokacijaView.xaml
    /// </summary>
    public partial class LokacijaView : Page
    {
        public LokacijaView()
        {
            InitializeComponent();
            DataContext = new LokacijaViewModel(this); // Postavljanje ViewModel-a
        }
    }
}
