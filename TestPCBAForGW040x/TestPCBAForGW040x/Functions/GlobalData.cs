using System;
using System.Collections.Generic;
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
    }
}
