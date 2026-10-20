using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;


namespace DVLD_PresentationLayer.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {

        public event Action OnAppoinmentScheduled; 

        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest; 
        private int _LocalAppID;
        private clsLocalDrivingLicenseApplication _LocalApp;
        private int _TotalTrails; // Check if you don't need it remove it
        private int _TestAppointmentID; 
        private clsTestAppointment _TestAppointment;


        public clsTestType.enTestType TestType
        {
            get { return _TestType; }
            set
            {
                _TestType = value;

                switch (_TestType)
                {
                    case clsTestType.enTestType.VisionTest:

                        lblTitle.Content = "Vision Test";
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        lblTitle.Content = "Written Test";
                        break;
                    case clsTestType.enTestType.StreetTest:
                        lblTitle.Content = "Street Test";
                        break;
                    default:
                        break;
                }
            }
        }

        private enum enMode { Add, Update }
        private enMode _Mode;

        private enum enCreationMode { FirstTimeSchedule, RetakeTestSchedule}
        private enCreationMode _CreationMode; 


        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        // Main Load Function
        public void LoadAppointmentInfo(int LocalAppID, int AppointmentID = -1)
        {
            _Mode = AppointmentID == -1 ? enMode.Add : enMode.Update;

            _TestAppointmentID = AppointmentID;
            _LocalAppID = LocalAppID;
            _LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalAppID);

            if (_LocalApp == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalAppID.ToString(),
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btnSave.IsEnabled = false;
                return; 
            }

            _CreationMode = _LocalApp.DoesAttendTestType(_TestType) ? enCreationMode.RetakeTestSchedule : enCreationMode.FirstTimeSchedule; 

