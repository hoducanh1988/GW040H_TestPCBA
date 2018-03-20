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
        public string macAddr;
        public exWriteMAC(string _mac) {
            this.macAddr = _mac.ToUpper().Replace(":","");
        }

        //check MAC Address
        public bool IsValid() {
            //////////////////
            try {
                //length of mac = 12 ???
                GlobalData.testingInfo.LOGSYSTEM += "... KIỂM TRA\n";
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<1/4: Chiều dài kí tự MAC\n");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = 12\n- Thực tế = {0}\n", macAddr.Length);
                if (macAddr.Length != 12) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\n");
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\n");

                //first 6 digits = A4F4C2 || A06518
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<2/4: 6 kí tự đầu của MAC\n");
                string sixDigits = macAddr.Substring(0, 6);
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = A4F4C2||A06518\n- Thực tế = {0}\n", sixDigits);
                string substr = GlobalData.initSetting.DutMacF6digit;
                if(substr.Contains(":")) {
                    string[] buffer = substr.Split(':');
                    bool isSame = false;
                    foreach(string item in buffer) {
                        if (sixDigits == item) { isSame = true; break; }
                    }
                    if (!isSame) {
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\n");
                        goto NG; } 
                } else {
                    if (sixDigits != substr) {
                        GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\n");
                        goto NG; } 
                }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\n");

                //Mac digit format is [0-9,A-F]
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<3/4: Định dạng kí tự MAC\n");
                string patterns="";
                for (int i = 0; i < 12; i++) {
                    patterns += "[0-9,A-F]";
                }
                if (!Regex.IsMatch(macAddr, patterns)) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = [0-9,A-F]\n- Thực tế = {0}\n", "False");
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\n");
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("Tiêu chuẩn = [0-9,A-F]\n- Thực tế = {0}\n", "True");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\n");

                //Open comport
                GlobalData.testingInfo.LOGSYSTEM += string.Format("<4/4: Mở cổng COM\n");
                string message = "";
                if (!GlobalData.serialPort.openSerialPort(out message)) {
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = True\n- Thực tế = {0}\n", "False");
                    GlobalData.testingInfo.LOGSYSTEM += string.Format("=> FAIL>\n");
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += string.Format("- Tiêu chuẩn = True\n- Thực tế = {0}\n", "True");
                GlobalData.testingInfo.LOGSYSTEM += string.Format("=> PASS>\n");

                goto OK;
            } catch {
                goto NG;
            }
            //////////////////
            OK:
            {
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Địa chỉ MAC hợp lệ\n\n");
                GlobalData.testingInfo.MAC = this.macAddr;
                return true;
            }
            //////////////////
            NG:
            {
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Lỗi\n\n");
                return false;
            }
        }

        //Write MAC Address
        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORMAC = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Wait DUT boot completed
                GlobalData.testingInfo.LOGSYSTEM += "<1/8: Đợi DUT boot complete...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Please press Enter to activate this console\n";
                if (!wait_DUTBootComplete(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Login DUT
                GlobalData.testingInfo.LOGSYSTEM += "<2/8: Login vào DUT...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\n";
                if (!login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Set GPON Serial Number
                GlobalData.testingInfo.LOGSYSTEM += "<3/8: Ghi GPON serial cho DUT...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: writeflash: total write\n";
                if (!setGPONSerialNumber(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    GlobalData.loginfo.WriteGPON = "FAIL";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                GlobalData.loginfo.WriteGPON = "PASS";
                //~~~~~~~~~~~~~~~~ Set WPS Number (pending)

                //~~~~~~~~~~~~~~~~ Set MAC Address
                GlobalData.testingInfo.LOGSYSTEM += "<5/8: Ghi địa chỉ MAC cho DUT...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: new mac addr = {0}:{1}:{2}:{3}:{4}:{5}\n";
                if (!setMac_forEthernet0(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Wait DUT boot completed
                GlobalData.testingInfo.LOGSYSTEM += "<6/8: Đợi DUT boot complete...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Please press Enter to activate this console\n";
                if (!wait_DUTBootComplete(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Login DUT
                GlobalData.testingInfo.LOGSYSTEM += "<7/8: Login vào DUT...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\n";
                if (!login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Confirm MAC Address
                GlobalData.testingInfo.LOGSYSTEM += "<8/8: Xác nhận địa chỉ MAC...\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: Link encap:Ethernet  HWaddr {0}:{1}:{2}:{3}:{4}:{5}\n";
                if (!confirm_MacAddress(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    GlobalData.loginfo.WriteMAC = "FAIL";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                GlobalData.loginfo.WriteMAC = "PASS";
                goto OK;
            }
            catch {
                goto NG;
            }

            OK:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thành công\n\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORMAC = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Ghi MAC thất bại\n\n");
                _err = _error;
                return false;
            }
        }
    }
}
