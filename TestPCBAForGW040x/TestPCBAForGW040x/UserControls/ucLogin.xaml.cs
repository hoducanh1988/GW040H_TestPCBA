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


namespace TestPCBAForGW040x.UserControls
{
    /// <summary>
    /// Interaction logic for ucLogin.xaml
    /// </summary>
    public partial class ucLogin : UserControl
    {
        public ucLogin()
        {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
           if (this.Visibility == Visibility.Visible) {
                GlobalData.testingInfo.USER = "";
                GlobalData.testingInfo.USER = "";
                txtUser.Text = "";
                txtPass.Password = "";
                txtUser.Focus();
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e) {
            GlobalData.testingInfo.USER = txtUser.Text;
            GlobalData.testingInfo.PASSWORD = txtPass.Password.ToString();
            MessageBox.Show(string.Format("{0},{1}", GlobalData.testingInfo.USER, GlobalData.testingInfo.PASSWORD));
        }
    }
}
