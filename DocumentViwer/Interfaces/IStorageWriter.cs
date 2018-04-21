using System;
namespace DocumentViwer.Interfaces
{
    public interface IStorageWriter
    {
        string CreateFile(string filename, byte[] bytes);
        bool FileExists(string filename);
        string ConstructFilePath(string filename);
        void DeleteFile(string filename);
    }
}
