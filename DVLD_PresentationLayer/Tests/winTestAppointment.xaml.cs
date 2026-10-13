using System.Windows;

namespace DVLD_PresentationLayer.Tests
{
    public partial class winTestAppointment : Window
    {

        public enum enTestType { VisionTest, WrittenTest, StreetTest }
        private enTestType _TestType;
        private int _LocalAppID; 
        public winTestAppointment(enTestType TestType, int LocalAppID)
        {
            _TestType = TestType;
            _LocalAppID = LocalAppID; 

            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivinAppID(_LocalAppID); 

            switch (_TestType)
            {
                case enTestType.VisionTest:
                    _LoadVisionTest(); 
                    break;
                case enTestType.WrittenTest:
                    _LoadWrittenTest(); 
                    break;
                case enTestType.StreetTest:
                    _LoadStreetTest(); 
                    break;
            }
        }

        private void _LoadVisionTest()
        {
            this.Title = "Vision Test";
            lblTitle.Content = "Vision Test"; 


        }

        private void _LoadWrittenTest()
        {

        }

        private void _LoadStreetTest()
        {

        }

        private void EditAppointment(object sender, RoutedEventArgs e)
        {

        }

        private void AddAppointment(object sender, RoutedEventArgs e)
        {

        }
    }
}
