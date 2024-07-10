using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Threading;

namespace Interface4GROB4TrackWinService
{
    public partial class FileWatcherService : ServiceBase
    {
        string WatchPath1 = ConfigurationManager.AppSettings["WatchPath1"];
        string WatchPath2 = ConfigurationManager.AppSettings["WatchPath2"];
        public FileWatcherService() 
        {
            InitializeComponent();
            fileWatcherWatchDdriveArticleimagefolder.Created += fileWatcherWatchDdriveArticleimagefolder_Created;
            fileWatcherWatchDDriveMYdataFolder.Created += fileWatcherWatchDDriveMYdataFolder_Created;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                fileWatcherWatchDdriveArticleimagefolder.Path = WatchPath1;
                fileWatcherWatchDDriveMYdataFolder.Path = WatchPath2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        protected override void OnStop()
        {
            try
            {
                Create_ServiceStoptextfile();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// This event monitor folder wheater file created or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileWatcherWatchDDriveMYdataFolder_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(70000);
                //Then we need to check file is exist or not which is created.
                if (CheckFileExistance(WatchPath2, e.Name))
                {
                    //Then write code for log detail of file in text file.
                    CreateTextFile(WatchPath2,e.Name);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// This event monitor folder wheater file created or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fileWatcherWatchDdriveArticleimagefolder_Created(object sender, System.IO.FileSystemEventArgs e)
        {


            try
            {
                Thread.Sleep(70000);
                //Then we need to check file is exist or not which is created.
                if (CheckFileExistance(WatchPath1, e.Name))
                {
                    //Then write code for log detail of file in text file.
                    CreateTextFile(WatchPath1, e.Name);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        #region Method
        private bool CheckFileExistance(string FullPath, string FileName)
        {
            // Get the subdirectories for the specified directory.'
            bool IsFileExist = false;
            DirectoryInfo dir = new DirectoryInfo(FullPath);
            if (!dir.Exists)
                IsFileExist = false;
            else
            {
                string FileFullPath = Path.Combine(FullPath, FileName);
                if (File.Exists(FileFullPath))
                    IsFileExist = true;
            }
            return IsFileExist;


        }
        private void CreateTextFile(string FullPath, string FileName)
        {
            StreamWriter SW;
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW = File.CreateText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"));
                SW.Close();
            }
            using (SW = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW.WriteLine("File Created with Name: " + FileName + " at this location: " + FullPath);
                SW.Close();
            }
        }
        public static void Create_ServiceStoptextfile()
        {
            string Destination = "C:\\Interface4GROB4Track\\FileWatcherWinService";
            StreamWriter SW;
            if (Directory.Exists(Destination))
            {
                Destination = System.IO.Path.Combine(Destination, "txtServiceStop_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                if (!File.Exists(Destination))
                {
                    SW = File.CreateText(Destination);
                    SW.Close();
                }
            }
            using (SW = File.AppendText(Destination))
            {
                SW.Write("\r\n\n");
                SW.WriteLine("Service Stopped at: " + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss"));
                SW.Close();
            }
        }
        #endregion Method
    }
}
