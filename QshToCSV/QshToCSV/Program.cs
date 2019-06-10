using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QScalp;
using QScalp.History;
using QScalp.History.Reader;

namespace QshToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            //Settings set = new Settings();
            //set.ReadSettings();
            //ConvertFile(fileName, format);
            WriteY();
            Console.WriteLine("ASD");

            Console.WriteLine("vAH");
            while (true)
                Console.Write("x"); // Все время печатать 'x'

            Console.ReadKey();
        }
        static void WriteY()
        {
            while (true)
                Console.Write("y"); // Все время печатать 'y'
        }


        private static void ConvertFile(string fileName, string format)
        {
            List<string> Quotes = new List<string>();

            var fileNameKey = format + "_" + fileName;

            //read .qsh file
            using (var qr = QshReader.Open(fileName))
            {
                var reader = qr;
                var currentDate = qr.CurrentDateTime;

                for (var i = 0; i < qr.StreamCount; i++)
                {
                    var stream = (ISecurityStream)qr[i];

                    switch (stream.Type)
                    {
                        case StreamType.Quotes:
                            {
                                    ((IQuotesStream)stream).Handler += quotes =>
                                    {
                                        string s = "";

                                        foreach (var item in quotes)
                                        {
                                            s = s + "||" + item.Price.ToString() +";" + item.Type.ToString() + ";" + item.Volume.ToString();
                                        }
                                        s = s.Remove(0,2) + "||" + qr.CurrentDateTime.ToString();
                                        Quotes.Add(s);
                                        
                                    };
                                break;
                            }
                        case StreamType.Deals:
                            {
                                //    ((IDealsStream)stream).Handler += deal =>
                                //    {
                                //        secData.Item2.Add(new ExecutionMessage
                                //        {
                                //            LocalTime = reader.CurrentDateTime.ApplyTimeZone(TimeHelper.Moscow),
                                //            HasTradeInfo = true,
                                //            ExecutionType = ExecutionTypes.Tick,
                                //            SecurityId = securityId,
                                //            OpenInterest = deal.OI == 0 ? (long?)null : deal.OI,
                                //            ServerTime = deal.DateTime.ApplyTimeZone(TimeHelper.Moscow),
                                //            TradeVolume = deal.Volume,
                                //            TradeId = deal.Id == 0 ? (long?)null : deal.Id,
                                //            TradePrice = deal.Price,
                                //            OriginSide =
                                //                deal.Type == DealType.Buy
                                //                    ? Sides.Buy
                                //                    : (deal.Type == DealType.Sell ? Sides.Sell : (Sides?)null)
                                //        });

                                //        TryFlushData(registry, security, format, ExecutionTypes.Tick, secData.Item2, reader);
                                //    };
                                break;
                            }
                        default:
                            throw new ArgumentOutOfRangeException("Неподдерживаемый тип потока {0}.");
                    }
                }

                while (qr.CurrentDateTime != DateTime.MaxValue)
                { qr.Read(true); }

            }
            TryFlushData(Quotes);
        }
        //record file result
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
