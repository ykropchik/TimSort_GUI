using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Windows.Data.Json;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Parsers.Core;

namespace Testing_TimSort
{
    public static class JsonParser
    {
        private static string Json { get; set; }
        private static JsonValue Value { get; set; }

        public static void Parse(string json)
        {
            Json = json;
            Value = JsonValue.Parse(Json);
        }
        public static List<String> GetNames()
        {
            var result = new List<string>();
            JsonArray testsList = Value.GetObject().GetNamedArray("experiments");

            for (int i = 0; i < testsList.Count; i++)
            {
                result.Add(testsList[i].GetObject().GetNamedString("testName"));
            }
            
            return result;
        }
        
        public static List<ChartsPage.SortingResult> GetTestData(int index)
        {
            var result = new List<ChartsPage.SortingResult>();
            JsonArray testsList = Value.GetObject().GetNamedArray("experiments");
            var array = testsList[index].GetObject().GetNamedArray("data");
            
            for (int j = 0; j < array.Count; j++)
            {
                var test = array[j].GetObject().GetNamedString("fileName");
                result.Add(
                    new ChartsPage.SortingResult()
                    {
                        FileName = array[j].GetObject().GetNamedString("fileName"),
                        TimSort = new ChartsPage.Results()
                        {
                            Time = (long) array[j].GetObject().GetNamedObject("TimSort").GetNamedNumber("time"),
                            Comparisons = (ulong) array[j].GetObject().GetNamedObject("TimSort").GetNamedNumber("comparisons"),
                            Transpositions = (ulong) array[j].GetObject().GetNamedObject("TimSort").GetNamedNumber("transpositions")
                        } ,
                        Insertion = new ChartsPage.Results()
                        {
                            Time = (long) array[j].GetObject().GetNamedObject("InsertionSort").GetNamedNumber("time"),
                            Comparisons = (ulong) array[j].GetObject().GetNamedObject("InsertionSort").GetNamedNumber("comparisons"),
                            Transpositions = (ulong) array[j].GetObject().GetNamedObject("InsertionSort").GetNamedNumber("transpositions")
                        }
                    });
            }

            return result;
        }
    }
}