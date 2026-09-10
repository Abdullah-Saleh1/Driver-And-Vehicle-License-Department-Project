using DVLD_BuisinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.People.Controls
{
    /// <summary>
    /// Interaction logic for ctrlPersonCardWithFilter.xaml
    /// </summary>
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        public event Action<int> OnPersonSelected;
        public event Action OnPersonSaved;
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();

            ctrlPersonCard1.OnPersonUpdated += () => OnPersonSaved?.Invoke();
        }


        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }
        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }

        private bool _FilterEnabled = true; 
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set { _FilterEnabled = value; spFilters.IsEnabled = value; }
        }

        private bool _ShowAddPerson = true; 
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set { _ShowAddPerson = value; btnAddPerson.Visibility = value ? Visibility.Visible : Visibility.Collapsed;  }
        }


        //Load the person card with the specified person ID
        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Text = PersonID.ToString();
            _FindPerson(); 
        }

        private void _FindPerson()
        {

            switch (cbFilterBy.Text)
            {
                case "Person ID":

                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text)); 
                    break;

                case "National No":

                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                    break; 
            }
            // Fire the event to notify that the person card has been loaded
            // when Filter is Enabled

            if (FilterEnabled) OnPersonSelected?.Invoke(PersonID);
        }

        private bool _ValidateFields()
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text)) return false;

            if (string.IsNullOrEmpty(cbFilterBy.Text)) return false;

            return true; 
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!_ValidateFields())
            {
                MessageBox.Show("Person Is Not Found.");
                return; 
            }

            _FindPerson();
        }

        private void _DataBackEvent(object sender, int PersonID)
        {
            LoadPersonInfo(PersonID); 
        }

        private void btnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            winAddEditPerson winAddEditPerson = new winAddEditPerson();

            winAddEditPerson.DataBack += _DataBackEvent; // Subscribe to the event

            winAddEditPerson.ShowDialog();
        }

        private void cbFilterBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtFilterValue != null)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(""); 

            if (cbFilterBy.Text == "Person ID")
            {
                regex = new Regex("[^0-9]+");
            }

            if (cbFilterBy.Text == "National No")
            {
                regex = new Regex("[^a-zA-Z0-9]+");
            }

            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
