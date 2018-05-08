using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPCBAForGW040x.Functions;

namespace TestPCBAForGW040x {
    public static class GlobalData {

        public static defaultSetting initSetting = new defaultSetting();
        public static TestingInfo testingInfo = new TestingInfo();
        public static mainLocation thisLocation = new mainLocation();
        public static RS232 serialPort = new RS232();
        public static LogInfomation loginfo = null;

        public static string ledResult = "";

        public static ObservableCollection<gridContent> datagridcontent = new ObservableCollection<gridContent>();

        static GlobalData() {
            GlobalData.datagridcontent.Add(new gridContent() { ID = "01", STEPCHECK = "Nạp Firmware", RESULT = "-", ERROR = "-" });
            GlobalData.datagridcontent.Add(new gridContent() { ID = "02", STEPCHECK = "Kiểm Tra LAN", RESULT = "-", ERROR = "-" });
            GlobalData.datagridcontent.Add(new gridContent() { ID = "03", STEPCHECK = "Kiểm Tra USB", RESULT = "-", ERROR = "-" });
            GlobalData.datagridcontent.Add(new gridContent() { ID = "04", STEPCHECK = "Kiểm Tra LEDs", RESULT = "-", ERROR = "-" });
            GlobalData.datagridcontent.Add(new gridContent() { ID = "05", STEPCHECK = "Kiểm Tra Nút Nhấn", RESULT = "-", ERROR = "-" });
            GlobalData.datagridcontent.Add(new gridContent() { ID = "06", STEPCHECK = "Ghi GPON,WPS,MAC", RESULT = "-", ERROR = "-" });
        }
    }
}
