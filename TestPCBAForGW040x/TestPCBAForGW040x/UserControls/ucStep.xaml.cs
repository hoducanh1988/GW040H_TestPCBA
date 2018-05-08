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
using TestPCBAForGW040x.Functions;

namespace TestPCBAForGW040x.UserControls {
    /// <summary>
    /// Interaction logic for ucStep.xaml
    /// </summary>
    public partial class ucStep : UserControl {
        baseFunctionNoLegend ba = new baseFunctionNoLegend();

        public ucStep() {
            InitializeComponent();
            this.DataContext = GlobalData.testingInfo;
        }

        #region Change Tab Index

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Dispatcher.Invoke(new Action(() => {
                TabControl t = sender as TabControl;
                switch (t.SelectedIndex) {
                    case 0: { //Upload FW
                            ClearFWContent();
                            break;
                        }
                    case 1: { //Ghi MAC
                            ClearMACContent();
                            break;
                        }
                    case 2: { //Check LAN
                            ClearLANContent();
                            break;
                        }
                    case 3: { //Check USB
                            ClearUSBContent();
                            break;
                        }
                    case 4: { //Check LED
                            rtbLED.Document.Blocks.Clear();
                            break;
                        }
                    case 5: { //Check Button
                            ClearButtonContent();
                            break;
                        }
                }

            }));
        }

        #endregion

        #region WriteDebug
        RichTextBox rtb = new RichTextBox();

