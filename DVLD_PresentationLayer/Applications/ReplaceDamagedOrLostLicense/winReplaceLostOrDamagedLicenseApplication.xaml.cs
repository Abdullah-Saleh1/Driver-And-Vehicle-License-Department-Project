using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DVLD_PresentationLayer.Applications.ReplaceDamagedOrLostLicense
{
    public partial class winReplaceLostOrDamagedLicenseApplication : Window
    {
        public winReplaceLostOrDamagedLicenseApplication()
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


            if (_License == null)
            {
                MessageBox.Show("The License Is Not Found");
                btnShowLicenseInfo.IsEnabled = false;
                btnShowLicenseHistory.IsEnabled = false;
                return;
            }

            if (!_License.IsActive)
            {
                btnShowLicenseHistory.IsEnabled = true;
                btnIssue.IsEnabled = false;

                MessageBox.Show($"The selected license is not Active ");
                return;
            }

            btnIssue.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblApplicationDate.Content = DateTime.Now.Date.ToShortDateString();
            lblFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense);
            lblCreatedBy.Content = clsGlobal.CurrentUser.UserName;
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


        private void ReplacementForChecked(object sender, RoutedEventArgs e)
        {

            if (lblFees == null || lblTitle == null) return; 

            if (rbDamanged.IsChecked == true)
            {
                lblTitle.Content = "Replacement For Damaged License";

                lblFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense);

            } else
            {
                lblTitle.Content = "Replacement For Lost License";

                lblFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense);
            }
        }

        private void Issue(object sender, RoutedEventArgs e)
        {

            clsLicense License; 

            if (rbDamanged.IsChecked == true)
            {
                License = _License.Replace(clsApplication.enApplicationType.ReplaceDamagedDrivingLicense, clsGlobal.CurrentUser.UserID);
            } else
            {
                License = _License.Replace(clsApplication.enApplicationType.ReplaceLostDrivingLicense, clsGlobal.CurrentUser.UserID);
            }


            if (License == null)
            {
                MessageBox.Show("Error: License Is Not Renewed");
                return;
            }

            lblApplicationID.Content = License.ApplicationID.ToString();
            lblReplacedLicenseID.Content = License.LicenseID.ToString();
            _NewLicenseID = License.LicenseID;

            MessageBox.Show("License Issued Successfully");

            btnShowLicenseInfo.IsEnabled = true;
            btnIssue.IsEnabled = false;
            ctrlDriverLicenseInfoWithFilters1.FilterEnabled = false;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
