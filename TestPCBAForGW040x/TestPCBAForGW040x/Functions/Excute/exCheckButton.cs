using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class exCheckButton : baseFunctions {

        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORBUTTON = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Đợi wifi boot complete
                GlobalData.testingInfo.LOGSYSTEM += "<1/3: Chờ wifi boot complete...\n";
                if (!wait_DUTWifiBootComplete(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ kiểm tra nút WPS
                GlobalData.testingInfo.LOGSYSTEM += "<2/3: Kiểm tra nút WPS...\n";
                if (!check_WPSbutton(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    GlobalData.loginfo.NutWps = "FAIL";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                GlobalData.loginfo.NutWps = "PASS";
                //~~~~~~~~~~~~~~~~ kiểm tra nút Reset
                GlobalData.testingInfo.LOGSYSTEM += "<3/3: Kiểm tra nút Reset...\n";
                if (!check_Resetbutton(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    GlobalData.loginfo.NutReset = "FAIL";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                GlobalData.loginfo.NutReset = "PASS";
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORBUTTON = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check Buttons thành công\n\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORBUTTON = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check Button thất bại\n\n");
                _err = _error;
                return false;
            }
        }
    }
}
