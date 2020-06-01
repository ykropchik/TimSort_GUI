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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Testing_TimSort
{
    public class ListItemData
    {
        public int Quantity { get; set; }
        public int SequenceType { get; set; }
        public ICommand Command { get; set; }
    }
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DatasetCreate : Page
    {
        public DatasetCreate()
        {
            this.InitializeComponent();
            //ApplicationView.PreferredLaunchViewSize = new Size(800, 800);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
        
        /// <summary>
        /// ListView item collection.
        /// </summary>
        ObservableCollection<ListItemData> collection = 
            new ObservableCollection<ListItemData>();

        /// <summary>
        /// Handler for the layout Grid control load event.
        /// </summary>
        /// <param name="sender">Source of the control loaded event</param>
        /// <param name="e">Event args for the loaded event</param>
        private void ControlExample_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the standard Delete command.
            var deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;

            for (var i = 0; i < 5; i++)
            {
                collection.Add(
                    new ListItemData {
                        Quantity = 1000000,
                        SequenceType = 0,
                        Command = deleteCommand });
            }
        }

        /// <summary>
        /// Handler for the ListView control load event.
        /// </summary>
        /// <param name="sender">Source of the control loaded event</param>
        /// <param name="e">Event args for the loaded event</param>
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            // Populate the ListView with the item collection.
            listView.ItemsSource = collection;
        }

        /// <summary>
        /// Handler for the Delete command.
        /// </summary>
        /// <param name="sender">Source of the command event</param>
        /// <param name="e">Event args for the command event</param>
        private void DeleteCommand_ExecuteRequested(
            XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            // If possible, remove specfied item from collection.
            if (args.Parameter != null)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i] == args.Parameter)
                    {
                        collection.RemoveAt(i);
                        return;
                    }
                }
            }
            if (ListViewRight.SelectedIndex != -1)
            {
                collection.RemoveAt(ListViewRight.SelectedIndex);
            }
        }

        /// <summary>
        /// Handler for the ListView selection changed event.
        /// </summary>
        /// <param name="sender">Source of the selection changed event</param>
        /// <param name="e">Event args for the selection changed event</param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewRight.SelectedIndex != -1)
            {
                var item = collection[ListViewRight.SelectedIndex];
            }
        }

        // private void QuantitySlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        // {
        //     for (int i = 0; i < collection.Count; i++)
        //     {
        //         if (collection[i] == sender)
        //         {
        //             collection[i].Quantity = (int) e.NewValue;
        //             return;
        //         }
        //     }
        // }

        private void QuantityBox_OnValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            
        }
    }
}