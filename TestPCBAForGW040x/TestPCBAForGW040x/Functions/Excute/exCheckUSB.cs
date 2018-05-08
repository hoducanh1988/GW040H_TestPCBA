using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class exCheckUSB : baseFunctions {
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
    }
}
