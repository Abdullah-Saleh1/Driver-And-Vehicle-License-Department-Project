using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Windows;

namespace DVLD_PresentationLayer.Tests
{
    public partial class winTakeTest : Window
    {

        public event Action OnTestSaved; 

        private int _TestAppointmentID;

        private clsTest _Test; 
        public winTakeTest(int TestAppointmentID)
        {
            InitializeComponent();

            _TestAppointmentID = TestAppointmentID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlScheduledTest1.LoadAppointmentInfo(_TestAppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID != -1)
            {
                btnSave.IsEnabled = true; 
            } else
            {
                btnSave.IsEnabled = false;  
            }

            if (ctrlScheduledTest1.TestID != -1)
            {
                _Test = clsTest.Find(ctrlScheduledTest1.TestID);

                if (_Test.TestResult)
                    rbPass.IsChecked = true;
                else
                    rbFail.IsChecked = true;
                txtNotes.Text = _Test.Notes; 


                stPassFail.IsEnabled = false; 
                lblUserMessage.Visibility = Visibility.Visible; 

            } else
                _Test = new clsTest();            
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
            "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return; 


            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.IsChecked == true;
            _Test.Notes = txtNotes.Text;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID; 


            if (_Test.Save())
            {
                MessageBox.Show("Test Saved Successfully");

                ctrlScheduledTest1.lblTestID.Content = _Test.ID; 

                OnTestSaved?.Invoke(); 

            } else
            {
                MessageBox.Show("Error: Test is not Saved."); 
            }
        }
    }
}
