using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Applications.InternationalApplications;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.Detain_License;
using DVLD_PresentationLayer.Licenses.InternationalLicenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using DVLD_PresentationLayer.People;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Applications.ReleaseDetainedLicense
{
    public partial class winManageDetainedLicenses : Window
    {

        public event Action OnWidnowClosed; 
        public winManageDetainedLicenses()
        {
            InitializeComponent();
        }

        DataTable _DetainedLicensesData;

        private void _LoadDetainedLicenses()
        {
            _DetainedLicensesData = clsDetainedLicense.GetAllDetainedLicenses();
            dgDetainedLicenses.ItemsSource = _DetainedLicensesData.DefaultView;
            _UpdateRecordsCount();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _DetainedLicensesData.DefaultView.Count;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadDetainedLicenses();
        }


        private void _ClearFilters()
        {
            _DetainedLicensesData.DefaultView.RowFilter = "";
            _UpdateRecordsCount();
        }

        private void cbDetainedLicensesFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _ClearFilters();

            if (cbDetainedLicensesFilter.SelectedIndex == 2) // Is Released
            {
                txtSearch.Visibility = Visibility.Collapsed;
                cbReleasedFilter.Visibility = Visibility.Visible;

                return;
            }

            cbReleasedFilter.Visibility = Visibility.Collapsed;
            cbReleasedFilter.SelectedIndex = 0;


            txtSearch.Visibility = cbDetainedLicensesFilter.SelectedIndex == 0 ? Visibility.Collapsed : Visibility.Visible;

            if (txtSearch.Visibility == Visibility.Visible)
            {
                txtSearch.Text = "";
                txtSearch.Focus();
            }
        }

        private void cbReleasedFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbReleasedFilter.SelectedIndex == 0)
            {
                _DetainedLicensesData.DefaultView.RowFilter = "";
            }
            else if (cbReleasedFilter.SelectedIndex == 1)
            {
                _DetainedLicensesData.DefaultView.RowFilter = "Is Released = 1";
            }
            else
            {
                _DetainedLicensesData.DefaultView.RowFilter = "Is Released = 0";
            }

            _UpdateRecordsCount();
        }

        private void _ApplyFilter(string ColumnName)
        {
            _DetainedLicensesData.DefaultView.RowFilter = $"{ColumnName} = {txtSearch.Text}";

            _UpdateRecordsCount();
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtSearch.Text == "")
            {
                _ClearFilters();
                return;
            }


            string ColumnName = ((ComboBoxItem)cbDetainedLicensesFilter.SelectedItem).Content.ToString(); // Get the selected filter column


            _ApplyFilter(ColumnName);
        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string ColumnName = ((ComboBoxItem)cbDetainedLicensesFilter.SelectedItem).Content.ToString().Replace(" ", "");

            Regex regex; 
            if (ColumnName == "FullName")
            {
                regex = new Regex("[^a-zA-Z]+"); 
            } else
            {
                regex = new Regex("[^0-9]+"); // Only allow numbers
            }

            e.Handled = regex.IsMatch(e.Text);
        }


        private void dgDetainedLicenses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDetainedLicenses.SelectedIndex < 0) return;

            if (dgDetainedLicenses.SelectedItem == null) return;

            bool IsReleased = Convert.ToBoolean((dgDetainedLicenses.SelectedItem as DataRowView)["IsReleased"]);

            btnReleaseDetainedLicense.IsEnabled = !IsReleased;
        }

        // Events 

        private void ShowPersonDetails(object sender, RoutedEventArgs e)
        {

            string NationalNo = Convert.ToString((dgDetainedLicenses.SelectedItem as DataRowView)["N NO"]);

            int PersonID = clsPerson.Find(NationalNo).PersonID; 

            winShowPersonInfo showPersonInfo = new winShowPersonInfo(PersonID);
            showPersonInfo.OnDataUpdated += _LoadDetainedLicenses; 
            showPersonInfo.ShowDialog();
        }

        private void ShowLicenseDetails(object sender, RoutedEventArgs e)
        {
            int LicenseID = Convert.ToInt32((dgDetainedLicenses.SelectedItem as DataRowView)["L ID"]);

            winShowDriverLicenseInfo showDriverLicenseInfo = new winShowDriverLicenseInfo(LicenseID);
            showDriverLicenseInfo.ShowDialog();
        }
        private void dgInternationalDrivingLicenseApplicaions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowLicenseDetails(null, null);
        }

        private void ShowPersonLicenseHistory(object sender, RoutedEventArgs e)
        {
            string NationalNo = Convert.ToString((dgDetainedLicenses.SelectedItem as DataRowView)["N NO"]);

            int PersonID = clsPerson.Find(NationalNo).PersonID;

            winShowPersonLicenseHistory showPersonLicenseHistory = new winShowPersonLicenseHistory(PersonID);
            showPersonLicenseHistory.ShowDialog();
        }

        private void ReleaseDetainedLicense(object sender, RoutedEventArgs e)
        {
            int LicenseID = Convert.ToInt32((dgDetainedLicenses.SelectedItem as DataRowView)["L ID"]);

            winReleaseDetainedLicenseApplication releaseDetainedLicenseApplication = new winReleaseDetainedLicenseApplication(LicenseID);
            releaseDetainedLicenseApplication.OnLicenseReleased += _LoadDetainedLicenses;
            releaseDetainedLicenseApplication.ShowDialog(); 
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnWidnowClosed?.Invoke(); 
        }

        private void btnDetainLicense_Click(object sender, RoutedEventArgs e)
        {
            winDetainLicenseApplication detainLicenseApplication = new winDetainLicenseApplication();
            detainLicenseApplication.OnLicenseDetained += _LoadDetainedLicenses; 
            detainLicenseApplication.ShowDialog(); 
        }

        private void btnReleaseLicense_Click(object sender, RoutedEventArgs e)
        {
            winReleaseDetainedLicenseApplication releaseDetainedLicenseApplication = new winReleaseDetainedLicenseApplication();
            releaseDetainedLicenseApplication.OnLicenseReleased += _LoadDetainedLicenses; 
            releaseDetainedLicenseApplication.ShowDialog(); 
        }
    }
}
