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
            AddTestCase();
        }

        public static void AddTestCase() {
            GlobalData.datagridcontent.Clear();
            gridContent[] arr = new gridContent[6];
            //Add Nap Firmware
            if (GlobalData.initSetting.EnableUploadFirmware == true) arr[0] = new gridContent() { ID = "01", STEPCHECK = "Nạp Firmware", RESULT = "-", ERROR = "-" };
            else arr[0] = null;
            //Add Check LAN
            if (GlobalData.initSetting.EnableCheckLAN == true) arr[1] = new gridContent() { ID = "02", STEPCHECK = "Kiểm Tra LAN", RESULT = "-", ERROR = "-" };
            else arr[1] = null;
            //Add Check USB
            if (GlobalData.initSetting.EnableCheckUSB == true) arr[2] = new gridContent() { ID = "03", STEPCHECK = "Kiểm Tra USB", RESULT = "-", ERROR = "-" };
            else arr[2] = null;
            //Add check LED
            if (GlobalData.initSetting.EnableCheckLED == true) arr[3] = new gridContent() { ID = "04", STEPCHECK = "Kiểm Tra LED", RESULT = "-", ERROR = "-" };
            else arr[3] = null;
            //Add Check button
            if (GlobalData.initSetting.EnableCheckButton == true) arr[4] = new gridContent() { ID = "05", STEPCHECK = "Kiểm Tra Nút Nhấn", RESULT = "-", ERROR = "-" };
            else arr[4] = null;
            //Add Write MAC
            if (GlobalData.initSetting.EnableWriteMAC == true) arr[5] = new gridContent() { ID = "06", STEPCHECK = "Ghi GPON, MAC", RESULT = "-", ERROR = "-" };
            else arr[5] = null;

            foreach (var item in arr) {
                if (item != null) {
                    GlobalData.datagridcontent.Add(item);
                }
            }

        }
    }
}
