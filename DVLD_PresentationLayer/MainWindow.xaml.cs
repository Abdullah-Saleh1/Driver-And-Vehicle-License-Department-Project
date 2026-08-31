using DVLD_PresentationLayer.Login;
using DVLD_PresentationLayer.People;
using DVLD_PresentationLayer.Users;
using System;
using System.Windows;


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

        private void _LoadStatistics()
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadStatistics();
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

        private void btnAccountSettings_Click(object sender, RoutedEventArgs e)
        {
            btnAccountSettings.ContextMenu.PlacementTarget = btnAccountSettings;
            btnAccountSettings.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom; 

            btnAccountSettings.ContextMenu.IsOpen = true;
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {

            winLogin Login = new winLogin();

            Login.Show(); 

            this.Close(); 
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            winManageUsers ManageUsers = new winManageUsers();
            ManageUsers.ShowDialog(); 
        }
    }
}
