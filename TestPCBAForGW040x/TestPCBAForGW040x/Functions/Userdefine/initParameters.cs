using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestPCBAForGW040x.Functions {

    public static class backGroundColors {
        public static SolidColorBrush ready = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        public static SolidColorBrush wait = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF7F"));
        public static SolidColorBrush pass = (SolidColorBrush)(new BrushConverter().ConvertFrom("#36E119"));
        public static SolidColorBrush fail = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9F7F"));
    }

    public static class Titles {
        public static string inputMAC = "NHẬP ĐỊA CHỈ MAC";
        //Upload FW
        public static string powerON = "BẬT NGUỒN DUT";
        public static string accessUboot = "TRUY CẬP UBOOT";
        public static string setFtpIP = "SET IP UPLOAD FW";
        public static string uploadFW = "NẠP FIRMWARE";
        public static string setDefaultIP = "SET IP MẶC ĐỊNH";
        public static string rebootDUT = "DUT REBOOTING...";
        //Write MAC
        public static string bootComplete = "WAIT BOOT COMPLETE...";
        public static string loginDUT = "LOGIN VÀO DUT";
        public static string writeGPON = "GHI GPON SERIAL";
        public static string writeWPS = "GHI MÃ WPS";
        public static string writeMAC = "GHI ĐỊA CHỈ MAC";
        public static string confirmMAC = "XÁC NHẬN LẠI MAC";
        //Check LAN
        public static string checkLAN = "KIỂM TRA CỔNG LAN";
        //Check USB
        public static string checkUSB = "KIỂM TRA CỔNG USB";
        //Check button
        public static string wifibootComplete = "WIFI BOOT COMPLETE...";
        public static string checkNutWPS = "KIỂM TRA NÚT WPS";
        public static string checkNutReset = "KIỂM TRA NÚT RESET";

        //Check LED
        public static string checkLED = "KIỂM TRA LED";
        //
        public static string checkComplete = "HOÀN THÀNH";
    }

    public static class Timeouts {
        public static int extralongtime = 180;
        public static int verylongtime = 60;
        public static int longtime = 30;
        public static int normaltime = 10;
        public static int shorttime = 3;
    }

    public static class Retries {
        public static int retry = 3;
    }


    public static class Contents {

    }

    public static class Statuses {
        public static string ready = "--";
        public static string wait = "Waiting";
        public static string pass = "PASS";
        public static string fail = "FAIL";
    }

    public static class initParameters {

        public static List<string> listStation = new List<string>() { "1", "2", "3", "4" };
        public static List<string> listUARTPort = new List<string>();
        public static List<string> listBaudRate = new List<string>() { "-","50","75","110","134","150","200","300","600",
                                                                       "1200","1800","2400","4800","9600",
                                                                       "19200","28800","38400","57600","76800",
                                                                       "115200","230400","460800","576000","921600"};
        public static List<string> listBarcodeType = new List<string>() {"USB", "UART" };

        static initParameters() {
            listUARTPort.Add("-");
            for (int i = 1; i < 100; i++) {
                listUARTPort.Add(string.Format("COM{0}", i));
            }
        }
    }

}
