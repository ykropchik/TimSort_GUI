using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Windows.Storage;

namespace Testing_TimSort
{
    public class FileCreator
    {
        string fileName;
        public async void CreateFiles(List<(int[], int, int)> sequences, StorageFolder folder)
        {
            for (int i = 0; i < sequences.Count; i++)
            {
                StorageFile newFile;   
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
                }
                
                newFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                RecordFile(newFile, ConvertToString(sequences[i].Item1));
            }
        }

        private async void RecordFile(StorageFile file, string sequence)
        {
            for (int i = 0; i < sequence.Length; i++)
            {
                await FileIO.WriteTextAsync(file, sequence);
            }
            
        }

        private string ConvertToString(int[] sequence)
        {
            string outputString = "";

            for (int i = 0; i < sequence.Length; i++)
            {
                outputString += sequence[i];
                outputString += " ";
            }

            return outputString;
        }
    }
}