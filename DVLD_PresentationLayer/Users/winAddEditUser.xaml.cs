using DVLD_BuisinessLayer;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.Users
{
    /// <summary>
    /// Interaction logic for winAddEditUser.xaml
    /// </summary>
    public partial class winAddEditUser : Window
    {

        public event Action OnPersonSaved;
        clsUser _User; 
        enum enMode { Add, Edit };
        enMode _Mode;

        // Add
        public winAddEditUser()
        {
            InitializeComponent();
            _Mode = enMode.Add; 
            txtTitle.Content = "Add User";

            _User = new clsUser(); 


            ctrlPersonCardWithFilter1.OnPersonSaved += () => OnPersonSaved?.Invoke();
        }

        // Edit
        public winAddEditUser(int UserID, int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Edit; 

            _FillUserInfo(UserID, PersonID); 
            
            tabLoginInfo.IsEnabled = true;

            txtTitle.Content = "Edit User";

            ctrlPersonCardWithFilter1.OnPersonSaved += () => OnPersonSaved?.Invoke();
        }

        private void _FillUserInfo(int UserID, int PersonID)
        {

            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);
            _User = clsUser.Find(UserID);

            if (_User != null)
            {
                txtUserName.Text = _User.UserName;
                txtPassword.Password = _User.Password; 
                txtConfirmPassword.Password = _User.Password;

                chkIsActive.IsChecked = _User.IsActive;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tabLoginInfo.IsEnabled = true;
            tcUserTabs.SelectedIndex = 1; 
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

            if (clsUser.IsUserNameExist(txtUserName.Text) && _Mode == enMode.Add)
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserName.Clear();
                return; 
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text; 
            _User.Password = txtPassword.Password;
            _User.IsActive = chkIsActive.IsChecked == true; 

            if (_User.Save())
            {
                MessageBox.Show("User saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to save user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _Mode = enMode.Edit; 
            OnPersonSaved?.Invoke(); 
        }
    }
}
