using System;
using System.IO;
using System.Linq;
using Binance;
using Binance.Cache;
using Binance.WebSocket;
using BinanceFlushMarketData.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BinanceFlushMarketData
{
    class Program
    {
        static void Main(string[] args)
        {
            ExampleMain();
        }

        private static void ExampleMain()
        {
            try
            {
                // Load configuration.
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, false)
                    .Build();

                // Configure services.
                var services = new ServiceCollection()
                    .AddBinance() 
                    .AddLogging(builder => builder 
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")))

                    .BuildServiceProvider();

                Console.Clear(); 

                var symbols = configuration.GetSection("OrderBook:Symbols").Get<string[]>()
                    ?? new string[] { Symbol.BTC_USDT };

                var limit = 20;
                try { limit = Convert.ToInt32(configuration.GetSection("OrderBook")?["Limit"]); }
                catch { /* ignore */ }

                var cache = services.GetService<IDepthWebSocketCache>();

                cache.Error += (s, e) => Console.WriteLine(e.Exception.Message);

                foreach (var symbol in symbols)
                {
                    cache.Subscribe(symbol, limit, Display); 

                    lock (_sync)
                    {
                        _message = symbol == symbols.Last()
                            ? $"Symbol: \"{symbol}\" ...press any key to exit."
                            : $"Symbol: \"{symbol}\" ...press any key to continue.";
                    }
                    Console.ReadKey(true); 

                   
                    cache.Unsubscribe();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("  ...press any key to close window.");
                Console.ReadKey(true);
            }
        }

        private static string _message;
        private static readonly object _sync = new object();

        private static void Display(OrderBookCacheEventArgs args)
        {
            lock (_sync)
            {
                Console.SetCursorPosition(0, 0);
                args.OrderBook.Print(Console.Out, 20);

                Console.WriteLine();
                Console.WriteLine(_message.PadRight(119));
            }
        }
    }
}
