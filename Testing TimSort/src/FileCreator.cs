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
        string fileName;
        public async Task CreateFiles(List<(int[], int, int)> sequences, StorageFolder folder)
        {
            StorageFile newFile; 
            for (int i = 0; i < sequences.Count; i++)
            {
                switch (sequences[i].Item3)
                {
                    case 0:    
                        fileName = "Increasing_" + sequences[i].Item2 + ".seq";
                        break;
                    case 1:
                        fileName = "Decreasing_" + sequences[i].Item2 + ".seq";
                        break;
                    case 2:
                        fileName = "Random_" + sequences[i].Item2 + ".seq";
                        break;
                    case 3:
                        fileName = "Same_" + sequences[i].Item2 + ".seq";
                        break;
                    case 4:
                        fileName = "PartiallyOrdered_" + sequences[i].Item2 + ".seq";
                        break;
                    case 5:
                        fileName = "WorsForTimSort_" + sequences[i].Item2 + ".seq";
                        break;
                }
                
                newFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await RecordFile(newFile, sequences[i].Item1);
            }
        }

        private async Task RecordFile(StorageFile file, int[] sequence)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outStream))
                    {
                        //dataWriter.WriteString(sequence.Length + "\n");
                        dataWriter.WriteInt32(sequence.Length);

                        for (int i = 0; i < sequence.Length; i++)
                        {
                            //dataWriter.WriteString(sequence[i] + " ");
                            dataWriter.WriteInt32(sequence[i]);
                        }
                    
                        await dataWriter.StoreAsync();
                        await outStream.FlushAsync();
                    }
                }
            }
        }
    }
}