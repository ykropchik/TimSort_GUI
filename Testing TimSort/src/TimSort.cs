﻿using System.Collections.Generic;
using System.Diagnostics;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Documents;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Testing_TimSort
{
    public class TimSort
    {
        private const byte MIN_GALLOP = 7;
        private const byte MAX_STACK = 40;
        private static ulong _transposition;
        private static ulong _comparisons;
        private static int[] _array;
        private static Stack<RunInfo> _runStack;
        
        private struct RunInfo
        {
            public int Start;
            public int Length;
        }

        private static int GetMinRun(int size)
        {
            int flag = 0;           
            while (size >= 64) {
                flag |= size & 1;
                size >>= 1;
            }
            return size + flag;
        }

        private static void Insertion(RunInfo subArr)
        {
            for (int i = subArr.Start + 1; i < subArr.Start + subArr.Length; i++)
            {
                for (int j = i; j > subArr.Start; j--)
                {
                    _comparisons++;
                    if (_array[j - 1] > _array[j])
                    {
                        int temp = _array[j - 1];
                        _array[j - 1] = _array[j];
                        _array[j] = temp;
                        _transposition++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void Overturn(RunInfo subArr)
        {
            int temp;
            int end = subArr.Start + subArr.Length - 1;
            for (int i = 0; i < subArr.Length / 2; i++)
            {
                temp = _array[end - i];
                _array[end - i] = _array[subArr.Start + i];
                _array[subArr.Start + i] = temp;
            }
        }

        private static void CheckInvariant()
        {
            bool isAlright = false;
            

            while (!isAlright)
            {
                RunInfo first, second;
                if (_runStack.Count > 2)
                {
                    var third = _runStack.Pop();
                    second = _runStack.Pop();
                    first = _runStack.Pop();

                    if (first.Length <= second.Length + third.Length || second.Length <= third.Length)
                    {
                        if (first.Length <= third.Length)
                        {
                            Merge(first, second);
                            _runStack.Push(new RunInfo()
                            {
                                Start = first.Start,
                                Length = first.Length + second.Length
                            });
                            _runStack.Push(third);
                        }
                        else
                        {
                            Merge(second, third);
                            _runStack.Push(first);
                            _runStack.Push(new RunInfo()
                            {
                                Start = second.Start,
                                Length = second.Length + third.Length
                            });
                        }
                    }
                    else
                    {
                        _runStack.Push(first);
                        _runStack.Push(second);
                        _runStack.Push(third);
                        isAlright = true;
                    }
                } else if (_runStack.Count > 1)
                {
                    second = _runStack.Pop();
                    first = _runStack.Pop();

                    if (first.Length <= second.Length)
                    {
                        Merge(first, second);
                        _runStack.Push(new RunInfo()
                        {
                            Start = first.Start,
                            Length = first.Length + second.Length
                        });
                    }
                    else
                    {
                        _runStack.Push(first);
                        _runStack.Push(second);
                        isAlright = true;
                    }
                }
                else
                {
                    isAlright = true;
                }
            }
        }

        private static void Merge(RunInfo first, RunInfo second)
        {
            int[] tempArray;

            if (first.Length <= second.Length)
            {
                tempArray = new int[first.Length];
                for (int i = 0; i < first.Length; i++)
                {
                    tempArray[i] = _array[first.Start + i];
                }
                
                int firstPointer = 0, 
                    secondPointer = second.Start;
                for (int i = first.Start; i < second.Start + second.Length - 1; i++)
                {
                    _comparisons++;
                    if (firstPointer >= first.Length)
                    {
                        _array[i] = _array[secondPointer++];
                    } else if (secondPointer >= second.Start + second.Length)
                    {
                        _array[i] = tempArray[firstPointer++];
                    }
                    else
                    {
                        _array[i] = tempArray[firstPointer] <= _array[secondPointer]
                            ? tempArray[firstPointer++]
                            : _array[secondPointer++];
                    }
                }
            }
            else
            {
                tempArray = new int[second.Length];
                for (int i = 0; i < second.Length; i++)
                {
                    tempArray[i] = _array[second.Start + i];
                }
                
                int firstPointer = first.Start + first.Length - 1, 
                    secondPointer = tempArray.Length - 1;
                for (int i = second.Start + second.Length - 1; i >= first.Start; i--)
                {
                    _comparisons++;
                    if (secondPointer < 0)
                    {
                        _array[i] = _array[firstPointer--];
                    } else if (firstPointer < first.Start)
                    {
                        _array[i] = tempArray[secondPointer--];
                    }
                    else
                    {
                        _array[i] = tempArray[secondPointer] > _array[firstPointer]
                            ? tempArray[secondPointer--]
                            : _array[firstPointer--];
                    }
                    
                }
            }
        }

        private static void CheckOrder()
        {
            int errorCount = 0;
            for (int i = 1; i < _array.Length; i++)
            {
                if (_array[i - 1] > _array[i])
                {
                    errorCount++;
                }
            }
        }
        
        public static (ulong, ulong, long) Sorting(int[] array)
        {
            _array = array;
            _transposition = 0;
            _comparisons = 0;
            var stopWatch = new Stopwatch();
            
            int arrayLength = array.Length;
            int minRun = GetMinRun(arrayLength);
            _runStack = new Stack<RunInfo>();
            RunInfo tempRun = new RunInfo();
            int pointer = 0;
            stopWatch.Start();

            while (pointer < arrayLength - 1)
            {
                tempRun.Start = pointer;
                tempRun.Length = 2;
                if (array[pointer] > array[pointer + 1])
                {
                    pointer++;
                    while (_array[pointer] > _array[pointer + 1] && pointer < arrayLength - 1)
                    {
                        tempRun.Length++;
                        pointer++;
                    }
                    
                    Overturn(tempRun);

                    while (tempRun.Length < minRun && pointer < arrayLength - 1)
                    {
                        tempRun.Length++;
                        pointer++;
                    }
                }
                else
                {
                    pointer++;
                    while ((tempRun.Length < minRun || _array[pointer] <= _array[pointer + 1]) && pointer < arrayLength - 1)
                    {
                        tempRun.Length++;
                        pointer++;
                    }
                }
                
                Insertion(tempRun);
                _runStack.Push(tempRun);
                CheckInvariant();
                if (_runStack.Count == MAX_STACK)
                {
                    var second = _runStack.Pop();
                    var first = _runStack.Pop();
                    Merge(first, second);
                    _runStack.Push(new RunInfo()
                    {
                        Start = first.Start,
                        Length = first.Length + second.Length
                    });
                    CheckInvariant();
                }
                
                pointer++;
            }

            while (_runStack.Count > 1)
            {
                var second = _runStack.Pop();
                var first = _runStack.Pop();
                Merge(first, second);
                _runStack.Push(new RunInfo()
                {
                    Start = first.Start,
                    Length = first.Length + second.Length
                });
                CheckInvariant();
            }
            
            stopWatch.Stop();

            CheckOrder();
            
            return (_comparisons, _transposition, stopWatch.ElapsedMilliseconds);
        }
    }
}