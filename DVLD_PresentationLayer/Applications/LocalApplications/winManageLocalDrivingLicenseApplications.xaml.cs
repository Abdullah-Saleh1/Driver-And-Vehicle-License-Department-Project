using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Tests;
using DVLD_PresentationLayer.Users;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Applications.LocalApplications
{
    public partial class winManageLocalDrivingLicenseApplications : Window
    {
        public winManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _LocalApplicationData;

        private void _LoadLocalApplications()
        {
            _LocalApplicationData = clsLocalDrivingLicenseApplication.GetAllLocalApplications();
            dgLocalDrivingLicenseApplicaions.ItemsSource = _LocalApplicationData.DefaultView;
            _UpdateRecordsCount(); 
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = _LocalApplicationData.DefaultView.Count;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadLocalApplications(); 
        }

        private void btnAddLocalApplication_Click(object sender, RoutedEventArgs e)
        {
            winAddUpdateLocalDrivingLilcenseApplication winAddUpdateLocalDrivingLilcenseApplication = new winAddUpdateLocalDrivingLilcenseApplication();
            winAddUpdateLocalDrivingLilcenseApplication.OnLocalApplicationSaved += _LoadLocalApplications; 

            winAddUpdateLocalDrivingLilcenseApplication.ShowDialog();
        }

        private bool _IsLocalApplicationSelected()
        {
            return dgLocalDrivingLicenseApplicaions.SelectedItems.Count > 0;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        // Filter and Search Functionality
        private void cbLocalApplicationsFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            txtSearch.Visibility = cbLocalApplicationsFilter.SelectedIndex == 0 ? Visibility.Collapsed : Visibility.Visible;

            txtSearch.Text = string.Empty;
            txtSearch.Focus();
        }
        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex; 

            if (cbLocalApplicationsFilter.SelectedIndex == 0)
            {
                e.Handled = true; // No input allowed
                return;
            }

            if (cbLocalApplicationsFilter.SelectedIndex == 1)
            {
                regex = new Regex("[^0-9]+"); // Only allow numbers
            }
            else if (cbLocalApplicationsFilter.SelectedIndex == 2)
            {
                regex = new Regex("[^a-zA-Z0-9]+"); // Only allow alphanumeric characters
            }
            else
            {
                regex = new Regex("[^a-zA-Z]+"); // Only allow letters
            }

            e.Handled = regex.IsMatch(e.Text);
        }
        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            if (txtSearch.Text == string.Empty)
            {
                _LocalApplicationData.DefaultView.RowFilter = ""; // Clear the filter
                _UpdateRecordsCount();
                return;
            }

            string ColumnName = ((ComboBoxItem)cbLocalApplicationsFilter.SelectedItem).Content.ToString().Replace(" ", ""); 

            switch (ColumnName)
            {
                case "LDLAppID":
                case "NationalNo":
                    _LocalApplicationData.DefaultView.RowFilter = $"Convert({ColumnName}, 'System.String') Like '%{txtSearch.Text}%'";
                    break;
                case "FullName":
                case "Status":
                    _LocalApplicationData.DefaultView.RowFilter = $"{ColumnName} Like '%{txtSearch.Text}%'";
                    break;
                default:
                    _LocalApplicationData.DefaultView.RowFilter = ""; // Clear the filter
                    break;
            }
            _UpdateRecordsCount(); 
        }


        // Menu Item Click Events
        private void dgLocalDrivingLicenseApplicaions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowApplicationDetails(null, null); 
        }
        private void ShowApplicationDetails(object sender, RoutedEventArgs e)
        {
            if (!_IsLocalApplicationSelected())
            {
                MessageBox.Show("Please select a local application.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int LDLAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]);

            winLocalDrivingLicenseApplicationInfo winLocalDrivingLicenseApplicationInfo = new winLocalDrivingLicenseApplicationInfo(LDLAppID);
            winLocalDrivingLicenseApplicationInfo.OnDataUpdated += _LoadLocalApplications; 
            winLocalDrivingLicenseApplicationInfo.ShowDialog(); 
        }

        private void EditApplication(object sender, RoutedEventArgs e)
        {
            if (!_IsLocalApplicationSelected())
            {
                MessageBox.Show("Please select a local application.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int LocalAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]);

            winAddUpdateLocalDrivingLilcenseApplication winAddUpdateLocalDrivingLilcenseApplication = new winAddUpdateLocalDrivingLilcenseApplication(LocalAppID);

            winAddUpdateLocalDrivingLilcenseApplication.ctrlPersonCardWithFilter1.FilterEnabled = false; // Disable the person filter in edit mode

            winAddUpdateLocalDrivingLilcenseApplication.OnLocalApplicationSaved += _LoadLocalApplications; 

            winAddUpdateLocalDrivingLilcenseApplication.ShowDialog();
        }

        private void DeleteApplication(object sender, RoutedEventArgs e)
        {
            if (!_IsLocalApplicationSelected())
            {
                MessageBox.Show("Please select a local application.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete this Application ?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            int LocalAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]); 

            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalAppID);

            if (LocalApp == null)
            {
                MessageBox.Show("Selected local application not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (LocalApp.Delete())
            {
                MessageBox.Show("Local application deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _LoadLocalApplications();
            }
            else
            {
                MessageBox.Show("Failed to delete the local application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CancelApplication(object sender, RoutedEventArgs e)
        {
            if (!_IsLocalApplicationSelected())
            {
                MessageBox.Show("Please select a local application.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to cancel this Applciation ?", "Confirm Cancellation", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            int LocalAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]);


            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalAppID);
            
            if (LocalApp.Cancel())
            {
                MessageBox.Show("Local application canceled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _LoadLocalApplications();
            }
            else
            {
                MessageBox.Show("Failed to cancel the local application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _ScheduleTest(clsTestType.enTestType TestType)
        {
            int LocalAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]);

            winTestAppointment TestAppointment = new winTestAppointment(TestType, LocalAppID);
            TestAppointment.OnTestAppointmentTaken += _LoadLocalApplications;

            TestAppointment.ShowDialog();
        }

        private void ScheduleVision(object sender, RoutedEventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void ScheduleWritten(object sender, RoutedEventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void ScheduleStreet(object sender, RoutedEventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void IssueDrivingLicenseForFirstTime(object sender, RoutedEventArgs e)
        {

        }

        private void ShowLicense(object sender, RoutedEventArgs e)
        {

        }

        private void ShowPersonLicenseHistory(object sender, RoutedEventArgs e)
        {

        }

        private void dgLocalDrivingLicenseApplicaions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLocalDrivingLicenseApplicaions.SelectedItem == null) return;

            // 1. Total TestPassed => from grid; ✅
            // 2. LicenseExists => database query ✅
            // 3. ApplicationStatus => from grid; ✅
            // - setp 2 and 3 will be given from LocapApp Object
            // 4. PassedVisionTest, PassedWrittenTest, PassedStreetTest => from TotalTestPassed ✅


            int LocalAppID = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["LDLAppID"]);
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalAppID);


            int TotalTestPassed = Convert.ToInt32((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)?["PassedTests"]);

            // Here I Create LocalApp to Know if There is an Active Licnese for this application
            // because I need Information like PersonId, LicenseClassID to check if there is license or no 
            // and these information Is Inherited from clsApplication
            bool LicenseExists = LocalApp.IsLicenseIssued();


            btnShowLicense.IsEnabled = LicenseExists;
            btnIssueDrivingLicenseForFirstTime.IsEnabled = (TotalTestPassed == 3) && !LicenseExists; 
            btnEditApplication.IsEnabled = !LicenseExists && (LocalApp.Status == clsApplication.enStatus.New);
            btnCancelApplication.IsEnabled = LocalApp.Status == clsApplication.enStatus.New;
            btnDeleteApplication.IsEnabled = LocalApp.Status == clsApplication.enStatus.New;



            bool PassedVisionTest = TotalTestPassed >= 1; 
            bool PassedWrittenTest = TotalTestPassed >= 2; 
            bool PassedStreetTest = TotalTestPassed >= 3;

            btnScheduleTests.IsEnabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && LocalApp.Status == clsApplication.enStatus.New; 


            if (btnScheduleTests.IsEnabled)
            {
                btnScheduleVisionTest.IsEnabled = !PassedVisionTest;

                btnScheduleWrittenTest.IsEnabled = PassedVisionTest && !PassedWrittenTest;

                btnScheduleStreetTest.IsEnabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }


            //if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["Status"].ToString() == "New")
            //{

            //    btnIssueDrivingLicenseForFirstTime.IsEnabled = false;
            //    btnShowLicense.IsEnabled = false;
            //    btnScheduleTests.IsEnabled = true;

            //    if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["PassedTests"].ToString() == "0")
            //    {
            //        btnScheduleVisionTest.IsEnabled = true;
            //        btnScheduleWrittenTest.IsEnabled = false;
            //        btnScheduleStreetTest.IsEnabled = false;

            //        return;
            //    }

            //    if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["PassedTests"].ToString() == "1")
            //    {
            //        btnScheduleVisionTest.IsEnabled = false;
            //        btnScheduleWrittenTest.IsEnabled = true;
            //        btnScheduleStreetTest.IsEnabled = false;

            //        return;
            //    }

            //    if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["PassedTests"].ToString() == "2")
            //    {
            //        btnScheduleVisionTest.IsEnabled = false;
            //        btnScheduleWrittenTest.IsEnabled = false;
            //        btnScheduleStreetTest.IsEnabled = true;

            //        return;
            //    }

            //    if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["PassedTests"].ToString() == "3")
            //    {
            //        btnScheduleTests.IsEnabled = false;
            //        btnShowLicense.IsEnabled = false;

            //        btnIssueDrivingLicenseForFirstTime.IsEnabled = true;
            //        return;
            //    }

            //}

            //if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["Status"].ToString() == "Cancelled")
            //{
            //    btnEditApplication.IsEnabled = false;
            //    btnDeleteApplication.IsEnabled = false;
            //    btnCancelApplication.IsEnabled = false;
            //    btnScheduleTests.IsEnabled = false;
            //    btnIssueDrivingLicenseForFirstTime.IsEnabled = false;
            //    btnShowLicense.IsEnabled = false;
            //    btnShowPersonLicenseHistory.IsEnabled = false;

            //    return;
            //}


            //if ((dgLocalDrivingLicenseApplicaions.SelectedItem as DataRowView)["Status"].ToString() == "Completed")
            //{

            //    btnEditApplication.IsEnabled = false;
            //    btnDeleteApplication.IsEnabled = false;
            //    btnCancelApplication.IsEnabled = false;
            //    btnScheduleTests.IsEnabled = false;
            //    btnIssueDrivingLicenseForFirstTime.IsEnabled = false;

            //    btnShowApplicationDetails.IsEnabled = true;
            //    btnShowLicense.IsEnabled = true;
            //    btnShowPersonLicenseHistory.IsEnabled = true;
            //}
        }
    }
}

// Context Menu Depend on Row Status and what to do: 

// 1. New AND Passed Tests is 0 ✅
// - Enable Schedule Vision Test and Disable Other Tests ✅

// 2. New AND Passed Tests is 1 ✅
// - Enable Schedule Writtin Tests and Disable Other Tests ✅

// 3. New AND Passed Tests is 2 ✅
// - Enable Schedule Street Test and Disable Other Tests ✅


// 4. New AND Passed Tests is 3 ✅
// - Disable Schedule Tests, Show License ✅
// - Enable Issue Driving License for First Time ✅
// - Enable Rest of the Buttons ✅

// These steps for all new Applications: ✅
// - Disable Issue Driving License for First Time, Show License(Unless when New AND Passed Tests is 3) ✅
// - Enable Rest of the Buttons ✅


// 5. Completed ✅
// - Disable All ✅
// - Enable ShowApplicationDetails, Show License, Show Person License History ✅


// 6. Cancelled ✅
// -- Disable All ✅
// -- Enable ShowApplicationDetails ✅



// Implement General Test Appointment Window Put ctrlLocalDrivinLicenseApplicationInfo ✅
// and DataGrid for Appointments, Columns: AppointmentID, AppointmentDate, PaidFees, IsLocked ✅
// with Records and Close Button ✅

// ContextMenu for: Edit, Take Test ✅
// Edit: show winScheduleTest with the selected AppointmentID ✅
// and if the appointment is locked show the window and disable ✅
// Save, Date then show Message sayed: Person Already sat for this test appointment Locked. ✅

// Take Test : show winTakeTest with the selected AppointmentID ✅

// Add Button:
// 1. check if there is a non-locked appointment if so show messagebox and return ✅
// MessageBox.Show("Person already has an active appointment for this test. you cannot add new appointment.", "Information", MessageBoxButton.OK, MessageBoxImage.Information); ✅
// 2. check if the person has passed the test if so show messagebox and return ✅
// MessageBox.Show("This person already Passed this test before, you can only retake failed Tests.", "Information", MessageBoxButton.OK, MessageBoxImage.Information); ✅
// Show winScheduleTest ✅


// 1. if the Person Failed the Test Lock the Appointmnet ✅
// - when scheduled a new appointment ✅
// - Enable Retake Test ctrl ✅
// - Calculate Total Fees: Retake Test Fees + Schedule Test Fees ✅
// - When Save the appointment Create an Application for Retake Test(7) Associated with that LocalDrivingLicenseAppID ✅



// Implement winLicenseInfo: Driver License Info Window with ctrlDriverLicenseInfo and Close Button
// When Issueing a new License we Take the Validity Lengh from LicenseClass Table
// and add it to the Current Date to get the Expiration Date of the License


// adding New Application for Completed LocalApp 
// Or Active License for that application

// Implemen winLicenseHistory : Show
// - ctrlPersonInfoWithFilters
// - Tab Control with 2 DataGrids: 
// -- LocalLicenses
// -- InternationalLicenses
// With Records and Close


// Implement Driver Menu Only simple filters and with no add or edit btns
// with this columns: DriverID, PersonID, NationalNO, FullName, Date, Active Licenses