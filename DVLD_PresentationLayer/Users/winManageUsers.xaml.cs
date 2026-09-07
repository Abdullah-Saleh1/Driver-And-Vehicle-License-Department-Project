using DVLD_BuisinessLayer;
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Users
{
    public partial class winManageUsers : Window
    {
        public winManageUsers()
        {
            InitializeComponent();
        }

        private DataTable _UsersData;

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _UsersData.DefaultView.Count;
        }
        private void _LoadUsers()
        {
            _UsersData = clsUser.GetAllUsers(); 
            dgUsers.ItemsSource = _UsersData.DefaultView;

            _UpdateRecordsCount(); 
        }
        private void _JustifyColumns()
        {
            dgUsers.Columns[0].Width = 100; 
            dgUsers.Columns[1].Width = 100; 
            
            if (dgUsers.Columns.Count > 3 && dgUsers.Columns[3] is System.Windows.Controls.DataGridCheckBoxColumn col)
            {
                col.IsReadOnly = true; // منع التعديل من الـ DataGrid

                Style style = new Style(typeof(System.Windows.Controls.CheckBox));
                style.Setters.Add(new Setter(HorizontalAlignmentProperty, HorizontalAlignment.Left));
                style.Setters.Add(new Setter(MarginProperty, new Thickness(10, 0, 0, 0)));
                style.Setters.Add(new Setter(IsHitTestVisibleProperty, false)); // يتجاهل كلكات الماوس تماماً فيفضل واضح وشغال

                col.ElementStyle = style;
            }
        }

        private void NotReady(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Feature is Not Implemented Yet!", "Not Ready!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadUsers();
            _JustifyColumns(); 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private bool _IsUserSelected()
        {
            return dgUsers.SelectedItems.Count > 0;
        }
        private void dgUsers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // show details

            if (!_IsUserSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            winAddEditUser AddEditUser = new winAddEditUser();
            AddEditUser.ShowDialog(); 
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {

            if (!_IsUserSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int PersonID = Convert.ToInt32((dgUsers.SelectedItem as DataRowView)["PersonID"]);

            winAddEditUser AddEditUser = new winAddEditUser();
            AddEditUser.ShowDialog();
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (!_IsUserSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete the selected person?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            int UserID = Convert.ToInt32((dgUsers.SelectedItem as DataRowView)["UserID"]);

            if (clsUser.Delete(UserID))
            {
                MessageBox.Show("Person deleted successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to delete person.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _LoadUsers();
        }


        private void _ClearFilters()
        {
            _UsersData.DefaultView.RowFilter = "";
            _UpdateRecordsCount();
        }

        private void cbUsersFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            _ClearFilters(); 

            if (cbUsersFilter.SelectedIndex == 5)
            {
                txtSearch.Visibility = Visibility.Collapsed; 
                cbActiveFilter.Visibility = Visibility.Visible; 

                return; 
            }

            cbActiveFilter.Visibility = Visibility.Collapsed;
            cbActiveFilter.SelectedIndex = 0; 


            txtSearch.Visibility = cbUsersFilter.SelectedIndex == 0 ? Visibility.Collapsed : Visibility.Visible;

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
                _UsersData.DefaultView.RowFilter = ""; 
            } else if (cbActiveFilter.SelectedIndex == 1)
            {
                _UsersData.DefaultView.RowFilter = "IsActive = 1"; 
            } else
            {
                _UsersData.DefaultView.RowFilter = "IsActive = 0"; 
            }

            _UpdateRecordsCount(); 
        }

        private void _ApplyFilter(string ColumnName)
        {
            if (ColumnName == "UserID" || ColumnName == "PersonID")
            {
                // Number
                _UsersData.DefaultView.RowFilter = $"{ColumnName} = {txtSearch.Text}"; 

            } else
            {
                // String
                _UsersData.DefaultView.RowFilter = $"{ColumnName} Like '%{txtSearch.Text}%'"; 
            }
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                _ClearFilters(); 
                return;
            }


            string ColumnName = ((ComboBoxItem)cbUsersFilter.SelectedItem).Content.ToString().Replace(" ", ""); // Get the selected filter column
            _ApplyFilter(ColumnName);
            _UpdateRecordsCount(); 
        }

        private void ShowDetails(object sender, RoutedEventArgs e)
        {
            if (!_IsUserSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string ColumnName = ((ComboBoxItem)cbUsersFilter.SelectedItem).Content.ToString().Replace(" ", "");
            Regex regex;
            if (ColumnName == "UserID" || ColumnName == "PersonID")
            {
                regex = new Regex("[^0-9]+"); // Only allow numbers
            }
            else
            {
                regex = new Regex("[^a-zA-Z0-9]+"); // Only allow letters and numbers
            }

            e.Handled = regex.IsMatch(e.Text);
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            if (!_IsUserSelected())
            {
                MessageBox.Show("Please select a person to show details.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int PersonID = Convert.ToInt32((dgUsers.SelectedItem as DataRowView)["PersonID"]);
            int UserID = Convert.ToInt32((dgUsers.SelectedItem as DataRowView)["UserID"]);

            winChangePassword ChangePassword = new winChangePassword(PersonID, UserID);
            ChangePassword.ShowDialog(); // Send the User ID Here 
        }



    }
}
