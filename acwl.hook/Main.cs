using EasyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace acwl.hook
{
    public class Main : EasyHook.IEntryPoint
    {

        CubeBridge Interface;
        Dictionary<string, LocalHook> _hooks;

        LocalHook ConnectHook;
        static Queue<String> PrintQueue = new Queue<String>();

        static Queue<Packet> SendPacketQueue = new Queue<Packet>();
        static Queue<Packet> RecvPacketQueue = new Queue<Packet>();

        static bool TrackPackets = false;
        static ushort Port = 0;

        public Main(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // connect to host...
            _hooks = new Dictionary<string, LocalHook>();
            Interface =
              RemoteHooking.IpcConnectClient<CubeBridge>(InChannelName);
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // install hook...
            try
            {
                _hooks.Add("connect", LocalHook.Create(
                    LocalHook.GetProcAddress("Ws2_32.dll", "connect"),
                    new ptrConnect(ptrConnect_Hooked),
                    this));
                // ConnectHook.ThreadACL.SetInclusiveACL(new Int32[] { 0 });
                // _hooks["connect"].ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                _hooks.Add("recv", LocalHook.Create(
                   LocalHook.GetProcAddress("Ws2_32.dll", "recv"),
                   new ptrRecv(ptrRecv_Hooked),
                   this));
                _hooks.Add("send", LocalHook.Create(
                    LocalHook.GetProcAddress("Ws2_32.dll", "send"),
                    new ptrSend(ptrSend_Hooked),
                    this));

                foreach (var v in _hooks)
                {
                    v.Value.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                }
            }
            catch (Exception ExtInfo)
            {
                Interface.ReportException(ExtInfo);

                return;
            }
            RemoteHooking.WakeUpProcess();
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    // transmit newly monitored file accesses...
                    if (PrintQueue.Count > 0)
                    {
                        String[] Output = null;

                        lock (PrintQueue)
                        {
                            Output = PrintQueue.ToArray();
                            PrintQueue.Clear();
                        }

                        Interface.PrintMessage(Output);
                    }

                    if (SendPacketQueue.Count > 0)
                    {
                        Packet[] sendOut = null;

                        lock (SendPacketQueue)
                        {
                            sendOut = RecvPacketQueue.ToArray();
                            SendPacketQueue.Clear();
                        }

                        Interface.ProcessSend(sendOut);
                    }
                }
            }
            catch
            {
            }
            // wait for host process termination...

        }
        //public static extern int send(SOCKET s, byte* buf, int len, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate int ptrSend(acwl.win32.ws2_32.SOCKET s, ref byte buf, int len, int flag);
        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]

        //public static extern int recv(SOCKET s, ref byte buf, int len, int flags);
        delegate int ptrRecv(acwl.win32.ws2_32.SOCKET s, ref byte buf, int len, int flag);
        //public static extern int connect(SOCKET s, sockaddr_in* addr, int addrsize);
        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate int ptrConnect(acwl.win32.ws2_32.SOCKET s, ref acwl.win32.ws2_32.sockaddr_in addr, int addrsize);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate int ptrWSAStartup(ushort Version, out acwl.win32.ws2_32.WSAData Data);
        static int ptrWSAStartup_Hooked(ushort Version, out acwl.win32.ws2_32.WSAData Data)
        {
            WriteMessage("WSAStartup");
            return acwl.win32.ws2_32.WSAStartup(Version, out Data);
        }
        static int ptrConnect_Hooked(acwl.win32.ws2_32.SOCKET s, ref acwl.win32.ws2_32.sockaddr_in addr, int addrsize)
        {
            //addr.
            if (Port > 0)
            {
                addr.sin_port = acwl.win32.ws2_32.htons(Port);
                WriteMessage("connect - New Address: {0}, Port: {1}", addr.sin_addr.ToString(), addr.sin_port);
            }


            return acwl.win32.ws2_32.connect(s, ref addr, addrsize);

        }

        public static int ptrSend_Hooked(acwl.win32.ws2_32.SOCKET s, ref byte buf, int len, int flags)
        {
            WriteMessage("send - Len: {0}, Flags: {1}", len, flags);
            //byte[] v = new byte[len];
            //Marshal.Copy(buf, v, 0,len);

            //ProcessPacket(v);
            return acwl.win32.ws2_32.send(s, ref buf, len, flags);
        }

        public static int ptrRecv_Hooked(acwl.win32.ws2_32.SOCKET s, ref byte buf, int len, int flags)
        {
            WriteMessage("recv - Len: {0}, Flags: {1}", len, flags);

            return acwl.win32.ws2_32.recv(s,ref buf, len, flags);
        }

        static void WriteMessage(string message, params object[] param)
        {
            lock (PrintQueue)
            {
                PrintQueue.Enqueue(String.Format(message, param));
            }
        }
        public static void ProcessPacket(byte[] buf)
        {
            byte[] b = new byte[buf.Length];

            Array.Copy(buf, b, buf.Length);

            Packet p = new Packet()
            {
                Captured = DateTime.Now,
                Data = b
            };
            lock (SendPacketQueue)
            {
                SendPacketQueue.Enqueue(p);
            }

        }
    }
}
