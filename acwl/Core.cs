using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
//using System.Windows.Forms;

namespace acwl
{
    public class Core //: ApplicationContext
    {
        static String ChannelName = null;
        static Process proc;
        ~Core()
        {


        }


        public Core()
        {



            Config.Register(
                    "Cubeworld Portchanger",
                     "acwl.dll",
                    "acwl.hook.dll");

            RemoteHooking.IpcCreateServer<CubeBridge>(
                 ref ChannelName,
                 WellKnownObjectMode.SingleCall);

            // Enter in the command line arguments, everything you would enter after the executable name itself
            // start.Arguments = arguments;
            // Enter the executable to run, including the complete path

            // Do you want to show a console window?
            // start.WindowStyle = ProcessWindowStyle.Hidden;
            //  start.CreateNoWindow = true;

            //Console.WriteLine("Looking for cube.exe");
            //Process proc = null;
            //do
            //{

            //    var processes = Process.GetProcessesByName("Cube");
            //    proc = processes.FirstOrDefault();
            //    if (proc != null)
            //        break;
            //    Thread.Sleep(500);
            //} while (true);
            Settings settings = new Settings();
            ProcessStartInfo start = new ProcessStartInfo();

            FileInfo fi = new FileInfo(settings.ProgramPath);
            if (fi.Exists)
            {
                Console.WriteLine("Launching: " + settings.ProgramPath);
                start.FileName = settings.ProgramPath;
                start.WorkingDirectory = fi.DirectoryName;

                //  Run the external process & wait for it to finish

                using (proc = Process.Start(start))
                {
                    Console.WriteLine("Injecting:  acwl.hook.dll");
                    RemoteHooking.Inject(proc.Id,
                        "acwl.hook.dll",
                        "acwl.hook.dll",
                        ChannelName,
                        settings.Port);

                    Console.WriteLine("Success, now waiting for exitcode.");
                    proc.WaitForExit();

                    // Retrieve the app's exit code
                    var exitCode = proc.ExitCode;

                    Console.WriteLine("ExitCode: " + exitCode.ToString());
                }



            }
            else
            {
                Console.WriteLine("Executable not found, unable to launch");
            }

            Console.ReadLine();


        }


    }
}
