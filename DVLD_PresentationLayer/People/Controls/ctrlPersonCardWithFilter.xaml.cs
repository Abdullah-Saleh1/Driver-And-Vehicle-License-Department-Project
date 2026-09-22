using DVLD_BuisinessLayer;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DVLD_PresentationLayer.People.Controls
{
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

        private bool _SearchByNationalNo; 

        public bool SearchByNationalNo {
            get { return _SearchByNationalNo; }
            set { _SearchByNationalNo = value; cbFilterBy.SelectedIndex = _SearchByNationalNo ? 1 : 0; }
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

            if (cbFilterBy.SelectedIndex == 0) // PersonID
            {
                regex = new Regex("[^0-9]+");
            }

            if (cbFilterBy.SelectedIndex == 1) // National No
            {
                regex = new Regex("[^a-zA-Z0-9]+");
            }

            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