        private void debugWriteLine(string data) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(data + "\r\n");
                rtb.ScrollToEnd();
            }));
        }
        private void debugWriteStep(string data) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(string.Format("- {0}, {1}...\r\n", DateTime.Now.ToString("HH:mm:ss ffff"), data));
                rtb.ScrollToEnd();
            }));
        }
        private void debugWriteResult(bool result) {
            Dispatcher.Invoke(new Action(() => {
                rtb.AppendText(string.Format("...Kết quả={0}\r\n", result == true ? "Thành công" : "Thất bại"));
                rtb.ScrollToEnd();
            }));
        }
        #endregion

        //OK
        #region NẠP FIRMWARE

        //Clear FW Content
        private void ClearFWContent() {
            rtbFW.Document.Blocks.Clear();
            lblFWResult.Content = "--";
            lblFWMessage.Content = "--";
        }


        /// <summary>
        /// ĐỌC THÔNG TIN FIRMWARE VERSION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFW_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                ClearFWContent();
                string message = "";
                lblFWResult.Content = "Waiting";
                btnReadFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
                btnReadFW.Content = "Đang đọc Firmware Version";
                rtb = rtbFW;
                debugWriteLine("ĐỌC PHIÊN BẢN FIRMWARE--------------\r\n");
                debugWriteLine(">");
                Thread t = new Thread(new ThreadStart(() => {
                    //Mở cổng COM
                    debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "MỞ CỔNG UART..."));
                    if (!GlobalData.serialPort.openSerialPort(out message)) {
                        debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                        debugWriteLine(string.Format("{0}", message));
                        goto NG;
                    }
                    debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                    //Login vào ONT
                    debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "LOGIN VÀO ONT..."));
                    if (!ba.login_toDUT(out message)) {
                        debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                        debugWriteLine(string.Format("{0}", message));
                        goto NG;
                    }
                    debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                    //Lấy thông tin Firmware Version
                    debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "ĐỌC FIRWARE VERSION..."));
                    Dispatcher.Invoke(new Action(() => {
                        string subStr = ba.get_FWVersion(out message);
                        subStr = subStr.Trim().Replace("\r\n", "").Replace("\r", "");
                        debugWriteLine(string.Format("...Kết quả={0}", subStr));
                        lblFWMessage.Content = subStr;
                    }));
                    goto OK;
                    //---------------------------------------------//
                    NG:
                    {
                        Dispatcher.Invoke(new Action(() => {
                            btnReadFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                            btnReadFW.Content = "Đọc Firmware Version";
                            lblFWResult.Content = "FAIL";
                            debugWriteLine("<");
                        }));
                        MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        //Đóng cổng COM
                        GlobalData.serialPort.closeSerialPort(out message);
                        return;
                    }
                    OK:
                    {
                        Dispatcher.Invoke(new Action(() => {
                            btnReadFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                            btnReadFW.Content = "Đọc Firmware Version";
                            lblFWResult.Content = "PASS";
                            debugWriteLine("<");
                        }));
                        MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        //Đóng cổng COM
                        GlobalData.serialPort.closeSerialPort(out message);
                        return;
                    }
                }));
                t.IsBackground = true;
                t.Start();
            }
            else return;
        }

        /// <summary>
        /// UPLOAD FIRMWARE CHO ONT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpFW_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Vui lòng tắt nguồn ONT trước khi nạp Firmware.\r\nChọn 'Yes' để tiếp tục, 'No' để hủy.", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            ClearFWContent();
            string message = "";
            lblFWResult.Content = "Waiting";
            btnUpFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            btnUpFW.Content = "Đang nạp Firmware";
            rtb = rtbFW;
            debugWriteLine("NẠP FIRMWARE--------------\r\n");
            debugWriteLine(">");

            Thread t = new Thread(new ThreadStart(() => {
                //Mở cổng COM
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "MỞ CỔNG UART..."));
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                //Đợi DUT online
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "BẬT NGUỒN ONT..."));
                if (!ba.wait_DUT_Online(out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                //Truy cap vao uboot
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "TRUY CẬP VÀO UBOOT..."));
                if (!ba.access_toUboot(out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                //Thiet lap dia chi IP
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "THIẾT LẬP IP..."));
                if (!ba.set_FTPServer_IPAddress(out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                //Ping mang toi ONT
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "PING MẠNG TỚI ONT..."));
                if (!ba.pingToIPAddress(GlobalData.initSetting.DutIPUploadFW, out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                //Nap FW
                debugWriteLine(string.Format("- {0}, {1}", DateTime.Now.ToString("HH:mm:ss ffff"), "NẠP FIRMWARE CHO ONT..."));
                if (!ba.putFirm_ThroughWPS(out message)) {
                    debugWriteLine(string.Format("...Kết quả={0}", "Thất bại"));
                    debugWriteLine(string.Format("{0}", message));
                    goto NG;
                }
                debugWriteLine(string.Format("...Kết quả={0}", "Thành công"));

                goto OK;
                NG:
                {
                    Dispatcher.Invoke(new Action(() => {
                        btnUpFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        btnUpFW.Content = "Nạp Firmware";
                        lblFWResult.Content = "FAIL";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
                OK:
                {
                    Dispatcher.Invoke(new Action(() => {
                        btnUpFW.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        btnUpFW.Content = "Nạp Firmware";
                        lblFWResult.Content = "PASS";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        }
        #endregion

        //OK
        #region GHI ĐỊA CHỈ GPON, WPS, MAC

        //Clear MAC Content
        private void ClearMACContent() {
            rtbMAC.Document.Blocks.Clear();
            lblMACResult.Content = "--";
            lblMACMessage.Content = "--";
            txtMAC.Text = "";
            txtMAC.Focus();
        }

        private void MAC_btnWrite_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;

            //kiểm tra địa chỉ MAC hợp lệ hay ko?
            string mac = txtMAC.Text.Trim();
            if (mac == "" || mac == string.Empty) {
                MessageBox.Show("Vui lòng nhập địa chỉ MAC trước.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ba.MacIsValid(mac)) {
                MessageBox.Show("Địa chỉ MAC nhập vào không đúng tiêu chuẩn.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMAC.Clear();
                txtMAC.Focus();
                return;
            }
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                txtMAC.Clear();
                txtMAC.Focus();
                return;
            }
            //
            string message = "";
            rtbMAC.Document.Blocks.Clear();
            lblMACMessage.Content = "--";
            lblMACResult.Content = "Waiting";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbMAC;


            switch (b.Name) {
                case "btnWriteMAC": {
                        b.Content = "Đang ghi địa chỉ MAC";
                        debugWriteLine("GHI ĐỊA CHỈ MAC--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //ghi ma GPON
                            debugWriteStep("GHI MÃ GPON");
                            if (!ba.setGPONSerialNumber(mac, out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Ghi địa chỉ MAC
                            debugWriteStep("GHI ĐỊA CHỈ MAC");
                            if (!ba.setMac_forEthernet0(mac, out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    b.Content = "Ghi địa chỉ MAC";
                                    lblMACResult.Content = "FAIL";
                                    debugWriteLine("<");
                                    lblMACMessage.Content = mac;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    b.Content = "Ghi địa chỉ MAC";
                                    lblMACResult.Content = "PASS";
                                    debugWriteLine("<");
                                    lblMACMessage.Content = mac;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }

                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnWriteGPON": {
                        //b.Content = "Đang ghi mã GPON SN";
                        //debugWriteLine("GHI MÃ GPON SERIAL--------------\r\n");
                        //debugWriteLine(">");

                        //Thread t = new Thread(new ThreadStart(() => {
                        //    //Mở cổng COM
                        //    debugWriteStep("MỞ CỔNG UART");
                        //    if (!GlobalData.serialPort.openSerialPort(out message)) {
                        //        debugWriteResult(false);
                        //        debugWriteLine(message);
                        //        goto NG;
                        //    }
                        //    debugWriteResult(true);
                        //    //Login
                        //    debugWriteStep("LOGIN VÀO ONT");
                        //    if (!ba.login_toDUT(out message)) {
                        //        debugWriteResult(false);
                        //        debugWriteLine(message);
                        //        goto NG;
                        //    }
                        //    debugWriteResult(true);
                        //    //ghi ma GPON
                        //    debugWriteStep("GHI MÃ GPON");
                        //    if (!ba.setGPONSerialNumber(mac, out message)) {
                        //        debugWriteResult(false);
                        //        debugWriteLine(message);
                        //        goto NG;
                        //    }
                        //    debugWriteResult(true);
                        //    goto OK;
                        //    NG:
                        //    {
                        //        Dispatcher.Invoke(new Action(() => {
                        //            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        //            b.Content = "Ghi mã GPON SN";
                        //            lblMACResult.Content = "FAIL";
                        //            debugWriteLine("<");
                        //            lblMACMessage.Content = mac;
                        //            txtMAC.Clear();
                        //            txtMAC.Focus();
                        //        }));
                        //        MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        //        //Đóng cổng COM
                        //        GlobalData.serialPort.closeSerialPort(out message);
                        //        return;
                        //    }
                        //    OK:
                        //    {
                        //        Dispatcher.Invoke(new Action(() => {
                        //            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        //            b.Content = "Ghi mã GPON SN";
                        //            lblMACResult.Content = "PASS";
                        //            debugWriteLine("<");
                        //            lblMACMessage.Content = mac;
                        //            txtMAC.Clear();
                        //            txtMAC.Focus();
                        //        }));
                        //        MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        //        //Đóng cổng COM
                        //        GlobalData.serialPort.closeSerialPort(out message);
                        //        return;
                        //    }

                        //}));
                        //t.IsBackground = true;
                        //t.Start();
                        break;
                    }
                case "btnWriteWPS": {
                        break;
                    }
            }
        }

        private void MAC_btnRead_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            //-------------------------------------//
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            string message = "";
            rtbMAC.Document.Blocks.Clear();
            lblMACMessage.Content = "--";
            lblMACResult.Content = "Waiting";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbMAC;
            //-------------------------------------//
            switch (b.Name) {
                case "btnReadMAC": {
                        b.Content = "Đang đọc địa chỉ MAC";
                        debugWriteLine("ĐỌC ĐỊA CHỈ MAC--------------\r\n");
                        debugWriteLine(">");
                        string mac = "";

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Doc dia chi MAC
                            debugWriteStep("ĐỌC ĐỊA CHỈ MAC");
                            mac = ba.getMAC(out message);
                            debugWriteLine(mac);
                            debugWriteResult(true);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    b.Content = "Đọc địa chỉ MAC";
                                    lblMACResult.Content = "FAIL";
                                    debugWriteLine("<");
                                    lblMACMessage.Content = mac;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    b.Content = "Đọc địa chỉ MAC";
                                    lblMACResult.Content = "PASS";
                                    debugWriteLine("<");
                                    lblMACMessage.Content = mac;
                                    txtMAC.Clear();
                                    txtMAC.Focus();
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnReadGPON": {
                        break;
                    }
                case "btnReadWPS": {
                        break;
                    }
            }
        }

        #endregion

        #region KIỂM TRA CỔNG LAN

        void ClearLANContent() {
            rtbLAN.Document.Blocks.Clear();
            lblLAN1.Content = "--";
            lblLAN2.Content = "--";
            lblLAN3.Content = "--";
            lblLAN4.Content = "--";
            lblLANTotal.Content = "--";
        }

        private void btnCheckLAN_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            //-----------------------------------//
            string message = "";
            rtbLAN.Document.Blocks.Clear();
            lblLAN1.Content = "Waiting";
            lblLAN2.Content = "Waiting";
            lblLAN3.Content = "Waiting";
            lblLAN4.Content = "Waiting";
            lblLANTotal.Content = "Waiting";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbLAN;
            //-----------------------------------//
            b.Content = "Đang kiểm tra cổng LAN";
            debugWriteLine("KIỂM TRA CỔNG LAN--------------\r\n");
            debugWriteLine(">");

            Thread t = new Thread(new ThreadStart(() => {
                //Mở cổng COM
                debugWriteStep("MỞ CỔNG UART");
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Login
                debugWriteStep("LOGIN VÀO ONT");
                if (!ba.login_toDUT(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Kiem tra cong LAN
                debugWriteStep("KIỂM TRA CỔNG LAN");
                bool lan1 = false, lan2 = false, lan3 = false, lan4 = false;
                bool ret = ba.checkLANPorts(ref lan1, ref lan2, ref lan3, ref lan4, out message);
                debugWriteLine(message);
                debugWriteResult(true);
                Dispatcher.Invoke(new Action(() => {
                    lblLAN1.Content = lan1 == true ? "PASS" : "FAIL";
                    lblLAN2.Content = lan2 == true ? "PASS" : "FAIL";
                    lblLAN3.Content = lan3 == true ? "PASS" : "FAIL";
                    lblLAN4.Content = lan4 == true ? "PASS" : "FAIL";
                    lblLANTotal.Content = ret == true ? "PASS" : "FAIL";
                }));
                goto OK;
                NG:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra cổng LAN";
                        debugWriteLine("<");
                        if (lblLAN1.Content.ToString() == "Waiting") {
                            lblLAN1.Content = "FAIL";
                            lblLAN2.Content = "FAIL";
                            lblLAN3.Content = "FAIL";
                            lblLAN4.Content = "FAIL";
                            lblLANTotal.Content = "FAIL";
                        }
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
                OK:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra cổng LAN";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        #region KIỂM TRA CỔNG USB

        void ClearUSBContent() {
            rtbUSB.Document.Blocks.Clear();
            lblUSB2.Content = "--";
            lblUSB3.Content = "--";
            lblUSBTotal.Content = "--";
        }

        private void btnCheckUSB_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            //-----------------------------------//
            string message = "";
            rtbUSB.Document.Blocks.Clear();
            lblUSB2.Content = "Waiting";
            lblUSB3.Content = "Waiting";
            lblUSBTotal.Content = "Waiting";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbUSB;
            //-----------------------------------//
            b.Content = "Đang kiểm tra cổng USB";
            debugWriteLine("KIỂM TRA CỔNG USB--------------\r\n");
            debugWriteLine(">");

            Thread t = new Thread(new ThreadStart(() => {
                //Mở cổng COM
                debugWriteStep("MỞ CỔNG UART");
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Login
                debugWriteStep("LOGIN VÀO ONT");
                if (!ba.login_toDUT(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Kiem tra cong USB
                debugWriteStep("KIỂM TRA CỔNG USB");
                bool usb2 = false, usb3 = false;
                bool ret = ba.checkUSBPorts(ref usb2, ref usb3, out message);
                debugWriteLine(message);
                debugWriteResult(true);
                Dispatcher.Invoke(new Action(() => {
                    lblUSB2.Content = usb2 == true ? "PASS" : "FAIL";
                    lblUSB3.Content = usb3 == true ? "PASS" : "FAIL";
                    lblUSBTotal.Content = ret == true ? "PASS" : "FAIL";
                }));
                goto OK;
                NG:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra cổng USB";
                        debugWriteLine("<");
                        if (lblUSB2.Content.ToString() == "Waiting") {
                            lblUSB2.Content = "FAIL";
                            lblUSB3.Content = "FAIL";
                            lblUSBTotal.Content = "FAIL";
                        }
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
                OK:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra cổng USB";
                        debugWriteLine("<");
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }


        #endregion

        #region KIỂM TRA LED

        private void btnLED_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            //-----------------------------------//
            string message = "";
            rtbLED.Document.Blocks.Clear();
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbLED;

            switch (b.Name) {
                case "btnLedPON": {
                        debugWriteLine("BẬT LED POWER--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED PON");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("sys memwl bfbf0204 0x2");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnLedINETX": {
                        debugWriteLine("BẬT LED INET XANH--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED INET XANH");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("echo \"1 0\" > proc/tc3162/led_internet");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnLedINETD": {
                        debugWriteLine("BẬT LED INET ĐỎ--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED INET ĐỎ");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("echo \"0 1\" > proc/tc3162/led_internet");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnLedWLAN": {
                        debugWriteLine("BẬT LED WLAN--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED WLAN");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("iwpriv ra0 set led_setting=00-00-00-00-00-00-00-00");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnLedWPS": {
                        debugWriteLine("BẬT LED WPS--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED WPS");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("iwpriv ra0 set led_setting=01-00-00-00-00-00-00-00");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "btnLedLOS": {
                        debugWriteLine("BẬT LED LOS--------------\r\n");
                        debugWriteLine(">");

                        Thread t = new Thread(new ThreadStart(() => {
                            //Mở cổng COM
                            debugWriteStep("MỞ CỔNG UART");
                            if (!GlobalData.serialPort.openSerialPort(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Login
                            debugWriteStep("LOGIN VÀO ONT");
                            if (!ba.login_toDUT(out message)) {
                                debugWriteResult(false);
                                debugWriteLine(message);
                                goto NG;
                            }
                            debugWriteResult(true);
                            //Bat led PON
                            debugWriteStep("BẬT LED LOS");
                            GlobalData.testingInfo.LOGUART = "";
                            GlobalData.serialPort.Port.WriteLine("echo 1 > /proc/xpon/los_led");
                            Thread.Sleep(300);
                            debugWriteResult(true);
                            debugWriteLine(GlobalData.testingInfo.LOGUART);
                            goto OK;
                            NG:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                            OK:
                            {
                                Dispatcher.Invoke(new Action(() => {
                                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                                    debugWriteLine("<");
                                }));
                                MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                //Đóng cổng COM
                                GlobalData.serialPort.closeSerialPort(out message);
                                return;
                            }
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }
        }

        #endregion

        #region KIỂM TRA NÚT NHẤN

        void ClearButtonContent() {
            rtbButton.Document.Blocks.Clear();
            lblButtonLegend.Content = "--";
            lblWPS.Content = "--";
            lblReset.Content = "--";
            lblButtonTotal.Content = "--";
        }

        private void btnCheckButton_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            if (MessageBox.Show("Đèn WLAN của ONT đã sáng màu xanh đúng không?", "Waring!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                return;
            }
            //-----------------------------------//
            string message = "";
            rtbButton.Document.Blocks.Clear();
            lblWPS.Content = "Waiting";
            lblReset.Content = "Waiting";
            lblButtonTotal.Content = "Waiting";
            lblButtonLegend.Content = "--";
            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3aed1e");
            rtb = rtbButton;
            b.Content = "Đang kiểm tra nút nhấn";
            debugWriteLine("KIỂM TRA NÚT NHẤN--------------\r\n");
            debugWriteLine(">");
            //-----------------------------------//
            Thread t = new Thread(new ThreadStart(() => {
                //Mở cổng COM
                debugWriteStep("MỞ CỔNG UART");
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Login
                debugWriteStep("LOGIN VÀO ONT");
                if (!ba.login_toDUT(out message)) {
                    debugWriteResult(false);
                    debugWriteLine(message);
                    goto NG;
                }
                debugWriteResult(true);
                //Kiểm tra nút WPS
                GlobalData.testingInfo.LOGUART = "";
                Dispatcher.Invoke(new Action(() => { lblButtonLegend.Content = "VUI LÒNG NHẤN NÚT WPS"; }));
                int index = 0;
                while (!GlobalData.testingInfo.LOGUART.Contains(completeString.wpsPressed)) {
                    Thread.Sleep(1000);
                    if (index < Timeouts.longtime) index++;
                    else break;
                }
                debugWriteLine(GlobalData.testingInfo.LOGUART);
                if (index < Timeouts.longtime) { debugWriteResult(true);
                    Dispatcher.Invoke(new Action(() => { lblWPS.Content = "PASS"; }));
                }
                else { debugWriteResult(false);
                    Dispatcher.Invoke(new Action(() => { lblWPS.Content = "FAIL"; lblReset.Content = "--"; lblButtonTotal.Content = "FAIL"; }));
                    goto NG; }

                //Kiểm tra nút Reset
                GlobalData.testingInfo.LOGUART = "";
                Dispatcher.Invoke(new Action(() => { lblButtonLegend.Content = "VUI LÒNG NHẤN NÚT RESET"; }));
                index = 0;
                while (!GlobalData.testingInfo.LOGUART.Contains(completeString.resetPressed)) {
                    Thread.Sleep(1000);
                    if (index < Timeouts.longtime) index++;
                    else break;
                }
                debugWriteLine(GlobalData.testingInfo.LOGUART);
                if (index < Timeouts.longtime) {
                    debugWriteResult(true);
                    Dispatcher.Invoke(new Action(() => { lblReset.Content = "PASS"; lblButtonTotal.Content = "PASS"; }));
                }
                else {
                    debugWriteResult(false);
                    Dispatcher.Invoke(new Action(() => { lblReset.Content = "FAIL"; lblButtonTotal.Content = "FAIL"; }));
                    goto NG;
                }
                goto OK;
                NG:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra nút nhấn";
                        debugWriteLine("<");
                        if (lblWPS.Content.ToString() == "Waiting") {
                            lblWPS.Content = "FAIL";
                            lblReset.Content = "FAIL";
                            lblButtonTotal.Content = "FAIL";
                        }
                        lblButtonLegend.Content = "--";
                    }));
                    MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
                OK:
                {
                    Dispatcher.Invoke(new Action(() => {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#333333");
                        b.Content = "Kiểm tra nút nhấn";
                        debugWriteLine("<");
                        lblButtonLegend.Content = "--";
                    }));
                    MessageBox.Show("Hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Đóng cổng COM
                    GlobalData.serialPort.closeSerialPort(out message);
                    return;
                }
            }));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

    }
}
