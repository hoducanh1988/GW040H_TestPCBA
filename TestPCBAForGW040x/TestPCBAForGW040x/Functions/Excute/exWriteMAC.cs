using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions
{
    public class exWriteMAC : baseFunctions
    {
        baseFunctionNoLegend ba = new baseFunctionNoLegend();

        public string macAddr;
        public exWriteMAC(string _mac) {
            this.macAddr = _mac.ToUpper().Replace(":","");
        }

        //check MAC Address
        public bool IsValid() {
            //////////////////
            try {
                //length of mac = 12 ???
                GlobalData.testingInfo.LOGSYSTEM += "... KIỂM TRA\r\n";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<1/4: Chiều dài kí tự MAC\r\n");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = 12\r\n- Thực tế = {0}\r\n", macAddr.Length);
                if (macAddr.Length != 12) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\r\n");
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\r\n");

                //first 6 digits = A4F4C2 || A06518
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<2/4: 6 kí tự đầu của MAC\r\n");
                string sixDigits = macAddr.Substring(0, 6);
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = A4F4C2||A06518\r\n- Thực tế = {0}\r\n", sixDigits);
                string substr = GlobalData.initSetting.DutMacF6digit;
                if(substr.Contains(":")) {
                    string[] buffer = substr.Split(':');
                    bool isSame = false;
                    foreach(string item in buffer) {
                        if (sixDigits == item) { isSame = true; break; }
                    }
                    if (!isSame) {
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\r\n");
                        goto NG; } 
                } else {
                    if (sixDigits != substr) {
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\r\n");
                        goto NG; } 
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\r\n");

                //Mac digit format is [0-9,A-F]
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<3/4: Định dạng kí tự MAC\r\n");
                string patterns="";
                for (int i = 0; i < 12; i++) {
                    patterns += "[0-9,A-F]";
                }
                if (!Regex.IsMatch(macAddr, patterns)) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = [0-9,A-F]\r\n- Thực tế = {0}\r\n", "False");
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\r\n");
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("Tiêu chuẩn = [0-9,A-F]\r\n- Thực tế = {0}\r\n", "True");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\r\n");

                //Open comport
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<4/4: Mở cổng COM\r\n");
                string message = "";
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = True\r\n- Thực tế = {0}\r\n", "False");
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\r\n");
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = True\r\n- Thực tế = {0}\r\n", "True");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\r\n");

                goto OK;
            } catch {
                goto NG;
            }
            //////////////////
            OK:
            {
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Địa chỉ MAC hợp lệ\r\n\r\n");
                GlobalData.testingInfo.MAC = this.macAddr;
                return true;
            }
            //////////////////
            NG:
            {
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Lỗi\r\n\r\n");
                return false;
            }
        }

        //Write MAC Address
        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORMAC = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Wait DUT boot completed
                GlobalData.testingInfo.LOGSYSTEM += "<1/8: Đợi DUT boot complete...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Please press Enter to activate this console\r\n";
                if (!wait_DUTBootComplete(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0001";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~ Login DUT
                GlobalData.testingInfo.LOGSYSTEM += "<2/8: Login vào DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\r\n";
                if (!login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0002";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //~~~~~~~~~~~~~~~~ Set GPON Serial Number
                GlobalData.testingInfo.LOGSYSTEM += "<3/8: Ghi GPON serial cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: writeflash: total write\r\n";
                if (!setGPONSerialNumber(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.WriteGPON = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0003";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.WriteGPON = "PASS";

                //~~~~~~~~~~~~~~~~ Set HW Version
                GlobalData.testingInfo.LOGSYSTEM += "<4/8: Ghi HWVersion cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: --\r\n";
                if (!writeHWVersion(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.WriteHW = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0004";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.WriteHW = "PASS";

                //~~~~~~~~~~~~~~~~ Set MAC Address
                GlobalData.testingInfo.LOGSYSTEM += "<5/8: Ghi địa chỉ MAC cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: new mac addr = {0}:{1}:{2}:{3}:{4}:{5}\r\n";
                if (!setMac_forEthernet0(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0005";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.testingInfo.LOGUART = "";

                //~~~~~~~~~~~~~~~~ Wait DUT boot completed
                GlobalData.testingInfo.LOGSYSTEM += "<6/8: Đợi DUT boot complete...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Please press Enter to activate this console\r\n";
                if (!wait_DUTWifiBootComplete(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0001";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //~~~~~~~~~~~~~~~~ Login DUT
                GlobalData.testingInfo.LOGSYSTEM += "<7/8: Login vào DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\r\n";
                if (!login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0002";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //~~~~~~~~~~~~~~~~ Confirm MAC Address
                GlobalData.testingInfo.LOGSYSTEM += "<8/8: Xác nhận địa chỉ MAC...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Link encap:Ethernet  HWaddr {0}:{1}:{2}:{3}:{4}:{5}\r\n";
                if (!confirm_MacAddress(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.WriteMAC = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0006";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.WriteMAC = "PASS";
                goto OK;
            }
            catch {
                goto NG;
            }

            OK:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }


        //Write MAC Address
        public bool Excute1(ref string _err) {
            GlobalData.testingInfo.CONTENT = "--";
            string _error = "";
            try {
                GlobalData.testingInfo.COLORMAC = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Login DUT
                GlobalData.testingInfo.TITLE = Titles.loginDUT;
                GlobalData.testingInfo.LOGSYSTEM += "<1/4: Login vào DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\r\n";
                if (!ba.login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0002";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //~~~~~~~~~~~~~~~~ Set GPON Serial Number
                GlobalData.testingInfo.TITLE = Titles.writeGPON;
                GlobalData.testingInfo.LOGSYSTEM += "<2/4: Ghi GPON serial cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: writeflash: total write\r\n";
                if (!setGPONSerialNumber(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.WriteGPON = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0003";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.WriteGPON = "PASS";

                //~~~~~~~~~~~~~~~~ Set HW version (pending)
                GlobalData.testingInfo.TITLE = Titles.writeHW;
                GlobalData.testingInfo.LOGSYSTEM += "<3/4: Ghi HWVersion cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: --\r\n";
                if (!writeHWVersion(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.WriteHW = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0004";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.WriteHW = "PASS";

                //~~~~~~~~~~~~~~~~ Set MAC Address
                GlobalData.testingInfo.TITLE = Titles.writeMAC;
                GlobalData.testingInfo.LOGSYSTEM += "<4/4: Ghi địa chỉ MAC cho DUT...\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: new mac addr = {0}:{1}:{2}:{3}:{4}:{5}\r\n";
                if (!setMac_forEthernet0(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pma0#0005";
                    GlobalData.loginfo.WriteMAC = "FAIL";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.testingInfo.LOGUART = "";
                GlobalData.loginfo.WriteMAC = "PASS";
                goto OK;
            }
            catch {
                goto NG;
            }

            OK:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }
    }
}
