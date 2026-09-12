using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System.Windows;
using System.Windows.Input;

namespace DVLD_PresentationLayer.Users
{

    public partial class winChangePassword : Window
    {
        private int _UserID;
        private clsUser _User; 
        public winChangePassword(int UserID)
        {
            InitializeComponent(); 
            _UserID = UserID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                MessageBox.Show("The User Is Not Exists", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return; 
            }

            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e);
        }

        private void txtCurrentPassword_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Password)) return;

            if (txtCurrentPassword.Password != _User.Password)
            {
                txtCurrentPassword.Clear();
                txtCurrentPassword.Focus();
                MessageBox.Show("Invalid current password.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);

                e.Handled = true; 
            }
        }


        private bool _ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Password))
            {
                MessageBox.Show("Please enter your current password.", "Missing Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCurrentPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtNewPassword.Password))
            {
                MessageBox.Show("Please enter a new password.", "Missing Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Password))
            {
                MessageBox.Show("Please confirm your new password.", "Missing Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmPassword.Focus();
                return false;
            }


            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("New password and confirmation do not match.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmPassword.Clear();
                txtConfirmPassword.Focus();
                return false;
            }

            return true; 
        }
        

        private void SavePerson(object sender, RoutedEventArgs e)
        {

            if (!_ValidateFields()) return;

            _User.Password = txtNewPassword.Password;

            if (clsUser.ChangePassword(_UserID, _User.Password))
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to change password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            txtCurrentPassword.Clear();
            txtNewPassword.Clear(); 
            txtConfirmPassword.Clear();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
