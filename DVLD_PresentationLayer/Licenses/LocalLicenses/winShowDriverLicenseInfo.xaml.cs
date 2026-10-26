using System;

using System.Windows;

namespace DVLD_PresentationLayer.Licenses.LocalLicenses
{
    public partial class winShowDriverLicenseInfo : Window
    {

        private int _LicenseID; 
        public winShowDriverLicenseInfo(int LicenseID)
        {
            InitializeComponent();

            _LicenseID = LicenseID; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadDriverLicenseInfo(_LicenseID); 

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
