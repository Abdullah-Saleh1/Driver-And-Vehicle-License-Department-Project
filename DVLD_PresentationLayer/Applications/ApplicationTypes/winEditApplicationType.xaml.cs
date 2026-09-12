using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.Applications.ApplicationTypes
{
    /// <summary>
    /// Interaction logic for winEditApplicationType.xaml
    /// </summary>
    public partial class winEditApplicationType : Window
    {

        public event Action OnApplicationTypeUpdated;
        private int _ApplicationTypeID;
        private clsApplicationType _ApplicationType;
        public winEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblID.Content = _ApplicationTypeID.ToString();

            _ApplicationType = clsApplicationType.FindApplicationTypeByID(_ApplicationTypeID);

            if (_ApplicationType != null)
            {
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString(); 
            }
        }
        private bool _ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a valid title.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitle.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtFees.Text) || !clsValidation.IsNumber(txtFees.Text) || float.Parse(txtFees.Text) < 0)
            {
                MessageBox.Show("Please enter a valid non-negative fee.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFees.Focus();
                return false;
            }

            return true; 
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveApplicationType(object sender, RoutedEventArgs e)
        {
            if (!_ValidateFields()) return; 

            if (txtTitle.Text != _ApplicationType.Title || float.Parse(txtFees.Text) != _ApplicationType.Fees)
            {
                _ApplicationType.Title = txtTitle.Text.Trim();
                _ApplicationType.Fees = Convert.ToSingle(txtFees.Text.Trim());

                if (_ApplicationType.Save())
                {
                    MessageBox.Show("Application Type updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnApplicationTypeUpdated?.Invoke();
                }
                else
                {
                    MessageBox.Show("Failed to update Application Type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No changes detected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
