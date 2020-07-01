using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Testing_TimSort
{
    public sealed partial class ChartsPage : Page
    {
        public struct Results
        {
            public long Time { get; set; }
            public ulong Comparisons { get; set; }
            public ulong Transpositions { get; set; }
        }
        public class SortingResult
        {
            public string FileName { get; set; }
            public Results TimSort { get; set; }
            public Results Insertion { get; set; }
        }
        
        private int TestIndex { get; set; }
        private List<SortingResult> resultsList = new List<SortingResult>();
        
        public ChartsPage()
        {
            this.InitializeComponent();
            
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TestIndex = (int) e.Parameter;
            resultsList = JsonParser.GetTestData(TestIndex);
            CreateCharts();
        }

        private void CreateCharts()
        {
            for (int i = 0; i < TimeChart.Series.Count; i++)
            {
                (TimeChart.Series[i] as ColumnSeries).ItemsSource = resultsList;
                (TranspositionChart.Series[i] as ColumnSeries).ItemsSource = resultsList;
                (ComparisonsChart.Series[i] as ColumnSeries).ItemsSource = resultsList;
            }
        }
    }
}