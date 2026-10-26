using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.InternationalLicenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace DVLD_PresentationLayer.Applications.InternationalApplications
{
    public partial class winNewInternationalLicenseApplication : Window
    {

        private int _LicenseID = -1;
        private clsLicense _License;

        private int _InternationaLicenseID = -1;

        public event Action OnInternationalLicenseIssued; 

        public winNewInternationalLicenseApplication()
        {
            InitializeComponent();

            ctrlDriverLicenseInfoWithFilters1.OnLicenseSelected += _OnLicenseSelected; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblApplicationDate.Content = DateTime.Now.Date.ToShortDateString();
            lblIssueDate.Content = DateTime.Now.Date.ToShortDateString();
            lblFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.NewInternationalLicense); 
            lblExpirationDate.Content = DateTime.Now.AddYears(1).Date.ToShortDateString();
            lblCreatedBy.Content = clsGlobal.CurrentUser.UserName; 
        }

        // Here you can access LicenseID from the event DataBack or from the exposed member in the UserControl
        private void _OnLicenseSelected(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = ctrlDriverLicenseInfoWithFilters1.License; 

            if (_LicenseID == -1 || _License == null) return;

            lblLocalLicenseID.Content = _LicenseID.ToString(); 

            btnShowLicenseHistory.IsEnabled = true; 

            if (_License.LicenseClassID != 3)
            {
                btnIssue.IsEnabled = false;
                MessageBox.Show("Selected license should be class 3, Selecet another one."); 
                return; 
            }


            if (!_License.IsActive && _License.IsExpired())
            {
                btnIssue.IsEnabled = false;

                MessageBox.Show("Your License is Not Active Or Expired.");
                return; 
            }

            _InternationaLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(_License.DriverID); 

            if (_InternationaLicenseID != -1)
            {
                btnIssue.IsEnabled = false;
                
                btnShowLicenseInfo.IsEnabled = true;
                

                MessageBox.Show("You Have Already an Active International License Associated with this License.");
                return;
            }

            btnIssue.IsEnabled = true; 

        }

        private void btnShowLicenseInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_InternationaLicenseID == -1) return;

            // 1. Make New Window: winShowInternationalDriverLicenseInfo

            winShowInternationalDriverLicenseInfo showInternationalDriverLicenseInfo = new winShowInternationalDriverLicenseInfo(_InternationaLicenseID);
            showInternationalDriverLicenseInfo.ShowDialog();
        }

        private void btnShowLicenseHistory_Click(object sender, RoutedEventArgs e)
        {
            int PersonID = _License.DriverInfo.PersonID; 

            winShowPersonLicenseHistory showPersonLicenseHistory = new winShowPersonLicenseHistory(PersonID);
            showPersonLicenseHistory.ShowDialog(); 
        }

        private void Issue(object sender, RoutedEventArgs e)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = _License.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            Application.Status = clsApplication.enStatus.Completed; 
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.NewInternationalLicense);
            Application.CreatedByUserID = clsGlobal.CurrentUser.UserID; 

            
            if (!Application.Save())
            {
                MessageBox.Show("Error: Application is not saved"); 
                return; 
            }

            lblApplicationID.Content = Application.ApplicationID.ToString(); 

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.ApplicationID = Application.ApplicationID;
            InternationalLicense.DriverID = _License.DriverID; 
            InternationalLicense.IssuedUsingLocalLicenseID = _LicenseID; 
            InternationalLicense.IssueDate = DateTime.Now; 
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1); 
            InternationalLicense.IsActive = true; 
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID; 

            if (InternationalLicense.Save())
            {
                _InternationaLicenseID = InternationalLicense.ID;
                btnShowLicenseInfo.IsEnabled = true;
                lblInternationalLicenseID.Content = InternationalLicense.ID.ToString(); 

                MessageBox.Show("License Issued Successfully");
                OnInternationalLicenseIssued?.Invoke(); 
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
