using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class exCheckLAN : baseFunctions {

        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORLAN = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Confirm LAN Port
                GlobalData.testingInfo.LOGSYSTEM += "<1/1: Kiểm tra cổng LAN...\r\n";
                if (!confirmLANPort(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            } catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORLAN = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LAN thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORLAN = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LAN thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }
    }
}
