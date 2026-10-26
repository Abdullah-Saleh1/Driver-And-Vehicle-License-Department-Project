using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Licenses.InternationalLicenses;
using DVLD_PresentationLayer.Licenses.LocalLicenses;
using System;
using System.Data;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace DVLD_PresentationLayer.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {

        private int _DriverID;

        DataTable _LocalLicensesData; 
        DataTable _InternationalLicensesData; 

        public int DriverID
        {
            get { return _DriverID; }
            set { _DriverID = value; }
        }
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        public void LoadDriverLicenses(int DriverID)
        {

            clsDriver Driver = clsDriver.FindByDriverID(DriverID);


            if (Driver == null)
            {
                MessageBox.Show($"No Driver with this DriverID: {DriverID}");
                return;
            }

            _DriverID = DriverID;

            _LoadLocalLicenses();
            _LoadInternationalLicenses(); 
        }

        public void LoadLicensesByPersonID(int PersonID)
        {
            clsDriver Driver = clsDriver.FindByPersonID(PersonID); 

            if (Driver == null)
            {
                MessageBox.Show($"No Driver linked with this PersonID: {PersonID}");
                return; 
            }

            _DriverID = Driver.ID;

            _LoadLocalLicenses();
            _LoadInternationalLicenses();
        }

        private void _LoadLocalLicenses()
        {
            _LocalLicensesData = clsLicense.GetDriverLicenses(_DriverID);

            dgDriverLocalLicenses.ItemsSource = _LocalLicensesData.DefaultView;

            lblLocalRecordsCount.Content = _LocalLicensesData.Rows.Count;
        }

        private void _LoadInternationalLicenses()
        {
            _InternationalLicensesData = clsInternationalLicense.GetDriverInternationalLicenses(_DriverID);

            dgDriverInternationalLicenses.ItemsSource = _InternationalLicensesData.DefaultView;

            lblInternationalRecordsCount.Content = _InternationalLicensesData.Rows.Count; 
        }


        private void btnShowLocalLicenseInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgDriverLocalLicenses.SelectedItems.Count > 0))
            {
                MessageBox.Show("Select License First."); 
                return; 
            }

            int LicenseID = Convert.ToInt32((dgDriverLocalLicenses.SelectedItem as DataRowView)["LicenseID"]);

            winShowDriverLicenseInfo showDriverLicenseInfo = new winShowDriverLicenseInfo(LicenseID);
            showDriverLicenseInfo.ShowDialog(); 
        }

        private void btnShowInternationalLicenseInfo_Click(object sender, RoutedEventArgs e)
        {

            if (!(dgDriverInternationalLicenses.SelectedItems.Count > 0)) {
                MessageBox.Show("Select License First."); 
                return; 
            }

            int InternationalLicenseID = Convert.ToInt32((dgDriverInternationalLicenses.SelectedItem as DataRowView)["Int License ID"]);

            winShowInternationalDriverLicenseInfo showDriverLicenseInfo = new winShowInternationalDriverLicenseInfo(InternationalLicenseID);
            showDriverLicenseInfo.ShowDialog();

        }

        public void Clear()
        {
            _LocalLicensesData.Clear();
            _InternationalLicensesData.Clear(); 
        }
    }
}
