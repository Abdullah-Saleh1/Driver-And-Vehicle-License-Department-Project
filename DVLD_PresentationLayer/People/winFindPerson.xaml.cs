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

namespace DVLD_PresentationLayer.People
{
    /// <summary>
    /// Interaction logic for winFindPerson.xaml
    /// </summary>
    public partial class winFindPerson : Window
    {

        public event Action<int> OnPersonSelected;
        public winFindPerson()
        {
            InitializeComponent();
        }
        public winFindPerson(int PersonID)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadPersonInfo(PersonID); 
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            OnPersonSelected?.Invoke(ctrlPersonCard1.PersonID); 
            this.Close(); 
        }
    }
}
