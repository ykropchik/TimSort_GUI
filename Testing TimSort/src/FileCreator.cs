using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;

namespace Testing_TimSort
{
    public class FileCreator
    {
        string _fileName;
        public async Task CreateFiles(List<(int[], int, int)> sequences, StorageFolder folder)
        {
            StorageFile newFile; 
            for (int i = 0; i < sequences.Count; i++)
            {
                switch (sequences[i].Item3)
                {
                    case 0:    
                        _fileName = "Increasing_" + sequences[i].Item2 + ".seq";
                        break;
                    case 1:
                        _fileName = "Decreasing_" + sequences[i].Item2 + ".seq";
                        break;
                    case 2:
                        _fileName = "Random_" + sequences[i].Item2 + ".seq";
                        break;
                    case 3:
                        _fileName = "Same_" + sequences[i].Item2 + ".seq";
                        break;
                    case 4:
                        _fileName = "PartiallyOrdered_" + sequences[i].Item2 + ".seq";
                        break;
                    case 5:
                        _fileName = "WorstForTimSort_" + sequences[i].Item2 + ".seq";
                        break;
                }
                
                newFile = await folder.CreateFileAsync(_fileName, CreationCollisionOption.ReplaceExisting);
                await RecordFile(newFile, sequences[i].Item1);
            }
        }

        private static async Task RecordFile(IStorageFile file, IReadOnlyCollection<int> sequences)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outStream))
                    {
                        dataWriter.WriteInt32(sequences.Count);

                        foreach (var sequence in sequences)
                        {
                            dataWriter.WriteInt32(sequence);
                        }
                    
                        await dataWriter.StoreAsync();
                        await outStream.FlushAsync();
                    }
                }
            }
        }
    }
}