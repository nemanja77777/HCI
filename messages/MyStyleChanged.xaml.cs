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
using System.Windows.Shapes;

namespace ProjekatA.messages
{
    /// <summary>
    /// Interaction logic for MyStyleChanged.xaml
    /// </summary>
    public partial class MyStyleChanged : Window
    {
        public MyStyleChanged()
        {
            InitializeComponent();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
