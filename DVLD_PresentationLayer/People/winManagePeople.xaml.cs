using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.People.Controls;
using System;
using System.Data;
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void _LoadPeople()
        {
            _AllPeopleData = clsPeople.GetAllPeople();
            // ToTable Makes a new DataTable with the same data as the original DataTable, but with only the specified columns.

            // Two cons of this approach: 
            // 1. It creates a new DataTable, which can consume more memory if the original DataTable is large.
            // 2. If the original DataTable is updated, the new DataTable will not reflect those changes, as it is a separate copy. 

            // Convert this approach to a more efficient one by 
            // changing the sql query to get the specified columns instead of make two DataTables in memory.
            _PeopleData = _AllPeopleData.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName", "GenderCaption", "DateOfBirth", "CountryName", "Phone", "Email");
            _PeopleData.Columns["CountryName"].ColumnName = "Nationality";
            _PeopleData.Columns["GenderCaption"].ColumnName = "Gender"; 


            dgPeople.ItemsSource = _PeopleData.DefaultView; 
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

        void _ReloadWindowData()
        {
            _LoadPeople();
            _UpdateRecordsCount();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ReloadWindowData();
            JustifyColumnsWidth();
        }

        // Context menu event handlers
        private void ShowDetails(object sender, RoutedEventArgs e)
        {
            if (dgPeople.SelectedItem == null)
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                
            int PersonID = Convert.ToInt32((dgPeople.SelectedItem as DataRowView)["PersonID"]);

            winShowPersonInfo showPersonInfo = new winShowPersonInfo(PersonID);
            showPersonInfo.ShowDialog();
        }

        private void DeletePerson(object sender, RoutedEventArgs e)
        {
            if (dgPeople.SelectedItem == null)
            {
                MessageBox.Show("Please select a person to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int PersonID = Convert.ToInt32((dgPeople.SelectedItem as DataRowView)["PersonID"]);
            MessageBox.Show(PersonID.ToString()); 
            
            //if (clsPeople.Delete(PersonID))
            //{
            //    MessageBox.Show("Person deleted successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Failed to delete person.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            _ReloadWindowData(); 
        }

        private void NotReady(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!", "Not Ready!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        // Filter and Search Functionality
        private void cbPeopleFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbPeopleFilter.SelectedIndex == 0) {
                // Show all People and Remove SearchBar
                txtSearch.Visibility = Visibility.Collapsed;
                _PeopleData.DefaultView.RowFilter = string.Empty;
                _UpdateRecordsCount(); 

                return;
            }

            {
                // Show SearchBar 
                txtSearch.Visibility = Visibility.Visible;

                _ClearSearch(); 
            }
        }

        private void _ClearSearch()
        {
            txtSearch.Text = string.Empty;
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            // If the search text is empty
            if (txtSearch.Text == string.Empty)
            {
                _PeopleData.DefaultView.RowFilter = string.Empty;
                _UpdateRecordsCount();
                return;
            }


            string ColumnName = ((ComboBoxItem)cbPeopleFilter.SelectedItem).Content.ToString().Replace(" ", ""); // Get the selected filter column

            // if the Columns is a Number
            if (ColumnName == "PersonID" || ColumnName == "NationalNo" || ColumnName == "Phone")
            {
                _PeopleData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') Like '%{txtSearch.Text}%'";
            }
            else
            {
                _PeopleData.DefaultView.RowFilter = $"{ColumnName} Like '%{txtSearch.Text}%'";
            }

            _UpdateRecordsCount();
        }


        private void btnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            winAddEditPerson AddEditPerson = new winAddEditPerson(); 

            AddEditPerson.ShowDialog();
        }
    }
}
