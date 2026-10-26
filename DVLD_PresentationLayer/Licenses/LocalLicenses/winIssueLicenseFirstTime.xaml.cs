using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Applications.Controls;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Windows;

namespace DVLD_PresentationLayer.Licenses.LocalLicenses
{
    public partial class winIssueLicenseFirstTime : Window
    {

        private int _LocaAppID;
        private clsLocalDrivingLicenseApplication _LocalApp; 
        public winIssueLicenseFirstTime(int LocalAppID)
        {
            InitializeComponent();

            _LocaAppID = LocalAppID; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocaAppID);

            if (_LocalApp == null)
            {
                MessageBox.Show("There Is No Application For this");
                this.Close();
                return;
            }

            if (!_LocalApp.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Tests First");
                this.Close();
                return; 
            }

            int LicenseID = _LocalApp.GetActiveLicneseID(); 
            if (LicenseID != -1)
            {
                MessageBox.Show("Person Has Already An Active License For this Class");
                this.Close();
                return;
            }


            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivinAppID(_LocaAppID); 
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void btnIssue_Click(object sender, RoutedEventArgs e)
        {

            int LicneseID = _LocalApp.IssueLicenseForTheFirstTime(txtNotes.Text, clsGlobal.CurrentUser.UserID); 

            if (LicneseID != -1)
            {
                MessageBox.Show($"License Issued Successfully with LicenseID = {LicneseID}");

                ctrlLocalDrivingLicenseApplicationInfo1.LicenseID = LicneseID; 
                ctrlLocalDrivingLicenseApplicationInfo1.btnShowLicenseInfo.IsEnabled = true; 
            } else
            {
                MessageBox.Show("Error: the License is not Issued"); 
            }


        }
    }
}
