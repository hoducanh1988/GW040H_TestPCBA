using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions {
    public class exCheckLAN : baseFunctions {
       private baseFunctionNoLegend ba = new baseFunctionNoLegend();

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

        public bool Excute1(ref string _err) {
            string _error = "";
            bool lan1 = false, lan2 = false, lan3 = false, lan4 = false;
            try {
                GlobalData.testingInfo.COLORLAN = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~ Confirm LAN Port
                //Login vao ONT
                GlobalData.testingInfo.TITLE = Titles.loginDUT;
                GlobalData.testingInfo.LOGSYSTEM += "<1/2: Login vào ONT...\r\n";
                if (!ba.login_toDUT(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";

                //Check LAN
                GlobalData.testingInfo.TITLE = Titles.checkLAN;
                int index = 0;
                REP:
                GlobalData.testingInfo.CONTENT = string.Format("Retry: {0}", Retries.retry - index);
                index++;
                GlobalData.testingInfo.LOGSYSTEM += "<2/2: Kiểm tra cổng LAN...\r\n";
                bool ret = ba.checkLANPorts(ref lan1, ref lan2, ref lan3, ref lan4, out _error);

                GlobalData.loginfo.Lan1 = lan1 == true ? "PASS" : "FAIL";
                GlobalData.loginfo.Lan2 = lan2 == true ? "PASS" : "FAIL";
                GlobalData.loginfo.Lan3 = lan3 == true ? "PASS" : "FAIL";
                GlobalData.loginfo.Lan4 = lan4 == true ? "PASS" : "FAIL";

                GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                GlobalData.testingInfo.LOGSYSTEM += lan1 == true ? "LAN 1 is passed.\r\n" : "LAN 1 is failed.\r\n";
                GlobalData.testingInfo.LOGSYSTEM += lan2 == true ? "LAN 2 is passed.\r\n" : "LAN 2 is failed.\r\n";
                GlobalData.testingInfo.LOGSYSTEM += lan3 == true ? "LAN 3 is passed.\r\n" : "LAN 3 is failed.\r\n";
                GlobalData.testingInfo.LOGSYSTEM += lan4 == true ? "LAN 4 is passed.\r\n" : "LAN 4 is failed.\r\n";
                GlobalData.testingInfo.ERRORCODE = string.Format("Pla1#{0}", GEN_ERRORCODE(lan1, lan2, lan3, lan4));
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
