using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.Users
{
    /// <summary>
    /// Interaction logic for winChangePassword.xaml
    /// </summary>
    public partial class winChangePassword : Window
    {
        private int _PersonID = -1; 
        private int _UserID = -1;
        clsUser CurrentUser; 
        public winChangePassword(int PersonID, int UserID)
        {
            InitializeComponent();

            _PersonID = PersonID; 
            _UserID = UserID;
            CurrentUser = clsUser.Find(UserID); 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlPersonCard1.LoadPersonInfo(_PersonID);

            ctrlUserCard1.LoadUserInfo(_UserID); 


        }

        private void txtCurrentPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e); 
        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e);
        }

        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e);
        }

        private void txtCurrentPassword_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Password)) return;

            if (txtCurrentPassword.Password != CurrentUser.Password)
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

            CurrentUser.Password = txtNewPassword.Password;

            if (CurrentUser.Save())
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
