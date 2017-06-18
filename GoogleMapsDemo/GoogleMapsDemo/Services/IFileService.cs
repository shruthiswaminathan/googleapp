using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsDemo.Services
{
    public interface IFileService
    {
        void StartFilePicker();

        string GetFilePath();

        string GetFileContent();

        string GetFileContent(string filePath);
    }
}
