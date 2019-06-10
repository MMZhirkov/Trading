using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QshToCSV
{
    class Settings
    {
       private string pathDir { get; set; }
       private string format { get; set; }

       public void ReadSettings()
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(Path.Combine(Environment.CurrentDirectory, "Settings", "Settings.xml"));
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;

                foreach (XmlNode xnode in xRoot)
                {
                    // обходим все дочерние узлы элемента set
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        // если узел - директория
                        if (childnode.Name == "dir")
                        {
                            pathDir = childnode.InnerText;
                        }
                        // если узел - формат
                        if (childnode.Name == "format")
                        {
                            format = childnode.InnerText;
                        }
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
