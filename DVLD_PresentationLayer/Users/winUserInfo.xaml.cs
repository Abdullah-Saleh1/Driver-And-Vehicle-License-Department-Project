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


        public event Action OnPersonUpdated; 
        public winUserInfo(int UserID, int PersonID)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadPersonInfo(PersonID);
            ctrlUserCard1.LoadUserInfo(UserID);

            ctrlPersonCard1.OnPersonUpdated += () => OnPersonUpdated?.Invoke(); 
        }

    }
}
