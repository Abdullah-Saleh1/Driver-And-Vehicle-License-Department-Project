using DVLD_BuisinessLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.People
{
    /// <summary>
    /// Interaction logic for winAddEditPerson.xaml
    /// </summary>
    public partial class winAddEditPerson : Window
    {

        enum enMode { Add, Edit };

        enMode _Mode;

        private static string _ImagesPath = @"D:\C#\Course 19 DVLD\DVLD\DVLD_PresentationLayer\Images\PeopleImages";
        private string _ImagePath;
        private clsPeople _Person; 

        public winAddEditPerson()
        {
            InitializeComponent();

            // Set the maximum date for Date of Birth to ensure the person is at least 18 years old
            dtpDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18); 
        }
        private void _LoadCountries()
        {
            cbCountries.ItemsSource = clsCountry.GetAllCountries().DefaultView;
            cbCountries.SelectedValuePath = "CountryName";
            cbCountries.SelectedValue = "Saudi Arabia"; 
            // Saudi Arabia Id is 150 in database but 149 in the application 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadCountries(); 
        }



        // Form Validation

        private bool _ValidateNationalNo(string NationalNo)
        {
            return clsPeople.IsNationalNoExists(txtNationalNo.Text); 
        }

        private void txtNationalNo_LostFocus(object sender, RoutedEventArgs e)
        {
            // Check if the National Number is valid and not already in use


            if (string.IsNullOrWhiteSpace(txtNationalNo.Text)) return;

            if (_ValidateNationalNo(txtNationalNo.Text))
            {
                MessageBox.Show("National Number already exists.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);

                txtNationalNo.Clear();
                txtNationalNo.Focus();
            }
        }

        private void GenderChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (imgPerson == null) return; 

            if (radioButton.Content.ToString() == "Male")
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Male 512.png", UriKind.Relative));
            }
            else
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Female 512.png", UriKind.Relative));
            }
        }

        private bool _ValidateEmail(string Email)
        {
            return (Email.Contains("@") && Email.Contains("."));
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == string.Empty) return;

            if (!_ValidateEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid Email Address.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Clear();
                txtEmail.Focus();
            }
        }


        private void _AddNewImage(string ImagePath)
        {

            Guid NewGuid = Guid.NewGuid();

            string Extension = System.IO.Path.GetExtension(ImagePath);

            string NewFileName = NewGuid.ToString() + Extension;

            string DestinationPath = _ImagesPath + '\\' + NewFileName;

            MessageBox.Show(ImagePath); 
            MessageBox.Show(DestinationPath); 
            MessageBox.Show(NewFileName); 

            File.Copy(ImagePath, DestinationPath);
        }


        // Set Image Logic
        private void btnSetImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {

                // 2. تحويل المسار لـ BitmapImage وعرضه في الشاشة
                imgPerson.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                _ImagePath = openFileDialog.FileName.ToString(); 
            }

        }

        private bool _CheckFields()
        {
            if (string.IsNullOrEmpty(txtFirstName.Text)) {

                MessageBox.Show("Please enter First Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; 
            }

            if (string.IsNullOrEmpty(txtSecondName.Text))
            {

                MessageBox.Show("Please enter Second Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtThirdName.Text))
            {

                MessageBox.Show("Please enter Third Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLastName.Text))
            {

                MessageBox.Show("Please Last Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // In NationalNo and Email we don't need to recall validation functions
            // becaues when use click save btn the textbox is lost focus so the Event Validate them
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {

                MessageBox.Show("Please enter National Number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {

                MessageBox.Show("Please enter Email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //if (string.IsNullOrEmpty(txtFirstName.Text))
            //{

            //    MessageBox.Show("Please select enter First Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return false;
            //}

            return true; 
        }


        // Saving Person
        private void SavePerson(object sender, RoutedEventArgs e)
        {

            if (!_CheckFields())
            {
                return; 
            }

            MessageBox.Show("Saving"); 

            // Save Person Logic Here
            //MessageBox.Show(ImagePath); 
            //MessageBox.Show("Person Saved Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);



            // Invoke Delegate to Refresh the DataGrid in winManagePeople


            // 1. open dialog 
            // 2. get the image path 
            // 3. if images folder not exists create it and reInvoke the function 
            // 4. Generate GUID
            // 5. if exists take the selecte ImagePath and copy it to ImagesPath with GUID Name



            //if (!Directory.Exists(_ImagesPath))
            //{
            //    Directory.CreateDirectory(_ImagesPath); 
            //}

            //if (_Mode == enMode.Add)
            //{
            //    _AddNewImage(_ImagePath);
            //}
        }


        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            // Because SelectedIndex is Started from 0 so the first Country 
            // with CountryID 1 is assigned to index 0 
            MessageBox.Show((cbCountries.SelectedIndex + 1).ToString());
            this.Close();
        }
    }
}
