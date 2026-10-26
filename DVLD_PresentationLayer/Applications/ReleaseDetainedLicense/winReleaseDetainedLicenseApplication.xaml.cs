using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.Windows;

namespace DVLD_PresentationLayer.Applications.ReleaseDetainedLicense
{
    public partial class winReleaseDetainedLicenseApplication : Window
    {
        public winReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
            ctrlDriverLicenseInfoWithFilters1.OnLicenseSelected += _OnLicenseSelecteed;
        }

        public winReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();

            ctrlDriverLicenseInfoWithFilters1.OnLicenseSelected += _OnLicenseSelecteed;
            ctrlDriverLicenseInfoWithFilters1.LoadLicenseInfo(LicenseID);
            ctrlDriverLicenseInfoWithFilters1.FilterEnabled = false; 

        }

        public event Action OnLicenseReleased; 

        private clsLicense _License;

        private int _LicenseID = -1;
        clsDetainedLicense _Detain;

        private void _ClearFields()
        {
            lblDetainID.Content = "";
            lblLicenseID.Content = "";
            lblDetainDate.Content = "";
            lblCreatedBy.Content = "";
            lblApplicationFees.Content = "";
            lblFineFees.Content = "";
            lblTotalFees.Content = "";
        }

        private void _OnLicenseSelecteed(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = ctrlDriverLicenseInfoWithFilters1.License;


            if (_LicenseID == -1 || _License == null) return;

            btnShowLicenseHistory.IsEnabled = true;


            if (_License == null)
            {
                _ClearFields(); 
                MessageBox.Show("The License Is Not Found");
                btnShowLicenseInfo.IsEnabled = false;
                btnShowLicenseHistory.IsEnabled = false;
                return;
            }

            if (!_License.IsDetained)
            {
                _ClearFields(); 
                btnShowLicenseHistory.IsEnabled = true;
                btnRelease.IsEnabled = false;

                MessageBox.Show($"The selected license is not detained, Choose another one.");
                return;
            }

            _Detain = clsDetainedLicense.FindByLicenseID(_LicenseID); 


            lblDetainID.Content = _Detain.DetainID.ToString();
            lblLicenseID.Content = _LicenseID.ToString();

            lblDetainDate.Content = _Detain.DetainDate.Date.ToShortDateString();
            lblCreatedBy.Content = _Detain.CreatedByUserInfo.UserName; 

            lblApplicationFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense);
            lblFineFees.Content = _Detain.FineFees.ToString();

            lblTotalFees.Content = (Convert.ToSingle(lblFineFees.Content) + Convert.ToSingle(lblApplicationFees.Content)).ToString();



            btnRelease.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // lblCreatedBy.Content = clsGlobal.CurrentUser.UserName;
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
            if (_LicenseID == -1) return;

            winShowDriverLicenseInfo showDriverLicenseInfo = new winShowDriverLicenseInfo(_LicenseID);
            showDriverLicenseInfo.ShowDialog();
        }

        private void Release(object sender, RoutedEventArgs e)
        {

            int ApplicationID = -1; 
            bool IsReleased = _License.ReleaseDetainedLicense(_Detain.DetainID, clsGlobal.CurrentUser.UserID, ref ApplicationID);


            if (!IsReleased)
            {
                MessageBox.Show("Error: License Is Not Released");
                return;
            }

            

            lblApplicationID.Content = ApplicationID.ToString(); 

            MessageBox.Show("License Issued Successfully");

            btnShowLicenseInfo.IsEnabled = true;
            btnRelease.IsEnabled = false;
            ctrlDriverLicenseInfoWithFilters1.FilterEnabled = false;

            OnLicenseReleased?.Invoke(); 
        }
    }
}
