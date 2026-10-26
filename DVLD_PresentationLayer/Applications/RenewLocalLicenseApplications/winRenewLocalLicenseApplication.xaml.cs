using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.ComponentModel;
using System.Windows;

namespace DVLD_PresentationLayer.Applications.RenewLocalLicenseApplications
{
    public partial class winRenewLocalLicenseApplication : Window
    {
        public winRenewLocalLicenseApplication()
        {
            InitializeComponent();

            ctrlDriverLicenseInfoWithFilters1.OnLicenseSelected += _OnLicenseSelecteed;
        }

        private clsLicense _License;

        private int _LicenseID = -1; 
        private int _NewLicenseID; 

        private void _OnLicenseSelecteed(int LicenseID)
        {
            _LicenseID = LicenseID; 
            _License = ctrlDriverLicenseInfoWithFilters1.License;

            if (_LicenseID == -1 || _License == null) return;

            btnShowLicenseHistory.IsEnabled = true;

            lblOldLicenseID.Content = _LicenseID.ToString();
            lblExpirationDate.Content = DateTime.Now.AddYears(_License.LicenseClassInfo.DefaultValidityLength);
            lblLicenseFees.Content = _License.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Content = Convert.ToInt32(lblApplicationFees.Content) + Convert.ToInt32(lblLicenseFees.Content); 


            if (_License == null)
            {
                MessageBox.Show("The License Is Not Found");
                btnShowLicenseInfo.IsEnabled = false;
                btnShowLicenseHistory.IsEnabled = false; 
                return; 
            }

            if (!_License.IsExpired())
            {
                btnShowLicenseHistory.IsEnabled = true; 
                btnRenew.IsEnabled = false;

                MessageBox.Show($"The selected license is not yet Expired. It will be expired on {_License.ExpirationDate.Date.ToShortDateString()}");
                return; 
            }

            if (!_License.IsActive)
            {
                btnShowLicenseHistory.IsEnabled = true;
                btnRenew.IsEnabled = false;

                MessageBox.Show($"The selected license is not Active ");
                return;
            }

            btnRenew.IsEnabled = true; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblApplicationDate.Content = DateTime.Now.Date.ToShortDateString();
            lblIssueDate.Content = DateTime.Now.Date.ToShortDateString();
            lblApplicationFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.RenewDrivingLicense);
            lblCreatedBy.Content = clsGlobal.CurrentUser.UserName;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void btnShowLicenseHistory_Click(object sender, RoutedEventArgs e)
        {

            winShowPersonLicenseHistory showPersonLicenseHistory = new winShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            showPersonLicenseHistory.ShowDialog(); 
        }

        private void btnShowLicenseInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_NewLicenseID == -1) return;

            winShowDriverLicenseInfo showDriverLicenseInfo = new winShowDriverLicenseInfo(_NewLicenseID);
            showDriverLicenseInfo.ShowDialog(); 
        }


        private void Renew(object sender, RoutedEventArgs e)
        {

            clsLicense License = _License.Renew(lblNotes.Text, clsGlobal.CurrentUser.UserID); 


            if (License == null)
            {
                MessageBox.Show("Error: License Is Not Renewed");
                return;
            }

            lblRLApplicationID.Content = License.ApplicationID.ToString();
            lblRenewedLicenseID.Content = License.LicenseID.ToString();
            _NewLicenseID = License.LicenseID;

            MessageBox.Show("License Issued Successfully");

            btnShowLicenseInfo.IsEnabled = true;
            btnRenew.IsEnabled = false;
            ctrlDriverLicenseInfoWithFilters1.FilterEnabled = false; 
        }
    }
}
