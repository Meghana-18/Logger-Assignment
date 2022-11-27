using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Getting the path of the current directory 
            DirectoryInfo directoryPath = new DirectoryInfo("../../../");

            //Getting the name of the current file
            string filePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            string fileName = Path.GetFileName(filePath);

            //Getting the name of the current function
            MethodBase m = MethodBase.GetCurrentMethod();
            string funcName = m.ReflectedType.Name + "." + m.Name;

            //Creating a logger object 
            Logger log = new Logger(directoryPath.FullName, fileName);

            //Calling methods for different logs
            log.Debug(funcName, "Creating debug log with custom message");
            log.Info(funcName);
            log.Error(funcName);

            // Tried implementing threading but there are some issues in thread synchronization
            // A more efficient lock system has to be implemented
            Thread thr1 = new Thread(() => FirstThread(log));
            Thread thr2 = new Thread(() => SecondThread(log));
            thr1.Start();
            thr2.Start();
        }

        public static void FirstThread(Logger log)
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            string funcName = m.ReflectedType.Name + "." + m.Name;
            log.Debug(funcName,"Creating debug log with custom message");
            Thread.Sleep(1000);
            log.Info(funcName);
        }
        public static void SecondThread(Logger log)
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            string funcName = m.ReflectedType.Name + "." + m.Name;
            log.Error(funcName);
        }
    }
}
