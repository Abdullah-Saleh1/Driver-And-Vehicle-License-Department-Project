using System;
using System.Data;
using System.Windows;
using DVLD_BuisinessLayer;


namespace DVLD_PresentationLayer.People
{
    /// <summary>
    /// Interaction logic for winShowPersonInfo.xaml
    /// </summary>
    public partial class winShowPersonInfo : Window
    {
        public winShowPersonInfo(int PersonID)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        public winShowPersonInfo(string NationalNO)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadPersonInfo(NationalNO);
        }



        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

    }
}
