
using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.InternationalLicenses;
using DVLD_PresentationLayer.People;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DVLD_PresentationLayer.Applications.InternationalApplications
{
    public partial class winManageInternationalLicenseApplications : Window
    {
        public winManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        DataTable _InternationalLicenseData;

        private void _LoadInternationalApplications()
        {
            _InternationalLicenseData = clsInternationalLicense.GetAllInternationalLicenses();
            dgInternationalDrivingLicenseApplicaions.ItemsSource = _InternationalLicenseData.DefaultView;
            _UpdateRecordsCount();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _InternationalLicenseData.DefaultView.Count;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadInternationalApplications(); 
        }


        private void _ClearFilters()
        {
            _InternationalLicenseData.DefaultView.RowFilter = "";
            _UpdateRecordsCount();
        }

        private void cbInternationalApplicationsFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _ClearFilters();

            if (cbInternationalApplicationsFilter.SelectedIndex == 5) // Is Active
            {
                txtSearch.Visibility = Visibility.Collapsed;
                cbActiveFilter.Visibility = Visibility.Visible;

                return;
            }

            cbActiveFilter.Visibility = Visibility.Collapsed;
            cbActiveFilter.SelectedIndex = 0;


            txtSearch.Visibility = cbInternationalApplicationsFilter.SelectedIndex == 0 ? Visibility.Collapsed : Visibility.Visible;

            if (txtSearch.Visibility == Visibility.Visible)
            {
                txtSearch.Text = "";
                txtSearch.Focus();
            }
        }

        private void cbActiveFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbActiveFilter.SelectedIndex == 0)
            {
                _InternationalLicenseData.DefaultView.RowFilter = "";
            }
            else if (cbActiveFilter.SelectedIndex == 1)
            {
                _InternationalLicenseData.DefaultView.RowFilter = "IsActive = 1";
            }
            else
            {
                _InternationalLicenseData.DefaultView.RowFilter = "IsActive = 0";
            }

            _UpdateRecordsCount();
        }

        private void _ApplyFilter(string ColumnName)
        {
            _InternationalLicenseData.DefaultView.RowFilter = $"{ColumnName} = {txtSearch.Text}";

            _UpdateRecordsCount();
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtSearch.Text == "")
            {
                _ClearFilters();
                return;
            }


            string ColumnName = ((ComboBoxItem)cbInternationalApplicationsFilter.SelectedItem).Content.ToString().Replace(" ", ""); // Get the selected filter column


            _ApplyFilter(ColumnName);
        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string ColumnName = ((ComboBoxItem)cbInternationalApplicationsFilter.SelectedItem).Content.ToString().Replace(" ", "");

            Regex regex = new Regex("[^0-9]+"); // Only allow numbers

            e.Handled = regex.IsMatch(e.Text);
        }



        // Events 
        private void btnAddInternationalApplication_Click(object sender, RoutedEventArgs e)
        {
            winNewInternationalLicenseApplication newInternationalLicenseApplication = new winNewInternationalLicenseApplication();
            newInternationalLicenseApplication.OnInternationalLicenseIssued += _LoadInternationalApplications; 
            newInternationalLicenseApplication.ShowDialog();
        }

        private void ShowPersonDetails(object sender, RoutedEventArgs e)
        {
            int DriverID = Convert.ToInt32((dgInternationalDrivingLicenseApplicaions.SelectedItem as DataRowView)["DriverID"]);

            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;


            winShowPersonInfo showPersonInfo = new winShowPersonInfo(PersonID);
            showPersonInfo.ShowDialog(); 
        }

        private void ShowLicenseDetails(object sender, RoutedEventArgs e)
        {
            int InternationalLicenseID = Convert.ToInt32((dgInternationalDrivingLicenseApplicaions.SelectedItem as DataRowView)["IntLicenseID"]);

            winShowInternationalDriverLicenseInfo showInternationalDriverLicenseInfo = new winShowInternationalDriverLicenseInfo(InternationalLicenseID);
            showInternationalDriverLicenseInfo.ShowDialog(); 
        }

        private void ShowPersonLicenseHistory(object sender, RoutedEventArgs e)
        {
            int DriverID = Convert.ToInt32((dgInternationalDrivingLicenseApplicaions.SelectedItem as DataRowView)["DriverID"]);

            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;

            winShowPersonLicenseHistory showPersonLicenseHistory = new winShowPersonLicenseHistory(PersonID);
            showPersonLicenseHistory.ShowDialog(); 
        }

        private void dgInternationalDrivingLicenseApplicaions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowLicenseDetails(null, null); 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
