using DVLD_BuisinessLayer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DVLD_PresentationLayer.Global_Classes; 

namespace DVLD_PresentationLayer.People
{
    /// <summary>
    /// Interaction logic for winAddEditPerson.xaml
    /// </summary>
    public partial class winAddEditPerson : Window
    {

        enum enMode { Add, Update };

        enMode _Mode;

        
        private string _ImagePath = "";
        private clsPerson _Person;
        private int _PersonID = -1;


        // Delegate
        public delegate void DataBackEventHanlder(object sender, int PersonID);

        public event DataBackEventHanlder DataBack;

        public winAddEditPerson(int PersonID = -1)
        {
            InitializeComponent();

            // Set the maximum date for Date of Birth to ensure the person is at least 18 years old
            dtpDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.DisplayDateStart = DateTime.Now.AddYears(-100); 

            this._PersonID = PersonID; 

            _Mode = PersonID == -1 ? enMode.Add : enMode.Update; 
        }
        private void _LoadCountries()
        {
            cbCountries.ItemsSource = clsCountry.GetAllCountries().DefaultView;
            cbCountries.SelectedValuePath = "CountryName";
            cbCountries.SelectedValue = "Saudi Arabia"; 
        }

        private void _DispalyPersonData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
          
            lblPersonID.Content = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName; 
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNO; 
                
            if ((clsPerson.enGender)_Person.Gender == clsPerson.enGender.Male)
            {
                rbMale.IsChecked = true; 
            } else
            {
                rbFemale.IsChecked = true; 
            } 

            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone; 
            dtpDateOfBirth.SelectedDate = _Person.DateOfBirth;
            cbCountries.SelectedIndex = _Person.CountryID - 1;

            _ImagePath = _Person.ImagePath; 

            // image
            if (!string.IsNullOrEmpty(_ImagePath) && File.Exists(_ImagePath))
            {
                imgPerson.Source = util._LoadImageSafely(_ImagePath);
                btnRemoveImage.Visibility = Visibility.Visible; 
            }
        }

        private void _ResetDefaultValues()
        {
            _LoadCountries();

            if (_Mode == enMode.Update)
            {
                lblAddEditPerson.Content = "Edit Person";
                _DispalyPersonData();
            }
            else
            {
                lblAddEditPerson.Content = "Add New Person";
                _Person = new clsPerson(); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetDefaultValues();  
        }



        // Form Validation

        private bool _ValidateNationalNo(string NationalNo)
        {
            return clsPerson.IsPersonExists(txtNationalNo.Text); 
        }

        private void txtNationalNo_LostFocus(object sender, RoutedEventArgs e)
        {
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
            if (imgPerson == null || _ImagePath != "") return; 

            RadioButton radioButton = sender as RadioButton;

            if (radioButton.Content.ToString() == "Male")
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Male 512.png", UriKind.Relative));
            }
            else
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Female 512.png", UriKind.Relative));
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == string.Empty) return;

            if (!clsValidation._ValidateEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid Email Address.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Clear();
                txtEmail.Focus();
            }
        }

        private void txtPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // if not number 

            // e.Text => the letter the use enter 

            // Prevent Typing the letter if not match the regex
            e.Handled = regex.IsMatch(e.Text); // if true it will prvent typing
        }

        private void btnSetImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {

                // 2. تحويل المسار لـ BitmapImage وعرضه في الشاشة
                _ImagePath = openFileDialog.FileName.ToString(); 
                imgPerson.Source = util._LoadImageSafely(_ImagePath);
            }
            btnRemoveImage.Visibility = Visibility.Visible; 
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            _ImagePath = "";
            imgPerson.Source = new BitmapImage(new Uri((bool)rbMale.IsChecked ? "/Images/Male 512.png" : "/Images/Female 512.png", UriKind.Relative)); 
            btnRemoveImage.Visibility = Visibility.Collapsed; 
        }
            
        private bool _CheckFields()
        {
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {

                MessageBox.Show("Please Enter First Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtSecondName.Text))
            {

                MessageBox.Show("Please Enter Second Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtThirdName.Text))
            {

                MessageBox.Show("Please Enter Third Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLastName.Text))
            {

                MessageBox.Show("Please Enter Last Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // In NationalNo and Email we don't need to recall validation functions
            // becaues when use click save btn the textbox is lost focus so the Event Validate them
            // and the Messagebox in LostFocus Event Will Block the window and SavePerson will not be executed
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {

                MessageBox.Show("Please Enter National Number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {

                MessageBox.Show("Please Enter Email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (dtpDateOfBirth.SelectedDate == null)
            {

                MessageBox.Show("Please Enter Date Of Birth.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtAddress.Text))
            {

                MessageBox.Show("Please Enter Address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {

                MessageBox.Show("Please Enter A Valid Phone.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; 
        }


        // Saving Person

        private bool HandlePersonImage()
        {

            if (_Person.ImagePath == _ImagePath) return true; 
            
            // When the image is changed
            if (_Person.ImagePath != "")
            {
                try
                {
                    File.Delete(_Person.ImagePath); // Old Image
                } catch (IOException iox)
                {
                    MessageBox.Show(iox.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (_ImagePath != "")
            {
                if (util.CopyImageToImagesFolder(ref _ImagePath)) // New Image
                {
                    _Person.ImagePath = _ImagePath; 
                    return true; 
                } else
                {
                    return false; 
                }
            }

            return true; // when the image is not changed or handled successfully
        }

        private void SavePerson(object sender, RoutedEventArgs e)
        {


            if (!_CheckFields()) return;


            if (!HandlePersonImage()) return; 

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNO = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim(); 
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.SelectedDate.Value;

            _Person.Gender = (bool)rbMale.IsChecked ? (byte)clsPerson.enGender.Male : (byte)clsPerson.enGender.Female;

            // Because SelectedIndex is Started from 0 so the first Country 
            // with CountryID 1 is assigned to the index 0 
            _Person.CountryID = cbCountries.SelectedIndex + 1;


            //if (_Mode == enMode.Add)
            //{
            //    if (!string.IsNullOrEmpty(_ImagePath))
            //    {
            //        _Person.ImagePath = _AddNewImage(_ImagePath);
            //    }
            //} else // Update
            //{

            //    // Change the Image
            //    if (_Person.ImagePath != _ImagePath && _ImagePath != "")
            //    {
            //        _Person.ImagePath = _UpdateImage(_ImagePath);
            //    }

            //    // Remove an existing image 
            //    if (_Person.ImagePath != _ImagePath && _ImagePath == "")
            //    {
            //        if (File.Exists(_Person.ImagePath))
            //        {
            //            File.Delete(_Person.ImagePath);
            //        }
            //        _Person.ImagePath = _ImagePath;
            //    }
            //}

            if (_Person.Save())
            {
                MessageBox.Show("Person Saved Successfully.");
            }
            else
            {
                MessageBox.Show("Failed To Save The Person.");
            }

            _Mode = enMode.Update;
            lblAddEditPerson.Content = "Edit Person";
            lblPersonID.Content = _Person.PersonID;

            DataBack?.Invoke(this, _Person.PersonID);

        }


        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
