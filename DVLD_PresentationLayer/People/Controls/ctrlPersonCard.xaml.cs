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

        private int _PersonID;


        private void _FillPersonInfo(clsPerson Person)
        {
            if (Person != null)
            {
                _PersonID = Person.PersonID; 

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
                    imgPerson.Source = util._LoadImageSafely(Person.ImagePath);
                } else
                {
                    //MessageBox.Show("Image file not found: " + Person.ImagePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    if (Person.GenderText == clsPerson.enGender.Male)
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
            clsPerson Person = clsPerson.Find(PersonID);

            if (Person != null)
            {
                _FillPersonInfo(Person);
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

            // لما بعدل الصوره بيضيفها تاني في الفولدر 
            // ال Edit هنا مش مزبوط
        }
    }
}
