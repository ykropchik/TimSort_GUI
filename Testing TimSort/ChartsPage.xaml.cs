using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Testing_TimSort
{
    public sealed partial class ChartsPage
    { 
        private int TestIndex { get; set; }
        private List<ExperimentResult> _resultsList = new List<ExperimentResult>();
        
        public ChartsPage()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TestIndex = (int) e.Parameter;
            _resultsList = JsonParser.GetTestData(TestIndex);
            CreateCharts();
        }

        private void CreateCharts()
        {
            for (var i = 0; i < TimeChart.Series.Count; i++)
            {
                ((ColumnSeries) TimeChart.Series[i]).ItemsSource = _resultsList;
                ((ColumnSeries) TranspositionChart.Series[i]).ItemsSource = _resultsList;
                ((ColumnSeries) ComparisonsChart.Series[i]).ItemsSource = _resultsList;
                ((ColumnSeries) AccelerationChart.Series[i]).ItemsSource = _resultsList; 
            }
        }
    }
}