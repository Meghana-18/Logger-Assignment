using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment
{
    public class Logger
    {
        private string timestamp;
        private string level;
        private string fileName;
        private string functionName;
        private int threadID;
        private string message;
        private string logPath;
        private int logFileCount;   //Count of the current number of log files in directory
        private static Object lockObj = new Object();

        //Initiliazing logger object
        public Logger(string currentDirectory, string fName)
        {
            timestamp = "";
            level = "";
            fileName = fName;
            functionName = "";
            threadID = 0;
            message = "";
            logPath = currentDirectory + "/" + fName.Split('.')[0] + "Logs.txt";
            logFileCount = Directory.GetFiles(currentDirectory, "*.txt", SearchOption.TopDirectoryOnly).Length;
        }

        //Function to create log
        public void CreateLog(string lvl, string funcName, string msg)
        {
            DateTime now = DateTime.Now;
            timestamp = now.ToString();
            level = lvl;
            functionName = funcName;
            message = msg;
            threadID = Thread.CurrentThread.ManagedThreadId;

            CheckLength(logPath);

            lock (lockObj)
            {
                using (StreamWriter sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine("Log Entry: ");
                    sw.Write(timestamp + " ");
                    sw.Write("Level: " + level + " ");
                    sw.Write("File: " + fileName + " ");
                    sw.Write("Function: " + functionName + " ");
                    sw.Write("Thread ID: " + threadID +" ");
                    sw.Write("Message: " + message + " ");
                    sw.WriteLine();
                    sw.Close();
                }
            }
        }

        //Function for log rotation
        public void CheckLength(string path)
        {
            if (File.Exists(path))
            {
                long length = new FileInfo(path).Length;
                if (length > 5000000)
                {
                    // Checks that the file count does not exceed 10
                    if(logFileCount < 10)
                    {
                        logFileCount = logFileCount + 1;

                        //The rotated logs are named including time so as to easily understand
                        //which is the older log from the file name itself.
                        path = path.Replace("Logs.txt", "Logs." + DateTime.Now.ToString("HH.mm.ss.fff") + ".txt");

                        File.Move(logPath, path);
                    }
                    else
                    {
                        //Program terminates when the number of files exceeds 10
                        Console.WriteLine("Number of files exceeded");
                        Environment.Exit(1);
                    }
                }
            }
        }

        //Individual functions for different logs
        //Incase the user does not send any message while calling the function,
        //a predefined message will be printed.
        public void Info(string funcName, string message="Info log recorded.") { this.CreateLog("INFO", funcName, message); }
        public void Debug(string funcName, string message = "Debug log recorded.") { this.CreateLog("DEBUG", funcName, message); }
        public void Error(string funcName, string message = "Error log recorded.") { this.CreateLog("ERROR", funcName, message); }
    }
}
