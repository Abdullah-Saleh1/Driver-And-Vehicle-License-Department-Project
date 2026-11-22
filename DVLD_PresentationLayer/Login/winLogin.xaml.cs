using DVLD_BuisinessLayer; 
using System.Windows;
using DVLD_PresentationLayer.Global_Classes;
using Microsoft.Win32;
using System;
using System.Reflection;


namespace DVLD_PresentationLayer.Login
{
    public partial class winLogin : Window
    {
        static string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
        static string UserNameKey = "UserName";
        static string PasswordKey = "Password"; 

        public winLogin()
        {
            InitializeComponent();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e);
        }

        public static void DeleteRegistryValue(string FullKeyPath, string ValueName)
        {
            string subFullKeyPath = FullKeyPath.Replace(@"HKEY_CURRENT_USER\", "");
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(subFullKeyPath, writable: true))
            {
                key?.DeleteValue(ValueName, throwOnMissingValue: false);
            }
        }

        private void _HandleRememberMe()
        {

            try
            {
                if (chkRememberMe.IsChecked == true)
                {
                    Registry.SetValue(KeyPath, UserNameKey, txtUserName.Text, RegistryValueKind.String);
                    Registry.SetValue(KeyPath, PasswordKey, txtPassword.Password, RegistryValueKind.String);
                } else
                {
                    DeleteRegistryValue(KeyPath, UserNameKey);
                    DeleteRegistryValue(KeyPath, PasswordKey); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errro: {ex}");
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            clsUser User = clsUser.FindByUsernameAndPassword(txtUserName.Text, txtPassword.Password);

            if (User == null)
            {
                MessageBox.Show("Username Or Password Is Incorrect.");
                return;
            }

            if (!User.IsActive)
            {
                MessageBox.Show("This User is InActive, Please Contact Your Admin.");
                return;
            }

            _HandleRememberMe();

            clsGlobal.CurrentUser = User; 

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            this.Close(); 
        }

        private void _LoadCredentials()
        {
            try
            {

                string UserName = Registry.GetValue(KeyPath, UserNameKey, null) as string;
                string Password = Registry.GetValue(KeyPath, PasswordKey, null) as string;

                if (UserName != null && Password != null)
                {
                    txtUserName.Text = UserName;
                    txtPassword.Password = Password;

                    chkRememberMe.IsChecked = true; 
                }

            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex}"); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadCredentials(); 
        }
    }
}
