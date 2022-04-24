using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
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
    public sealed partial class GeneratorPage
    {
        public GeneratorPage()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// ListView item collection.
        /// </summary>
        private readonly ObservableCollection<ListItemData> _collection = 
            new ObservableCollection<ListItemData>();
        
        // Create the standard Delete command.
        private readonly StandardUICommand _deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);

        /// <summary>
        /// Handler for the layout Grid control load event.
        /// </summary>
        /// <param name="sender">Source of the control loaded event</param>
        /// <param name="e">Event args for the loaded event</param>
        private void ControlExample_Loaded(object sender, RoutedEventArgs e)
        {
            _deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
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
            listView.ItemsSource = _collection;
        }

        /// <summary>
        /// Handler for the Delete command.
        /// </summary>
        /// <param name="sender">Source of the command event</param>
        /// <param name="e">Event args for the command event</param>
        private void DeleteCommand_ExecuteRequested(
            XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            // If possible, remove specified item from collection.
            if (args.Parameter != null)
            {
                for (var i = 0; i < _collection.Count; i++)
                {
                    if (_collection[i] != args.Parameter) continue;
                    
                    _collection.RemoveAt(i);
                    return;
                }
            }
            if (ListViewRight.SelectedIndex != -1)
            {
                _collection.RemoveAt(ListViewRight.SelectedIndex);
            }
        }

        private void AddListElem(object sender, RoutedEventArgs e)
        {
            _collection.Add(
                new ListItemData {
                    Quantity = 500000,
                    SequenceType = 2,
                    Command = _deleteCommand });
        }

        private async void SequenceGenerate(object sender, RoutedEventArgs e)
        {
            if (_collection.Count == 0)
            {
                var warningDialog = new ContentDialog()
                {
                    Title = "Нет настроек для генерации",
                    Content = "Добавьте хотя бы одну конфигурацию последовательности",
                    CloseButtonText = "OK"
                };
                
                await warningDialog.ShowAsync();
                return;
            }
            var savePicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            savePicker.FileTypeFilter.Add("*");

            var folder = await savePicker.PickSingleFolderAsync();

            if (folder == null) return;
            
            var progressDialog = new ProgressDialog("Генерируем последовательности");
            progressDialog.ShowAsync();
                
            var sequences = new List<(int[], int, int)>();


            while (_collection.Count != 0)
            {
                sequences.Add((
                    SequencesGenerator.GenerateSequence(_collection[0].Quantity, _collection[0].SequenceType),
                    _collection[0].Quantity, _collection[0].SequenceType
                ));
                _collection.RemoveAt(0);
            }

            progressDialog.ContentText = "Сохраняем последовательности";
            var fileCreator = new FileCreator();
            await fileCreator.CreateFiles(sequences, folder);
            progressDialog.Hide();
                
            var completeDialog = new CompleteDialog("Готово!", "ОК");
            await completeDialog.ShowAsync();
        }
        
        private void HelpStart(object sender, RoutedEventArgs e)
        {
            HelpTip1.IsOpen = true;
            
        }
        
        private void TeachingTip_OnActionButtonClick(TeachingTip sender, object args)
        {
            if (sender == HelpTip1)
            {
                sender.IsOpen = false;
                HelpTip2.IsOpen = true;
            }

            if (sender != HelpTip2) return;
            
            sender.IsOpen = false;
            HelpTip3.IsOpen = true;
        }
        
        private void TeachingTip_OnCloseButtonClick(TeachingTip sender, object args)
        {
            sender.IsOpen = false;
        }
    }
}