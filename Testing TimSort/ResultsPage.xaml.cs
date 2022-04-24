using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;

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
        
        private readonly ObservableCollection<ResultsItem> _resultsNamesCollection =
            new ObservableCollection<ResultsItem>();

        private readonly StandardUICommand _deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
        
        public ResultsPage()
        {
            this.InitializeComponent();
            
            _deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
        }

        private async void UpdateResultsNamesCollection()
        {
            if (File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}"))
            {
                _resultsNamesCollection.Clear();
                
                StorageFile jsonFile = await ApplicationData.Current.LocalFolder.GetFileAsync("Results.json");
                JsonParser.Parse(await FileIO.ReadTextAsync(jsonFile, UnicodeEncoding.Utf8));
                var names = JsonParser.GetNames();
                
                foreach (var t in names)
                {
                    _resultsNamesCollection.Add(
                        new ResultsItem {
                            ResultName = t,
                            Command = _deleteCommand });
                }
            }

            GridViewRight.ItemsSource = _resultsNamesCollection;
        }
        
        private void GridViewRight_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateResultsNamesCollection();
        }
        
        private void GridViewRight_OnItemClick(object sender, ItemClickEventArgs e)
        {
            
            this.Frame.Navigate(typeof(ChartsPage), _resultsNamesCollection.IndexOf(e.ClickedItem as ResultsItem));
        }
        
        private async void DeleteCommand_ExecuteRequested(
            XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            // If possible, remove specified item from collection.
            if (args.Parameter != null)
            {
                for (var i = 0; i < _resultsNamesCollection.Count; i++)
                {
                    if (_resultsNamesCollection[i] != args.Parameter) continue;
                    
                    _resultsNamesCollection.RemoveAt(i);
                    await JsonWriter.RemoveTest(i);
                    return;
                }
            }

            if (GridViewRight.SelectedIndex == -1) return;
            
            _resultsNamesCollection.RemoveAt(GridViewRight.SelectedIndex);
            await JsonWriter.RemoveTest(GridViewRight.SelectedIndex);
        }

        private async void CreateBut_onClick(object sender, RoutedEventArgs e)
        {
            var filesPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            filesPicker.FileTypeFilter.Add(".seq");
            
            var filesList = await filesPicker.PickMultipleFilesAsync();

            if (filesList.Count == 0) return;
            
            var requestDialog = new NameRequestDialog();
            var dialogResult = await requestDialog.ShowAsync();

            if (dialogResult != ContentDialogResult.Primary) return;
                
            var progressDialog = new ProgressDialog("Проводится эксперимент, это может занять несколько минут.\n" +
                                                    "Пожалуйста не закрывайте приложение!");
            progressDialog.ShowAsync();
            await JsonWriter.WriteResult(await new Experiment().Start(filesList), requestDialog.FileName);
            UpdateResultsNamesCollection();
            progressDialog.Hide();
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