using System;
using Android.OS;
using AccelMonitor.App.Services;
using Xamarin.Forms;
using AccelMonitor.App.Droid;
using System.IO;

[assembly: Dependency(typeof(FileHelper))]
namespace AccelMonitor.App.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}