using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace DVLD_PresentationLayer.Applications.LocalApplications
{
    public partial class winLocalDrivingLicenseApplicationInfo : Window
    {

        int _LocalAppID = -1;

        public event Action OnDataUpdated; // Event to notify when data is updated
        public winLocalDrivingLicenseApplicationInfo(int LDLAppID)
        {
            InitializeComponent();

            ctrlLocalDrivingLicenseApplicationInfo1.OnDataUpdated += () =>
            {
                OnDataUpdated?.Invoke();
            };

            _LocalAppID = LDLAppID;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivinAppID(_LocalAppID);
        }
    }
}
