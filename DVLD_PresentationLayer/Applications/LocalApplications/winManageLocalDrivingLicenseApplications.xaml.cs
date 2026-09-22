using DVLD_BuisinessLayer;
using System;
using System.Data;
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

        private void _JustifyColumns()
        {

        }

        private void _LoadLocalApplications()
        {
            _LocalApplicationData = clsLocalDrivingLicenseApplication.GetAllLocalApplications();
            dgLocalDrivingLicenseApplicaions.ItemsSource = _LocalApplicationData.DefaultView; 

            _JustifyColumns(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadLocalApplications(); 
        }

        private void btnAddLocalApplication_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void cbLocalApplicationsFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void txtSearch_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void dgLocalDrivingLicenseApplicaions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
