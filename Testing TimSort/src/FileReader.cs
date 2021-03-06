﻿using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Testing_TimSort
{
    public static class FileReader
    {
        public static async Task<int[]> ReadFile(StorageFile file)
        {
            int[] result;
            var stream = await file.OpenAsync(FileAccessMode.Read);
            ulong size = stream.Size;
            using (var inputStream = stream.GetInputStreamAt(0))
            {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                { 
                    await dataReader.LoadAsync((uint)size);
                    int quantity = dataReader.ReadInt32();
                    result = new int[quantity];

                    for (int i = 0; i < quantity; i++)
                    {
                        result[i] = dataReader.ReadInt32();
                    }
                }
            }
            return result;
        }
    }
}