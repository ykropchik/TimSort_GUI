using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Testing_TimSort
{
    public static class ReportCreator
    {
        public static async  void ReportCreate()
        {
            if (IsReportExisting())
            {
                
            }
            else
            {
                var test = ApplicationData.Current.LocalFolder.Path;
                await ApplicationData.Current.LocalFolder.CreateFileAsync("Results.json");
                var temp = ApplicationData.Current.LocalFolder.GetFileAsync("Results.json");
            }
        }
        private static bool IsReportExisting()
        {
            return File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
        }
    }
}