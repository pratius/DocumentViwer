using System;
using System.IO;
using DocumentViwer.Droid.Services;
using DocumentViwer.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidExternalStorageWriter))]
namespace DocumentViwer.Droid.Services
{
    public class AndroidExternalStorageWriter:IStorageWriter
    {
        public string CreateFile(string filename, byte[] bytes)
        {
            //if (!Directory.Exists(Path.Combine(global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "DocumentViwer")))
                //Directory.CreateDirectory(Path.Combine(global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "DocumentViwer"));
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            //var path = Path.Combine(filePath, filename);

            File.WriteAllBytes(filePath, bytes);

            return filePath;
        }

        public bool FileExists(string filename)
        {
            var path = ConstructFilePath(filename);

            return File.Exists(path);
        }

        public void DeleteFile(string filename)
        {
            var path = ConstructFilePath(filename);

            File.Delete(path);
        }

        public string ConstructFilePath(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return filePath;
        }
    }
}
