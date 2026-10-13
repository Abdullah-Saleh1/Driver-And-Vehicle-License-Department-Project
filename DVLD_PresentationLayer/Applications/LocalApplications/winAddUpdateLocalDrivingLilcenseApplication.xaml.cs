using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Data;
using System.Windows;
using static DVLD_BuisinessLayer.clsApplication;

namespace DVLD_PresentationLayer.Applications.LocalApplications
{
    public partial class winAddUpdateLocalDrivingLilcenseApplication : Window
    {

        enum enMode { Add, Update };
        enMode _Mode;

        clsLocalDrivingLicenseApplication _LocalAppliation;
        private int _LocalDrivingLicenseApplicationID = -1; 
        

        public event Action OnLocalApplicationSaved; 
        public winAddUpdateLocalDrivingLilcenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.Add; 
        }

        public winAddUpdateLocalDrivingLilcenseApplication(int LocalDrivingLicenseApplication)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication;
            _Mode = enMode.Update;  
        }

        private void _LoadClassLicenses()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            cbLicenseClasses.ItemsSource = dtLicenseClasses.DefaultView;
            cbLicenseClasses.DisplayMemberPath = "ClassName";
            cbLicenseClasses.SelectedValuePath = "LicenseClassID";

            if (cbLicenseClasses.Items.Count > 0)
                cbLicenseClasses.SelectedIndex = 2;
        }

        private void _ResetDefaultValues()
        {
            ctrlPersonCardWithFilter1.SearchByNationalNo = true;
            _LoadClassLicenses();

            if (_Mode == enMode.Add)
            {

                _LocalAppliation = new clsLocalDrivingLicenseApplication(); 

                txtTitle.Content = "Add Local Driving License Application"; 

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();

                lblApplicationFees.Text = clsApplicationType.GetApplicationFees((int)clsApplication.enApplicationType.NewDrivingLicense).ToString();

                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            } else
            {
                txtTitle.Content = "Update Local Driving License Application";
                tabApplicationInfo.IsEnabled = true; 
            }
        }

        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            _LocalAppliation = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID); 

            if (_LocalAppliation == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                this.Close();
                return; 
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalAppliation.ApplicantPersonID); 


            cbLicenseClasses.SelectedIndex = _LocalAppliation.LicenseClassID - 1;

            lblApplicationID.Text = _LocalAppliation.ApplicationID.ToString();
            lblApplicationDate.Text = _LocalAppliation.ApplicationDate.ToShortDateString();
            lblApplicationFees.Text = _LocalAppliation.PaidFees.ToString(); // Paid Fees on that application not the current Application Fees
            lblCreatedBy.Text = _LocalAppliation.CreatedByUserInfo.UserName.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData(); 
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_Mode == enMode.Add && ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update
            tabApplicationInfo.IsEnabled = true;
            tcApplicationTabs.SelectedIndex = 1;
        }


        private bool _ValidateFields()
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (cbLicenseClasses.SelectedIndex == -1 || cbLicenseClasses.SelectedValue == null)
            {
                MessageBox.Show("Please select a License Class to continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbLicenseClasses.Focus();
                return false;
            }
            return true;
        }

        private bool _CheckAgeAndActiveApplicationAndActiveLicense(int PersonID, int LicenseClassID)
        {

            if (!clsLicenseClass.IsAgeAllowedForClass(ctrlPersonCardWithFilter1.SelectedPersonInfo.DateOfBirth, LicenseClassID))
            {
                MessageBox.Show("Person does not meet the minimum age required for this license class.",
                    "Age Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(PersonID, enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show(
                $"Choose another License Class, the selected Person Already have an active application for the selected class with ID ={ActiveApplicationID}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return false;
            }

            int ActiveLicenseID = clsLicense.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID); 

            if (ActiveLicenseID != -1)
            {
                MessageBox.Show(
                $"Choose another License Class, the selected Person Already have an active license for the selected class with ID ={ActiveLicenseID}",
                "Error",
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

            if (!_CheckAgeAndActiveApplicationAndActiveLicense(PersonID, LicenseClassID)) return; 


            _LocalAppliation.ApplicantPersonID = PersonID;
            if (_Mode == enMode.Add)
                _LocalAppliation.ApplicationDate = DateTime.Now; // To Prevent Update the Application Date
            _LocalAppliation.ApplicationTypeID = 1; // New Local Driving License Application Type ID
            _LocalAppliation.Status = enStatus.New;
            _LocalAppliation.LastStatusDate = DateTime.Now;
            _LocalAppliation.PaidFees = clsApplicationType.GetApplicationFees(_LocalAppliation.ApplicationTypeID);
            _LocalAppliation.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            _LocalAppliation.LicenseClassID = LicenseClassID;



            if (_LocalAppliation.Save())
            {
                lblApplicationID.Text = _LocalAppliation.ApplicationID.ToString();
                MessageBox.Show("Application Saved Succesfully");

                _Mode = enMode.Update;
                txtTitle.Content = "Update Local Driving License Application";
                OnLocalApplicationSaved?.Invoke(); 

            } else
            {
                MessageBox.Show("Data is not saved successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
