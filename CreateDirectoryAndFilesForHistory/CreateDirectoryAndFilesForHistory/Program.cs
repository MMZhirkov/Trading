using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDirectoryAndFilesForHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            string subpath = "02";

            foreach (var item in System.IO.Directory.GetDirectories(@"E:\HistoryQuick\"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(item);
                dirInfo.CreateSubdirectory(subpath);
                for (int i = 1; i < 29; i++)
                {
                    string n;
                    if (i < 10)
                    {
                        n = "0" + i;
                    }
                    else
                    {
                        n = i.ToString();
                    }
                    string NameDirSubPath = item + "\\" + subpath;
                    DirectoryInfo dirPathSub = new DirectoryInfo(NameDirSubPath);
                    if (dirPathSub.Exists)
                    {
                        dirPathSub.CreateSubdirectory(n);
                    }
                    String NameTxt = NameDirSubPath.Split('\\')[2] + n + NameDirSubPath.Split('\\')[3] + "2019";
                    FileInfo fileInf = new FileInfo(item + "\\" + subpath + "\\" + n + "\\" + NameTxt + ".txt");
                    if (fileInf.Exists)
                    {
                        
                        fileInf.CreateText();
                    }
                }
            }


        }




    }

}
