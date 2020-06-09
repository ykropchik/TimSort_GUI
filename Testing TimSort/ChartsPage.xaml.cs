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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Testing_TimSort
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChartsPage : Page
    {
        
        private string TestName { get; set; }
        public class SortingResult
        {
            public string Name { get; set; }
            public int TimeTimSort { get; set; }
            public int ComparisonsTimSort { get; set; }
            public int TranspositionTimSort { get; set; }
            public int TimeInsert { get; set; }
            public int ComparisonsInsert { get; set; }
            public int TranspositionInsert { get; set; }
        }

        private List<SortingResult> resultsList = new List<SortingResult>();
        
        public ChartsPage()
        {
            this.InitializeComponent();
            
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TestName = (string) e.Parameter;
            resultsList = JsonParser.GetTestData(TestName);
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