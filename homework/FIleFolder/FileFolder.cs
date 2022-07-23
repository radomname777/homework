using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework.FIleFolder
{
    public  class FileFolder
    {
        private string _FileName { get; set; }
        private FileFolder() {}
        public FileFolder(string fileName="Source.txt")
        {
            _FileName = fileName;
        }
        public string Read(bool bol = false)
        {
           using FileStream fs = new FileStream(_FileName, FileMode.OpenOrCreate, FileAccess.Read);
           using StreamReader sr = new StreamReader(fs);
           return sr.ReadToEnd();
        }
        public  string CompressString(string text)
        {
            FileInfo fileToCompress = new FileInfo(_FileName);
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                        }
                    }
                }
            }
            File.Delete(_FileName);
            return "";
        }




        public bool DecompressString()
        {
            try
            {
                FileInfo fileToDecompress = new FileInfo(_FileName+".gz");
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                            decompressionStream.CopyTo(decompressedFileStream);
                        
                    File.Delete(fileToDecompress.Name);
                }
                
            }
            catch (Exception) { return false; }
            return true;
        }
        public void Write(string data, bool bol = false)
        {
            using FileStream fs = new FileStream(_FileName, FileMode.Create, FileAccess.Write);
            //if (bol) fs = new FileStream(_FileName, FileMode.Open, FileAccess.Write);
            //else fs = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
            using StreamWriter sw = new StreamWriter(fs); sw.Write(data);

    
        }
    }
}
