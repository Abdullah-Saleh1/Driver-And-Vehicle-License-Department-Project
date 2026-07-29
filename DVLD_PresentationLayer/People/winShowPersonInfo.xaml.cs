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


        private void EditPersonInfo(object sender, RoutedEventArgs e)
        {
            // Show Edit Person Window
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

    }
}
