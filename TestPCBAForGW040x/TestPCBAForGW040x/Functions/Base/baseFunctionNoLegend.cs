using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class baseFunctionNoLegend {

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

        private bool setFTPServerIPAddress(string ipAddress, out string message) {
            message = "";
            try {
                GlobalData.serialPort.Port.Write(string.Format("ipaddr {0}\r\n", ipAddress));
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
                System.IO.File.WriteAllText(rootPath + "cmd.txt", string.Format("tftp -i {0} put {1}", GlobalData.initSetting.DutIPUploadFW, GlobalData.initSetting.DutFwPath));
                try {
                    System.IO.File.Delete(rootPath + "wps.txt");
                }
                catch { }
                Thread.Sleep(100);
                ProcessStartInfo psi = new ProcessStartInfo(rootPath + "RunPowerShell.exe");
                Thread.Sleep(100);
                psi.UseShellExecute = true;
                Thread.Sleep(100);
                Process.Start(psi);
                Thread.Sleep(100);
                return true;
            }
            catch (Exception ex) {
                message = ex.ToString();
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

        public bool pingToIPAddress(string IP, out string _error) {
            _error = "";
            bool _flag = false;
            int index = 0;
            while (!_flag) {
                try {
                    index++;
                    Ping pingSender = new Ping();
                    IPAddress address = IPAddress.Parse(IP);
                    PingReply reply = pingSender.Send(address);
                    if (reply.Status == IPStatus.Success) {
                        _flag = true;
                        break;
                    }
                    else {
                        _error = reply.Status.ToString();
                        if (index >= Retries.retry) break;
                    }
                }
                catch (Exception ex) {
                    _error = ex.ToString();
                    if (index >= Retries.retry) break;
                }
            }
            return _flag;
        }

        public bool sendDataToDUT(string data) {
            try {
                GlobalData.serialPort.Port.Write(data);
                return true;
            }
            catch {
                return false;
            }
        }

  
        public bool setGPONSerialNumber(string mac, out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                GlobalData.testingInfo.LOGUART = "";
                while (!_flag) {
                    string gponSeri = GEN_SERIAL_ONT(mac);
                    this.sendDataToDUT(string.Format("prolinecmd gponsn set {0}\n", gponSeri));
                    string st = string.Format("writeflash: total write");
                    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.verylongtime) break;
                        else index++;
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

        public bool setMac_forEthernet0(string mac, out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                GlobalData.testingInfo.LOGUART = "";
                while (!_flag) {
                    this.sendDataToDUT(string.Format("sys mac {0}\n", mac));
                    //string st = string.Format("new mac addr = {0}:{1}:{2}:{3}:{4}:{5}",
                    //    mac.Substring(0, 2).ToLower(),
                    //    mac.Substring(2, 2).ToLower(),
                    //    mac.Substring(4, 2).ToLower(),
                    //    mac.Substring(6, 2).ToLower(),
                    //    mac.Substring(8, 2).ToLower(),
                    //   mac.Substring(10, 2).ToLower()
                    //    );
                    string st = "BMT & BBT Init Success";
                    while (!GlobalData.testingInfo.LOGUART.Contains(st)) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.normaltime) break;
                        else index++;
                    }
                    if (index >= Timeouts.normaltime) {
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

        public bool login_toDUT(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                GlobalData.testingInfo.LOGUART = "";
                while (!_flag) {
                    this.sendDataToDUT("\r\n");
                    Thread.Sleep(300);
                    if (GlobalData.testingInfo.LOGUART.Contains("#")) return true;
                    while (!GlobalData.testingInfo.LOGUART.Contains("tc login:")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.normaltime) break;
                        else index++;
                    }
                    if (index >= Timeouts.normaltime) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.initSetting.DutUser + "\n");
                    while (!GlobalData.testingInfo.LOGUART.Contains("Password:")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.normaltime) break;
                        else index++;
                    }
                    if (index >= Timeouts.normaltime) { _error = "Request time out"; break; }
                    this.sendDataToDUT(GlobalData.initSetting.DutPass + "\n");
                    while (!GlobalData.testingInfo.LOGUART.Contains("root login  on `console'")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.normaltime) break;
                        else index++;
                    }
                    if (index >= Timeouts.normaltime) {
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

        public bool check_Resetbutton(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (GlobalData.testingInfo.LOGUART.Contains(completeString.resetPressed)) {
                        _flag = true; break;
                    }
                    if (index < Timeouts.extralongtime) { index++; Thread.Sleep(1000); }
                    else { _error = "Request timeout"; break; }
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

        public bool check_WPSbutton(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (GlobalData.testingInfo.LOGUART.Contains(completeString.wpsPressed)) {
                        _flag = true; break;
                    }
                    if (index < Timeouts.extralongtime) { index++; Thread.Sleep(1000); }
                    else { _error = "Request timeout"; break; }
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

        public bool wait_DUTWifiBootComplete(out string _error) {
            _error = "";
            bool _flag = false;
            //////////////////////////////////////
            for (int i = 0; i < 10; i++) {
                string pattern = string.Format("br0: port {0}(ra0) entering forwarding state", i);
                if (GlobalData.testingInfo.LOGUART.Contains(pattern) == true) {
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
                                ret = true;
                                break;
                            }
                        }
                        if (ret == true) break;
                        Thread.Sleep(1000);
                        if (index >= Timeouts.veryextralongtime) break;
                        else index++;
                    }
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

        public bool wait_DUTBootComplete(out string _error) {
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
                                ret = true;
                                break;
                            }
                        }
                        if (ret == true) break;
                        Thread.Sleep(1000);
                        if (index >= Timeouts.veryextralongtime) break;
                        else index++;
                    }
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

        public bool putFirm_ThroughWPS(out string _error) {
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
                        }
                        if (index >= Timeouts.verylongtime) { _error = "Request time out."; break; }
                        string tmpStr = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "wps.txt");
                        if (tmpStr.ToUpper().Contains("ERROR") || tmpStr.Trim().Replace("\r\n", "").Replace("\r", "") == string.Empty) {
                            _error = tmpStr;
                            break;
                        }
                        bool _end = false;
                        while (!_end) {
                            if (GlobalData.testingInfo.LOGUART.Contains(completeString.fwSuccessed)) {
                                _end = true;
                                break;
                            }
                            if (index >= Timeouts.verylongtime) break;
                            else index++;
                            if (index >= Timeouts.verylongtime) { _error = "Request time out."; break; }
                            Thread.Sleep(1000);
                        }
                        _flag = _end;
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

        public bool set_FTPServer_IPAddress(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (!this.setFTPServerIPAddress(GlobalData.initSetting.DutIPUploadFW, out _error)) {
                        if (index >= Retries.retry) { _error = "Request time out."; break; }
                        else index++;
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

        public bool set_Default_IPAddress(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    if (!this.setFTPServerIPAddress(GlobalData.initSetting.DutIPdefault, out _error)) {
                        if (index >= Retries.retry) { _error = "Request time out."; break; }
                        else index++;
                    }
                    else {
                        Thread.Sleep(1000);
                        if (pingToIPAddress(GlobalData.initSetting.DutIPdefault, out _error)) { _flag = true; break; }
                        else index++;
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

        public bool access_toUboot(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    while (!GlobalData.testingInfo.LOGUART.Contains("Press any key in 3 secs to enter boot command mode.")) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.shorttime) { _error = "Request time out."; break; }
                        else index++;
                    }
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

        public bool wait_DUT_Online(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                while (!_flag) {
                    GlobalData.testingInfo.LOGUART = "";
                    while (GlobalData.testingInfo.LOGUART.Length == 0) {
                        Thread.Sleep(1000);
                        if (index >= Timeouts.longtime) { _error = "Request time out."; break; }
                        else index++;
                    }
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

        public bool check_LEDs(out string _error) {
            _error = "";
            bool _flag = false;
            try {
                int index = 0;
                GlobalData.ledResult = "";
                while (GlobalData.ledResult.Length == 0) {
                    online_All_LEDs();
                    Thread.Sleep(1000);
                    if (index >= Timeouts.extralongtime) { _error = "Request time out."; break; }
                    else index++;
                }
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

        public string get_FWVersion(out string _error) {
            _error = "";
            try {
                GlobalData.testingInfo.LOGUART = "";
                GlobalData.serialPort.Port.WriteLine("cat /etc/fwver.conf");
                Thread.Sleep(200);
                string ret = "";
                ret = GlobalData.testingInfo.LOGUART;
                ret = ret.Replace("cat /etc/fwver.conf","").Replace("#","");
                return ret;
            } catch (Exception ex) {
                _error = ex.ToString();
                return "";
            }
        }

        //check MAC Address
        public bool MacIsValid(string macAddr) {
            //////////////////
            try {
                //length of mac = 12 ???
                if (macAddr.Length != 12) {
                    goto NG;
                }

                //first 6 digits = A4F4C2 || A06518
                string sixDigits = macAddr.Substring(0, 6);
                string substr = GlobalData.initSetting.DutMacF6digit;
                if (substr.Contains(":")) {
                    string[] buffer = substr.Split(':');
                    bool isSame = false;
                    foreach (string item in buffer) {
                        if (sixDigits == item) { isSame = true; break; }
                    }
                    if (!isSame) {
                        goto NG;
                    }
                }
                else {
                    if (sixDigits != substr) {
                        goto NG;
                    }
                }

                //Mac digit format is [0-9,A-F]
                string patterns = "";
                for (int i = 0; i < 12; i++) {
                    patterns += "[0-9,A-F]";
                }
                if (!Regex.IsMatch(macAddr, patterns)) {
                    goto NG;
                }
                goto OK;
            }
            catch {
                goto NG;
            }
            //////////////////
            OK:
            {
                return true;
            }
            //////////////////
            NG:
            {
                return false;
            }
        }

        public string getMAC(out string _error) {
            _error = "";
            try {
                GlobalData.testingInfo.LOGUART = "";
                this.sendDataToDUT(string.Format("ifconfig\n"));
                Thread.Sleep(1000);
                string str = GlobalData.testingInfo.LOGUART;
                str = str.Replace("\r\n", "").Replace("\r", "");
                str = str.Replace("ifconfig", "").Replace("#", "");
                string[] buffer = str.Split(new string[] { "HWaddr" }, StringSplitOptions.None);
                str = buffer[1].Trim();
                str = str.Substring(0, 17);
                return str;
            } catch (Exception ex) {
                _error = ex.ToString();
                return "";
            }
        }

        public bool checkLANPorts(ref bool lan1, ref bool lan2, ref bool lan3, ref bool lan4, out string _error) {
            _error = "";
            try {
                GlobalData.testingInfo.LOGUART = "";
                string ret = "";
                //-------------------------------------------//
                this.sendDataToDUT(string.Format("ethphxcmd eth0 media-type port 0\r\n"));
                Thread.Sleep(300);
                ret = GlobalData.testingInfo.LOGUART;
                _error += ret;
                GlobalData.testingInfo.LOGUART = "";
                lan1 = ret.Contains("Link is up");
                //-------------------------------------------//
                this.sendDataToDUT(string.Format("ethphxcmd eth0 media-type port 1\r\n"));
                Thread.Sleep(300);
                ret = GlobalData.testingInfo.LOGUART;
                _error += ret;
                GlobalData.testingInfo.LOGUART = "";
                lan2 = ret.Contains("Link is up");
                //-------------------------------------------//
                this.sendDataToDUT(string.Format("ethphxcmd eth0 media-type port 2\r\n"));
                Thread.Sleep(300);
                ret = GlobalData.testingInfo.LOGUART;
                _error += ret;
                GlobalData.testingInfo.LOGUART = "";
                lan3 = ret.Contains("Link is up");
                //-------------------------------------------//
                this.sendDataToDUT(string.Format("ethphxcmd eth0 media-type port 3\r\n"));
                Thread.Sleep(300);
                ret = GlobalData.testingInfo.LOGUART;
                _error += ret;
                GlobalData.testingInfo.LOGUART = "";
                lan4 = ret.Contains("Link is up");
                //-------------------------------------------//
                return lan1 && lan2 && lan3 && lan4;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                return false;
            }
        }

        public bool checkUSBPorts(out string _error) {
            _error = "";
            try {
                GlobalData.testingInfo.LOGUART = "";
                string getStr = "";
                //-------------------------------------------//
                this.sendDataToDUT("mount -t usbfs usbfs /proc/bus/usb/\r\n");
                Thread.Sleep(1000);
                this.sendDataToDUT("cat /proc/bus/usb/devices\r\n");
                Thread.Sleep(2000);
                getStr = GlobalData.testingInfo.LOGUART;
                _error += getStr;
                //-------------------------------------------//
                int _serialNumberAppearTime = Regex.Matches(getStr, "SerialNumber=").Count;
                return _serialNumberAppearTime == 3 ? true : false;
            }
            catch (Exception ex) {
                _error = ex.ToString();
                return false;
            }
        }

        //public bool checkUSBPorts(ref bool usb2, ref bool usb3, out string _error) {
        //    _error = "";
        //    try {
        //        GlobalData.testingInfo.LOGUART = "";
        //        string getStr = "";
        //        //-------------------------------------------//
        //        this.sendDataToDUT("mount -t usbfs usbfs /proc/bus/usb/\r\n");
        //        Thread.Sleep(1000);
        //        this.sendDataToDUT("cat /proc/bus/usb/devices\r\n");
        //        Thread.Sleep(2000);
        //        getStr = GlobalData.testingInfo.LOGUART;
        //        _error += getStr;
        //        //-------------------------------------------//
        //        string _usb2text = "", _usb3text = "";
        //        bool _format1 = getStr.Contains("Product=USB3.0 Hub") || getStr.Contains("Product=USB2.0 Hub");
        //        bool _format2 = getStr.Contains("Product=4-Port USB 3.0 Hub") || getStr.Contains("Product=4-Port USB 2.0 Hub");

        //        if (_format1 == true) {
        //            _usb2text = "Product=USB2.0 Hub";
        //            _usb3text = "Product=USB3.0 Hub";
        //        }
        //        if (_format2 == true) {
        //            _usb2text = "Product=4-Port USB 2.0 Hub";
        //            _usb3text = "Product=4-Port USB 3.0 Hub";
        //        }

        //        //Không có USB hub
        //        bool ret = _format1 || _format2;
        //        if (ret == false) {
        //            usb2 = false; usb3 = false;
        //        }
        //        //Có USB HUB
        //        string[] buffer = null;
        //        string u2 = null, u3 = null;
                
        //        int IndexofUsb3 = getStr.IndexOf(_usb3text);
        //        int IndexofUsb2 = getStr.IndexOf(_usb2text);
        //        if (IndexofUsb3 < IndexofUsb2) {
        //            buffer = getStr.Split(new string[] { _usb3text }, StringSplitOptions.None);
        //            getStr = buffer[1];
        //            buffer = getStr.Split(new string[] { _usb2text }, StringSplitOptions.None);
        //            u3 = buffer[0];
        //            u2 = buffer[1];
        //            //Kiểm tra có USB3.0 hay không?
        //            buffer = u3.Split(new string[] { "SerialNumber=xhc_mtk" }, StringSplitOptions.None);
        //            u3 = buffer[0];
        //            usb3 = u3.Contains("SerialNumber=");
        //            //Kiểm tra có USB2.0 hay không?
        //            usb2 = u2.Contains("SerialNumber=");
        //        }
        //        else {
        //            buffer = getStr.Split(new string[] { _usb2text }, StringSplitOptions.None);
        //            getStr = buffer[1];
        //            buffer = getStr.Split(new string[] { _usb3text }, StringSplitOptions.None);
        //            u2 = buffer[0];
        //            u3 = buffer[1];
        //            //Kiểm tra có USB2.0 hay không?
        //            buffer = u2.Split(new string[] { "SerialNumber=xhc_mtk" }, StringSplitOptions.None);
        //            u2 = buffer[0];
        //            usb2 = u2.Contains("SerialNumber=");
        //            //Kiểm tra có USB3.0 hay không?
        //            usb3 = u3.Contains("SerialNumber=");
        //        }
        //        return usb2 && usb3;
        //    } catch (Exception ex) {
        //        _error = ex.ToString();
        //        return false;
        //    }
        //}
    }
}
