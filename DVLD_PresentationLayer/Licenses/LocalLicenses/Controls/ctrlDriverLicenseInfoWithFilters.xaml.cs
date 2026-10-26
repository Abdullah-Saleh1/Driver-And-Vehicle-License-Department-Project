using DVLD_BuisinessLayer;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Licenses.LocalLicenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilters : UserControl
    {
        public ctrlDriverLicenseInfoWithFilters()
        {
            InitializeComponent();
        }

        public event Action<int> OnLicenseSelected;


        private int _LicenseID = -1; 

        // Expose Members

        public int LicenseID { get { return ctrlDriverLicenseInfo1.LicenseID; } }
        public clsLicense License { get { return ctrlDriverLicenseInfo1.License; } }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            set
            {
                _FilterEnabled = value;
                spFilters.IsEnabled = _FilterEnabled;
            }

            get { return _FilterEnabled; }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text))
            {
                MessageBox.Show("License Not Found.");
                return;
            }

            _LicenseID = Convert.ToInt32(txtFilterValue.Text); 

            ctrlDriverLicenseInfo1.LoadDriverLicenseInfo(_LicenseID);
            OnLicenseSelected?.Invoke(_LicenseID); 
        }

        private void txtFilterValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // Only allow numbers

            e.Handled = regex.IsMatch(e.Text);
        }

        public void txtLicenseIDFocus()
        {
            txtFilterValue.Focus(); 
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtFilterValue.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadDriverLicenseInfo(LicenseID);
            _LicenseID = LicenseID;

            // Only Rais the event when the Filter is not Enabled 
            // so when the License selected from the filter 
            if (FilterEnabled) OnLicenseSelected?.Invoke(LicenseID);
        }
    }
}
