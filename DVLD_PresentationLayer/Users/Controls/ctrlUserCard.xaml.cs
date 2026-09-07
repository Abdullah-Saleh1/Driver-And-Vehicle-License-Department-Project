using DVLD_BuisinessLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DVLD_PresentationLayer.Users.Controls
{
    /// <summary>
    /// Interaction logic for ctrlUserCard.xaml
    /// </summary>
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
            lblUserID.Content = _User.UserID.ToString();
            lblUserName.Content = _User.UserName;
            lblIsActive.Content = _User.IsActive ? "Yes" : "No";
        }

        public void LoadUserInfo (int UserID)
        {
            _User = clsUser.Find(UserID); 

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
