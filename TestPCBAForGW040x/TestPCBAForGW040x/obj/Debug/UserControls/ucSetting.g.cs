﻿#pragma checksum "..\..\..\UserControls\ucSetting.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E46F2FAAA7CDBB658E980314F2807141E8B94B03"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TestPCBAForGW040x.UserControls;


namespace TestPCBAForGW040x.UserControls {
    
    
    /// <summary>
    /// ucSetting
    /// </summary>
    public partial class ucSetting : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbStation;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbUSBPort;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbUSBBaudRate;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbBarcodeType;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbBRPort;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\UserControls\ucSetting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbbBRBaudRate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TestPCBAForGW040x;component/usercontrols/ucsetting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\ucSetting.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cbbStation = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            
            #line 23 "..\..\..\UserControls\ucSetting.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cbbUSBPort = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.cbbUSBBaudRate = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.cbbBarcodeType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.cbbBRPort = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.cbbBRBaudRate = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            
            #line 57 "..\..\..\UserControls\ucSetting.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
