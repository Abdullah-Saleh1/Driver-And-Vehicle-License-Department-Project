using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace DVLD_PresentationLayer.Licenses.Detain_License
{
    public partial class winDetainLicenseApplication : Window
    {
        public winDetainLicenseApplication()
        {
            InitializeComponent();
            ctrlDriverLicenseInfoWithFilters1.OnLicenseSelected += _OnLicenseSelecteed;
        }

        public event Action OnLicenseDetained; 

        private clsLicense _License;

        private int _LicenseID = -1;
        private int _DetainID = -1;

        private void _OnLicenseSelecteed(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = ctrlDriverLicenseInfoWithFilters1.License;
            lblLicenseID.Content = _LicenseID.ToString(); 

            if (_LicenseID == -1 || _License == null) return;

            btnShowLicenseHistory.IsEnabled = true;



            if (_License == null)
            {
                MessageBox.Show("The License Is Not Found");
                btnShowLicenseInfo.IsEnabled = false;
                btnShowLicenseHistory.IsEnabled = false;
                return;
            }

            if (_License.IsDetained)
            {
                btnDetain.IsEnabled = false;

                MessageBox.Show($"The selected license is already detained.");
                return;
            }

            btnDetain.IsEnabled = true;
            txtFineFees.Focus(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblDetainDate.Content = DateTime.Now.Date.ToShortDateString();
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
            if (_DetainID == -1) return;

            winShowDriverLicenseInfo showDriverLicenseInfo = new winShowDriverLicenseInfo(_LicenseID);
            showDriverLicenseInfo.ShowDialog();
        }

        private void Detain(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                MessageBox.Show("Enter Fine Fees First."); 
                return; 
            }

            _DetainID = _License.Detain(Convert.ToSingle(txtFineFees.Text), clsGlobal.CurrentUser.UserID);


            if (_DetainID == -1)
            {
                MessageBox.Show("Error: License Is Not Detained");
                return;
            }

            lblDetainID.Content = _DetainID.ToString(); 

            MessageBox.Show($"License Detained Successfully with ID = {_DetainID}");

            btnShowLicenseInfo.IsEnabled = true;
            btnDetain.IsEnabled = false; 
            ctrlDriverLicenseInfoWithFilters1.FilterEnabled = false;

            OnLicenseDetained?.Invoke(); 
        }

        private void txtFineFees_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");

            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
