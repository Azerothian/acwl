using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acwl
{
    public class CubeBridge : MarshalByRefObject
    {
        public void IsActive()
        {
            
        }



        public void ReportException(Exception ExtInfo)
        {
            Console.WriteLine("An Error has occurred");
        }

        public void IsInstalled(int p)
        {
            Console.WriteLine("Is Active!" + p.ToString());
        }

        public void Ping()
        {
            Console.WriteLine("Ping");
        }

        public void PrintMessage(string[] Package)
        {
            foreach(var pack in Package)
            {
                Console.WriteLine(pack);
            }
        }

        public void ProcessSend(hook.Packet[] sendOut)
        {
            
        }

        public void ProcessRecv(hook.Packet[] recvOut)
        {
            
        }
    }
}
