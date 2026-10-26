using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DVLD_PresentationLayer.Licenses.InternationalLicenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {

        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense; 

        public int InternationalLicenseID { get { return _InternationalLicenseID; } }
        public clsInternationalLicense InternationalLicense { get { return _InternationalLicense; } }
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void LoadDriverLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);

            if (_InternationalLicense != null)
            {
                _FillLicenseInfo();
            }
            else
            {
                MessageBox.Show("License not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _LoadPersonImage(clsPerson Person)
        {
            if (!string.IsNullOrEmpty(Person.ImagePath) && File.Exists(Person.ImagePath))
            {
                imgPerson.Source = util._LoadImageSafely(Person.ImagePath);
                return;
            }

            if (Person.GenderText == clsPerson.enGender.Male)
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Male 512.png", UriKind.Relative));
            }
            else
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Female 512.png", UriKind.Relative));
            }
        }
        private void _FillLicenseInfo()
        {
            lblName.Content = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblInternationalLicenseID.Content = _InternationalLicenseID.ToString(); 
            lblLicenseID.Content = _InternationalLicense.IssuedUsingLocalLicenseID.ToString(); 
            lblNationalNo.Content = _InternationalLicense.DriverInfo.PersonInfo.NationalNO;
            lblGender.Content = _InternationalLicense.DriverInfo.PersonInfo.GenderText;
            lblIssueDate.Content = _InternationalLicense.IssueDate.ToShortDateString();

            lblApplicationID.Content = _InternationalLicense.ApplicationID.ToString(); 
            lblIsActive.Content = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Content = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Content = _InternationalLicense.DriverID;
            lblExpirationDate.Content = _InternationalLicense.ExpirationDate.ToShortDateString();

            _LoadPersonImage(_InternationalLicense.DriverInfo.PersonInfo);

        }
    }
}
