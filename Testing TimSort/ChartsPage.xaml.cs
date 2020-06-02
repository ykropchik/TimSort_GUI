﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Testing_TimSort
{

    class FolderButtonItem
    {
        public string FolderName { get; set; }
    }
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartsPage : Page
    {
        
        private ObservableCollection<FolderButtonItem> folderNamesCollection =
            new ObservableCollection<FolderButtonItem>();
        
        public ChartsPage()
        {
            this.InitializeComponent();
        }
        
        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                folderNamesCollection.Add(
                    new FolderButtonItem {
                        FolderName = "Folder" + i });
            }
            
            var gridView = (GridView)sender;
            gridView.ItemsSource = folderNamesCollection;
        }
    }
}