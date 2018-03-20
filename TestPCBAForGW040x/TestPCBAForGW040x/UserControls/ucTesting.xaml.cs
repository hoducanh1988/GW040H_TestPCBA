using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TestPCBAForGW040x.Functions;
using TestPCBAForGW040x.SubControls;

namespace TestPCBAForGW040x.UserControls {
    /// <summary>
    /// Interaction logic for ucTesting.xaml
    /// </summary>
    public partial class ucTesting : UserControl {
        //DispatcherTimer timer;


        public ucTesting() {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "1", STEPCHECK = "Nạp Firmware", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "2", STEPCHECK = "Ghi địa chỉ MAC", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "3", STEPCHECK = "Ghi mã GPON", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "4", STEPCHECK = "Ghi WPS PIN", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "5", STEPCHECK = "Kiểm tra USB", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "6", STEPCHECK = "Kiểm tra LAN", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "7", STEPCHECK = "Kiểm tra nút WPS", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "8", STEPCHECK = "Kiểm tra nút Reset", RESULT = "PASS" });
            //GlobalData.GridContent.Add(new GridTestContent() { ID = "9", STEPCHECK = "Kiểm tra LED", RESULT = "FAIL" });
            //this.datagrid.ItemsSource = GlobalData.GridContent;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += ((sender, e) => {
                GlobalData.testingInfo.LOGCOUNTER = GlobalData.testingInfo.DATAUART.Split('\n').Length.ToString();
                if (int.Parse(GlobalData.testingInfo.LOGCOUNTER) > 10) GlobalData.testingInfo.DATAUART = "";
                //_scrollViewer1.ScrollToEnd();
                if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight) {
                    _scrollViewer.ScrollToEnd();
                }
            });
            timer.Start();
        }

        //private void changeForeGround() {
        //    App.Current.Dispatcher.Invoke((Action)delegate {
        //        foreach (GridTestContent dataItem in GlobalData.GridContent) {
        //            if (dataItem.RESULT == "FAIL") {
        //                try {
        //                    var gridRow = datagrid.ItemContainerGenerator.ContainerFromItem(dataItem) as DataGridRow;
        //                    gridRow.Foreground = Brushes.Red;
        //                }
        //                catch { }
        //            }
        //            if (dataItem.RESULT == "PASS") {
        //                try {
        //                    var gridRow = datagrid.ItemContainerGenerator.ContainerFromItem(dataItem) as DataGridRow;
        //                    gridRow.Foreground = Brushes.Lime;
        //                }
        //                catch { }
        //            }
        //        }
        //    });
        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            txtMAC.Focus();
        }

        private void txtMAC_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                string subStr = txtMAC.Text.Trim();
                GlobalData.testingInfo.LOGSYSTEM += string.Format("... MAC=\"{0}\"\n", subStr);
                if (subStr == string.Empty || subStr == "") {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Phán định: FAIL\n", subStr);
                }
                else {
                    bool ret = new exWriteMAC(subStr).IsValid();
                    if (ret) {
                        txtMAC.IsEnabled = false;
                        GlobalData.loginfo = new LogInfomation();
                        //Timer change background
                        DispatcherTimer timer = new DispatcherTimer();
                        int counter = 0;
                        int time = 0;
                        timer.Interval = new TimeSpan(0, 0, 1);
                        timer.Tick += ((sd, ev) => {
                            time++;
                            TimeSpan span = new TimeSpan(0, 0, time);
                            GlobalData.testingInfo.ELAPSEDTIME = string.Format("Thời gian kiểm tra: {0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
                            if (GlobalData.testingInfo.TITLE == Titles.powerON ||
                                GlobalData.testingInfo.TITLE == Titles.checkNutWPS ||
                                GlobalData.testingInfo.TITLE == Titles.checkNutReset ||
                                GlobalData.testingInfo.TITLE == Titles.checkLED) {
                                counter++;
                                if (counter % 2 == 0) this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#AAAAAA"));
                                else this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#777777"));
                            }
                            else {
                                this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#777777"));
                                counter = 0;
                            }
                        });
                        timer.Start();

                        //start test PCBA
                        Thread t = new Thread(new ThreadStart(() => {
                            string message = "";
                            GlobalData.testingInfo.STATUS = Statuses.wait;
                            //1.Upload Firmware //23.02
                            GlobalData.testingInfo.LOGSYSTEM += "> UPLOAD FIRMWARE----------\n";
                            if (!(new exUploadFirmware().Excute(ref message))) goto NG;
                            //2.Write MAC //23.02
                            GlobalData.testingInfo.LOGSYSTEM += "> GHI SN,GPON,WPS,MAC------\n";
                            if (!(new exWriteMAC(GlobalData.testingInfo.MAC).Excute(ref message))) goto NG;
                            //3.Check LAN //24.02
                            GlobalData.testingInfo.LOGSYSTEM += "> CHECK CỔNG LAN-----------\n";
                            if (!(new exCheckLAN().Excute(ref message))) goto NG;
                            //4.Check USB //24.02
                            GlobalData.testingInfo.LOGSYSTEM += "> CHECK CỔNG USB-----------\n";
                            if (!(new exCheckUSB().Excute(ref message))) goto NG;
                            //5.Check Button //26/02
                            GlobalData.testingInfo.LOGSYSTEM += "> CHECK NÚT NHẤN-----------\n";
                            if (!(new exCheckButton().Excute(ref message))) goto NG;
                            //6.Check LED //26/02
                            GlobalData.testingInfo.LOGSYSTEM += "> CHECK LEDS---------------\n";
                            if (!(new exCheckLED().Excute(ref message))) goto NG;
                            goto OK;

                            NG:
                            {

                                GlobalData.serialPort.closeSerialPort(out message);
                                GlobalData.testingInfo.STATUS = Statuses.fail;
                                GlobalData.testingInfo.LOGSYSTEM += "-----------------------\n";
                                GlobalData.testingInfo.LOGSYSTEM += "=> Tổng phán định: FAIL\n";
                                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                                    this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#777777"));
                                    timer.Stop();
                                    txtMAC.IsEnabled = true;
                                    txtMAC.Focus();
                                    txtMAC.SelectAll();
                                }));
                                GlobalData.loginfo.Save("FAIL", message);
                                return;
                            }
                            OK:
                            {
                                GlobalData.testingInfo.TITLE = Titles.checkComplete;
                                GlobalData.testingInfo.CONTENT = "--";
                                GlobalData.serialPort.closeSerialPort(out message);
                                GlobalData.testingInfo.STATUS = Statuses.pass;
                                GlobalData.testingInfo.LOGSYSTEM += "-----------------------\n";
                                GlobalData.testingInfo.LOGSYSTEM += "=> Tổng phán định: PASS\n";
                                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                                    this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#777777"));
                                    timer.Stop();
                                    txtMAC.IsEnabled = true;
                                    txtMAC.Focus();
                                    txtMAC.SelectAll();
                                }));
                                GlobalData.loginfo.Save("PASS", "--");
                                return;
                            }

                        }));
                        t.IsBackground = true;
                        t.Start();
                    }
                    else {
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Phán định: FAIL\n\n", subStr);
                        txtMAC.Clear();
                    }
                }
            }
        }

        private void txtMAC_TextChanged(object sender, TextChangedEventArgs e) {
            if (txtMAC.Text.Length > 0)
                GlobalData.testingInfo.Initialize();
        }

        private void lblTestTitle_MouseDown(object sender, MouseButtonEventArgs e) {
           
        }
    }
}