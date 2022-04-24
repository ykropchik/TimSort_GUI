using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Documents;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Testing_TimSort
{
    class TimSort: Sorting
    {
        private const byte MaxStack = 40;
        private static ulong _transposition;
        private static ulong _transpositionDoub;
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
            var flag = 0;           
            while (size >= 64) {
                flag |= size & 1;
                size >>= 1;
            }
            return size + flag;
        }

        private static void Insertion(RunInfo subArr)
        {
            for (var i = subArr.Start + 1; i < subArr.Start + subArr.Length; i++)
            {
                for (var j = i; j > subArr.Start; j--)
                {
                    _comparisons++;
                    if (_array[j - 1] > _array[j])
                    {
                        (_array[j - 1], _array[j]) = (_array[j], _array[j - 1]);
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
            var end = subArr.Start + subArr.Length - 1;
            for (var i = 0; i < subArr.Length / 2; i++)
            {
                (_array[end - i], _array[subArr.Start + i]) = (_array[subArr.Start + i], _array[end - i]);
                _transposition++;
            }
        }

        private static void CheckInvariant()
        {
            var isAlright = false;

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
                for (var i = 0; i < first.Length; i++)
                {
                    tempArray[i] = _array[first.Start + i];
                }
                
                int firstPointer = 0, 
                    secondPointer = second.Start;
                for (var i = first.Start; i < second.Start + second.Length - 1; i++)
                {
                    _comparisons++;
                    if (firstPointer >= first.Length)
                    {
                        _array[i] = _array[secondPointer++];
                        _transpositionDoub++;
                    } else if (secondPointer >= second.Start + second.Length)
                    {
                        _array[i] = tempArray[firstPointer++];
                        _transpositionDoub++;
                    }
                    else
                    {
                        _array[i] = tempArray[firstPointer] <= _array[secondPointer]
                            ? tempArray[firstPointer++]
                            : _array[secondPointer++];
                        _transpositionDoub++;
                    }
                }
            }
            else
            {
                tempArray = new int[second.Length];
                for (var i = 0; i < second.Length; i++)
                {
                    tempArray[i] = _array[second.Start + i];
                }
                
                int firstPointer = first.Start + first.Length - 1, 
                    secondPointer = tempArray.Length - 1;
                for (var i = second.Start + second.Length - 1; i >= first.Start; i--)
                {
                    _comparisons++;
                    if (secondPointer < 0)
                    {
                        _array[i] = _array[firstPointer--];
                        _transpositionDoub++;
                    } else if (firstPointer < first.Start)
                    {
                        _array[i] = tempArray[secondPointer--];
                        _transpositionDoub++;
                    }
                    else
                    {
                        _array[i] = tempArray[secondPointer] > _array[firstPointer]
                            ? tempArray[secondPointer--]
                            : _array[firstPointer--];
                        _transpositionDoub++;
                    }
                    
                }
            }
        }

        protected override (ulong, ulong) StartSort(int[] array)
        {
            _array = array;
            _transposition = 0;
            _comparisons = 0;
            _transpositionDoub = 0;
            
            var arrayLength = array.Length;
            var minRun = GetMinRun(arrayLength);
            _runStack = new Stack<RunInfo>();
            var tempRun = new RunInfo();
            var pointer = 0;

            while (pointer < arrayLength - 1)
            {
                tempRun.Start = pointer;
                tempRun.Length = 2;
                _comparisons++;
                if (array[pointer] > array[pointer + 1])
                {
                    pointer++;
                    while (pointer < arrayLength - 1 && _array[pointer] > _array[pointer + 1])
                    {
                        _comparisons++;
                        tempRun.Length++;
                        pointer++;
                    }
                    
                    Overturn(tempRun);

                    while (pointer < arrayLength - 1 && tempRun.Length < minRun)
                    {
                        _comparisons++;
                        tempRun.Length++;
                        pointer++;
                    }
                }
                else
                {
                    pointer++;
                    while (pointer < arrayLength - 1 && (tempRun.Length < minRun || _array[pointer] <= _array[pointer + 1]))
                    {
                        _comparisons++;
                        tempRun.Length++;
                        pointer++;
                    }
                }
                
                _comparisons += 2;
                Insertion(tempRun);
                _runStack.Push(tempRun);
                CheckInvariant();
                if (_runStack.Count == MaxStack)
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
            
            return (_comparisons,  _transposition + _transpositionDoub*2);
        }
    }
}