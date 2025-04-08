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
    /// Interaction logic for MyIntegrityWarning.xaml
    /// </summary>
    public partial class MyIntegrityWarning : Window
    {
        public MyIntegrityWarning()
        {
            InitializeComponent();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
