using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Newtonsoft.Json;

namespace Testing_TimSort
{
    public static class JsonWriter
    {
        public static async Task WriteResult(List<ChartsPage.SortingResult> results, string testName)
        {
            if (!File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}"))
            {
                CreateFile();
            }

            var streamReader = File.OpenText($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
            var inputString = streamReader.ReadLine();
            streamReader.Dispose();
            
            var inputValue = JsonValue.Parse(inputString);
            var inputArray = inputValue.GetObject().GetNamedArray("experiments");
            var dataArray = new JsonArray();

            for (int i = 0; i < results.Count; i++)
            {
                dataArray.Add(new JsonObject()
                {
                    ["fileName"] = JsonValue.CreateStringValue(results[i].FileName.Replace(".seq", "")),
                    ["TimSort"] = new JsonObject()
                    {
                        ["time"] = JsonValue.CreateNumberValue(results[i].TimSort.Time),
                        ["comparisons"] = JsonValue.CreateNumberValue(results[i].TimSort.Comparisons),
                        ["transpositions"] = JsonValue.CreateNumberValue(results[i].TimSort.Transpositions)
                    },
                    ["InsertionSort"] = new JsonObject()
                    {
                        ["time"] = JsonValue.CreateNumberValue(results[i].Insertion.Time),
                        ["comparisons"] = JsonValue.CreateNumberValue(results[i].Insertion.Comparisons),
                        ["transpositions"] = JsonValue.CreateNumberValue(results[i].Insertion.Transpositions)
                    }
                });
            }
            
            var outputData = new JsonObject();
            outputData["testName"] = JsonValue.CreateStringValue(testName);
            outputData["data"] = dataArray;
            inputArray.Add(outputData);
            var outputObject = new JsonObject();
            outputObject["experiments"] = inputArray;

            var streamWriter = File.CreateText($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
            streamWriter.WriteLine(outputObject.Stringify());
            streamWriter.Dispose();
        }

        public static async Task RemoveTest(int testIndex)
        {
            var streamReader = File.OpenText($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
            var inputString = streamReader.ReadLine();
            streamReader.Dispose();
            
            var inputValue = JsonValue.Parse(inputString);
            var inputArray = inputValue.GetObject().GetNamedArray("experiments");
            inputArray.RemoveAt(testIndex);
            var outputObject = new JsonObject();
            outputObject["experiments"] = inputArray;

            var streamWriter = File.CreateText($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
            streamWriter.WriteLine(outputObject.Stringify());
            streamWriter.Dispose();
        }

        private static void CreateFile()
        {
            var streamWriter = File.CreateText($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");

            var jsonObject = new JsonObject();
            jsonObject["experiments"] = new JsonArray();
            streamWriter.WriteLine(jsonObject.Stringify());
            streamWriter.Dispose();
        }
    }
}