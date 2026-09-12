using DVLD_BuisinessLayer;
using Microsoft.SqlServer.Server;
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

namespace DVLD_PresentationLayer.Users
{
    /// <summary>
    /// Interaction logic for winUserInfo.xaml
    /// </summary>
    public partial class winUserInfo : Window
    {

        private int _UserID; 

        public event Action OnPersonUpdated; 
        public winUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID; 

            // Pass The Action 
            ctrlUserCard1.ctrlPersonCard1.OnPersonUpdated += () => OnPersonUpdated?.Invoke();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
    }
}
