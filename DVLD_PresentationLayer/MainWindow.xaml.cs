using DVLD_PresentationLayer.People;
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

namespace DVLD_PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btnPeople_Click(object sender, RoutedEventArgs e)
        {
            winManagePeople ManagePeople = new winManagePeople();
            ManagePeople.ShowDialog(); 
        }

        private void btnApplications_Click(object sender, RoutedEventArgs e)
        {
            if (btnApplications.ContextMenu != null)
            {
                // تحديد أن مكان ظهور المنيو يكون تحت الزرار بالظبط
                btnApplications.ContextMenu.PlacementTarget = btnApplications;
                btnApplications.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

                // فتح المنيو
                btnApplications.ContextMenu.IsOpen = true;
            }
        }

    }
}
