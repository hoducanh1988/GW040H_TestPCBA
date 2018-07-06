﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using TestPCBAForGW040x.SubControls;

namespace TestPCBAForGW040x.Functions {
    public abstract class baseFunctions {
        LED led = null;

        private bool accessDUT(out string message) {
            message = "";
            try {
                GlobalData.serialPort.Port.Write("b");
                Thread.Sleep(100);
                return GlobalData.testingInfo.LOGUART.Contains("bldr>") == true ? true : false;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        private void popupLED() {
            double top, left, width, height;
            double ht = GlobalData.thisLocation.height - 238;
            top = GlobalData.thisLocation.top + 160 + ht / 3.5;
            left = GlobalData.thisLocation.left + 17;
            width = GlobalData.thisLocation.width - 35;
            height = (ht / 3.5) * 2.5 - 72;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                led = new LED(top, left, width, height);
                led.ShowDialog();
            }));
        }

        private void destroyLED() {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                led.Close();
            }));
        }

        private bool setFTPServerIPAddress(string ipAddress, out string message) {
            message = "";
            try {
                GlobalData.serialPort.Port.Write(string.Format("ipaddr {0}\r\n", ipAddress));
                GlobalData.testingInfo.LOGSYSTEM += string.Format("ipaddr {0}\r\n", ipAddress);
                Thread.Sleep(100);
                return GlobalData.testingInfo.LOGUART.Contains(string.Format("Change IP address to {0}", ipAddress)) == true ? true : false;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        private bool putFirwareThroughWPS(out string message) {
            message = "";
            try {
                string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<1/3: Ghi dữ liệu '{0}' vào file {1}, ", string.Format("tftp -i {0} put {1}", GlobalData.initSetting.DutIPUploadFW, GlobalData.initSetting.DutFwPath), rootPath + "cmd.txt");
                System.IO.File.WriteAllText(rootPath + "cmd.txt", string.Format("tftp -i {0} put {1}", GlobalData.initSetting.DutIPUploadFW, GlobalData.initSetting.DutFwPath));
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                try {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("<2/3: Delete file {0}, ", rootPath + "wps.txt");
                    System.IO.File.Delete(rootPath + "wps.txt");
                    GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                }
                catch { }
                Thread.Sleep(100);
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<3/3: Call process {0},", rootPath + "RunPowerShell.exe");
                ProcessStartInfo psi = new ProcessStartInfo(rootPath + "RunPowerShell.exe");
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                Thread.Sleep(100);
                psi.UseShellExecute = true;
                Thread.Sleep(100);
                Process.Start(psi);
                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex) {
                message = ex.ToString();
                GlobalData.testingInfo.LOGSYSTEM += message + "\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                return false;
            }
        }

        private Byte[] HexToBin(string pHexString) {
            if (String.IsNullOrEmpty(pHexString))
                return new Byte[0];

            if (pHexString.Length % 2 != 0)
                throw new Exception("Hexstring must have an even length");

            Byte[] bin = new Byte[pHexString.Length / 2];
            int o = 0;
            int i = 0;
            for (; i < pHexString.Length; i += 2, o++) {
                switch (pHexString[i]) {
                    case '0': bin[o] = 0x00; break;
                    case '1': bin[o] = 0x10; break;
                    case '2': bin[o] = 0x20; break;
                    case '3': bin[o] = 0x30; break;
                    case '4': bin[o] = 0x40; break;
                    case '5': bin[o] = 0x50; break;
                    case '6': bin[o] = 0x60; break;
                    case '7': bin[o] = 0x70; break;
                    case '8': bin[o] = 0x80; break;
                    case '9': bin[o] = 0x90; break;
                    case 'A': bin[o] = 0xa0; break;
                    case 'a': bin[o] = 0xa0; break;
                    case 'B': bin[o] = 0xb0; break;
                    case 'b': bin[o] = 0xb0; break;
                    case 'C': bin[o] = 0xc0; break;
                    case 'c': bin[o] = 0xc0; break;
                    case 'D': bin[o] = 0xd0; break;
                    case 'd': bin[o] = 0xd0; break;
                    case 'E': bin[o] = 0xe0; break;
                    case 'e': bin[o] = 0xe0; break;
                    case 'F': bin[o] = 0xf0; break;
                    case 'f': bin[o] = 0xf0; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }

                switch (pHexString[i + 1]) {
                    case '0': bin[o] |= 0x00; break;
                    case '1': bin[o] |= 0x01; break;
                    case '2': bin[o] |= 0x02; break;
                    case '3': bin[o] |= 0x03; break;
                    case '4': bin[o] |= 0x04; break;
                    case '5': bin[o] |= 0x05; break;
                    case '6': bin[o] |= 0x06; break;
                    case '7': bin[o] |= 0x07; break;
                    case '8': bin[o] |= 0x08; break;
                    case '9': bin[o] |= 0x09; break;
                    case 'A': bin[o] |= 0x0a; break;
                    case 'a': bin[o] |= 0x0a; break;
                    case 'B': bin[o] |= 0x0b; break;
                    case 'b': bin[o] |= 0x0b; break;
                    case 'C': bin[o] |= 0x0c; break;
                    case 'c': bin[o] |= 0x0c; break;
                    case 'D': bin[o] |= 0x0d; break;
                    case 'd': bin[o] |= 0x0d; break;
                    case 'E': bin[o] |= 0x0e; break;
                    case 'e': bin[o] |= 0x0e; break;
                    case 'F': bin[o] |= 0x0f; break;
                    case 'f': bin[o] |= 0x0f; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }
            }
            return bin;
        }

        private string BinToHex(string bin) {
            string output = "";
            try {
                int rest = bin.Length % 4;
                bin = bin.PadLeft(rest, '0'); //pad the length out to by divideable by 4

                for (int i = 0; i <= bin.Length - 4; i += 4) {
                    output += string.Format("{0:X}", Convert.ToByte(bin.Substring(i, 4), 2));
                }

                return output;
            }
            catch {
                return "ERROR";
            }
        }

        private string GEN_SERIAL_ONT(string MAC) {
            try {
                string low_MAC = MAC.Substring(6, 6);
                string origalByteString = Convert.ToString(HexToBin(low_MAC)[0], 2).PadLeft(8, '0');
                string VNPT_SERIAL_ONT = null;

                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[1], 2).PadLeft(8, '0');
                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[2], 2).PadLeft(8, '0');
                //----HEX to BIN Cach 2-------
                string value = low_MAC;
                var s = String.Join("", low_MAC.Select(x => Convert.ToString(Convert.ToInt32(x + "", 16), 2).PadLeft(4, '0')));
                //----HEX to BIN Cach 2-------
                string shiftByteString = "";
                shiftByteString = origalByteString.Substring(1, origalByteString.Length - 1) + origalByteString[0];

                if (MAC.Contains("A06518")) {
                    VNPT_SERIAL_ONT = "VNPT" + "00" + BinToHex(shiftByteString); //"'00' --> dải MAC đang được đăng ký, sau này nếu đăng ký thêm dải mới thì giá trị này sẽ thành '01'"
                }
                else if (MAC.Contains("A4F4C2")) //Dải mác mới của VNPT. Hòa Add: 16/03/2017
                {
                    VNPT_SERIAL_ONT = "VNPT" + "01" + BinToHex(shiftByteString);
                }
                return VNPT_SERIAL_ONT;
            }
            catch {
                return "ERROR";
            }
        }

        /// <summary>
        /// Nhập dữ liệu vào từ mã có giá trị nhỏ nhất tới mã có giá trị lớn nhất
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GEN_ERRORCODE(params bool[] data) {
            if (data.Length > 0) {
                int i = 0, sum = 0;
                foreach (var item in data) {
                    sum += (item == true ? 0 : 1) * ((int)Math.Pow(2, i));
                    i++;
                }
                string s = sum.ToString();
                int l = s.Length;
                for (int j = 0; j < 4 - l; j++) { s = "0" + s; }
                return s;
            }
            else return "0000";
        }

        protected bool pingToIPAddress(string IP, out string _error) {
            GlobalData.testingInfo.TITLE = string.Format("PING TO {0}", IP);
            GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", 10);
            _error = "";
            bool _flag = false;
            int index = 0;
            while (!_flag) {
                try {
                    index++;
                    GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", 10 - index);
                    Ping pingSender = new Ping();
                    IPAddress address = IPAddress.Parse(IP);
                    PingReply reply = pingSender.Send(address);
                    if (reply.Status == IPStatus.Success) {
                        _flag = true;
                        break;
                    }
                    else {
                        _error = reply.Status.ToString();
                        if (index >= 10) break;
                    }
                }
                catch (Exception ex) {
                    Thread.Sleep(500);
                    _error = ex.ToString();
                    if (index >= 10) break;
                }
            }
            return _flag;
        }

        protected bool sendDataToDUT(string data) {
            try {
                GlobalData.serialPort.Port.Write(data);
                return true;
            }
            catch {
                return false;
            }
        }

        protected bool confirmUSBPort(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.checkUSB;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.x15);
            _error = "";
            bool _flag = false;
            GlobalData.loginfo.Usb2 = "FAIL";
            GlobalData.loginfo.Usb3 = "FAIL";
            try {
                int index = 0;
                bool showusb2 = true, showusb3 = true;
                bool usb2 = false;
                bool usb3 = false;
                while (!_flag) {
                    for (int i = 1; i <= 8; i++) {
                        for (int j = 1; j <= 4; j++) {
                            if (GlobalData.testingInfo.LOGUART.Contains(string.Format("usb {0}-1.{1}: new high speed USB device number", i, j))) {
                                if (showusb2) {
                                    GlobalData.testingInfo.LOGSYSTEM += string.Format("usb {0}-1.{1}: new high speed USB device number\r\n", i, j);
                                    showusb2 = false;
                                    GlobalData.loginfo.Usb2 = "PASS";
                                }
                                usb2 = true;
                                break;
                            }
                        }
                        if (usb2) break;
                    }
                    for (int i = 1; i <= 8; i++) {
                        for (int j = 1; j <= 4; j++) {
                            if (GlobalData.testingInfo.LOGUART.Contains(string.Format("usb {0}-1.{1}: new SuperSpeed USB device number", i, j))) {
                                if (showusb3) {
                                    GlobalData.testingInfo.LOGSYSTEM += string.Format("usb {0}-1.{1}: new SuperSpeed USB device number\r\n", i, j);
                                    showusb3 = false;
                                    GlobalData.loginfo.Usb3 = "PASS";
                                }
                                usb3 = true;
                                break;
                            }
                        }
                        if (usb3) break;
                    }
                    bool ret = usb2 && usb3;
                    Thread.Sleep(1000);
                    if (ret) { _flag = true; break; }
                    else {
                        if (index >= Timeouts.x15) { _error = "Request time out."; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.x15 - index);
                    }
                }
                GlobalData.testingInfo.ERRORCODE = string.Format("Pus1#{0}", GEN_ERRORCODE(usb2, usb3));
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool confirmLANPort(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.checkLAN;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.x15);
            _error = "";
            bool _flag = false;
            GlobalData.loginfo.Lan1 = "FAIL";
            GlobalData.loginfo.Lan2 = "FAIL";
            GlobalData.loginfo.Lan3 = "FAIL";
            GlobalData.loginfo.Lan4 = "FAIL";
            try {
                int index = 0;
                bool wlan1 = true, wlan2 = true, wlan3 = true, wlan4 = true;
                while (!_flag) {
                    if (GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_1 up") && wlan1) { GlobalData.testingInfo.LOGSYSTEM += "Link State: LAN_1 up\r\n"; wlan1 = false; GlobalData.loginfo.Lan1 = "PASS"; }
                    if (GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_2 up") && wlan2) { GlobalData.testingInfo.LOGSYSTEM += "Link State: LAN_2 up\r\n"; wlan2 = false; GlobalData.loginfo.Lan2 = "PASS"; }
                    if (GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_3 up") && wlan3) { GlobalData.testingInfo.LOGSYSTEM += "Link State: LAN_3 up\r\n"; wlan3 = false; GlobalData.loginfo.Lan3 = "PASS"; }
                    if (GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_4 up") && wlan4) { GlobalData.testingInfo.LOGSYSTEM += "Link State: LAN_4 up\r\n"; wlan4 = false; GlobalData.loginfo.Lan4 = "PASS"; }

                    bool ret = GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_1 up") &&
                               GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_2 up") &&
                               GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_3 up") &&
                               GlobalData.testingInfo.LOGUART.Contains("Link State: LAN_4 up");

                    Thread.Sleep(1000);
                    if (ret) { _flag = true; break; }
                    else {
                        if (index >= Timeouts.x15) { _error = "Request time out."; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.x15 - index);
                    }
                }
                GlobalData.testingInfo.ERRORCODE = string.Format("Pla1#{0}", GEN_ERRORCODE(!wlan1, !wlan2, !wlan3, !wlan4));
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool confirm_MacAddress(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.confirmMAC;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT(string.Format("ifconfig\n"));
                    string st = string.Format("Link encap:Ethernet  HWaddr {0}:{1}:{2}:{3}:{4}:{5}",
                       GlobalData.testingInfo.MAC.Substring(0, 2),
                       GlobalData.testingInfo.MAC.Substring(2, 2),
                       GlobalData.testingInfo.MAC.Substring(4, 2),
                       GlobalData.testingInfo.MAC.Substring(6, 2),
                       GlobalData.testingInfo.MAC.Substring(8, 2),
                       GlobalData.testingInfo.MAC.Substring(10, 2)
                       );
                    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) {
                        _error = "Request time out";
                        break;
                    }
                    else {
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool setGPONSerialNumber(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.writeGPON;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    string gponSeri = GEN_SERIAL_ONT(GlobalData.testingInfo.MAC);
                    this.sendDataToDUT(string.Format("prolinecmd gponsn set {0}\n", gponSeri));
                    GlobalData.testingInfo.LOGSYSTEM += "Generate GPON serial: " + gponSeri;
                    string st = string.Format("writeflash: total write");
                    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) {
                        _error = "Request time out";
                        break;
                    }
                    else {
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                //GlobalData.testingInfo.LOGUART = "";
                return _flag;
            }
        }

        protected bool setMac_forEthernet0(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.writeMAC;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT(string.Format("sys mac {0}\n", GlobalData.testingInfo.MAC));
                    //string st = string.Format("new mac addr = {0}:{1}:{2}:{3}:{4}:{5}",
                    //    GlobalData.testingInfo.MAC.Substring(0, 2).ToLower(),
                    //    GlobalData.testingInfo.MAC.Substring(2, 2).ToLower(),
                    //    GlobalData.testingInfo.MAC.Substring(4, 2).ToLower(),
                    //    GlobalData.testingInfo.MAC.Substring(6, 2).ToLower(),
                    //    GlobalData.testingInfo.MAC.Substring(8, 2).ToLower(),
                    //    GlobalData.testingInfo.MAC.Substring(10, 2).ToLower()
                    //    );
                    string st = "BMT & BBT Init Success";
                    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) {
                        _error = "Request time out";
                        break;
                    }
                    else {
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                //GlobalData.testingInfo.LOGUART = "";
                return _flag;
            }
        }

        protected bool login_toDUT(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.loginDUT;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    this.sendDataToDUT("\r\n");
                    while (!GlobalData.testingInfo.LOGUART.Contains("tc login:")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.initSetting.DutUser + "\n");
                    while (!GlobalData.testingInfo.LOGUART.Contains("Password:")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.initSetting.DutPass + "\n");
                    while (!GlobalData.testingInfo.LOGUART.Contains("root login  on `console'")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                    }
                    if (index >= Timeouts.verylongtime) {
                        _error = "Request time out";
                        break;
                    }
                    else {
                        _flag = true;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool check_Resetbutton(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.checkNutReset;
            GlobalData.testingInfo.CONTENT = string.Format("Nhấn nút reset, Timeout:{0}s", Timeouts.longtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    //string pattern1 = string.Format("cc.c, 5767 h_sec");
                    //string pattern2 = string.Format("cc.c, 5735 h_sec");
                    //if (GlobalData.testingInfo.LOGUART.Contains(pattern1)) {
                    //    GlobalData.testingInfo.LOGSYSTEM += pattern1 + "\r\n";
                    //}
                    //if (GlobalData.testingInfo.LOGUART.Contains(pattern2)) {
                    //    GlobalData.testingInfo.LOGSYSTEM += pattern2 + "\r\n";
                    //}
                    //if (GlobalData.testingInfo.LOGUART.Contains(pattern1)|| GlobalData.testingInfo.LOGUART.Contains(pattern2)) {
                    //    _flag = true;
                    //    break;
                    //}
                    if (GlobalData.testingInfo.LOGUART.Contains(completeString.resetPressed)) {
                        GlobalData.testingInfo.LOGSYSTEM += completeString.resetPressed;
                        _flag = true; break;
                    }

                    GlobalData.testingInfo.CONTENT = string.Format("Nhấn nút reset, Timeout:{0}s", Timeouts.longtime - index);
                    if (index < Timeouts.longtime) { index++; Thread.Sleep(1000); }
                    else { _error = "Request timeout"; break; }
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}sec/{1}sec\r\n", index, Timeouts.longtime);
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }


        protected bool check_WPSbutton(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.checkNutWPS;
            GlobalData.testingInfo.CONTENT = string.Format("Nhấn và giữ, Timeout:{0}s", Timeouts.longtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                //string actPattern = "";
                while (!_flag) {
                    //Get actPattern
                    //if(actPattern=="") {
                    //for (int i = 0; i < 10; i++) {
                    //    string pattern = string.Format("br0: port {0}(rai0) entering disabled state", i);
                    //    if (GlobalData.testingInfo.LOGUART.Contains(pattern)) {
                    //        GlobalData.testingInfo.LOGSYSTEM += string.Format("br0: port {0}(rai0) entering disabled state\r\n", i);
                    //        actPattern = pattern;
                    //        break;
                    //    }
                    //}
                    //}
                    //Wait WPSbutton Press Fninish
                    //string[] buffer = GlobalData.testingInfo.LOGUART.Split(new string[] { actPattern }, StringSplitOptions.None);
                    //if (buffer.Length == 4) { _flag = true; break; }


                    if (GlobalData.testingInfo.LOGUART.Contains(completeString.wpsPressed)) {
                        GlobalData.testingInfo.LOGSYSTEM += completeString.wpsPressed;
                        _flag = true; break;
                    }

                    GlobalData.testingInfo.CONTENT = string.Format("Nhấn và giữ, Timeout:{0}s", Timeouts.longtime - index);
                    if (index < Timeouts.longtime) { index++; Thread.Sleep(1000); }
                    else { _error = "Request timeout"; break; }
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}sec/{1}sec\r\n", index, Timeouts.longtime);
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }


        protected bool wait_DUTWifiBootComplete(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.wifibootComplete;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.veryextralongtime);
            _error = "";
            bool _flag = false;
            //////////////////////////////////////
            for (int i = 0; i < 10; i++) {
                string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                if (GlobalData.testingInfo.LOGUART.Contains(pattern) == true) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("br0: port {0}(ra0) entering forwarding state\r\n", i);
                    _flag = true;
                    goto END;
                }
            }
            //////////////////////////////////////
            try {
                int index = 0;
                while (!_flag) {
                    bool _end = false;
                    while (!_end) {
                        bool ret = false;
                        for (int i = 0; i < 10; i++) {
                            string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                            if (GlobalData.testingInfo.LOGUART.Contains(pattern) == true) {
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("br0: port {0}(ra0) entering forwarding state\r\n", i);
                                ret = true;
                                //GlobalData.testingInfo.LOGUART = ""; //clear log UART
                                break;
                            }
                        }
                        if (ret == true) break;
                        Thread.Sleep(1000);
                        if (index >= Timeouts.veryextralongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.veryextralongtime - index);
                    }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}sec/{1}sec", index, Timeouts.veryextralongtime);
                    if (index < Timeouts.veryextralongtime) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool wait_DUTBootComplete(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.bootComplete;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.veryextralongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    bool _end = false;
                    while (!_end) {
                        if (GlobalData.testingInfo.LOGUART.Contains("Please press Enter to activate this console")) break;
                        bool ret = false;
                        for (int i = 0; i < 10; i++) {
                            string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                            if (GlobalData.testingInfo.LOGUART.Contains(pattern) == true) {
                                GlobalData.testingInfo.LOGSYSTEM += string.Format("br0: port {0}(ra0) entering forwarding state\r\n", i);
                                ret = true;
                                //GlobalData.testingInfo.LOGUART = ""; //clear log UART
                                break;
                            }
                        }
                        if (ret == true) break;
                        Thread.Sleep(1000);
                        if (index >= Timeouts.veryextralongtime) break;
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.veryextralongtime - index);
                    }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("{0}sec/{1}sec", index, Timeouts.veryextralongtime);
                    if (index < Timeouts.veryextralongtime) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool putFirm_ThroughWPS(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.uploadFW;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (!this.putFirwareThroughWPS(out _error)) {
                        if (index >= Retries.retry) break;
                        else index++;
                    }
                    else {
                        index = 0;
                        while (!System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "wps.txt")) {
                            Thread.Sleep(1000);
                            if (index >= Timeouts.verylongtime) break;
                            else index++;
                            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                        }
                        if (index >= Timeouts.verylongtime) { _error = "Request time out."; break; }
                        string tmpStr = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "wps.txt");
                        GlobalData.testingInfo.LOGSYSTEM += tmpStr;
                        if (tmpStr.ToUpper().Contains("ERROR") || tmpStr.Trim().Replace("\r\n", "").Replace("\r", "") == string.Empty) {
                            _error = tmpStr;
                            break;
                        }
                        bool _end = false;
                        while (!_end) {
                            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.verylongtime - index);
                            if (GlobalData.testingInfo.LOGUART.Contains(completeString.fwSuccessed)) {
                                _end = true;
                                break;
                            }
                            if (GlobalData.testingInfo.LOGUART.Contains(completeString.fwFailed)) {
                                break;
                            }
                            if (index >= Timeouts.verylongtime) break;
                            else index++;
                            if (index >= Timeouts.verylongtime) { _error = "Request time out."; break; }
                            Thread.Sleep(1000);
                        }
                        _flag = _end;
                        break;
                        //if (GlobalData.testingInfo.LOGUART.ToUpper().Contains("STARTING THE TFTP DOWNLOAD") &&
                        //    GlobalData.testingInfo.LOGUART.ToUpper().Contains("CHECK DATA SUCCESS, PREPARE TO UPLOAD")) {
                        //    _flag = true;
                        //    break;
                        //}
                        //else {
                        //    break;
                        //}
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }


        protected bool set_FTPServer_IPAddress(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.setFtpIP;
            GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (!this.setFTPServerIPAddress(GlobalData.initSetting.DutIPUploadFW, out _error)) {
                        if (index >= Retries.retry) { _error = "Request time out."; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry - index);
                    }
                    else {
                        _flag = true;
                        break;
                    }
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Retry: {0}/{1}\r\n", index, Retries.retry);
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool set_Default_IPAddress(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.setDefaultIP;
            GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (!this.setFTPServerIPAddress(GlobalData.initSetting.DutIPdefault, out _error)) {
                        if (index >= Retries.retry) { _error = "Request time out."; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry - index);
                    }
                    else {
                        Thread.Sleep(1000);
                        if (pingToIPAddress(GlobalData.initSetting.DutIPdefault, out _error)) { _flag = true; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry - index);
                    }
                    if (index > Retries.retry) break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }


        protected bool access_toUboot(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.accessUboot;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.shorttime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    while (!GlobalData.testingInfo.LOGUART.Contains("Press any key in 3 secs to enter boot command mode.")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.shorttime) { _error = "Request time out."; break; }
                        else index++;
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.shorttime - index);
                    }
                    GlobalData.testingInfo.LOGSYSTEM += "Press any key in 3 secs to enter boot command mode\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Thời gian: {0}sec/{1}sec\r\n", index, Timeouts.shorttime);
                    int rep = 0;
                    REPEAT:
                    if (!this.accessDUT(out _error)) {
                        if (rep < Retries.retry) { rep++; goto REPEAT; }
                        else break;
                    }
                    else {
                        _flag = true;
                        break;
                    }
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool wait_DUT_Online(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.powerON;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.longtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    GlobalData.testingInfo.LOGUART = "";
                    while (GlobalData.testingInfo.LOGUART.Length == 0) {
                        Thread.Sleep(1000);
                        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.longtime - index);
                        if (index >= Timeouts.longtime) { _error = "Request time out."; break; }
                        else index++;
                    }
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Thực tế: LOGUART.length = {0}\r\n", GlobalData.testingInfo.LOGUART.Length);
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Thời gian: {0}sec/{1}sec\r\n", index, Timeouts.longtime);
                    if (index < Timeouts.longtime) _flag = true;
                    else break;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        private bool online_All_LEDs() {
            try {
                //bat den LOS
                GlobalData.serialPort.Port.WriteLine("echo 1 > /proc/xpon/los_led");
                Thread.Sleep(100);
                //bat den WLAN
                GlobalData.serialPort.Port.WriteLine("iwpriv ra0 set led_setting=00-00-00-00-00-00-00-00");
                Thread.Sleep(100);
                //bat den WLAN 5G
                GlobalData.serialPort.Port.WriteLine("echo 1 > proc/tc3162/led_wlan5g");
                Thread.Sleep(100);
                //bat den WPS
                GlobalData.serialPort.Port.WriteLine("iwpriv ra0 set led_setting=01-00-00-00-00-00-00-00");
                Thread.Sleep(100);
                //bat den PON
                GlobalData.serialPort.Port.WriteLine("sys memwl bfbf0204 0x2");
                Thread.Sleep(100);
                //Enet
                //Bật LED INET xanh
                GlobalData.serialPort.Port.WriteLine("echo \"1 0\" > proc/tc3162/led_internet");
                Thread.Sleep(1000);
                //Bật LED INET đỏ
                GlobalData.serialPort.Port.WriteLine("echo \"0 1\" > proc/tc3162/led_internet");
                Thread.Sleep(1000);

                return true;
            }
            catch {
                return false;
            }
        }

        protected bool check_LEDs(out string _error) {
            GlobalData.testingInfo.TITLE = Titles.checkLED;
            GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.extralongtime);
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                popupLED();
                //while (!_flag) {
                GlobalData.ledResult = "";
                while (GlobalData.ledResult.Length == 0) {
                    online_All_LEDs();
                    Thread.Sleep(1000);
                    if (index >= Timeouts.extralongtime) { _error = "Request time out."; break; }
                    else index++;
                    GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.extralongtime - index);
                }
                destroyLED();
                GlobalData.testingInfo.LOGSYSTEM += string.Format("POWER LED = {0}\r\n", GlobalData.testingInfo.POWERLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("PON LED = {0}\r\n", GlobalData.testingInfo.PONLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("INET LED = {0}\r\n", GlobalData.testingInfo.INETLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("WLAN 2G LED = {0}\r\n", GlobalData.testingInfo.WLANLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("WLAN 5G LED = {0}\r\n", GlobalData.testingInfo.WLAN5GLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("LAN1 LED = {0}\r\n", GlobalData.testingInfo.LAN1LED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("LAN2 LED = {0}\r\n", GlobalData.testingInfo.LAN2LED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("LAN3 LED = {0}\r\n", GlobalData.testingInfo.LAN3LED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("LAN4 LED = {0}\r\n", GlobalData.testingInfo.LAN4LED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("WPS LED = {0}\r\n", GlobalData.testingInfo.WPSLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("LOS LED = {0}\r\n", GlobalData.testingInfo.LOSLED == true ? "PASS" : "FAIL");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Thời gian: {0}sec/{1}sec\r\n", index, Timeouts.extralongtime);
                GlobalData.testingInfo.ERRORCODE = string.Format("Ple1#{0}", GEN_ERRORCODE(GlobalData.testingInfo.LOSLED,
                                                                                           GlobalData.testingInfo.WPSLED,
                                                                                           GlobalData.testingInfo.LAN4LED,
                                                                                           GlobalData.testingInfo.LAN3LED,
                                                                                           GlobalData.testingInfo.LAN2LED,
                                                                                           GlobalData.testingInfo.LAN1LED,
                                                                                           GlobalData.testingInfo.WLANLED,
                                                                                           GlobalData.testingInfo.WLAN5GLED,
                                                                                           GlobalData.testingInfo.INETLED,
                                                                                           GlobalData.testingInfo.PONLED,
                                                                                           GlobalData.testingInfo.POWERLED));
                if (index < Timeouts.extralongtime) {
                    if (GlobalData.ledResult == "PASS") _flag = true;
                }
                goto END;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                goto END;
            }
            END:
            {
                return _flag;
            }
        }

        protected bool writeHWVersion(out string _error) {
            _error = "";
            try {
                GlobalData.testingInfo.TITLE = Titles.writeHW;
                GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.normaltime);
                bool _flag = false;
                try {
                    //int index = 0;

                    this.sendDataToDUT("echo '#!/bin/sh' > /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo 'if [ \"$1\" -gt 0 ] && [ \"$1\" -lt 100 ]; then' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo ' echo $1 > /tmp/userdata/hardwareversion' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo ' echo \"Write hardware version success.\"' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo 'else' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo ' echo \"Hardware version must be number.\"' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("echo 'fi' >> /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT("chmod -R 777 /tmp/write_hardwarever.sh\n");
                    Thread.Sleep(100);
                    this.sendDataToDUT(string.Format("./tmp/write_hardwarever.sh {0}\n", GlobalData.initSetting.DutHWVersion));
                    Thread.Sleep(100);
                    _flag = true;
                    //while (!_flag) {
                    //    this.sendDataToDUT(string.Format("sys mac {0}\n", GlobalData.testingInfo.MAC));
                    //    string st = string.Format("new mac addr = {0}:{1}:{2}:{3}:{4}:{5}",
                    //        GlobalData.testingInfo.MAC.Substring(0, 2).ToLower(),
                    //        GlobalData.testingInfo.MAC.Substring(2, 2).ToLower(),
                    //        GlobalData.testingInfo.MAC.Substring(4, 2).ToLower(),
                    //        GlobalData.testingInfo.MAC.Substring(6, 2).ToLower(),
                    //        GlobalData.testingInfo.MAC.Substring(8, 2).ToLower(),
                    //        GlobalData.testingInfo.MAC.Substring(10, 2).ToLower()
                    //        );
                    //    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                    //        Thread.Sleep(1000);
                    //        if (index >= Timeouts.normaltime) break;
                    //        else index++;
                    //        GlobalData.testingInfo.CONTENT = string.Format("Timeout: {0}s", Timeouts.normaltime - index);
                    //    }
                    //    if (index >= Timeouts.normaltime) {
                    //        _error = "Request time out";
                    //        break;
                    //    }
                    //    else {
                    //        _flag = true;
                    //    }
                    //}
                    goto END;
                }
                catch (Exception ex) {
                    _error = ex.ToString();
                    goto END;
                }

                END:
                {
                    return _flag;
                }
            } catch {
                return false;
            }
        }


    }
}
