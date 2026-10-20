using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Tests.Controls;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Tests
{
    public partial class winTestAppointment : Window
    {

        public event Action OnTestAppointmentTaken; 

        private clsTestType.enTestType _TestType;
        private int _LocalAppID;
        private DataTable _dtAppointments; 

        public winTestAppointment(clsTestType.enTestType TestType, int LocalAppID)
        {
            _TestType = TestType;
            _LocalAppID = LocalAppID; 

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivinAppID(_LocalAppID);

            _LoadAppointments();

            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    _LoadVisionTest(); 
                    break;
                case clsTestType.enTestType.WrittenTest:
                    _LoadWrittenTest(); 
                    break;
                case clsTestType.enTestType.StreetTest:
                    _LoadStreetTest(); 
                    break;
            }
        }

        private void _LoadAppointments()
        {
            _dtAppointments = clsTestAppointment.GetLocalApplicationAppointmentsPerTestType(_LocalAppID, _TestType);
            dgAppointments.ItemsSource = _dtAppointments.DefaultView; 

            lblScheduleRecordsCount.Content = _dtAppointments.DefaultView.Count;
        }
        private void _LoadVisionTest()
        {
            this.Title = "Vision Test";
            lblTitle.Content = "Vision Test";
            _TestType = clsTestType.enTestType.VisionTest; 
        }

        private void _LoadWrittenTest()
        {
            this.Title = "Written Test";
            lblTitle.Content = "Written Test";
            _TestType = clsTestType.enTestType.WrittenTest;
        }

        private void _LoadStreetTest()
        {
            this.Title = "Street Test";
            lblTitle.Content = "Street Test";
            _TestType = clsTestType.enTestType.StreetTest;
        }


        private void AddAppointment(object sender, RoutedEventArgs e)
        {
            // Check first without loading the full object 
            if (clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalAppID, _TestType))
            {
                MessageBox.Show("Person already has an active appointment for this test. you cannot add new appointment.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (clsLocalDrivingLicenseApplication.DoesPassTestType(_LocalAppID, _TestType))
            {
                MessageBox.Show("This person already Passed this test before, you can only retake failed Tests.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            winScheduleTest ScheduleTest = new winScheduleTest(_LocalAppID, (clsTestType.enTestType)_TestType);
            ScheduleTest.OnAppointmentScheduled += _LoadAppointments;

            ScheduleTest.ShowDialog();
        }

        private void EditAppointment(object sender, RoutedEventArgs e)
        {

            if (dgAppointments.SelectedItems.Count < 1 )
            {
                MessageBox.Show("Select an appointment first");
                return; 
            }

            int AppointmentID = Convert.ToInt32((dgAppointments.SelectedItem as DataRowView)["TestAppointmentID"]);


            winScheduleTest ScheduleTest = new winScheduleTest(_LocalAppID, _TestType, AppointmentID);
            ScheduleTest.OnAppointmentScheduled += _LoadAppointments;
            ScheduleTest.ShowDialog();
        }

        private void TakeTest(object sender, RoutedEventArgs e)
        {
            if (dgAppointments.SelectedItems.Count < 1)
            {
                MessageBox.Show("Select an appointment first");
                return; 
            }


            int AppointmentID = Convert.ToInt32((dgAppointments.SelectedItem as DataRowView)["TestAppointmentID"]);


            winTakeTest TakeTest = new winTakeTest(AppointmentID);
            TakeTest.OnTestSaved += _LoadAppointments;

            TakeTest.OnTestSaved += () =>
            {
                OnTestAppointmentTaken?.Invoke();
            }; 

            TakeTest.ShowDialog(); 
        }
    }
}
