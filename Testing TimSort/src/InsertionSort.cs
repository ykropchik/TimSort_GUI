using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Testing_TimSort
{
    public static class InsertionSort
    {
        public static (ulong, ulong, long) Sorting(int[] array)
        {
            ulong transposition = 0;
            ulong comparisons = 0;
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            for (int i = 1; i < array.Length; i++)
            {
                for (int j = i; (j > 0); j--)
                {
                    comparisons++;
                    if (array[j - 1] > array[j])
                    {
                        int temp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = temp;
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