            _LoadApplicationInfo();

            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                gdRetakeTest.IsEnabled = true;
                _LoadRetakTestInfo();
            } else
            {
                gdRetakeTest.IsEnabled = false; 
                _ResetRetakeTestInfo(); 
            }


            if (_Mode == enMode.Add)
            {
                lblFees.Content = clsTestType.GetTestTypeFees((int)_TestType);
                dtpAppointmentDate.DisplayDateStart = DateTime.Today;
                dtpAppointmentDate.SelectedDate = DateTime.Today;
                _TestAppointment = new clsTestAppointment(); 
            } else
            {
                if (!_LoadTestAppointmentData()) return; 
            }

            lblTotalFees.Content = (Convert.ToSingle(lblFees.Content) + Convert.ToSingle(lblRAppFees.Content)).ToString();


            // this condition is most likely to be true so put in first
            if (!HandleAppointmentLockedConstraint()) return;

            if (!HandleActiveTestAppointmentConstraint()) return;

            if (!_HandlePreviousTestConstraint()) return; 
        }

        // Validation Functions
        private void ShowErrorMessage(string Message)
        {
            dtpAppointmentDate.IsEnabled = false; 
            btnSave.IsEnabled = false;
            lblUserMessage.Visibility = Visibility.Visible;
            lblUserMessage.Content = Message; 
        }
        private bool HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.Add && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalAppID, _TestType))
            {
                ShowErrorMessage("Person Already have an active appointment for this test"); 
                return false;
            }

            return true;                 
        }

        private bool HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                ShowErrorMessage("Person already sat for this test, Appointment Locked"); 
                return false; 
            }

            return true; 
        }

        private bool _HandlePreviousTestConstraint()
        {

            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    return true; 
                case clsTestType.enTestType.WrittenTest:
                    
                    if (!_LocalApp.DoesAttendTestType(clsTestType.enTestType.VisionTest))
                    {
                        ShowErrorMessage("Cannot Sechule, Vision Test should be passed first"); 
                        return false; 
                    }
                    return true; 
                case clsTestType.enTestType.StreetTest:
                    if (!_LocalApp.DoesAttendTestType(clsTestType.enTestType.WrittenTest))
                    {
                        ShowErrorMessage("Cannot Sechule, Written Test should be passed first");
                        return false;
                    }
                    return true;
                default:
                    return true; 
            }
        }

        // Load Helper Functions
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btnSave.IsEnabled = false; 
                return false; 
            }

            lblFees.Content = _TestAppointment.PaidFees.ToString();

            // < 0 means the DateTime1 is before DateTime2 
            // we see the earlier date and set it as DispalyDateStart
            if (DateTime.Compare(DateTime.Now, _TestAppointment.Date) < 0)
                dtpAppointmentDate.DisplayDateStart = DateTime.Now;
            else
                dtpAppointmentDate.DisplayDateStart = _TestAppointment.Date;

            dtpAppointmentDate.SelectedDate = _TestAppointment.Date; 

            if (_TestAppointment.RetakeTestAppID.HasValue)
            {
                lblTitle.Content = "Schedule Retake Test"; 
                gdRetakeTest.IsEnabled = true; 
                lblRTestAppID.Content = _TestAppointment.RetakeTestAppID.ToString();
                lblRAppFees.Content = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString(); 

            } else
            {
                _ResetRetakeTestInfo(); // i think this is not matter because i already ran it in LoadAppointmentInfo
            }


            return true; 
        }

        private void _LoadApplicationInfo()
        {

            _TotalTrails = _LocalApp.GetToalTrailsPerTest(_TestType);

            lblLocalAppID.Content = _LocalAppID.ToString();
            lblClassName.Content = _LocalApp.LicenseClassInfo.ClassName;
            lblPersonName.Content = _LocalApp.PersonInfo.FullName;
            lblTrial.Content = _TotalTrails.ToString();

            dtpAppointmentDate.DisplayDateStart = DateTime.Today;
            dtpAppointmentDate.SelectedDate = DateTime.Today;

            lblFees.Content = clsTestType.GetTestTypeFees((int)_TestType);


       
        }

        private void _LoadRetakTestInfo()
        {
            lblRAppFees.Content = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.RetakeTest);
            lblRTestAppID.Content = "0"; 
        }

        private void _ResetRetakeTestInfo()
        {
            lblRAppFees.Content = "0";
            lblRTestAppID.Content = "N/A";
        }


        // Save Functions
        private bool _HandleRetakeTestApplication()
        {
            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {

                // NOTE: Update database Diagram Link RetakeTestApplicationID 
                // from TestAppointment Table to ApplicationID in the Application Table

                clsApplication RetakeTestApplication = new clsApplication();
                RetakeTestApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                RetakeTestApplication.ApplicantPersonID = _LocalApp.ApplicantPersonID;
                RetakeTestApplication.ApplicationDate = DateTime.Now;
                RetakeTestApplication.Status = clsApplication.enStatus.Completed;
                RetakeTestApplication.LastStatusDate = _LocalApp.LastStatusDate;
                RetakeTestApplication.PaidFees = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.RetakeTest) ;
                RetakeTestApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (RetakeTestApplication.Save())
                {
                    MessageBox.Show("Retake Test Fees is Applied");

                    lblRTestAppID.Content = RetakeTestApplication.ApplicationID.ToString();
                    _TestAppointment.RetakeTestAppID = RetakeTestApplication.ApplicationID;
                }
                else
                {
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true; 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!_HandleRetakeTestApplication()) return; 


            _TestAppointment.LocalAppID = _LocalAppID;
            _TestAppointment.TestTypeID = _TestType; 
            _TestAppointment.PaidFees = Convert.ToInt32(lblFees.Content); 
            _TestAppointment.Date = dtpAppointmentDate.SelectedDate.Value;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _TestAppointment.IsLocked = false; 


            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update; 
                MessageBox.Show("Test Appointment Saved Successfully.");
                OnAppoinmentScheduled?.Invoke(); 
            } else
            {
                MessageBox.Show("Error Test Appointment is Not Saved."); 
            }
        }
    }
}
