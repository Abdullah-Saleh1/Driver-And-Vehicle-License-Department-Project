using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.People;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Converters;


namespace DVLD_PresentationLayer.Applications.Controls
{
    public partial class ctrlLocalDrivinLicenseApplicationInfo : UserControl
    {

        public event Action OnDataUpdated; // Event to notify when data is updated

        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalApp;
        private int _LicenseID;


        // Expose the LocalDrivingLicenseApplicationID as a public property
        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }
        public ctrlLocalDrivinLicenseApplicationInfo()
        {
            InitializeComponent();
        }


        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            _LocalApp = LocalApp;

            if (LocalApp == null)
            {
                MessageBox.Show("Local Driving License Application not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _FillLocalDrivingApplicationInfo(); 
        }
        public void LoadApplicationInfoByLocalDrivinAppID(int LDLAppID)
        {
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LDLAppID);
            _LocalApp = LocalApp;

            if (LocalApp == null)
            {
                MessageBox.Show("Local Driving License Application not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _FillLocalDrivingApplicationInfo(); 
        }


        private void _FillLocalDrivingApplicationInfo()
        {
            _LicenseID = _LocalApp.GetActiveLicenseID(); 

            btnShowLicenseInfo.IsEnabled = _LicenseID != -1;

            _LoadLocalDrivingLicenseApplicationInfo();
            _LoadBaseApplicationInfo(); 
        }

        private void _LoadLocalDrivingLicenseApplicationInfo()
        {
            lblLocalDrivingLicenseAppID.Content = _LocalApp.LocalAppID.ToString();
            lblAppliedForLicense.Content = _LocalApp.LicenseClassInfo.ClassName;
            lblPassedTests.Content = _LocalApp.GetPassedTestCount().ToString() + "/3";
        }

        private void _LoadBaseApplicationInfo()
        {
            lblApplicationID.Content = _LocalApp.ApplicationID.ToString();
            lblStatus.Content = _LocalApp.StatusText.ToString(); 
            lblFees.Content = _LocalApp.PaidFees.ToString("C2");
            lblType.Content = _LocalApp.ApplicationTypeInfo.Title.ToString();
            lblApplicant.Content = _LocalApp.PersonFullName.ToString();
            lblDate.Content = _LocalApp.ApplicationDate.ToString("dd/MM/yyyy");
            lblStatusDate.Content = _LocalApp.LastStatusDate.ToString("dd/MM/yyyy");
            lblCreatedByUser.Content = _LocalApp.CreatedByUserInfo.UserName; 
        }

        private void btnShowLicenseInfo_Click(object sender, RoutedEventArgs e)
        {
            //winShowLicenseInfo winLicenseInfo = new winShowLicenseInfo(_LicenseID);
            //winLicenseInfo.ShowDialog();
        }

        private void btnViewPersonInfo_Click(object sender, RoutedEventArgs e)
        {
            winShowPersonInfo winPersonInfo = new winShowPersonInfo(_LocalApp.ApplicantPersonID);
            winPersonInfo.OnDataUpdated += () =>
            {
                // Refresh the person info in the control after the window is closed
                _LoadBaseApplicationInfo();
            };

            winPersonInfo.OnDataUpdated += () =>
            {
                this.OnDataUpdated?.Invoke();
            };

            winPersonInfo.ShowDialog();
        }
    }
}
