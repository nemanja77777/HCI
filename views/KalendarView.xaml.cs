using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjekatA.viewModels;

namespace ProjekatA.views
{
    /// <summary>
    /// Interaction logic for KalendarView.xaml
    /// </summary>
    public partial class KalendarView : Page
    {
        public KalendarView()
        {
            InitializeComponent();
            this.DataContext = new KalendarViewModel(this);
        }
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            new KalendarViewModel(this).Calendar_SelectedDatesChanged(sender, e);
            
        }

        private void Kalendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            //new KalendarViewModel(this).Kalendar_DisplayModeChanged(sender, e);
        }
    }
}
