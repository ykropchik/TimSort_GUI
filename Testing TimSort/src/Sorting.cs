using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Testing_TimSort
{
    public class Sorting : IBackgroundTask
    {
        
        volatile bool _cancelRequested = false;
        
        public Sorting()
        {
            
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var cost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            if (cost == BackgroundWorkCostValue.High)
            {
                return;
            }
            
            var cancel = new CancellationTokenSource();
            taskInstance.Canceled += (s, e) =>
            {
                cancel.Cancel();
                cancel.Dispose();
                _cancelRequested = true;
            };
        }

        private async Task DoWork(IBackgroundTaskInstance taskInstance)
        {
            
            taskInstance.Progress = (10 * 100 / 1);
        }
    }
}