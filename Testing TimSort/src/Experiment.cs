using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace Testing_TimSort
{
    public class Experiment
    {
        public async Task<List<ExperimentResult>> Start(IReadOnlyList<StorageFile> filesList)
        {
            var result = new List<ExperimentResult>();

            foreach (var file in filesList)
            {
                var array = await FileReader.ReadFile(file);
                var (insertComp, insertTrans, insertTime) = await new InsertionSort().StartTask(array);
                var (timComp, timTrans, timTime) = await new TimSort().StartTask(array);
                var (mergeComp, mergeTrans, mergeTime) = await new MergeSort().StartTask(array);
                var (timAcceleration, insertAcceleration, mergeAcceleration) = GetAccelerations(timTime, insertTime, mergeTime);
                
                result.Add(new ExperimentResult()
                {
                    FileName = file.Name,
                    TimSort = new SortingResult()
                    {
                        Time = timTime,
                        Comparisons = timComp,
                        Transpositions = timTrans,
                        Acceleration = timAcceleration
                    } ,
                    Insertion = new SortingResult()
                    {
                        Time = insertTime,
                        Comparisons = insertComp,
                        Transpositions = insertTrans,
                        Acceleration = insertAcceleration
                    },
                    Merge = new SortingResult()
                    {
                        Time = mergeTime,
                        Comparisons = mergeComp,
                        Transpositions = mergeTrans,
                        Acceleration = mergeAcceleration
                    }
                });
            }
            
            return result;
        }

        /**
         * @return List(timsort, insertion, merge)
         */
        private static (long, long, long) GetAccelerations(long timSortTime, long insertionTime, long mergeTime)
        {
            if (insertionTime >= timSortTime && insertionTime >= mergeTime)
            {
                return (
                    insertionTime/(timSortTime == 0 ? 1 : timSortTime),
                    1,
                    insertionTime/(mergeTime == 0 ? 1 : mergeTime)
                    );
            }
            
            if (mergeTime >= timSortTime && mergeTime >= insertionTime)
            {
                return (
                    mergeTime/(timSortTime == 0 ? 1 : timSortTime),
                    mergeTime/(insertionTime == 0 ? 1 : insertionTime),
                    1);
            }
            
            return (
                1,
                timSortTime/(insertionTime == 0 ? 1 : insertionTime),
                timSortTime/(mergeTime == 0 ? 1 : mergeTime)
                );
        }
    }
}