using DVLD_BuisinessLayer; 
using System;
using System.IO;
using System.Windows;
using DVLD_PresentationLayer.Global_Classes; 


namespace DVLD_PresentationLayer.Login
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        public winLogin()
        {
            InitializeComponent();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            util.HandlePasswordBoxPlaceHolder(sender, e);
        }

        static string CredentialsPath = "D:\\C#\\Course 19 DVLD\\DVLD\\DVLD_PresentationLayer\\Login\\Credentials.txt";


        private void _HandleRememberMe()
        {
            if (chkRememberMe.IsChecked == true)
            {
                File.WriteAllLines(CredentialsPath, new string[] { txtUserName.Text, txtPassword.Password });
            }
            else
            {
                File.WriteAllText(CredentialsPath, string.Empty);
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            _HandleRememberMe();

            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            clsUser User = clsUser.Find(txtUserName.Text, txtPassword.Password);

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

            clsGlobal.CurrentUser = User; 

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            this.Close(); 
        }

        private void _LoadCredentials()
        {
            string[] lines = File.ReadAllLines(CredentialsPath);

            if (lines.Length >= 2)
            {
                txtUserName.Text = lines[0].Trim();
                txtPassword.Password = lines[1].Trim();

                chkRememberMe.IsChecked = true; // Set the checkbox to checked if credentials are loaded
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadCredentials(); 
        }
    }
}
