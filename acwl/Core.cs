using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"J:\Cube World\Cube.exe";
            start.WorkingDirectory = @"J:\Cube World\";

            //  Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {

                RemoteHooking.Inject(proc.Id,
                    "acwl.hook.dll",
                    "acwl.hook.dll",
                    ChannelName);


                proc.WaitForExit();

                // Retrieve the app's exit code
                var exitCode = proc.ExitCode;
                Console.WriteLine("Exitcode " + exitCode.ToString());
            }




            Console.ReadLine();


        }


    }
}
