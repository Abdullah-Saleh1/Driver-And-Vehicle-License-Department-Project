using DVLD_BuisinessLayer;
using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DVLD_PresentationLayer.Global_Classes;


namespace DVLD_PresentationLayer.People.Controls
{
    /// <summary>
    /// Interaction logic for ctrlPersonCard.xaml
    /// </summary>
    public partial class ctrlPersonCard : UserControl
    {
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private int _PersonID = -1; 
        public int PersonID { get { return _PersonID; } }

        private clsPerson _Person; 

        public clsPerson SelectedPersonInfo { get { return _Person; } }

        private void _LoadPersonImage()
        {
            if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
                imgPerson.Source = util._LoadImageSafely(_Person.ImagePath);
                return; 
            }

            if (_Person.GenderText == clsPerson.enGender.Male)
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Male 512.png", UriKind.Relative));
            }
            else
            {
                imgPerson.Source = new BitmapImage(new Uri("/Images/Female 512.png", UriKind.Relative));
            }
        }
        private void _FillPersonInfo()
        {
            if (_Person != null)
            {
                _PersonID = _Person.PersonID; 

                lblName.Content = _Person.FirstName + " " + _Person.LastName;
                lblPersonID.Content = _Person.PersonID;
                lblNational.Content = _Person.NationalNO; 
                lblGender.Content = _Person.Gender;
                lblEmail.Content = _Person.Email;
                lblAddress.Content = _Person.Address; 
                lblDateOfBirth.Content = _Person.DateOfBirth.Date.ToShortDateString();
                lblPhone.Content = _Person.Phone;
                lblCountry.Content = _Person.CountryID.ToString();

                _LoadPersonImage(); 

            }
            else
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                _FillPersonInfo();
            }
            else
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadPersonInfo(string NationalNO)
        {
            _Person = clsPerson.Find(NationalNO);

            if (_Person != null)
            {
                _FillPersonInfo();
            }
            else
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void _LoadPersonInfo(object sender, int PersonID)
        {
            LoadPersonInfo(PersonID);
        }
        private void EditPerson(object sender, RoutedEventArgs e)
        {
            winAddEditPerson AddEditPerson = new winAddEditPerson(_PersonID);

            AddEditPerson.DataBack += _LoadPersonInfo; // Subscribe to the event


            AddEditPerson.ShowDialog();
        }
    }
}
