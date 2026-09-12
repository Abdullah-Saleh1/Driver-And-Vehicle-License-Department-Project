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

namespace DVLD_PresentationLayer.Applications.ApplicationTypes
{
    /// <summary>
    /// Interaction logic for winEditApplicationType.xaml
    /// </summary>
    public partial class winEditApplicationType : Window
    {

        private int _ApplicationTypeID;
        public winEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblID.Content = _ApplicationTypeID.ToString();
            
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void SavePerson(object sender, RoutedEventArgs e)
        {

        }
    }
}
