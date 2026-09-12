using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Windows;
using System.Windows.Input;

namespace DVLD_PresentationLayer.Users
{
    public partial class winAddEditUser : Window
    {

        public event Action OnPersonSaved;
        clsUser _User;
        private int _UserID; 
        enum enMode { Add, Edit };
        enMode _Mode;

        // Add
        public winAddEditUser()
        {
            InitializeComponent();
        }
        // Edit
        public winAddEditUser(int UserID)
        {
            InitializeComponent();

            _Mode = enMode.Edit;
            _UserID = UserID; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Mode == enMode.Add)
            {
                _Mode = enMode.Add;
                txtTitle.Content = "Add User";
                _User = new clsUser(); 
            } else
            {
                _LoadUserData();

                tabLoginInfo.IsEnabled = true;

                txtTitle.Content = "Edit User";
            }

            ctrlPersonCardWithFilter1.OnPersonSaved += () => OnPersonSaved?.Invoke();
        }

        private void _LoadUserData()
        {

            _User = clsUser.FindByUserID(_UserID);

            if (_User != null)
            {
                lblUserID.Content = _UserID.ToString(); 
                txtUserName.Text = _User.UserName;
                txtPassword.Password = _User.Password; 
                txtConfirmPassword.Password = _User.Password;

                chkIsActive.IsChecked = _User.IsActive;
                ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_Mode == enMode.Edit) {
                tabLoginInfo.IsEnabled = true;
                tcUserTabs.SelectedIndex = 1;
            }


            if (_Mode == enMode.Add)
            {
                if (ctrlPersonCardWithFilter1.PersonID == -1)
                {
                    MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                if (clsUser.IsUserExistsForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                tabLoginInfo.IsEnabled = true; 
                tcUserTabs.SelectedIndex = 1; 
            } 


            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void txtConfirmPassword_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtConfirmPassword.Password != txtPassword.Password)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtConfirmPassword.Clear(); 
            }
        }

        private bool _ValidateInformation()
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                return false;
            }

            return true; 
        }

        private void SavePerson(object sender, RoutedEventArgs e)
        {
            if (!_ValidateInformation())
            {
                MessageBox.Show("Please Select A Person then Enter A Valid Username And Password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check If Username is not the same as the current _User 
            // And is not used by another user
            if ((txtUserName.Text != _User?.UserName) && clsUser.IsUserExists(txtUserName.Text))
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserName.Clear();
                txtUserName.Focus();
                return; 
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim(); 
            _User.Password = txtPassword.Password.Trim();
            _User.IsActive = chkIsActive.IsChecked == true; 

            if (_User.Save())
            {
                _Mode = enMode.Edit;
                lblUserID.Content = _User.UserID.ToString();
                txtTitle.Content = "Update User";

                if (_User.UserID == clsGlobal.CurrentUser.UserID) clsGlobal.CurrentUser = _User;

                OnPersonSaved?.Invoke();


                MessageBox.Show("User saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to save user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
