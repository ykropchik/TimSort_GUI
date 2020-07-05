using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Testing_TimSort
{
    public static class InsertionSort
    {
        public static async Task<(ulong, ulong, long)> Sorting(int[] array)
        {
            int[] tempArray = new int[array.Length];
            array.CopyTo(tempArray, 0);
            ulong transposition = 0;
            ulong comparisons = 0;
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            for (int i = 1; i < tempArray.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    comparisons++;
                    if (tempArray[j - 1] > tempArray[j])
                    {
                        int temp = tempArray[j - 1];
                        tempArray[j - 1] = tempArray[j];
                        tempArray[j] = temp;
                        transposition++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            stopWatch.Stop();
            
            return (comparisons, transposition, stopWatch.ElapsedMilliseconds);
        }
    }
}