using System;
using System.Data;
using System.Windows;
using DVLD_BuisinessLayer;


namespace DVLD_PresentationLayer.People
{
    public partial class winShowPersonInfo : Window
    {

        private bool _IsPersonExists = false; 

        public event Action OnDataUpdated; // Event to notify when data is updated
        public winShowPersonInfo(int PersonID)
        {
            InitializeComponent();

            _IsPersonExists = clsPerson.IsPersonExists(PersonID); 

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_IsPersonExists)
            {
                MessageBox.Show("The person does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}
