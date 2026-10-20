using DVLD_BuisinessLayer;
using System;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Tests.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {

        private clsTestAppointment _TestAppointment;
        private clsTestType.enTestType _TestType;
        private int _LocalAppID;
        private clsLocalDrivingLicenseApplication _LocalApp;


        private int _TestAppointmentID; 

        public int TestAppointmentID
        {
            set { _TestAppointmentID = value; }
            get { return _TestAppointmentID; }
        }

        private int _TestID;

        public int TestID
        {
            set { _TestID = value; }
            get { return _TestID; }
        }

        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        private void _LoadTestInfo()
        {
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

        private void _LoadApplicationInfo()
        {
            _LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalAppID);

            lblLocalAppID.Content = _LocalAppID.ToString();
            lblClassName.Content = _LocalApp.LicenseClassInfo.ClassName;
            lblPersonName.Content = _LocalApp.PersonInfo.FullName;
            lblTrial.Content = _LocalApp.GetToalTrailsPerTest(_TestType); 


            lblFees.Content = clsTestType.GetTestTypeFees((int)_TestType);
        }

        public void LoadAppointmentInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID; 

            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            _TestType = _TestAppointment.TestTypeID;
            _TestID = _TestAppointment.TestID; 

            _LocalAppID = _TestAppointment.LocalAppID;

            lblDate.Content = _TestAppointment.Date.ToShortDateString();

            lblTestID.Content = (_TestID == -1) ? "Not Take Yet" : _TestID.ToString(); 

            _LoadApplicationInfo();
            _LoadTestInfo();
        }
    }
}
