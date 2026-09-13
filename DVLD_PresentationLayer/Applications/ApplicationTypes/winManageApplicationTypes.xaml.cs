using DVLD_BuisinessLayer;
using System;
using System.Data;
using System.Windows;

namespace DVLD_PresentationLayer.Applications.ApplicationTypes
{
    public partial class winManageApplicationTypes : Window
    {

        DataTable _ApplicationTypesData; 
        public winManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = dgApplicationTypes.Items.Count;
        }

        private void _JustifyColumns()
        {
            dgApplicationTypes.Columns[0].Width = 100;
            //dgApplicationTypes.Columns[1].Width = 100;
            dgApplicationTypes.Columns[2].Width = 120;


            dgApplicationTypes.Columns[0].Header = "ID";
            dgApplicationTypes.Columns[1].Header = "Title";
            dgApplicationTypes.Columns[2].Header = "Fees";

        }

        private void _LoadApplicationTypesData()
        {
            _ApplicationTypesData = clsApplicationType.GetAllAplicationTypes();

            dgApplicationTypes.ItemsSource = _ApplicationTypesData.DefaultView; 
            _UpdateRecordsCount();
            _JustifyColumns();
        }

        private void EditApplicationType(object sender, RoutedEventArgs e)
        {
            int ApplicationTypeID = Convert.ToInt32((dgApplicationTypes.SelectedItem as DataRowView)["ApplicationTypeID"]);

            winEditApplicationType winEditApplicationType = new winEditApplicationType(ApplicationTypeID);

            winEditApplicationType.OnApplicationTypeUpdated += _LoadApplicationTypesData; 

            winEditApplicationType.ShowDialog(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadApplicationTypesData();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
