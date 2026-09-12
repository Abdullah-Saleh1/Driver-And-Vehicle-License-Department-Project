using DVLD_BuisinessLayer;
using System.Windows;
using System.Windows.Controls;

namespace DVLD_PresentationLayer.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {


        private int _UserID; 
        public int UserID { get { return _UserID; } }

        private clsUser _User;

        public clsUser SelectedUserInfo { get { return _User; } }
        public ctrlUserCard()
        {
            InitializeComponent();
        }


        private void _FillUserInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID); 
            lblUserID.Content = _User.UserID.ToString();
            lblUserName.Content = _User.UserName;
            lblIsActive.Content = _User.IsActive ? "Yes" : "No";
        }

        public void LoadUserInfo (int UserID)
        {
            _User = clsUser.FindByUserID(UserID); 
            _UserID = UserID;   

            if (_User != null)
            {
                _FillUserInfo();
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
