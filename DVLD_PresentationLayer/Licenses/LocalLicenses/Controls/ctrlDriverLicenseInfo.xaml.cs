using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DVLD_PresentationLayer.Licenses.LocalLicenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        private int _LicenseID; 
        public int LicenseID { get { return _LicenseID; } }

        // Composition Of an object inside a User Controls
        private clsLicense _License; 
        public clsLicense License {
            get { return _License; }
        }

        public void LoadDriverLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID; 
            _License = clsLicense.Find(LicenseID);

            if (_License != null)
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
            lblClass.Content = _License.LicenseClassInfo.ClassName;
            lblName.Content = _License.DriverInfo.PersonInfo.FullName;
            lblNational.Content = _License.DriverInfo.PersonInfo.NationalNO;
            lblGender.Content = _License.DriverInfo.PersonInfo.GenderText;
            lblIssueDate.Content = _License.IssueDate.ToShortDateString();
            lblIssueReason.Content = _License.IssueReasonText; 
            lblNotes.Content = _License.Notes; 
            lblIsActive.Content = _License.IsActive ? "Yes" : "No"; 

            lblDateOfBirth.Content = _License.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString(); 
            lblExpirationDate.Content = _License.ExpirationDate.ToShortDateString();
            lblDriverID.Content = _License.DriverID; 
            lblIsDetained.Content = _License.IsDetained ? "Yes" : "No";

            _LoadPersonImage(_License.DriverInfo.PersonInfo); 

        }
    }
}
