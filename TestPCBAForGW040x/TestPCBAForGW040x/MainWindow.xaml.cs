﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestPCBAForGW040x {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        //calculate startup location
        void _calStartupLocation() {
            double x = 0.25 * (int.Parse(GlobalData.initSetting.JigNumber) - 1);
            GlobalData.thisLocation.top = 0;
            GlobalData.thisLocation.left = SystemParameters.WorkArea.Width * x;
            GlobalData.thisLocation.width = SystemParameters.WorkArea.Width * 0.25;
            GlobalData.thisLocation.height = SystemParameters.WorkArea.Height;

            this.Top = 0;
            this.Left = SystemParameters.WorkArea.Width * x;
            this.Width = SystemParameters.WorkArea.Width * 0.25;
            this.Height = SystemParameters.WorkArea.Height;

            GlobalData.testingInfo.Width = (this.Width / 6) - 10;
            GlobalData.testingInfo.Height = GlobalData.testingInfo.Width;
            GlobalData.testingInfo.ConerRadius = GlobalData.testingInfo.Width / 2;
            GlobalData.testingInfo.HeightMAC = (int)this.Height / 18;
            //MessageBox.Show(GlobalData.testingInfo.HeightMAC.ToString());
            GlobalData.testingInfo.FontMAC = GlobalData.testingInfo.HeightMAC - 10;
        }

        //Constructor MainWindow
        public MainWindow() {
            InitializeComponent();
            this._calStartupLocation();
            this.DataContext = GlobalData.initSetting;
        }

        //Distructor MainWindow
        ~MainWindow() {
            string message = "";
            GlobalData.serialPort.closeSerialPort(out message);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label l = sender as Label;
            switch (l.Content.ToString()) {
                case "X": { Application.Current.Shutdown(); break; }
                case "[test]": {
                        Process.Start("explorer.exe", string.Format("{0}Log",System.AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    }
                case "[detail]": {
                        Process.Start("explorer.exe", string.Format("{0}LogDetail", System.AppDomain.CurrentDomain.BaseDirectory));
                        break;
                    }
                case "Version 1.0.0.0": {
                        if (GlobalData.testingInfo.DEVELOPER < 10) GlobalData.testingInfo.DEVELOPER++;
                        else GlobalData.testingInfo.DEVELOPER = 0;
                        break;
                    }
                case "?": {
                        if (l.Foreground != Brushes.Lime) {
                            l.Foreground = Brushes.Lime;
                            this.Cursor = Cursors.Help;
                            GlobalData.testingInfo.HELP = true;
                        }
                        else {
                            l.Foreground = Brushes.White;
                            this.Cursor = Cursors.Arrow;
                            GlobalData.testingInfo.HELP = false;
                        }
                        break;
                    }
                case "TEST ALL": {
                        this.lblMinus.Margin = new Thickness(5, 0, 0, 0);
                        ucTesting.Visibility = Visibility.Visible;
                        ucSetting.Visibility = Visibility.Collapsed;
                        ucStep.Visibility = Visibility.Collapsed;
                        ucLogin.Visibility = Visibility.Collapsed;
                        Canvas.SetZIndex(ucTesting, 1);
                        break;
                    }
                case "SETTING": {
                        this.lblMinus.Margin = new Thickness(185, 0, 0, 0);
                        ucTesting.Visibility = Visibility.Collapsed;
                        ucStep.Visibility = Visibility.Collapsed;
                        ucSetting.Visibility = Visibility.Collapsed;
                        ucLogin.Visibility = Visibility.Collapsed;
                        LOGIN login = new LOGIN();
                        login.ShowDialog();
                        if (GlobalData.testingInfo.USER=="admin" && GlobalData.testingInfo.PASSWORD == "vnpt") {
                            ucSetting.Visibility = Visibility.Visible;
                            Canvas.SetZIndex(ucSetting, 1);
                        } else {
                            ucLogin.Visibility = Visibility.Visible;
                            Canvas.SetZIndex(ucLogin, 1);
                        }
                        
                        break;
                    }
                case "TEST ONE": {
                        this.lblMinus.Margin = new Thickness(95, 0, 0, 0);
                        ucTesting.Visibility = Visibility.Collapsed;
                        ucSetting.Visibility = Visibility.Collapsed;
                        ucStep.Visibility = Visibility.Visible;
                        ucLogin.Visibility = Visibility.Collapsed;
                        Canvas.SetZIndex(ucStep, 1);
                        break;
                    }
                default: break;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            this._calStartupLocation();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            //this.DragMove();
        }


        //private void Label_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
        //    MessageBox.Show("w");
        //    this._calStartupLocation();
        //}
    }
}
