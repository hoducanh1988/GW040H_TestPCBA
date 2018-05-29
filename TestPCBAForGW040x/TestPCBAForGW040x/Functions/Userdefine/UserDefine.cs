using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestPCBAForGW040x.Functions {

    public class gridContent {
        public string ID { get; set; }
        public string STEPCHECK { get; set; }
        public string RESULT { get; set; }
        public string ERROR { get; set; }
    }

    public class mainLocation {
        public double top { get; set; }
        public double left { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class defaultSetting : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
                //Auto save settings when properties changed
                //Properties.Settings.Default.Save();
            }
        }

        public void Save() {
            //Auto save settings when properties changed
            Properties.Settings.Default.Save();
        }

        #region Enable Test Case

        public bool EnableUploadFirmware {
            get { return Properties.Settings.Default.EnableUploadFW; }
            set {
                Properties.Settings.Default.EnableUploadFW = value;
                OnPropertyChanged(nameof(EnableUploadFirmware));
            }
        }
        public bool EnableCheckLAN {
            get { return Properties.Settings.Default.EnableChkLAN; }
            set {
                Properties.Settings.Default.EnableChkLAN = value;
                OnPropertyChanged(nameof(EnableCheckLAN));
            }
        }
        public bool EnableCheckUSB {
            get { return Properties.Settings.Default.EnableChkUSB; }
            set {
                Properties.Settings.Default.EnableChkUSB = value;
                OnPropertyChanged(nameof(EnableCheckUSB));
            }
        }
        public bool EnableCheckLED {
            get { return Properties.Settings.Default.EnableChkLED; }
            set {
                Properties.Settings.Default.EnableChkLED = value;
                OnPropertyChanged(nameof(EnableCheckLED));
            }
        }
        public bool EnableCheckButton {
            get { return Properties.Settings.Default.EnableChkButton; }
            set {
                Properties.Settings.Default.EnableChkButton = value;
                OnPropertyChanged(nameof(EnableCheckButton));
            }
        }
        public bool EnableWriteMAC {
            get { return Properties.Settings.Default.EnableWriteMAC; }
            set {
                Properties.Settings.Default.EnableWriteMAC = value;
                OnPropertyChanged(nameof(EnableWriteMAC));
            }
        }

        #endregion

        public string StationNumber {
            get { return Properties.Settings.Default.StationNumber; }
            set {
                Properties.Settings.Default.StationNumber = value;                
                OnPropertyChanged(nameof(StationNumber));
            }
        }
        public string JigNumber {
          
            get {
                try {
                    int n = int.Parse(Properties.Settings.Default.JigNumber);
                    DutIPUploadFW = string.Format("192.168.1.{0}", 9 + n);
                }
                catch { }
                return Properties.Settings.Default.JigNumber;
            }
            set {
                Properties.Settings.Default.JigNumber = value;
                try {
                    int n = int.Parse(value);
                    DutIPUploadFW = string.Format("192.168.1.{0}", 9 + n);
                }
                catch { }
                OnPropertyChanged(nameof(JigNumber));
            }
        }
        public string DutMacF6digit {
            get { return Properties.Settings.Default.DUT_MacF6digit; }
            set {
                Properties.Settings.Default.DUT_MacF6digit = value;
                OnPropertyChanged(nameof(DutMacF6digit));
            }
        }
        public string DutFwPath {
            get { return Properties.Settings.Default.DUT_FWPath; }
            set {
                Properties.Settings.Default.DUT_FWPath = value;
                OnPropertyChanged(nameof(DutFwPath));
            }
        }
        public string USBPort {
            get { return Properties.Settings.Default.USBPort; }
            set {
                Properties.Settings.Default.USBPort = value;
                OnPropertyChanged(nameof(USBPort));
            }
        }
        public string USBBaudRate {
            get { return Properties.Settings.Default.USBBaudRate; }
            set {
                Properties.Settings.Default.USBBaudRate = value;
                OnPropertyChanged(nameof(USBBaudRate));
            }
        }
        public string BRType {
            get { return Properties.Settings.Default.BRType; }
            set {
                Properties.Settings.Default.BRType = value;
                OnPropertyChanged(nameof(BRType));
            }
        }
        public string BRPort {
            get { return Properties.Settings.Default.BRPort; }
            set {
                Properties.Settings.Default.BRPort = value;
                OnPropertyChanged(nameof(BRPort));
            }
        }
        public string BRBaudRate {
            get { return Properties.Settings.Default.BRBaudRate; }
            set {
                Properties.Settings.Default.BRBaudRate = value;
                OnPropertyChanged(nameof(BRBaudRate));
            }
        }
        public string DutIPdefault {
            get { return Properties.Settings.Default.DUT_IPdefault; }
            set {
                Properties.Settings.Default.DUT_IPdefault = value;
                OnPropertyChanged(nameof(DutIPdefault));
            }
        }
        public string DutIPUploadFW {
            get { return Properties.Settings.Default.DUT_IPUploadFW; }
            set {
                Properties.Settings.Default.DUT_IPUploadFW = value;
                OnPropertyChanged(nameof(DutIPUploadFW));
            }
        }
        public string DutUser {
            get { return Properties.Settings.Default.DUT_User; }
            set {
                Properties.Settings.Default.DUT_User = value;
                OnPropertyChanged(nameof(DutUser));
            }
        }
        public string DutPass {
            get { return Properties.Settings.Default.DUT_Pass; }
            set {
                Properties.Settings.Default.DUT_Pass = value;
                OnPropertyChanged(nameof(DutPass));
            }
        }

    }

    public class TestingInfo : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        bool _help;
        public bool HELP {
            get { return _help; }
            set {
                _help = value;
                OnPropertyChanged(nameof(HELP));
            }
        }

        int _developer;
        public int DEVELOPER {
            get { return _developer; }
            set {
                _developer = value;
                OnPropertyChanged(nameof(DEVELOPER));
            }
        }

        string _user;
        public string USER {
            get { return _user; }
            set {
                _user = value;
                OnPropertyChanged(nameof(USER));
            }
        }
        string _pass;
        public string PASSWORD {
            get { return _pass; }
            set {
                _pass = value;
                OnPropertyChanged(nameof(PASSWORD));
            }
        }

        public string ERRORCODE { get; set; }

        #region layout
        double _width;
        double _height;
        double _conerRadius;
        int _fontmac;
        int _heightmac;

        public double Width {
            get { return _width; }
            set {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        public double Height {
            get { return _height; }
            set {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public double ConerRadius {
            get { return _conerRadius; }
            set {
                _conerRadius = value;
                OnPropertyChanged(nameof(ConerRadius));
            }
        }
        public int FontMAC {
            get { return _fontmac; }
            set {
                _fontmac = value;
                OnPropertyChanged(nameof(FontMAC));
            }
        }
        public int HeightMAC {
            get { return _heightmac; }
            set {
                _heightmac = value;
                OnPropertyChanged(nameof(HeightMAC));
            }
        }
        #endregion

        #region UserInterface
        string _testtitle;
        public string TITLE {
            get { return _testtitle; }
            set {
                _testtitle = value;
                OnPropertyChanged(nameof(TITLE));
            }
        }

        string _testcontent;
        public string CONTENT {
            get { return _testcontent; }
            set {
                _testcontent = value;
                OnPropertyChanged(nameof(CONTENT));
            }
        }

        string _teststatus;
        public string STATUS {
            get { return _teststatus; }
            set {
                _teststatus = value;
                OnPropertyChanged(nameof(STATUS));
            }
        }

        string _macaddress;
        public string MAC {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(MAC));
            }
        }

        string _elapsedtime;
        public string ELAPSEDTIME {
            get { return _elapsedtime; }
            set {
                _elapsedtime = value;
                OnPropertyChanged(nameof(ELAPSEDTIME));
            }
        }

        string _logcounter;
        public string LOGCOUNTER {
            get { return _logcounter; }
            set {
                _logcounter = value;
                OnPropertyChanged(nameof(LOGCOUNTER));
            }
        }

        string _logSystem;
        public string LOGSYSTEM {
            get { return _logSystem; }
            set {
                _logSystem = value;
                OnPropertyChanged(nameof(LOGSYSTEM));
            }
        }

        string _loguart;
        public string LOGUART {
            get { return _loguart; }
            set {
                _loguart = value;
                OnPropertyChanged(nameof(LOGUART));
            }
        }

        string _datauart;
        public string DATAUART {
            get { return _datauart; }
            set {
                _datauart = value;
                OnPropertyChanged(nameof(DATAUART));
            }
        }

        SolidColorBrush _colorfw;
        public SolidColorBrush COLORFW {
            get { return _colorfw; }
            set {
                _colorfw = value;
                OnPropertyChanged(nameof(COLORFW));
            }
        }

        SolidColorBrush _colormac;
        public SolidColorBrush COLORMAC {
            get { return _colormac; }
            set {
                _colormac = value;
                OnPropertyChanged(nameof(COLORMAC));
            }
        }

        SolidColorBrush _colorlan;
        public SolidColorBrush COLORLAN {
            get { return _colorlan; }
            set {
                _colorlan = value;
                OnPropertyChanged(nameof(COLORLAN));
            }
        }

        SolidColorBrush _colorusb;
        public SolidColorBrush COLORUSB {
            get { return _colorusb; }
            set {
                _colorusb = value;
                OnPropertyChanged(nameof(COLORUSB));
            }
        }

        SolidColorBrush _colorbutton;
        public SolidColorBrush COLORBUTTON {
            get { return _colorbutton; }
            set {
                _colorbutton = value;
                OnPropertyChanged(nameof(COLORBUTTON));
            }
        }

        SolidColorBrush _colorled;
        public SolidColorBrush COLORLED {
            get { return _colorled; }
            set {
                _colorled = value;
                OnPropertyChanged(nameof(COLORLED));
            }
        }

        int _progressvalue;
        public int PROGRESSVALUE {
            get { return _progressvalue; }
            set {
                _progressvalue = value;
                OnPropertyChanged(nameof(PROGRESSVALUE));
            }
        }

        int _progresstotal;
        public int PROGRESSTOTAL {
            get { return _progresstotal; }
            set {
                _progresstotal = value;
                OnPropertyChanged(nameof(PROGRESSTOTAL));
            }
        }

        //---------------------------------------------------//
        bool _powerled;
        string _powercontent;
        SolidColorBrush _powerbackground;
        public bool POWERLED {
            get { return _powerled; }
            set {
                _powerled = value;
                if (value) {
                    POWERCONTENT = "OK";
                    POWERBACKGROUND = Brushes.Lime;
                } else {
                    POWERCONTENT = "NG";
                    POWERBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(POWERLED));
            }
        }
        public string POWERCONTENT {
            get { return _powercontent; }
            set {
                _powercontent = value;
                OnPropertyChanged(nameof(POWERCONTENT));
            }
        }
        public SolidColorBrush POWERBACKGROUND {
            get { return _powerbackground; }
            set {
                _powerbackground = value;
                OnPropertyChanged(nameof(POWERBACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _ponled;
        string _poncontent;
        SolidColorBrush _ponbackground;
        public bool PONLED {
            get { return _ponled; }
            set {
                _ponled = value;
                if (value) {
                    PONCONTENT = "OK";
                    PONBACKGROUND = Brushes.Lime;
                }
                else {
                    PONCONTENT = "NG";
                    PONBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(PONLED));
            }
        }
        public string PONCONTENT {
            get { return _poncontent; }
            set {
                _poncontent = value;
                OnPropertyChanged(nameof(PONCONTENT));
            }
        }
        public SolidColorBrush PONBACKGROUND {
            get { return _ponbackground; }
            set {
                _ponbackground = value;
                OnPropertyChanged(nameof(PONBACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _inetled;
        string _inetcontent;
        SolidColorBrush _inetbackground;
        public bool INETLED {
            get { return _inetled; }
            set {
                _inetled = value;
                if (value) {
                    INETCONTENT = "OK";
                    INETBACKGROUND = Brushes.Lime;
                }
                else {
                    INETCONTENT = "NG";
                    INETBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(INETLED));
            }
        }
        public string INETCONTENT {
            get { return _inetcontent; }
            set {
                _inetcontent = value;
                OnPropertyChanged(nameof(INETCONTENT));
            }
        }
        public SolidColorBrush INETBACKGROUND {
            get { return _inetbackground; }
            set {
                _inetbackground = value;
                OnPropertyChanged(nameof(INETBACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _wlanled;
        string _wlancontent;
        SolidColorBrush _wlanbackground;
        public bool WLANLED {
            get { return _wlanled; }
            set {
                _wlanled = value;
                if (value) {
                    WLANCONTENT = "OK";
                    WLANBACKGROUND = Brushes.Lime;
                }
                else {
                    WLANCONTENT = "NG";
                    WLANBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(WLANLED));
            }
        }
        public string WLANCONTENT {
            get { return _wlancontent; }
            set {
                _wlancontent = value;
                OnPropertyChanged(nameof(WLANCONTENT));
            }
        }
        public SolidColorBrush WLANBACKGROUND {
            get { return _wlanbackground; }
            set {
                _wlanbackground = value;
                OnPropertyChanged(nameof(WLANBACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _lan1led;
        string _lan1content;
        SolidColorBrush _lan1background;
        public bool LAN1LED {
            get { return _lan1led; }
            set {
                _lan1led = value;
                if (value) {
                    LAN1CONTENT = "OK";
                    LAN1BACKGROUND = Brushes.Lime;
                }
                else {
                    LAN1CONTENT = "NG";
                    LAN1BACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(LAN1LED));
            }
        }
        public string LAN1CONTENT {
            get { return _lan1content; }
            set {
                _lan1content = value;
                OnPropertyChanged(nameof(LAN1CONTENT));
            }
        }
        public SolidColorBrush LAN1BACKGROUND {
            get { return _lan1background; }
            set {
                _lan1background = value;
                OnPropertyChanged(nameof(LAN1BACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _lan2led;
        string _lan2content;
        SolidColorBrush _lan2background;
        public bool LAN2LED {
            get { return _lan2led; }
            set {
                _lan2led = value;
                if (value) {
                    LAN2CONTENT = "OK";
                    LAN2BACKGROUND = Brushes.Lime;
                }
                else {
                    LAN2CONTENT = "NG";
                    LAN2BACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(LAN2LED));
            }
        }
        public string LAN2CONTENT {
            get { return _lan2content; }
            set {
                _lan2content = value;
                OnPropertyChanged(nameof(LAN2CONTENT));
            }
        }
        public SolidColorBrush LAN2BACKGROUND {
            get { return _lan2background; }
            set {
                _lan2background = value;
                OnPropertyChanged(nameof(LAN2BACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _lan3led;
        string _lan3content;
        SolidColorBrush _lan3background;
        public bool LAN3LED {
            get { return _lan3led; }
            set {
                _lan3led = value;
                if (value) {
                    LAN3CONTENT = "OK";
                    LAN3BACKGROUND = Brushes.Lime;
                }
                else {
                    LAN3CONTENT = "NG";
                    LAN3BACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(LAN3LED));
            }
        }
        public string LAN3CONTENT {
            get { return _lan3content; }
            set {
                _lan3content = value;
                OnPropertyChanged(nameof(LAN3CONTENT));
            }
        }
        public SolidColorBrush LAN3BACKGROUND {
            get { return _lan3background; }
            set {
                _lan3background = value;
                OnPropertyChanged(nameof(LAN3BACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _lan4led;
        string _lan4content;
        SolidColorBrush _lan4background;
        public bool LAN4LED {
            get { return _lan4led; }
            set {
                _lan4led = value;
                if (value) {
                    LAN4CONTENT = "OK";
                    LAN4BACKGROUND = Brushes.Lime;
                }
                else {
                    LAN4CONTENT = "NG";
                    LAN4BACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(LAN4LED));
            }
        }
        public string LAN4CONTENT {
            get { return _lan4content; }
            set {
                _lan4content = value;
                OnPropertyChanged(nameof(LAN4CONTENT));
            }
        }
        public SolidColorBrush LAN4BACKGROUND {
            get { return _lan4background; }
            set {
                _lan4background = value;
                OnPropertyChanged(nameof(LAN4BACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _wpsled;
        string _wpscontent;
        SolidColorBrush _wpsbackground;
        public bool WPSLED {
            get { return _wpsled; }
            set {
                _wpsled = value;
                if (value) {
                    WPSCONTENT = "OK";
                    WPSBACKGROUND = Brushes.Lime;
                }
                else {
                    WPSCONTENT = "NG";
                    WPSBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(WPSLED));
            }
        }
        public string WPSCONTENT {
            get { return _wpscontent; }
            set {
                _wpscontent = value;
                OnPropertyChanged(nameof(WPSCONTENT));
            }
        }
        public SolidColorBrush WPSBACKGROUND {
            get { return _wpsbackground; }
            set {
                _wpsbackground = value;
                OnPropertyChanged(nameof(WPSBACKGROUND));
            }
        }
        //---------------------------------------------------//

        //---------------------------------------------------//
        bool _losled;
        string _loscontent;
        SolidColorBrush _losbackground;
        public bool LOSLED {
            get { return _losled; }
            set {
                _losled = value;
                if (value) {
                    LOSCONTENT = "OK";
                    LOSBACKGROUND = Brushes.Lime;
                }
                else {
                    LOSCONTENT = "NG";
                    LOSBACKGROUND = Brushes.Red;
                }
                OnPropertyChanged(nameof(LOSLED));
            }
        }
        public string LOSCONTENT {
            get { return _loscontent; }
            set {
                _loscontent = value;
                OnPropertyChanged(nameof(LOSCONTENT));
            }
        }
        public SolidColorBrush LOSBACKGROUND {
            get { return _losbackground; }
            set {
                _losbackground = value;
                OnPropertyChanged(nameof(LOSBACKGROUND));
            }
        }
        //---------------------------------------------------//
        #endregion
      
        public TestingInfo() {
            Initialize();
        }

        public void Initialize() {
            TITLE = Titles.inputMAC;
            CONTENT = "--";
            MAC = "";
            ELAPSEDTIME = "Thời gian kiểm tra: 00:00:00";
            LOGUART = "";
            DATAUART = "";
            LOGSYSTEM = "> NHẬP ĐỊA CHỈ MAC:\r\n";
            LOGCOUNTER = "0";
            PROGRESSVALUE = 0;
            PROGRESSTOTAL = 6;
            STATUS = Statuses.ready;
            COLORFW = backGroundColors.ready; //0= white, 1= yellow, 2= ok, 3= NG
            COLORMAC = backGroundColors.ready;
            COLORLAN = backGroundColors.ready;
            COLORUSB = backGroundColors.ready;
            COLORBUTTON = backGroundColors.ready;
            COLORLED = backGroundColors.ready;
            POWERLED = true;
            PONLED = true;
            INETLED = true;
            WLANLED = true;
            LAN1LED = true;
            LAN2LED = true;
            LAN3LED = true;
            LAN4LED = true;
            WPSLED = true;
            LOSLED = true;
            HELP = false;
            ERRORCODE = "";
        }
    }

    public class LogInfomation {

        public string dateTime { get; set; }
        public string PCName { get; set; }
        public string MacAddress { get; set; }
        public string UploadFW { get; set; }
        public string WriteGPON { get; set; }
        public string WriteWPS { get; set; }
        public string WriteMAC { get; set; }
        public string Lan1 { get; set; }
        public string Lan2 { get; set; }
        public string Lan3 { get; set; }
        public string Lan4 { get; set; }
        public string Usb2 { get; set; }
        public string Usb3 { get; set; }
        public string NutWps { get; set; }
        public string NutReset { get; set; }
        public string LedPower { get; set; }
        public string LedPon { get; set; }
        public string LedInet { get; set; }
        public string LedWlan { get; set; }
        public string LedLan1 { get; set; }
        public string LedLan2 { get; set; }
        public string LedLan3 { get; set; }
        public string LedLan4 { get; set; }
        public string LedWps { get; set; }
        public string LedLos { get; set; }
        public string ErrMessage { get; set; }
        public string Judged { get; set; }

        public LogInfomation() {
            this.dateTime = "--";
            this.PCName = "--";
            this.MacAddress = "--";
            this.UploadFW = "--";
            this.WriteGPON = "--";
            this.WriteWPS = "--";
            this.WriteMAC = "--";
            this.Lan1 = "--";
            this.Lan2 = "--";
            this.Lan3 = "--";
            this.Lan4 = "--";
            this.Usb2 = "--";
            this.Usb3 = "--";
            this.NutWps = "--";
            this.NutReset = "--";
            this.LedPower = "--";
            this.LedPon = "--";
            this.LedInet = "--";
            this.LedWlan = "--";
            this.LedLan1 = "--";
            this.LedLan2 = "--";
            this.LedLan3 = "--";
            this.LedLan4 = "--";
            this.LedWps = "--";
            this.LedLos = "--";
            this.ErrMessage = "--";
            this.Judged = "--";
        }

        private string _Titles() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26}",
                                 "DATETIME",
                                 "PCNAME",
                                 "MACADDRESS",
                                 "UPLOADFW_RESULT",
                                 "WRITEGPON_RESULT",
                                 "WRITEWPS_RESULT",
                                 "WRITEMAC_RESULT",
                                 "LANPORT1_RESULT",
                                 "LANPORT2_RESULT",
                                 "LANPORT3_RESULT",
                                 "LANPORT4_RESULT",
                                 "USB2_RESULT",
                                 "USB3_RESULT",
                                 "BUTTON_WPS_RESULT",
                                 "BUTTON_RESET_RESULT",
                                 "LEDPOWER_RESULT",
                                 "LEDPON_RESULT",
                                 "LEDINET_RESULT",
                                 "LEDWLAN_RESULT",
                                 "LEDLAN1_RESULT",
                                 "LEDLAN2_RESULT",
                                 "LEDLAN3_RESULT",
                                 "LEDLAN4_RESULT",
                                 "LEDWPS_RESULT",
                                 "LEDLOS_RESULT",
                                 "MESSAGE",
                                 "TOTAL_RESULT");
        }
        private string _Contents() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26}",
                                  this.dateTime,
                                  this.PCName,
                                  this.MacAddress,
                                  this.UploadFW,
                                  this.WriteGPON,
                                  this.WriteWPS,
                                  this.WriteMAC,
                                  this.Lan1,
                                  this.Lan2,
                                  this.Lan3,
                                  this.Lan4,
                                  this.Usb2,
                                  this.Usb3,
                                  this.NutWps,
                                  this.NutReset,
                                  this.LedPower,
                                  this.LedPon,
                                  this.LedInet,
                                  this.LedWlan,
                                  this.LedLan1,
                                  this.LedLan2,
                                  this.LedLan3,
                                  this.LedLan4,
                                  this.LedWps,
                                  this.LedLos,
                                  this.ErrMessage,
                                  this.Judged);
        }

        public void Save(string Jud, string message) {
            this.PCName = System.Environment.MachineName;
            this.dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.MacAddress = GlobalData.testingInfo.MAC;
            this.ErrMessage = message;
            this.Judged = Jud;

            string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //Create folder log
            if (!System.IO.Directory.Exists(rootPath + "Log")) System.IO.Directory.CreateDirectory(rootPath + "Log");
            //Create log file
            string file = string.Format("{0}Log\\{1}.csv", rootPath, DateTime.Now.ToString("yyyyMMdd"));
            if (!System.IO.File.Exists(file)) {
                System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
                st.WriteLine(_Titles());
                st.WriteLine(_Contents());
                st.Dispose();
            } else {
                System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
                st.WriteLine(_Contents());
                st.Dispose();
            }
        }

        public void SaveSystemLog() {
            string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //Create folder log
            string dir = rootPath + "LogDetail";
            if (!System.IO.Directory.Exists(rootPath + "LogDetail")) System.IO.Directory.CreateDirectory(rootPath + "LogDetail");
            //If file count > 30. Delete oldest file.
            DirectoryInfo dinfo = new DirectoryInfo(dir);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            var orderFiles = Files.OrderBy(f => f.LastWriteTime);
            if (Files.Length > 30) {
                foreach(var f in orderFiles) {
                    string fname = f.Name.Replace(".txt", "");
                    string _txt = "";
                    if (fname.Length == 8) _txt = string.Format("{0},{1},{2}", fname.Substring(0,4), fname.Substring(4,2), fname.Substring(6,2));
                    if (_txt != "") {
                        DateTime _old;
                        if (DateTime.TryParse(_txt, out _old)) {
                            if (DateTime.Now.Subtract(_old).Days > 30) File.Delete(f.FullName);
                        }
                    }
                }
            }
            
            //Create log file
            string file = string.Format("{0}LogDetail\\{1}.txt", rootPath, DateTime.Now.ToString("yyyyMMdd"));
            System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
            st.WriteLine(GlobalData.testingInfo.LOGSYSTEM);
            st.Dispose();
        }

        public void SaveUARTLog() {
            string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //Create folder log
            string dir = rootPath + "LogUART";
            if (!System.IO.Directory.Exists(rootPath + "LogUART")) System.IO.Directory.CreateDirectory(rootPath + "LogUART");
            //If file count > 30. Delete oldest file.
            DirectoryInfo dinfo = new DirectoryInfo(dir);
            FileInfo[] Files = dinfo.GetFiles("*.txt");
            var orderFiles = Files.OrderBy(f => f.LastWriteTime);
            if (Files.Length > 30) {
                foreach (var f in orderFiles) {
                    string fname = f.Name.Replace(".txt", "");
                    string _txt = "";
                    if (fname.Length == 8) _txt = string.Format("{0},{1},{2}", fname.Substring(0, 4), fname.Substring(4, 2), fname.Substring(6, 2));
                    if (_txt != "") {
                        DateTime _old;
                        if (DateTime.TryParse(_txt, out _old)) {
                            if (DateTime.Now.Subtract(_old).Days > 30) File.Delete(f.FullName);
                        }
                    }
                }
            }

            //Create log file
            string file = string.Format("{0}LogUART\\{1}.txt", rootPath, DateTime.Now.ToString("yyyyMMdd"));
            System.IO.StreamWriter st = new System.IO.StreamWriter(file, true);
            st.WriteLine(GlobalData.testingInfo.LOGUART);
            st.Dispose();
        }
    }
}
