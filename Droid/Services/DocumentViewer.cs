using System;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using DocumentViwer.Droid.Services;
using DocumentViwer.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DocumentViewer))]
namespace DocumentViwer.Droid.Services
{
    public class DocumentViewer:IDocumentViewer
    {
        public DocumentViewer()
        {
        }

      
        public void ShowDocumentFile(string filepath, string mimeType)
        {
            Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filepath));
            //var uri = global::Android.Net.Uri.Parse("file://" + filepath);
            var intent = new Intent(Intent.ActionView);
           
            intent.SetDataAndType(uri, mimeType);
            intent.SetFlags(ActivityFlags.ClearTop);
            Forms.Context.StartActivity(intent);
            return;
        }
    }
}
