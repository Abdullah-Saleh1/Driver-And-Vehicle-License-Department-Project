using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using DVLD_PresentationLayer.People;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Drivers
{
    public partial class winManageDrivers : Window
    {

        DataTable _DriversData; 

        public winManageDrivers()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadDrivers(); 
        }

        private void _LoadDrivers()
        {
            _DriversData = clsDriver.GetAllDrivers();
            dgDrivers.ItemsSource = _DriversData.DefaultView;

            _UpdateRecordsCount(); 
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _DriversData.DefaultView.Count;
        }


        // Filter Logic 

        private void cbDriversFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSearch.Visibility = (cbDriversFilter.SelectedIndex != 0) ? Visibility.Visible : Visibility.Collapsed;

            txtSearch.Text = string.Empty;
            txtSearch.Focus();
        }

        private void _ApplyFilter(string ColumnName, string Query)
        {
            // if the Columns is a Number
            if (ColumnName == "DriverID" || ColumnName == "PersonID" || ColumnName == "NationalNo")
            {
                _DriversData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') Like '%{Query}%'";
                //_PeopleData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') = '{Query}'";
            }
            else
            {
                _DriversData.DefaultView.RowFilter = $"{ColumnName} Like '%{Query}%'";
            }

            _UpdateRecordsCount(); 
        }
        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // If the search text is empty
            if (txtSearch.Text == string.Empty)
            {
                _DriversData.DefaultView.RowFilter = ""; // Clear the filter
                _UpdateRecordsCount();
                return;
            }

            string ColumnName = ((ComboBoxItem)cbDriversFilter.SelectedItem).Content.ToString().Replace(" ", "");

            _ApplyFilter(ColumnName, txtSearch.Text); 
        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string ColumnName = ((ComboBoxItem)cbDriversFilter.SelectedItem).Content.ToString().Replace(" ", "");
            Regex regex;
            if (ColumnName == "FullName")
            {
                regex = new Regex("[^a-zA-z]+"); // Only allow letters
            }
            else if (ColumnName == "NationalNo")
            {
                regex = new Regex("[^a-zA-Z0-9]+"); // Only allow letters and numbers
            }
            else
            {
                regex = new Regex("[^0-9]+"); // Only allow numbers
            }

            e.Handled = regex.IsMatch(e.Text);
        }


        // Menu Buttons


        private void ShowPersonInfo(object sender, RoutedEventArgs e)
        {
            int PersonID = Convert.ToInt32((dgDrivers.SelectedItem as DataRowView)["PersonID"]);

            winShowPersonInfo winPersonInfo = new winShowPersonInfo(PersonID);
            winPersonInfo.OnDataUpdated += _LoadDrivers; 
            winPersonInfo.ShowDialog(); 
        }

        private void dgDriver_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowPersonInfo(null, null); 
        }


        private void ShowPersonLicenseHistory(object sender, RoutedEventArgs e)
        {
            int PersonID = Convert.ToInt32((dgDrivers.SelectedItem as DataRowView)["PersonID"]);

            winShowPersonLicenseHistory showPersonLicenseHistory = new winShowPersonLicenseHistory(PersonID);
            showPersonLicenseHistory.ShowDialog(); 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
