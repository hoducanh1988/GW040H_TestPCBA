using System.Windows;
using System.Windows.Controls;
using TestPCBAForGW040x.Functions;
using Microsoft.Win32;

namespace TestPCBAForGW040x.UserControls {
    /// <summary>
    /// Interaction logic for ucSetting.xaml
    /// </summary>
    public partial class ucSetting : UserControl {

        private void InitializeItemSource() {
            cbbStation.ItemsSource = initParameters.listStation;
            cbbJig.ItemsSource = initParameters.listJig;
            cbbUSBPort.ItemsSource = initParameters.listUARTPort;
            cbbUSBBaudRate.ItemsSource = initParameters.listBaudRate;
            cbbBarcodeType.ItemsSource = initParameters.listBarcodeType;
            cbbBRPort.ItemsSource = initParameters.listUARTPort;
            cbbBRBaudRate.ItemsSource = initParameters.listBaudRate;
        }

        public ucSetting() {
            InitializeComponent();
            InitializeItemSource();
            this.DataContext = GlobalData.initSetting;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch (b.Content.ToString()) {
                case "browser": {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "firmware *.bin|*.bin";
                        openFileDialog.Title = "Select path of file 'tclinux.bin'";
                        openFileDialog.FileName = "tclinux.bin";
                        if (openFileDialog.ShowDialog() == true) {
                            GlobalData.initSetting.DutFwPath = openFileDialog.FileName;
                        }
                        break;
                    }
                case "Lưu cài đặt": {
                        GlobalData.initSetting.Save();
                        GlobalData.AddTestCase();
                        MessageBox.Show("Thành công.", string.Format("Lưu cài đặt-[DUT{0}]", GlobalData.initSetting.StationNumber), MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }
            
        }
    }
}
