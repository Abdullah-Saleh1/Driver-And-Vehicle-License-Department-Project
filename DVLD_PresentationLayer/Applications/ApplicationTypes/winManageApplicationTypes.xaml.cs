using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for winManageApplicationTypes.xaml
    /// </summary>
    public partial class winManageApplicationTypes : Window
    {
        public winManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void EditApplicationType(object sender, RoutedEventArgs e)
        {
            int ApplicationTypeID = Convert.ToInt32((dgApplicationTypes.SelectedItem as DataRowView)["ApplicationTypeID"]);

            winEditApplicationType winEditApplicationType = new winEditApplicationType(ApplicationTypeID);
            winEditApplicationType.ShowDialog(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load Application Types
            lblRecordsCount.Content = dgApplicationTypes.Items.Count;

        }

        private void dgPeople_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
