using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Tests.Controls;
using System;
using System.Windows;

namespace DVLD_PresentationLayer.Tests
{
    public partial class winScheduleTest : Window
    {

        public event Action OnAppointmentScheduled;
        public winScheduleTest(int LocalAppID, clsTestType.enTestType TestType, int AppointmentID = -1)
        {
            InitializeComponent();

            ctrlScheduleTest1.TestType = TestType; 
            ctrlScheduleTest1.LoadAppointmentInfo(LocalAppID, AppointmentID);
            ctrlScheduleTest1.OnAppoinmentScheduled += () =>
            {
                OnAppointmentScheduled?.Invoke();
            }; 
        }
    }
}
