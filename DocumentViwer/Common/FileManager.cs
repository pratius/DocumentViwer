using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentViwer.Interfaces;
using PCLStorage;
using Xamarin.Forms;


namespace DocumentViwer.Common
{
    public static class FileManager
    {
        private static string ConstructFilePath(string filename)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    var androidWriter = DependencyService.Get<IStorageWriter>();
                    return androidWriter.ConstructFilePath(filename);

                case Device.iOS:
                    var rootFolder = FileSystem.Current.LocalStorage;
                    return Path.Combine(rootFolder.Path, filename);

                default:
                    return null;
            }
        }

        public async static Task<bool> FileExists(string filename)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    var androidWriter = DependencyService.Get<IStorageWriter>();
                    return androidWriter.FileExists(filename);

                case Device.iOS:
                    var filepath = ConstructFilePath(filename);
                    var result = await FileSystem.Current.LocalStorage.CheckExistsAsync(filepath);
                    return result == ExistenceCheckResult.FileExists;

                default:
                    return false;
            }
        }

        public async static void DeleteFile(string filename)
        {
            try
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        var androidWriter = DependencyService.Get<IStorageWriter>();
                        androidWriter.DeleteFile(filename);
                        break;

                    case Device.iOS:
                        var filepath = ConstructFilePath(filename);
                        var file = await FileSystem.Current.LocalStorage.GetFileAsync(filepath);
                        await file.DeleteAsync();
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        public static Exception OpenFile(string filename)
        {
            try
            {

                var documentViewer = DependencyService.Get<IDocumentViewer>();

                var mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                if (Path.GetExtension(filename).ToLower() == ".pdf")
                    mimeType = "application/pdf";
                else if (Path.GetExtension(filename).ToLower() == ".doc")
                    mimeType = "application/msword";
                else if (Path.GetExtension(filename).ToLower() == ".xls")
                    mimeType = "application/vnd.ms-excel";
                else if (Path.GetExtension(filename).ToLower() == ".xlsx")
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                else if (Path.GetExtension(filename).ToLower() == ".ppt")
                    mimeType = "application/vnd.ms-powerpoint";
                else if (Path.GetExtension(filename).ToLower() == ".jpg")
                    mimeType = "image/jpeg";

                var filepath = ConstructFilePath(filename);

                documentViewer.ShowDocumentFile(filepath, mimeType);
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        public static async Task<string> SaveFile(byte[] fileBytes, string filename)
        {
            try
            {
                string filepath = null;

                if (Device.RuntimePlatform == Device.Android)
                {
                    var androidWriter = DependencyService.Get<IStorageWriter>();
                    filepath = androidWriter.CreateFile(filename, fileBytes.ToArray());
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    var rootFolder = FileSystem.Current.LocalStorage;

                    using (var pdfBytes = new MemoryStream(fileBytes))
                    {
                        var file = await rootFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                        filepath = file.Path;
                        var newFile = await file.OpenAsync(FileAccess.ReadAndWrite);
                        using (var outputStream = newFile)
                            pdfBytes.CopyTo(outputStream);
                    }
                }

                if (string.IsNullOrEmpty(filepath))
                    return "Error: Unable to save file";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return "";
        }
    }
}
