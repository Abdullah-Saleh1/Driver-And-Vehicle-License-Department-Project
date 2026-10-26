using DVLD_PresentationLayer.Applications.ApplicationTypes;
using DVLD_PresentationLayer.Applications.InternationalApplications;
using DVLD_PresentationLayer.Applications.LocalApplications;
using DVLD_PresentationLayer.Applications.ReleaseDetainedLicense;
using DVLD_PresentationLayer.Applications.RenewLocalLicenseApplications;
using DVLD_PresentationLayer.Applications.ReplaceDamagedOrLostLicense;
using DVLD_PresentationLayer.Drivers;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses.Detain_License;
using DVLD_PresentationLayer.Login;
using DVLD_PresentationLayer.People;
using DVLD_PresentationLayer.Tests.Test_Types;
using DVLD_PresentationLayer.Users;
using System;
using System.Windows;


namespace DVLD_PresentationLayer
{

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
            clsGlobal.CurrentUser = null; 
            winLogin Login = new winLogin();
            Login.Show(); 
            this.Close(); 
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            winManageUsers ManageUsers = new winManageUsers();
            ManageUsers.ShowDialog(); 
        }

        private void btnCurrentUserInfo_Click(object sender, RoutedEventArgs e)
        {
            winUserInfo UserInfo = new winUserInfo(clsGlobal.CurrentUser.UserID);

            UserInfo.ShowDialog(); 
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            winChangePassword ChangePassword = new winChangePassword(clsGlobal.CurrentUser.UserID);

            ChangePassword.ShowDialog(); 
        }

        private void btnManageApplicationTypes_Click(object sender, RoutedEventArgs e)
        {
            winManageApplicationTypes winManageApplicationTypes = new winManageApplicationTypes();
            winManageApplicationTypes.ShowDialog();
        }

        private void btnManageTestTypes_Click(object sender, RoutedEventArgs e)
        {
            winManageTestTypes winManageTestTypes = new winManageTestTypes();
            winManageTestTypes.ShowDialog(); 
        }

        private void btnManageLocalDrivingLicenseApplication_Click(object sender, RoutedEventArgs e)
        {
            winManageLocalDrivingLicenseApplications winLocalApplications = new winManageLocalDrivingLicenseApplications();
            winLocalApplications.ShowDialog(); 
        }

        private void btnCreateNewLicense_Click(object sender, RoutedEventArgs e)
        {
            btnNewLocalDrivingLisenceApplication_Click(null, null); 
        }

        private void btnNewLocalDrivingLisenceApplication_Click(object sender, RoutedEventArgs e)
        {
            winAddUpdateLocalDrivingLilcenseApplication winNewLocalApplication = new winAddUpdateLocalDrivingLilcenseApplication();
            winNewLocalApplication.ShowDialog();
        }

        private void btnRetakeTest_Click(object sender, RoutedEventArgs e)
        {
            winManageLocalDrivingLicenseApplications winManageLocalApps = new winManageLocalDrivingLicenseApplications();
            winManageLocalApps.ShowDialog(); 
        }

        private void btnDrivers_Click(object sender, RoutedEventArgs e)
        {
            winManageDrivers ManageDrivers = new winManageDrivers();
            ManageDrivers.ShowDialog(); 
        }

        private void btnInternational_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked!");
            winNewInternationalLicenseApplication winInternationalApplication = new winNewInternationalLicenseApplication();
            winInternationalApplication.ShowDialog();
        }

        private void btnNewInternationalApplication_Click(object sender, RoutedEventArgs e)
        {
            winNewInternationalLicenseApplication winInternationalApplication = new winNewInternationalLicenseApplication();
            winInternationalApplication.ShowDialog();
        }

        private void btnManageInternationalDrivingLicenseApplication_Click(object sender, RoutedEventArgs e)
        {
            winManageInternationalLicenseApplications internationalLicenseApplications = new winManageInternationalLicenseApplications();
            internationalLicenseApplications.ShowDialog(); 
        }

        private void btnRenewDrivingLicense_Click(object sender, RoutedEventArgs e)
        {
            winRenewLocalLicenseApplication renewLocalLicenseApplication = new winRenewLocalLicenseApplication();
            renewLocalLicenseApplication.ShowDialog(); 
        }

        private void btnReplaceLostOrDamangedLicense_Click(object sender, RoutedEventArgs e)
        {
            winReplaceLostOrDamagedLicenseApplication replaceLostOrDamagedLicenseApplication = new winReplaceLostOrDamagedLicenseApplication();
            replaceLostOrDamagedLicenseApplication.ShowDialog(); 
        }

        private void btnDetainLicense_Click(object sender, RoutedEventArgs e)
        {
            winDetainLicenseApplication detainLicenseApplication = new winDetainLicenseApplication();
            detainLicenseApplication.ShowDialog(); 
        }

        private void btnManageDetainedLicenses_Click(object sender, RoutedEventArgs e)
        {
            winManageDetainedLicenses detainedLicenses = new winManageDetainedLicenses();
            detainedLicenses.ShowDialog(); 
        }

        private void btnReleaseDetainedLicense_Click(object sender, RoutedEventArgs e)
        {
            winReleaseDetainedLicenseApplication releaseDetainedLicenseApplication = new winReleaseDetainedLicenseApplication();
            releaseDetainedLicenseApplication.ShowDialog(); 
        }

    }
}
