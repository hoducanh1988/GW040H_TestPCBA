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
using System.Windows.Threading;
using TestPCBAForGW040x.Functions;

namespace TestPCBAForGW040x.SubControls {
    /// <summary>
    /// Interaction logic for LED.xaml
    /// </summary>
    public partial class LED : Window {

        public LED(double top, double left, double width, double height) {
            InitializeComponent();
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;
            this.DataContext = GlobalData.testingInfo;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label l = sender as Label;
            switch (l.Name) {
                case "ledPower": {
                        GlobalData.testingInfo.POWERLED = !GlobalData.testingInfo.POWERLED;
                        break;
                    }
                case "ledPon": {
                        GlobalData.testingInfo.PONLED = !GlobalData.testingInfo.PONLED;
                        break;
                    }
                case "ledInet": {
                        GlobalData.testingInfo.INETLED = !GlobalData.testingInfo.INETLED;
                        break;
                    }
                case "ledWlan": {
                        GlobalData.testingInfo.WLANLED = !GlobalData.testingInfo.WLANLED;
                        break;
                    }
                case "ledLan1": {
                        GlobalData.testingInfo.LAN1LED = !GlobalData.testingInfo.LAN1LED;
                        break;
                    }
                case "ledLan2": {
                        GlobalData.testingInfo.LAN2LED = !GlobalData.testingInfo.LAN2LED;
                        break;
                    }
                case "ledLan3": {
                        GlobalData.testingInfo.LAN3LED = !GlobalData.testingInfo.LAN3LED;
                        break;
                    }
                case "ledLan4": {
                        GlobalData.testingInfo.LAN4LED = !GlobalData.testingInfo.LAN4LED;
                        break;
                    }
                case "ledWps": {
                        GlobalData.testingInfo.WPSLED = !GlobalData.testingInfo.WPSLED;
                        break;
                    }
                case "ledLos": {
                        GlobalData.testingInfo.LOSLED = !GlobalData.testingInfo.LOSLED;
                        break;
                    }
            }
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e) {
            //if (MessageBox.Show("Bạn đã chắc chắn với kết quả này?","CẢNH BÁO!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
            bool ret = GlobalData.testingInfo.POWERLED &&
                       GlobalData.testingInfo.PONLED &&
                       GlobalData.testingInfo.INETLED &&
                       GlobalData.testingInfo.WLANLED &&
                       GlobalData.testingInfo.LAN1LED &&
                       GlobalData.testingInfo.LAN2LED &&
                       GlobalData.testingInfo.LAN3LED &&
                       GlobalData.testingInfo.LAN4LED &&
                       GlobalData.testingInfo.WPSLED &&
                       GlobalData.testingInfo.LOSLED;

            GlobalData.ledResult = ret == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedPower = GlobalData.testingInfo.POWERLED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedPon = GlobalData.testingInfo.PONLED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedInet = GlobalData.testingInfo.INETLED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedWlan = GlobalData.testingInfo.WLANLED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedLan1 = GlobalData.testingInfo.LAN1LED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedLan2 = GlobalData.testingInfo.LAN2LED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedLan3 = GlobalData.testingInfo.LAN3LED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedLan4 = GlobalData.testingInfo.LAN4LED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedWps = GlobalData.testingInfo.WPSLED == true ? "PASS" : "FAIL";
            GlobalData.loginfo.LedLos = GlobalData.testingInfo.LOSLED == true ? "PASS" : "FAIL";
            //}
        }
    }
}
