using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Input;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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
    public class ResultsItem
    {
        public string ResultName { get; set; }
        public ICommand Command { get; set; }
    }
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultsPage : Page
    {
        
        private ObservableCollection<ResultsItem> resultsNamesCollection =
            new ObservableCollection<ResultsItem>();
        
        StandardUICommand deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
        
        public ResultsPage()
        {
            this.InitializeComponent();
            
            deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
        }
        
        private async void GridViewRight_OnLoaded(object sender, RoutedEventArgs e)
        {
            // for (int i = 0; i < 17; i++)
            // {
            //     resultsNamesCollection.Add(
            //         new ResultsItem {
            //             ResultName = "Отчет №" + i,
            //             Command = deleteCommand });
            // }
            
            // var filesPicker = new FileOpenPicker();
            // filesPicker.ViewMode = PickerViewMode.Thumbnail;
            // filesPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            // filesPicker.FileTypeFilter.Add(".json");
            // StorageFile jsonFile = await filesPicker.PickSingleFileAsync();
            // string jsonString = await FileIO.ReadTextAsync(jsonFile, UnicodeEncoding.Utf8);
            //
            // JsonParser.Parse(jsonString);
            // var names = JsonParser.GetNames();
            //
            

            if (File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}"))
            {
                StorageFile jsonFile = await ApplicationData.Current.LocalFolder.GetFileAsync("Results.json");
                JsonParser.Parse(await FileIO.ReadTextAsync(jsonFile, UnicodeEncoding.Utf8));
                var names = JsonParser.GetNames();
                
                foreach (var t in names)
                {
                    resultsNamesCollection.Add(
                        new ResultsItem {
                            ResultName = t,
                            Command = deleteCommand });
                }
                
            }
            
            ReportCreator.ReportCreate();
            
            var gridView = (GridView)sender;
            gridView.ItemsSource = resultsNamesCollection;
        }
        
        private void GridViewRight_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ChartsPage), (e.ClickedItem as ResultsItem).ResultName);
        }
        
        private void DeleteCommand_ExecuteRequested(
            XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            // If possible, remove specified item from collection.
            if (args.Parameter != null)
            {
                for (int i = 0; i < resultsNamesCollection.Count; i++)
                {
                    if (resultsNamesCollection[i] == args.Parameter)
                    {
                        resultsNamesCollection.RemoveAt(i);
                        return;
                    }
                }
            }
            if (GridViewRight.SelectedIndex != -1)
            {
                resultsNamesCollection.RemoveAt(GridViewRight.SelectedIndex);
            }
        }

        private async void CreateBut_onClick(object sender, RoutedEventArgs e)
        {
            var filesPicker = new FileOpenPicker();
            filesPicker.ViewMode = PickerViewMode.Thumbnail;
            filesPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filesPicker.FileTypeFilter.Add(".seq");
            
            IReadOnlyList<StorageFile> filesList = new List<StorageFile>();
            filesList = await filesPicker.PickMultipleFilesAsync();
        }
    }
}