using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GoogleMapsDemo.Services;
using GoogleMapsDemo.Droid.DependencyService;

[assembly:Xamarin.Forms.Dependency(typeof(FilePickerService))]
namespace GoogleMapsDemo.Droid.DependencyService
{
    public class FilePickerService : IFileService
    {
        public string GetFileContent()
        {
            throw new NotImplementedException();
        }

        public string GetFileContent(string filePath)
        {
            throw new NotImplementedException();
        }

        public string GetFilePath()
        {
            throw new NotImplementedException();
        }

        public void StartFilePicker()
        {
            
        }
    }
}