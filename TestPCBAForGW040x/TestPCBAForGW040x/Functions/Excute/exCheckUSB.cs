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
                GlobalData.testingInfo.LOGSYSTEM += "<1/1: Kiểm tra cổng USB...\n";
                if (!confirmUSBPort(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thành công\n\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORUSB = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check USB thất bại\n\n");
                _err = _error;
                return false;
            }
        }
    }
}
