using System.IO;
using System.Reflection;
using DocumentViwer.Common;
using Xamarin.Forms;

namespace DocumentViwer
{
    public partial class DocumentViwerPage : ContentPage
    {
       

        public DocumentViwerPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var fileName = "DocumentViwer.Data.sample.doc";

            if (! await FileManager.FileExists("sample1.txt"))
            {
                var assembly = typeof(DocumentViwerPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(fileName);
                using (var memoryStream = new MemoryStream())
                {
                    BinaryReader br = new BinaryReader(stream);
                    byte[] bytArray = br.ReadBytes((int)stream.Length);
                    await FileManager.SaveFile(bytArray, "sample1.txt");
                }
            }
           
            FileManager.OpenFile("sample1.txt");
        }
    }
}
