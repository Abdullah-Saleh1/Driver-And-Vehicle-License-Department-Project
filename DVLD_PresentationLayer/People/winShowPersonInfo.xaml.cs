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

        public event Action OnDataUpdated; // Event to notify when data is updated
        public winShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfo(PersonID);

            ctrlPersonCard1.OnPersonUpdated += _PersonUpdatedInControl;
        }

        public winShowPersonInfo(string NationalNO)
        {
            InitializeComponent(); 
            ctrlPersonCard1.LoadPersonInfo(NationalNO);

            ctrlPersonCard1.OnPersonUpdated += _PersonUpdatedInControl;
        }

        private void _PersonUpdatedInControl()
        {
            OnDataUpdated?.Invoke(); // Raise the event to notify subscribers
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

    }
}
