using System;
using System.Collections.Generic;
using System.IO;
using QScalp.History;
using QScalp.History.Reader;

namespace QshToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "E:\\Download\\test\\SBER.2019-12-18.Quotes.qsh";

            ConvertFile(fileName, "csv");

            Console.ReadKey();
        }

        private static void ConvertFile(string fileName, string format)
        {
            var Quotes = new List<string>();
            var fileNameKey = $"{format}_{fileName}";

            using (var qr = QshReader.Open(fileName))
            {
                var reader = qr;
                var currentDate = qr.CurrentDateTime;

                for (var i = 0; i < qr.StreamCount; i++)
                {
                    var stream = (ISecurityStream)qr[i];

                    if (stream.Type != StreamType.Quotes)
                        continue;

                    ((IQuotesStream)stream).Handler += quotes =>
                    {
                        string s = string.Empty;

                        foreach (var item in quotes)
                            s = s + "||" + item.Price.ToString() + ";" + item.Type.ToString() + ";" + item.Volume.ToString();

                        s = s.Remove(0, 2) + "||" + qr.CurrentDateTime.ToString();
                        Quotes.Add(s);
                    };

                    break;
                }

                while (qr.CurrentDateTime != DateTime.MaxValue)
                { qr.Read(true); }
            }

            TryFlushData(Quotes);
        }

        private static void TryFlushData(List<string> Quotes)
        {
            try
            {
                using (var str = new FileStream(Path.Combine(Environment.CurrentDirectory, "result.csv"), FileMode.Create, FileAccess.Write))
                {
                    using (var wr = new StreamWriter(str))
                    {
                        foreach (string st in Quotes)
                        {
                            wr.WriteLine(st);
                        }
                        wr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
