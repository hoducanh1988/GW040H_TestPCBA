using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class exCheckUSB : baseFunctions {
        baseFunctionNoLegend ba = new baseFunctionNoLegend();

        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORUSB = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Confirm USB Port
                GlobalData.testingInfo.LOGSYSTEM += "<1/1: Kiểm tra cổng USB...\r\n";
                if (!confirmUSBPort(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }

        public bool Excute1(ref string _err) {
            string _error = "";
            bool usb2 = false, usb3 = false;

            try {
                GlobalData.testingInfo.COLORUSB = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Login to ONT
                GlobalData.testingInfo.TITLE = Titles.loginDUT;
                GlobalData.testingInfo.LOGSYSTEM += "<1/2: Login vào ONT...\r\n";
                if (!ba.login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //~~~~~~~~~~~~~~~~ Confirm USB Port
                GlobalData.testingInfo.TITLE = Titles.checkUSB;
                int index = 0;
                REP:
                GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry - index);
                index++;
                GlobalData.testingInfo.LOGSYSTEM += "<2/2: Kiểm tra cổng USB...\r\n";
                bool ret = ba.checkUSBPorts(ref usb2, ref usb3, out _error);

                GlobalData.loginfo.Usb2 = usb2 == true ? "PASS" : "FAIL";
                GlobalData.loginfo.Usb3 = usb3 == true ? "PASS" : "FAIL";

                GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                GlobalData.testingInfo.LOGSYSTEM += usb2 == true ? "Usb2.0 is passed.\r\n" : "Usb2.0 is failed.\r\n";
                GlobalData.testingInfo.LOGSYSTEM += usb3 == true ? "Usb3.0 is passed.\r\n" : "Usb3.0 is failed.\r\n";
                GlobalData.testingInfo.ERRORCODE = string.Format("Pus1#{0}", GEN_ERRORCODE(usb2, usb3));

                if (!ret) {
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    if (index < Retries.retry) goto REP;
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }
    }
}
