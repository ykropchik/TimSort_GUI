using System.Diagnostics;
using System.Threading.Tasks;

namespace Testing_TimSort
{
    class InsertionSort : Sorting
    {
        protected override (ulong, ulong) StartSort(int[] array)
        {
            ulong transposition = 0;
            ulong comparisons = 0;
            for (var i = 1; i < array.Length; i++)
            {
                for (var j = i; j > 0; j--)
                {
                    comparisons++;
                    if (array[j - 1] > array[j])
                    {
                        (array[j - 1], array[j]) = (array[j], array[j - 1]);
                        transposition++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            return (comparisons, transposition);
        }
    }
}