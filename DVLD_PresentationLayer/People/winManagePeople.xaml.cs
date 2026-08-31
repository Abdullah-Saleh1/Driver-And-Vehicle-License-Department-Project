using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.People.Controls;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace DVLD_PresentationLayer.People
{
    /// <summary>
    /// Interaction logic for winManagePeople.xaml
    /// </summary>
    public partial class winManagePeople : Window
    {
        public winManagePeople()
        {
            InitializeComponent();
        }

        private DataTable _AllPeopleData;
        private DataTable _PeopleData; 

        private void _LoadPeople()
        {
            _AllPeopleData = clsPerson.GetAllPeople();
            // ToTable Makes a new DataTable with the same data as the original DataTable, but with only the specified columns.

            // Two cons of this approach: 
            // 1. It creates a new DataTable, which can consume more memory if the original DataTable is large.
            // 2. If the original DataTable is updated, the new DataTable will not reflect those changes, as it is a separate copy. 

            // Convert this approach to a more efficient one by 
            // changing the sql query to get the specified columns instead of make two DataTables in memory.
            _PeopleData = _AllPeopleData.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName", "GenderCaption", "DateOfBirth", "CountryName", "Phone", "Email");
            _PeopleData.Columns["CountryName"].ColumnName = "Nationality";
            _PeopleData.Columns["GenderCaption"].ColumnName = "Gender"; 

            _PeopleData.Columns["FirstName"].ColumnName = "First Name";
            _PeopleData.Columns["SecondName"].ColumnName = "Second Name";
            _PeopleData.Columns["ThirdName"].ColumnName = "Third Name";
            _PeopleData.Columns["LastName"].ColumnName = "Last Name";
            _PeopleData.Columns["DateOfBirth"].ColumnName = "Date Of Birth";


            dgPeople.ItemsSource = _PeopleData.DefaultView;

            _UpdateRecordsCount(); 
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _PeopleData.DefaultView.Count;
        }

        private void JustifyColumnsWidth()
        {
            dgPeople.Columns[0].Width = 80;
            dgPeople.Columns[1].Width = 100;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadPeople();
            JustifyColumnsWidth();
        }

        // Context menu event handlers
        private bool _IsPersonSelected()
        {
            return dgPeople.SelectedItems.Count > 0;
        }
        private void ShowDetails(object sender, RoutedEventArgs e)
        {
            if (!_IsPersonSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int PersonID = Convert.ToInt32((dgPeople.SelectedItem as DataRowView)["PersonID"]);

            winShowPersonInfo showPersonInfo = new winShowPersonInfo(PersonID);

            showPersonInfo.OnDataUpdated += _LoadPeople; // Subscribe to the event
            // Only Refresh the data if the user updated the data in the child window.

            showPersonInfo.ShowDialog();

            //winFindPerson showPersonInfo = new winFindPerson(PersonID);
            //showPersonInfo.ShowDialog(); 
        }

        private void dgPeople_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowDetails(sender, e); 
        }

        

        private void _RefreshData(object sender, int PersonID)
        {
            _LoadPeople();
        }

        private void btnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            winAddEditPerson AddEditPerson = new winAddEditPerson();

            AddEditPerson.DataBack += _RefreshData; // Subscribe to the event

            AddEditPerson.ShowDialog();
        }

        private void EditPerson(object sender, RoutedEventArgs e)
        {
            if (!_IsPersonSelected())
            {
                MessageBox.Show("Please select a person to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int PersonID = Convert.ToInt32((dgPeople.SelectedItem as DataRowView)["PersonID"]);

            winAddEditPerson AddEditPerson = new winAddEditPerson(PersonID);

            AddEditPerson.DataBack += _RefreshData; // Subscribe to the event

            AddEditPerson.ShowDialog();
        }

        private void DeletePerson(object sender, RoutedEventArgs e)
        {
            if (!_IsPersonSelected())
            {
                MessageBox.Show("Please select a person to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected person?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return; 

            int PersonID = Convert.ToInt32((dgPeople.SelectedItem as DataRowView)["PersonID"]);

            if (clsPerson.Delete(PersonID))
            {
                MessageBox.Show("Person deleted successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to delete person.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _LoadPeople(); 
        }

        private void NotReady(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!", "Not Ready!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }



        // Filter and Search Functionality
        private void cbPeopleFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            txtSearch.Visibility = (cbPeopleFilter.SelectedIndex != 0) ? Visibility.Visible : Visibility.Collapsed;

            // when clearing the search the filter should be cleared as well
            if (txtSearch.Visibility == Visibility.Visible)
            {
                txtSearch.Text = string.Empty; 
                txtSearch.Focus();
            }
        }

        private void _ApplyFilter(string ColumnName, string Query)
        {

            // if the Columns is a Number
            if (ColumnName == "PersonID" || ColumnName == "NationalNo" || ColumnName == "Phone")
            {
                _PeopleData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') Like '%{Query}%'";
                //_PeopleData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') = '{Query}'";
            }
            else
            {
                _PeopleData.DefaultView.RowFilter = $"{ColumnName} Like '%{Query}%'";
            }
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            // If the search text is empty
            if (txtSearch.Text == string.Empty)
            {
                _PeopleData.DefaultView.RowFilter = ""; // Clear the filter
                _UpdateRecordsCount();
                return;
            }


            string ColumnName = ((ComboBoxItem)cbPeopleFilter.SelectedItem).Content.ToString().Replace(" ", ""); // Get the selected filter column

            _ApplyFilter(ColumnName, txtSearch.Text); 

            _UpdateRecordsCount();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string ColumnName = ((ComboBoxItem)cbPeopleFilter.SelectedItem).Content.ToString().Replace(" ", "");
            Regex regex;
            if (ColumnName == "PersonID" || ColumnName == "Phone")
            {
                regex = new Regex("[^0-9]+"); // Only allow numbers
            }
            else
            {
                regex = new Regex("[^a-zA-Z0-9]+"); // Only allow letters and numbers
            }

            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
