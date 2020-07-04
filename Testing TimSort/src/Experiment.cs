using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace Testing_TimSort
{
    public static class Experiment
    {
        public static async Task<List<ChartsPage.SortingResult>> Start(IReadOnlyList<StorageFile> filesList)
        {
            var result = new List<ChartsPage.SortingResult>();

            for (int i = 0; i < filesList.Count; i++)
            {
                var array = await FileReader.ReadFile(filesList[i]);
                var insert = await InsertionSort.Sorting(array); //((ulong) 0, (ulong) 0, 0);
                var timsort = await TimSort.Sorting(array);
                
                result.Add(new ChartsPage.SortingResult()
                {
                    FileName = filesList[i].Name,
                    TimSort = new ChartsPage.Results()
                    {
                        Time = timsort.Item3,
                        Comparisons = timsort.Item1,
                        Transpositions = timsort.Item2
                    } ,
                    Insertion = new ChartsPage.Results()
                    {
                        Time = insert.Item3,
                        Comparisons = insert.Item1,
                        Transpositions = insert.Item2
                    }
                });
            }
            
            return result;
        }
    }
}