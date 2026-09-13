using DVLD_BuisinessLayer;
using System;
using System.Data;
using System.Windows;

namespace DVLD_PresentationLayer.Tests.Test_Types
{
    public partial class winManageTestTypes : Window
    {
        public winManageTestTypes()
        {
            InitializeComponent(); 
        }

        private DataTable _TestTypesData; 

        private void EditTestType(object sender, RoutedEventArgs e)
        {
            if (dgTestTypes.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select a test type to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return; 
            }

            clsTestType.enTestType TestTypeID = (clsTestType.enTestType)(dgTestTypes.SelectedItem as System.Data.DataRowView)["TestTypeID"];

            winEditTestType winEditTestType = new winEditTestType(TestTypeID);

            winEditTestType.OnTestTypeUpdated += _LoadTestTypesData; 

            winEditTestType.ShowDialog(); 

        }

        private void _JustifyColumns()
        {
            dgTestTypes.Columns[0].Width = 80;

            dgTestTypes.Columns[0].Header = "ID";
            dgTestTypes.Columns[1].Header = "Title";
            dgTestTypes.Columns[2].Header = "Description";
            dgTestTypes.Columns[3].Header = "Fees";
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Content = dgTestTypes.Items.Count.ToString();
        }
        private void _LoadTestTypesData()
        {
            _TestTypesData = clsTestType.GetAllTestTypes(); 
            dgTestTypes.ItemsSource = _TestTypesData.DefaultView;

            _UpdateRecordsCount();
            _JustifyColumns();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadTestTypesData(); 
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


