using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestPCBAForGW040x.Functions {
    public class exCheckLED : baseFunctions {
        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORLED = backGroundColors.wait;
                
                //~~~~~~~~~~~~~~~~ Đợi wifi boot complete
                //GlobalData.testingInfo.LOGSYSTEM += "<1/3: Chờ wifi boot complete...\n";
                //if (!wait_DUTWifiBootComplete(out _error)) {
                //    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                //    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                //    goto NG;
                //}
                //GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Đăng nhập vào ONT
                //Thread.Sleep(1000);
                //GlobalData.testingInfo.LOGSYSTEM += "<2/3: Login vào DUT...\n";
                //GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: root login  on `console'\n";
                //if (!login_toDUT(out _error)) {
                //    GlobalData.testingInfo.LOGSYSTEM += _error;
                //    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                //    goto NG;
                //}
                //GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~ Xác nhận LEDs
                GlobalData.testingInfo.LOGSYSTEM += "<3/3: Kiểm tra LEDs...\n";
                if (!check_LEDs(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\n";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\n";
                //~~~~~~~~~~~~~~~~
                goto OK;
            }
            catch {
                goto NG;
            }
            OK:
            {
                GlobalData.testingInfo.COLORLED = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LED thành công\n\n");
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORLED = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Check LED thất bại\n\n");
                _err = _error;
                return false;
            }
        }
    }
}
