using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
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
        public static List<string> GetNames()
        {
            var testsList = Value.GetObject().GetNamedArray("experiments");

            return testsList.Select(test => test.GetObject().GetNamedString("testName")).ToList();
        }
        
        public static List<ExperimentResult> GetTestData(int index)
        {
            var testsList = Value.GetObject().GetNamedArray("experiments");
            var array = testsList[index].GetObject().GetNamedArray("data");

            return (
                from item in array 
                let test = item
                    .GetObject()
                    .GetNamedString("fileName") select new ExperimentResult()
                {
                    FileName = item.GetObject().GetNamedString("fileName"),
                    TimSort = new SortingResult()
                    {
                        Time = (long) item.GetObject().GetNamedObject("TimSort").GetNamedNumber("time"),
                        Comparisons = (ulong) item.GetObject().GetNamedObject("TimSort").GetNamedNumber("comparisons"),
                        Transpositions = (ulong) item.GetObject().GetNamedObject("TimSort").GetNamedNumber("transpositions"),
                        Acceleration = (long) item.GetObject().GetNamedObject("TimSort").GetNamedNumber("acceleration")
                    },
                    Insertion = new SortingResult()
                    {
                        Time = (long) item.GetObject().GetNamedObject("InsertionSort").GetNamedNumber("time"),
                        Comparisons = (ulong) item.GetObject().GetNamedObject("InsertionSort").GetNamedNumber("comparisons"),
                        Transpositions = (ulong) item.GetObject().GetNamedObject("InsertionSort").GetNamedNumber("transpositions"),
                        Acceleration = (long) item.GetObject().GetNamedObject("InsertionSort").GetNamedNumber("acceleration")
                    },
                    Merge = new SortingResult()
                    {
                        Time = (long) item.GetObject().GetNamedObject("MergeSort").GetNamedNumber("time"),
                        Comparisons = (ulong) item.GetObject().GetNamedObject("MergeSort").GetNamedNumber("comparisons"),
                        Transpositions = (ulong) item.GetObject().GetNamedObject("MergeSort").GetNamedNumber("transpositions"),
                        Acceleration = (long) item.GetObject().GetNamedObject("MergeSort").GetNamedNumber("acceleration")
                    }
                }).ToList();
        }
    }
}