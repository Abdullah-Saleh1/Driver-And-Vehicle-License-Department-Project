using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using DVLD_PresentationLayer.People;
using DVLD_PresentationLayer.People.Controls;
using System;
using System.Data;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static DVLD_BuisinessLayer.clsApplication;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_PresentationLayer.Applications.LocalApplications
{
    public partial class winNewLocalDrivingLilcenseApplication : Window
    {

        public event Action OnLocalApplicationSaved; 
        public winNewLocalDrivingLilcenseApplication()
        {
            InitializeComponent();
        }

        enum enMode { Add, Update};
        enMode _Mode;
        clsApplication _Application; 
        clsLocalDrivingLicenseApplication _LocalAppliation; 

        private void _LoadClassLicenses()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            // ربط مباشر
            cbLicenseClasses.ItemsSource = dtLicenseClasses.DefaultView;
            cbLicenseClasses.DisplayMemberPath = "ClassName";
            cbLicenseClasses.SelectedValuePath = "LicenseClassID";

            if (cbLicenseClasses.Items.Count > 0)
                cbLicenseClasses.SelectedIndex = 2;
        }

        private void _HandleApplicationInfo()
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            _LoadClassLicenses(); 

            lblApplicationFees.Text = clsApplicationType.GetApplicationFees(1).ToString();

            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlPersonCardWithFilter1.SearchByNationalNo = true;

            _HandleApplicationInfo();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tabApplicationInfo.IsEnabled = true;
            tcApplicationTabs.SelectedIndex = 1; 



            //if (_Mode == enMode.Edit)
            //{
            //    tabLoginInfo.IsEnabled = true;
            //    tcUserTabs.SelectedIndex = 1;
            //}


            //if (_Mode == enMode.Add)
            //{
            //    if (ctrlPersonCardWithFilter1.PersonID == -1)
            //    {
            //        MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        return;
            //    }

            //    if (clsUser.IsUserExistsForPersonID(ctrlPersonCardWithFilter1.PersonID))
            //    {
            //        MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButton.OK, MessageBoxImage.Error);
            //        return;
            //    }

            //    tabLoginInfo.IsEnabled = true;
            //    tcUserTabs.SelectedIndex = 1;
            //}


            //if (ctrlPersonCardWithFilter1.PersonID == -1)
            //{
            //    MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }


        private bool _ValidateFields()
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("من فضلك اختر أو ابحث عن شخص أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (cbLicenseClasses.SelectedIndex == -1 || cbLicenseClasses.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر فئة الرخصة أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbLicenseClasses.Focus();
                return false;
            }
            return true;
        }

        private bool _CheckAgeAndActiveApplication(int PersonID, int LicenseClassID)
        {
            int ActiveApplicationID = clsLocalDrivingLicenseApplication.DoesHaveActiveApplication(PersonID, LicenseClassID);
            if (ActiveApplicationID != -1)
            {
                MessageBox.Show(
                $"Choose another License Class, the selected Person Already have an active application for the selected class with id={ActiveApplicationID}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return false;
            }
            if (!clsLicenseClass.IsAgeAllowedForClass(ctrlPersonCardWithFilter1.SelectedPersonInfo.DateOfBirth, LicenseClassID))
            {
                MessageBox.Show("Person does not meet the minimum age required for this license class.",
                    "Age Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return true; 
        }
        private void Save(object sender, RoutedEventArgs e)
        {

            if (!_ValidateFields()) return;

            int PersonID = ctrlPersonCardWithFilter1.PersonID;
            int LicenseClassID = cbLicenseClasses.SelectedIndex + 1; 

            if (!_CheckAgeAndActiveApplication(PersonID, LicenseClassID)) return; 

            _Application = new clsApplication();
            _LocalAppliation = new clsLocalDrivingLicenseApplication();

            _Application.PersonID = PersonID;
            _Application.Date = DateTime.Now;
            _Application.TypeID = 1; // New Local Driving License Application Type ID
            _Application.Status = enStatus.New;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = clsApplicationType.GetApplicationFees(_Application.TypeID);
            _Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            _LocalAppliation.LicenseClassID = LicenseClassID;

            if (_Application.Save())
            {
                lblApplicationID.Text = _Application.ID.ToString();
                _LocalAppliation.ApplicationID = _Application.ID;
                MessageBox.Show("Application Saved Succesfully"); 

                if (_LocalAppliation.Save())
                {
                    MessageBox.Show("Local Application Saved Succefully");
                    OnLocalApplicationSaved?.Invoke();
                } else
                {
                    // RollBack if the Application is Saved and the LocalApplication is not 
                    clsApplication.Delete(_Application.ID); 
                    MessageBox.Show("Local Application Is not saved"); 
                }
            } else
            {
                MessageBox.Show("Applicatin is not saved"); 
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
