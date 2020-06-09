using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Testing_TimSort
{
    public static class ReportCreator
    {
        public static void ReportCreate()
        {
            if (IsReportExisting())
            {
                
            }
            else
            {
                
            }
        }
        private static bool IsReportExisting()
        {
            return File.Exists($@"{ApplicationData.Current.LocalFolder.Path}\{"Results.json"}");
        }
    }
}