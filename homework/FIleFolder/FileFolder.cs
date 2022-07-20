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


           using FileStream fs = new FileStream(_FileName, FileMode.Open, FileAccess.Read);
           using StreamReader sr = new StreamReader(fs);
           return sr.ReadToEnd();
        }
        public  string CompressString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))

                zip.Write(buffer, 0, buffer.Length);
            

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }




        public void DecompressString()
        {
            using (Stream fileStream = File.OpenRead(_FileName),
                     zippedStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(zippedStream))
                    Console.WriteLine(reader.ReadToEnd());

            }
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
