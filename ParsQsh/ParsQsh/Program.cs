using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Drawing;
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


            //get old folder contain .qsh
            string[] foldersQsh = Directory.GetDirectories(pathQshSave);
            for (int i = 0; i < foldersQsh.Length; i++)
            {
                try
                {
                    foldersQsh[i] = foldersQsh[i].Replace(pathQshSave + "\\", "");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }

            //go in browser
            Browser.Navigate().GoToUrl("http://erinrv.qscalp.ru/");
            System.Threading.Thread.Sleep(2000);

            //get list dates for qsh 
            ReadOnlyCollection<IWebElement> SearchDates = Browser.FindElements(By.XPath("/html/body/pre/a"));
            List<dayUrl> DatesUrl = new List<dayUrl>();
            foreach (var date in SearchDates)
            {
                if ( Array.IndexOf(foldersQsh, date.Text)<0)
                {
                    DatesUrl.Add(new dayUrl("http://erinrv.qscalp.ru/" + date.Text, date.Text));
                }
            }

            //get history qsh
            foreach (var DateUrl in DatesUrl)
            {

                Browser.Navigate().GoToUrl(DateUrl.url);
                System.Threading.Thread.Sleep(2000);
               
                ReadOnlyCollection<IWebElement> equitys = Browser.FindElements(By.XPath("/html/body/pre/a"));
                List<equityURL> equitysURLs = new List<equityURL>();
                
                for (int i = 1; i < equitys.Count; i++)
                {
                    equitysURLs.Add(new equityURL(DateUrl.url +"/" + equitys[i].Text, equitys[i].Text ));
                }
             //prepare folder for moving
                if (!Directory.Exists(pathQshSave + "\\" + DateUrl.day))
                {
                    Directory.CreateDirectory(pathQshSave + "\\" + DateUrl.day);
                }
             //download
                foreach (var equityURL in equitysURLs)
                {
                    try
                    {
                        Browser.FindElement(By.LinkText(equityURL.nameFolder)).Click();
                        System.Threading.Thread.Sleep(1000);
                        File.Move(pathQshSave.Replace("qsh","") + equityURL.nameFolder, pathQshSave + "\\" + DateUrl.day + "\\" + equityURL.nameFolder);
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
    struct equityURL
    {
        public string pathUrl { get; set; }
        public string nameFolder { get; set; }
        public equityURL(string pathUrl, string nameFolder)
        {
            this.pathUrl = pathUrl;
            this.nameFolder = nameFolder;
        }
    }
    struct dayUrl
    {
        public string url { get; set; }
        public string day { get; set; }
        public dayUrl(string url, string day)
        {
            this.url = url;
            this.day = day;
        }
    }
 }
