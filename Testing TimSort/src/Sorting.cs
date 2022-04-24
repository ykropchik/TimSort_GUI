using System.Diagnostics;
using System.Threading.Tasks;

namespace Testing_TimSort
{
    internal abstract class Sorting
    {
        public async Task<(ulong, ulong, long)> StartTask(int[] array)
        {
            var tempArray = new int[array.Length];
            array.CopyTo(tempArray, 0);
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            var (comparisons, transpositions) = StartSort(tempArray);
            stopWatch.Stop();
            
            return (comparisons, transpositions, stopWatch.ElapsedMilliseconds);
        }

        /**
         * return (comparisons, transposition)
         */
        protected abstract (ulong, ulong) StartSort(int[] array);
    }
}