using DVLD_BuisinessLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;

using System.Windows.Media.Imaging;


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

        private void _FillPersonInfo(clsPeople Person)
        {
            if (Person != null)
            {
                lblName.Content = Person.FirstName + " " + Person.LastName;
                lblPersonID.Content = Person.PersonID;
                lblNational.Content = Person.NationalNO; 
                lblGender.Content = Person.Gender;
                lblEmail.Content = Person.Email;
                lblAddress.Content = Person.Address; 
                lblDateOfBirth.Content = Person.DateOfBirth.Date.ToShortDateString();
                lblPhone.Content = Person.Phone;
                lblCountry.Content = Person.CountryID.ToString();

                if (!string.IsNullOrEmpty(Person.ImagePath) && File.Exists(Person.ImagePath))
                {
                    imgPerson.Source = new BitmapImage(new Uri(Person.ImagePath, UriKind.RelativeOrAbsolute));
                } else
                {
                    //MessageBox.Show("Image file not found: " + Person.ImagePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (Person.GenderText == clsPeople.enGender.Male)
                    {
                        imgPerson.Source = new BitmapImage(new Uri("/Images/Male 512.png", UriKind.Relative));
                    }
                    else
                    {
                        imgPerson.Source = new BitmapImage(new Uri("/Images/Female 512.png", UriKind.Relative));
                    }
                }

            }
            else
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadPersonInfo(int PersonID)
        {
            clsPeople Person = clsPeople.GetPersonByID(PersonID);

            if (Person != null)
            {
                _FillPersonInfo(Person);
            }
            else
            {
                MessageBox.Show("Person not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
