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
            this.datagrid.ItemsSource = GlobalData.datagridcontent;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += ((sender, e) => {
                GlobalData.testingInfo.LOGCOUNTER = GlobalData.testingInfo.DATAUART.Split('\n').Length.ToString();
                if (int.Parse(GlobalData.testingInfo.LOGCOUNTER) > 10) GlobalData.testingInfo.DATAUART = "";
                _scrollViewer1.ScrollToEnd();
                if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight) {
                    _scrollViewer.ScrollToEnd();
                }
            });
            timer.Start();
        }

        private void updateGridTest(string _id, string _Result, string _errCode) {
            App.Current.Dispatcher.Invoke((Action)delegate{
                foreach (var item in GlobalData.datagridcontent) {
                    if(item.ID == _id) {
                        if (_Result !="" && _Result !=string.Empty) item.RESULT = _Result;
                        if (_errCode !="" && _errCode!=string.Empty) item.ERROR = _errCode;
                        this.datagrid.Items.Refresh();
                        break;
                    }
                }
            });
        }

        private void ResetGridTest() {
            App.Current.Dispatcher.Invoke((Action)delegate {
                foreach (var item in GlobalData.datagridcontent) {
                    item.RESULT = "-";
                    item.ERROR = "-";
                    this.datagrid.Items.Refresh();
                }
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            txtMAC.Focus();
        }

        private void txtMAC_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Return) {
                string subStr = txtMAC.Text.Trim();
                GlobalData.testingInfo.LOGSYSTEM += string.Format("... MAC=\"{0}\"\r\n", subStr);
                if (subStr == string.Empty || subStr == "") {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Phán định: FAIL\r\n", subStr);
                } else {
                    bool ret = GlobalData.initSetting.EnableCheckMAC == false ? new exWriteMAC(subStr).IsValid() : new exWriteMAC(subStr).IsValid() && new exWriteMAC(subStr).lastByteOfMacIsEvenNumber();
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
                                if (counter % 2 == 0) this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
                                else this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#777777"));
                            }
                            else {
                                this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
                                counter = 0;
                            }
                        });
                        timer.Start();

                        //start test PCBA
                        Thread t = new Thread(new ThreadStart(() => {
                            string message = "";
                            string step = "";
                            GlobalData.testingInfo.STATUS = Statuses.wait;

                            //1.Upload Firmware //23.02
                            if (GlobalData.initSetting.EnableUploadFirmware == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> UPLOAD FIRMWARE----------\r\n";
                                this.updateGridTest("01", "Waiting", "");
                                if (!(new exUploadFirmware().Excute(ref message))) {
                                    this.updateGridTest("01", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Upload Firmware";
                                    goto NG;
                                }
                                this.updateGridTest("01", "PASS", "");
                            }
                           
                            //2.Check LAN //24.02
                            if (GlobalData.initSetting.EnableCheckLAN == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> CHECK CỔNG LAN-----------\r\n";
                                this.updateGridTest("02", "Waiting", "");
                                if (!(new exCheckLAN().Excute1(ref message))) {
                                    this.updateGridTest("02", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Check LAN";
                                    goto NG;
                                }
                                this.updateGridTest("02", "PASS", "");
                            }
                            
                            //3.Check USB //24.02
                            if (GlobalData.initSetting.EnableCheckUSB == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> CHECK CỔNG USB-----------\r\n";
                                this.updateGridTest("03", "Waiting", "");
                                if (!(new exCheckUSB().Excute1(ref message))) {
                                    this.updateGridTest("03", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Check USB";
                                    goto NG;
                                }
                                this.updateGridTest("03", "PASS", "");
                            }
                           
                            //4.Check LED //26/02
                            if (GlobalData.initSetting.EnableCheckLED == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> CHECK LEDS---------------\r\n";
                                this.updateGridTest("04", "Waiting", "");
                                if (!(new exCheckLED().Excute(ref message))) {
                                    this.updateGridTest("04", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Check LEDs";
                                    goto NG;
                                }
                                this.updateGridTest("04", "PASS", "");
                            }
                            
                            //5.Check Button //26/02
                            if (GlobalData.initSetting.EnableCheckButton == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> CHECK NÚT NHẤN-----------\r\n";
                                this.updateGridTest("05", "Waiting", "");
                                if (!(new exCheckButton().Excute(ref message))) {
                                    this.updateGridTest("05", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Check BUTTONs";
                                    goto NG;
                                }
                                this.updateGridTest("05", "PASS", "");
                            }
                            
                            //6.Write MAC //23.02
                            if (GlobalData.initSetting.EnableWriteMAC == true) {
                                GlobalData.testingInfo.LOGSYSTEM += "> GHI SN,GPON,WPS,MAC------\r\n";
                                this.updateGridTest("06", "Waiting", "");
                                if (!(new exWriteMAC(GlobalData.testingInfo.MAC).Excute1(ref message))) {
                                    this.updateGridTest("06", "FAIL", GlobalData.testingInfo.ERRORCODE);
                                    step = "Write MAC";
                                    goto NG;
                                }
                                this.updateGridTest("06", "PASS", "");
                            }
                            
                            goto OK;

                            NG:
                            {
                                
                                GlobalData.serialPort.closeSerialPort(out message);
                                GlobalData.testingInfo.STATUS = Statuses.fail;
                                GlobalData.testingInfo.LOGSYSTEM += "-----------------------\r\n";
                                GlobalData.testingInfo.LOGSYSTEM += "=> Tổng phán định: FAIL\r\n";
                                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                                    this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
                                    timer.Stop();
                                    txtMAC.IsEnabled = true;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                GlobalData.testingInfo.TITLE = string.Format("ONT: {0}", GlobalData.testingInfo.MAC);
                                GlobalData.testingInfo.CONTENT = string.Format("Lỗi: {0}", step);
                                GlobalData.loginfo.Save("FAIL", message);
                                GlobalData.loginfo.SaveSystemLog();
                                GlobalData.loginfo.SaveUARTLog();
                                return;
                            }
                            OK:
                            {
                                GlobalData.testingInfo.TITLE = Titles.checkComplete;
                                GlobalData.testingInfo.CONTENT = "--";
                                GlobalData.serialPort.closeSerialPort(out message);
                                GlobalData.testingInfo.STATUS = Statuses.pass;
                                GlobalData.testingInfo.LOGSYSTEM += "-----------------------\r\n";
                                GlobalData.testingInfo.LOGSYSTEM += "=> Tổng phán định: PASS\r\n";
                                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                                    this.testBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
                                    timer.Stop();
                                    txtMAC.IsEnabled = true;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                GlobalData.testingInfo.TITLE = string.Format("ONT: {0}", GlobalData.testingInfo.MAC);
                                GlobalData.testingInfo.CONTENT = string.Format("Sản phẩm là OK");
                                GlobalData.loginfo.Save("PASS", "--");
                                GlobalData.loginfo.SaveSystemLog();
                                GlobalData.loginfo.SaveUARTLog();
                                return;
                            }

                        }));
                        t.IsBackground = true;
                        t.Start();
                    }
                    else {
                        string _msg = "";
                        GlobalData.testingInfo.TITLE = string.Format("MAC: {0}", subStr);
                        GlobalData.testingInfo.CONTENT = "Không hợp lệ";
                        GlobalData.serialPort.closeSerialPort(out _msg);
                        GlobalData.testingInfo.STATUS = Statuses.fail;
                        GlobalData.testingInfo.LOGSYSTEM += "-----------------------\r\n";
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> Phán định: FAIL\r\n\r\n", subStr);
                        txtMAC.Clear();
                    }
                }
            }
        }

        private void txtMAC_TextChanged(object sender, TextChangedEventArgs e) {
            if (txtMAC.Text.Length > 0) {
                GlobalData.testingInfo.Initialize();
                ResetGridTest();
            }
        }

        private void lblTestTitle_MouseDown(object sender, MouseButtonEventArgs e) {
            //double top, left, width, height;
            //double ht = GlobalData.thisLocation.height - 238;
            //top = GlobalData.thisLocation.top + 160 + ht / 3.5;
            //left = GlobalData.thisLocation.left + 17;
            //width = GlobalData.thisLocation.width - 35;
            //height = (ht / 3.5) * 2.5 - 72;
            //Application.Current.Dispatcher.BeginInvoke(new Action(() => {
            //    LED led = new LED(top, left, width, height);
            //    led.ShowDialog();
            //}));
        }

        private void datagrid_LostFocus(object sender, RoutedEventArgs e) {
            this.datagrid.UnselectAllCells();
        }

    }
}