
using DVLD_BuisinessLayer;
using System.Windows;

namespace DVLD_PresentationLayer.Licenses.InternationalLicenses
{
    public partial class winShowInternationalDriverLicenseInfo : Window
    {

        private int _InternationalLicenseID;
        private clsInternationalLicense _License; 

        public int LicenseID { get { return _InternationalLicenseID; } }
        public clsInternationalLicense License { get { return _License; } }
        public winShowInternationalDriverLicenseInfo(int InternationalLicenseID)
        {

            _InternationalLicenseID = InternationalLicenseID; 
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            ctrlDriverLicenseInfo1.LoadDriverLicenseInfo(_InternationalLicenseID); 
        }
    }
}
