using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using System.IO;

namespace ParsQsh
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver Browser;
            Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            string pathQshSave = @"C:\Users\mmzhi\Downloads\qsh";

            if (Directory.Exists)
            {
                Console.WriteLine($"{pathQshSave} - не существует");
                Console.ReadKey();
            }

            string[] foldersQsh = Directory.GetDirectories(pathQshSave);

            for (int i = 0; i < foldersQsh.Length; i++)
            {
                try
                {
                    foldersQsh[i] = foldersQsh[i].Replace($"{pathQshSave}\\", string.Empty);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Browser.Navigate().GoToUrl("http://erinrv.qscalp.ru/");
            
            System.Threading.Thread.Sleep(2000);

            var xPath = By.XPath("/html/body/pre/a");

            ReadOnlyCollection <IWebElement> SearchDates = Browser.FindElements(xPath);
            
            List<DayUrl> DatesUrl = new List<DayUrl>();
            
            foreach (var date in SearchDates)
                if (Array.IndexOf(foldersQsh, date.Text)<0)
                    DatesUrl.Add(new DayUrl("http://erinrv.qscalp.ru/" + date.Text, date.Text));

            foreach (var DateUrl in DatesUrl)
            {
                Browser.Navigate().GoToUrl(DateUrl.url);
                
                System.Threading.Thread.Sleep(2000);
               
                ReadOnlyCollection<IWebElement> equitys = Browser.FindElements(By.XPath(xPath));
                
                List<EquityURL> equitysURLs = new List<EquityURL>();
                
                for (int i = 1; i < equitys.Count; i++)
                    equitysURLs.Add(new EquityURL($"{DateUrl.url}/{equitys[i].Text}", equitys[i].Text));

                if (!Directory.Exists($"{pathQshSave}\\{DateUrl.day}"))
                    Directory.CreateDirectory($"{pathQshSave}\\{DateUrl.day}");

                foreach (var equityURL in equitysURLs)
                {
                    try
                    {
                        Browser.FindElement(By.LinkText(equityURL.nameFolder)).Click();

                        System.Threading.Thread.Sleep(1000);

                        File.Move($"{ pathQshSave.Replace("qsh", string.Empty)}{equityURL.nameFolder}", $"{pathQshSave}\\{DateUrl.day}\\{equityURL.nameFolder}");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }

            Console.ReadKey();
        }
    }

    struct EquityURL
    {
        private string pathUrl { get; set; }
        private string nameFolder { get; set; }

        public equityURL(string pathUrl, string nameFolder)
        {
            this.pathUrl = pathUrl;
            this.nameFolder = nameFolder;
        }
    }

    struct DayUrl
    {
        private string url { get; set; }
        private string day { get; set; }

        public dayUrl(string url, string day)
        {
            this.url = url;
            this.day = day;
        }
    }
 }
