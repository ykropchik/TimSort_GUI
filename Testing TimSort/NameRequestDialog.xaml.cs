using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documentedTextBox_OnTextChangingextBoTextBox_OnTextChanged52&clcid=0x409

namespace Testing_TimSort
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NameRequestDialog : ContentDialog
    {
        public string FileName { get; set; }

        public NameRequestDialog()
        {
            this.InitializeComponent();
            IsPrimaryButtonEnabled = false;
        }
        
        private void TextBox_OnTextChanging(object sender, TextBoxTextChangingEventArgs args)
        {
            IsPrimaryButtonEnabled = ((TextBox) sender).Text.Length != 0;
        }
    }
}