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
                GlobalData.testingInfo.LOGSYSTEM += "<1/1: Kiểm tra cổng LAN...\n";
                if (!confirmLANPort(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            } catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORLAN = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LAN thành công\n\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORLAN = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LAN thất bại\n\n");
                _err = _error;
                return false;
            }
        }
    }
}
