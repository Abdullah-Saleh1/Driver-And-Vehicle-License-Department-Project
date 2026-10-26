using DVLD_BuisinessLayer;
using System;
using System.Windows;

namespace DVLD_PresentationLayer.Licenses
{
    public partial class winShowPersonLicenseHistory : Window
    {

        private int _PersonID = -1; 
        public winShowPersonLicenseHistory(int PersonID)
        {
            _PersonID = PersonID; 

            InitializeComponent();
        }

        public winShowPersonLicenseHistory()
        {
            InitializeComponent();

            ctrlPersonCardWithFilter1.OnPersonSelected += _OnPersonSelected; 
        }

        private void _OnPersonSelected(int PersonID)
        {

            if (PersonID == -1) ctrlDriverLicenses1.Clear(); 
            else
            {
                ctrlDriverLicenses1.LoadLicensesByPersonID(PersonID); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_PersonID == -1)
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true; 

                return; 
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);

            ctrlPersonCardWithFilter1.OnPersonSaved += () =>
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
            };

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            ctrlDriverLicenses1.LoadLicensesByPersonID(_PersonID); 
        }
    }
}
