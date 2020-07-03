using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Input;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Microsoft.UI.Xaml.Controls;

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

        private async void UpdateResultsNamesCollection()
        {
            if (File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}"))
            {
                resultsNamesCollection.Clear();
                
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

            GridViewRight.ItemsSource = resultsNamesCollection;
        }
        
        private void GridViewRight_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateResultsNamesCollection();
        }
        
        private void GridViewRight_OnItemClick(object sender, ItemClickEventArgs e)
        {
            
            this.Frame.Navigate(typeof(ChartsPage), resultsNamesCollection.IndexOf(e.ClickedItem as ResultsItem));
        }
        
        private async void DeleteCommand_ExecuteRequested(
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
                        await JsonWriter.RemoveTest(i);
                        return;
                    }
                }
            }
            if (GridViewRight.SelectedIndex != -1)
            {
                resultsNamesCollection.RemoveAt(GridViewRight.SelectedIndex);
                await JsonWriter.RemoveTest(GridViewRight.SelectedIndex);
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

            if (filesList.Count != 0)
            {
                var requestDialog = new NameRequestDialog();
                var dialogResult = await requestDialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary)
                {
                    var progressDialog = new ProgressDialog("Проводится эксперимент, это может занять несколько минут.\n" +
                                                            "Пожалуйста не закрывайте приложение!");
                    progressDialog.ShowAsync();
                    await JsonWriter.WriteResult(await Experiment.Start(filesList), requestDialog.FileName);
                    UpdateResultsNamesCollection();
                    progressDialog.Hide();
                }
            }
        }
        
        private void HelpBut_OnClick(object sender, RoutedEventArgs e)
        {
            HelpTip1.IsOpen = true;
        }
        
        private void TeachingTip_OnActionButtonClick(TeachingTip sender, object args)
        {
            sender.IsOpen = false;
            HelpTip2.IsOpen = true;
        }
        
        private void TeachingTip_OnCloseButtonClick(TeachingTip sender, object args)
        {
            sender.IsOpen = false;
        }
    }
}