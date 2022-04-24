using System.Collections.Generic;
using Testing_TimSort;

namespace Testing_TimSort
{
    internal class MergeSort: Sorting
    {
        private static ulong _comparisons;
        protected override (ulong, ulong) StartSort(int[] array)
        {
            _comparisons = 0;
            
            Sort(array, 0, array.Length - 1);
            
            return (_comparisons, 0);
        }
        
        void Merge(IList<int> arr, int left, int mid, int right)
        {
            // Find sizes of two
            // subarrays to be merged
            var subArraySize1 = mid - left + 1;
            var subArraySize2 = right - mid;
  
            // Create temp arrays
            var tempArray1 = new int[subArraySize1];
            var tempArray2 = new int[subArraySize2];
            int i, j;
  
            // Copy data to temp arrays
            for (i = 0; i < subArraySize1; ++i)
                tempArray1[i] = arr[left + i];
            for (j = 0; j < subArraySize2; ++j)
                tempArray2[j] = arr[mid + 1 + j];
  
            // Merge the temp arrays
  
            // Initial indexes of first
            // and second subarrays
            i = 0;
            j = 0;
  
            // Initial index of merged
            // subarray array
            int k = left;
            while (i < subArraySize1 && j < subArraySize2) {
                if (tempArray1[i] <= tempArray2[j])
                {
                    _comparisons++;
                    arr[k] = tempArray1[i];
                    i++;
                }
                else
                {
                    arr[k] = tempArray2[j];
                    j++;
                }
                k++;
            }
  
            // Copy remaining elements
            // of L[] if any
            while (i < subArraySize1)
            {
                arr[k] = tempArray1[i];
                i++;
                k++;
            }
  
            // Copy remaining elements
            // of R[] if any
            while (j < subArraySize2)
            {
                arr[k] = tempArray2[j];
                j++;
                k++;
            }
        }

        private void Sort(IList<int> array, int left, int right)
        {
            if (left >= right) return;
            
            // Find the middle
            // point
            var mid = left+ (right-left)/2;
  
            // Sort first and
            // second halves
            Sort(array, left, mid);
            Sort(array, mid + 1, right);
  
            // Merge the sorted halves
            Merge(array, left, mid, right);
        }
    }
}
