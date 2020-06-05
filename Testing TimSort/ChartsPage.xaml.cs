using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Testing_TimSort
{
    class FolderButtonItem
    {
        public string FolderName { get; set; }
        public ICommand Command { get; set; }
    }
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartsPage : Page
    {
        
        private ObservableCollection<FolderButtonItem> folderNamesCollection =
            new ObservableCollection<FolderButtonItem>();
        
        StandardUICommand deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
        
        public ChartsPage()
        {
            this.InitializeComponent();
            
            deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
        }
        
        private void GridViewRight_OnLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 17; i++)
            {
                folderNamesCollection.Add(
                    new FolderButtonItem {
                        FolderName = "Отчет №" + i,
                        Command = deleteCommand });
            }
            
            var gridView = (GridView)sender;
            gridView.ItemsSource = folderNamesCollection;
        }
        
        private void GridViewRight_OnItemClick(object sender, ItemClickEventArgs e)
        {
        
        }
        
        private void DeleteCommand_ExecuteRequested(
            XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            // If possible, remove specified item from collection.
            if (args.Parameter != null)
            {
                for (int i = 0; i < folderNamesCollection.Count; i++)
                {
                    if (folderNamesCollection[i] == args.Parameter)
                    {
                        folderNamesCollection.RemoveAt(i);
                        return;
                    }
                }
            }
            if (GridViewRight.SelectedIndex != -1)
            {
                folderNamesCollection.RemoveAt(GridViewRight.SelectedIndex);
            }
        }

        private async void CreateBut_onClick(object sender, RoutedEventArgs e)
        {
            var filesPicker = new FileOpenPicker();
            filesPicker.ViewMode = PickerViewMode.Thumbnail;
            // TODO: Save the last place where saved the generated sequences and place it here
            filesPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            filesPicker.FileTypeFilter.Add(".seq");
            
            IReadOnlyList<StorageFile> filesList = new List<StorageFile>();
            filesList = await filesPicker.PickMultipleFilesAsync();
        }
    }
}