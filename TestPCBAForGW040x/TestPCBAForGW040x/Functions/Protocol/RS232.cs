using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Windows;

namespace TestPCBAForGW040x.Functions
{
    public class RS232
    {
        public SerialPort Port = null;

        public RS232() {
            //string message;
            //if(!openSerialPort(out message)) {
            //    GlobalData.testingInfo.LOGSYSTEM += message;
            //}
        }

        /// <summary>
        /// Open Serial Port
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool openSerialPort(out string message) {
            try {
                message = "";
                this.Port = new SerialPort();
                this.Port.PortName = GlobalData.initSetting.USBPort;
                this.Port.BaudRate = int.Parse(GlobalData.initSetting.USBBaudRate);
                this.Port.Parity = Parity.None;
                this.Port.DataBits = 8;
                this.Port.StopBits = StopBits.One;
                this.Port.Open();
                this.Port.DataReceived += new SerialDataReceivedEventHandler(port_OnReceiveDatazz);
                return this.Port.IsOpen == true ? true : false;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void port_OnReceiveDatazz(object sender, SerialDataReceivedEventArgs e) {
            SerialPort s = (SerialPort)sender;
            string receiveData = s.ReadExisting();
            if (receiveData != string.Empty) {
                GlobalData.testingInfo.LOGUART += receiveData;
                GlobalData.testingInfo.DATAUART += receiveData;
            }
            Thread.Sleep(100);
        }

        /// <summary>
        /// Close the serialport
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool closeSerialPort(out string message) {
            try {
                message = "";
                this.Port.Close();
                return true;
            }
            catch (Exception ex) {
                message = ex.ToString();
                return false;
            }
        }

    }
}
