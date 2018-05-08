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
               
                //~~~~~~~~~~~~~~~~ kiểm tra nút WPS
                GlobalData.testingInfo.LOGSYSTEM += "<2/3: Kiểm tra nút WPS...\r\n";
                if (!check_WPSbutton(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.NutWps = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pbu1#0001";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.NutWps = "PASS";
                //~~~~~~~~~~~~~~~~ kiểm tra nút Reset
                GlobalData.testingInfo.LOGSYSTEM += "<3/3: Kiểm tra nút Reset...\r\n";
                if (!check_Resetbutton(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.loginfo.NutReset = "FAIL";
                    GlobalData.testingInfo.ERRORCODE = "Pbu1#0002";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.loginfo.NutReset = "PASS";
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORBUTTON = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check Buttons thành công\r\n\r\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORBUTTON = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check Button thất bại\r\n\r\n");
                _err = _error;
                return false;
            }
        }
    }
}
