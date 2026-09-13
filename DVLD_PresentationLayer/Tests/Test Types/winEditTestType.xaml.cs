using DVLD_BuisinessLayer;
using DVLD_PresentationLayer.Global_Classes;
using System;
using System.Net.Mail;
using System.Windows; 

namespace DVLD_PresentationLayer.Tests.Test_Types
{
    public partial class winEditTestType : Window
    {

        public event Action OnTestTypeUpdated;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType; 

        public winEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblID.Content = _TestTypeID.ToString(); 

            _TestType = clsTestType.Find((clsTestType.enTestType)_TestTypeID);

            if (_TestType != null)
            {
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString();
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

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Please enter a valid description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDescription.Focus();
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

            if (txtTitle.Text != _TestType.Title || txtDescription.Text != _TestType.Description || float.Parse(txtFees.Text) != _TestType.Fees)
            {
                _TestType.Title = txtTitle.Text.Trim();
                _TestType.Description = txtDescription.Text.Trim(); 
                _TestType.Fees = Convert.ToSingle(txtFees.Text.Trim());

                if (_TestType.Save())
                {
                    MessageBox.Show("Application Type updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnTestTypeUpdated?.Invoke();
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
