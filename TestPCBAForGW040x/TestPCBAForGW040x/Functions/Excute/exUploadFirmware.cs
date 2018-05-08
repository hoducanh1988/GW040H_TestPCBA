using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPCBAForGW040x.Functions
{
    public class exUploadFirmware : baseFunctions
    {

        public bool Excute(ref string _err) {
            string _error = "";
            try {
                GlobalData.testingInfo.COLORFW = backGroundColors.wait;
                //~~~~~~~~~~~~~~~~
                GlobalData.testingInfo.LOGSYSTEM += "<1/7: Chờ bật nguồn DUT\r\n";
                GlobalData.testingInfo.LOGSYSTEM += "- Tiêu chuẩn: LOGUART.length>0\r\n";
                if (!wait_DUT_Online(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error;
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pfw0#0001";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                GlobalData.testingInfo.LOGSYSTEM += "<2/7: Truy nhập vào Uboot\r\n";
                if (!access_toUboot(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pfw0#0002";
                    goto NG;}
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                GlobalData.testingInfo.LOGSYSTEM += "<3/7: Thiết lập IP nạp firmware\r\n";
                if (!set_FTPServer_IPAddress(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pfw0#0003";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                GlobalData.testingInfo.LOGSYSTEM += "<4/7: Kiểm tra kết nối mạng tới ONT\r\n";
                if (!pingToIPAddress(GlobalData.initSetting.DutIPUploadFW, out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pfw0#0004";
                    goto NG;
                }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                GlobalData.testingInfo.LOGSYSTEM += "<5/7: Nạp firmware\r\n";
                if (!putFirm_ThroughWPS(out _error)) {
                    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                    GlobalData.testingInfo.ERRORCODE = "Pfw0#0005";
                    goto NG; }
                GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                GlobalData.testingInfo.LOGUART = "";
                //~~~~~~~~~~~~~~~~
                //Thread.Sleep(1000);
                //sendDataToDUT("\r");
                //~~~~~~~~~~~~~~~~
                //GlobalData.testingInfo.LOGSYSTEM += "<6/7: Thiết lập IP mặc định\r\n";
                //if (!set_Default_IPAddress(out _error)) {
                //    GlobalData.testingInfo.LOGSYSTEM += _error + "\r\n";
                //    GlobalData.testingInfo.LOGSYSTEM += "=> FAIL>\r\n";
                //    goto NG; }
                //GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                //~~~~~~~~~~~~~~~~
                //GlobalData.testingInfo.TITLE = Titles.rebootDUT;
                //GlobalData.testingInfo.LOGSYSTEM += "<7/7: Reset DUT\r\n";
                //sendDataToDUT("\rgo\r\n");
                //Thread.Sleep(1000);
                //GlobalData.testingInfo.LOGUART = "";
                //GlobalData.testingInfo.LOGSYSTEM += "=> PASS>\r\n";
                goto OK;
            } catch {
                goto NG;
            }

            OK:
            {
                GlobalData.testingInfo.COLORFW = backGroundColors.pass;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Nạp firmware thành công\r\n\r\n");
                GlobalData.loginfo.UploadFW = "PASS";
                return true;
            }
            NG:
            {
                GlobalData.testingInfo.COLORFW = backGroundColors.fail;
                GlobalData.testingInfo.LOGSYSTEM += string.Format("...Phán định: Nạp firmware thất bại\r\n\r\n");
                GlobalData.loginfo.UploadFW = "FAIL";
                _err = _error;
                return false;
            }

        }
    }
}
