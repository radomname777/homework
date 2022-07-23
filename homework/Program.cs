using homework.FIleFolder;
using System;
using System.Text;

namespace MyApp
{
    interface DataSource
    {
        public static FileFolder FF { get; set; }
        void WriteData(string data);
        string ReadData();
    }
    public class FileDataSource : DataSource
    {
        public static FileFolder FF { get; set; } = new();

        public FileDataSource(){}

        public  string ReadData()
        {
            return FF.Read();
        }

        public void WriteData(string data)
        {
           FF.Write(data);
        }
    }
    class DataSourceDecorator : DataSource
    {
        protected DataSource Wrappee { get; set; }
        public static FileFolder FF { get; set; } = new();

        public DataSourceDecorator(DataSource? source)
        {
            Wrappee = source;
        }


        public virtual void WriteData(string data) => Wrappee.WriteData(data);

        public virtual string ReadData() => Wrappee.ReadData();
    }
    class EncryptionDecarator : DataSourceDecorator
    {
        public EncryptionDecarator(DataSource? source) : base(source)    { }
        private bool _isok { get; set; } = false;
        private string bytestring { get; set; }
        public override string ReadData()
        {
            if (!_isok) return FF.Read();
            _isok = false;
            WriteData(bytestring);
            
            return FF.Read();
        }

        public override void WriteData(string? data)
        {
            if (_isok) return;
            string Data = FF.Read();
            if (Data == null) return;
            bytestring = data;
            char[] ch = data.ToCharArray();
            var Data2 = Encoding.Default.GetBytes(Data);
            for (int i = 0; i < Data2.Length; i++)
                foreach (var item in ch)Data2[i]^=(byte)item;
            FF.Write(Encoding.Default.GetString(Data2));
            _isok = true;
        }
    }
    class CompressionDecorator : DataSourceDecorator
    {
        public CompressionDecorator(DataSource? source) : base(source){  }
        public override string  ReadData()
        {
            if (FF.DecompressString()) return FF.Read();
            else { Console.WriteLine("Not File"); return null; }
        }

        public override void WriteData(string data) => FF.Write(FF.CompressString(FF.Read()));
        
        
    }
    public class Program
    {
        static void Main(string[] args)
        {

            DataSource sad = new FileDataSource() ;
            sad = new DataSourceDecorator(sad);
            sad.WriteData("Niahd");
            Console.WriteLine(sad.ReadData());
        }
    }
}
