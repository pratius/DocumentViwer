

using System;
namespace DocumentViwer.Interfaces
{
    public interface IDocumentViewer
    {
        void ShowDocumentFile(string filepath, string mimeType);
    }
}
