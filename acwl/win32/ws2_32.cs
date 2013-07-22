using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace acwl.win32
{
    public class ws2_32
    {

        // Taken from http://www.elitepvpers.com/forum/co2-programming/159327-advanced-winsock-c.html

        //TODO: Cleanup code
        public const int SOCKET_ERROR = -1;
        public const int INVALID_SOCKET = ~0;


        public enum SocketType : int
        {
            Unknown = 0,
            Stream = 1,
            DGram = 2,
            Raw = 3,
            Rdm = 4,
            SeqPacket = 5
        }
        public enum ProtocolType : int
        {
            BlueTooth = 3,
            Tcp = 6,
            Udp = 17,
            ReliableMulticast = 113
        }
        public struct SOCKET
        {
            private IntPtr handle;
            private SOCKET(IntPtr _handle)
            {
                handle = _handle;
            }
            private SOCKET(int _handle)
            {
                handle = (IntPtr)_handle;
            }
            public static bool operator ==(SOCKET s, int i)
            {
                return ((int)s.handle == i);
            }
            public static bool operator !=(SOCKET s, int i)
            {
                return ((int)s.handle != i);
            }
            public static implicit operator SOCKET(int i)
            {
                return new SOCKET(i);
            }
            public static implicit operator uint(SOCKET s)
            {
                return (uint)s.handle;
            }
            public override bool Equals(object obj)
            {
                return (obj is SOCKET) ? (((SOCKET)obj).handle == this.handle) : base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return (int)handle;
            }
        }
        //  fd_set used in 'select' method
        public struct fd_set
        {
            public const int FD_SETSIZE = 64;
            public uint fd_count;
            public IntPtr fd_array;// public fixed uint fd_array[FD_SETSIZE];
        }

        // C# equivilent to C++'s sockaddr_in / SOCKADDR_IN
        [StructLayout(LayoutKind.Explicit, Size = 4)]
        public struct in_addr
        {
            [FieldOffset(0)]
            public byte s_b1;
            [FieldOffset(1)]
            public byte s_b2;
            [FieldOffset(2)]
            public byte s_b3;
            [FieldOffset(3)]
            public byte s_b4;

            [FieldOffset(0)]
            public ushort s_w1;
            [FieldOffset(2)]
            public ushort s_w2;


            /// <summary>
            /// can be used for most tcp & ip code
            /// </summary>
            public uint s_addr { get { return s_b1; } }

            /// <summary>
            /// host on imp
            /// </summary>
            public byte s_host { get { return s_b2; } }

            /// <summary>
            /// network
            /// </summary>
            public byte s_net { get { return s_b1; } }

            /// <summary>
            /// imp
            /// </summary>
            public ushort s_imp { get { return s_w2; } }

            /// <summary>
            /// imp #
            /// </summary>
            public byte s_impno { get { return s_b4; } }

            /// <summary>
            /// logical host
            /// </summary>
            public byte s_lh { get { return s_b3; } }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct sockaddr_in
        {
            public AddressFamily sin_family;
            public ushort sin_port;
            public in_addr sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sin_zero;
        }



        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct WSAData
        {
            public ushort Version;
            public ushort HighVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
            public string Description;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string SystemStatus;
            public ushort MaxSockets;
            public ushort MaxUdpDg;
            IntPtr lpVendorInfo;//sbyte* lpVendorInfo;

        }
        //public enum AddressFamily : int
        //{
        //    Unknown = 0,
        //    InterNetworkv4 = 2,
        //    Ipx = 4,
        //    AppleTalk = 17,
        //    NetBios = 17,
        //    InterNetworkv6 = 23,
        //    Irda = 26,
        //    BlueTooth = 32
        //}
        public enum AddressFamily : short
        {
            /// <summary>
            /// Unspecified [value = 0].
            /// </summary>
            AF_UNSPEC = 0,
            /// <summary>
            /// Local to host (pipes, portals) [value = 1].
            /// </summary>
            AF_UNIX = 1,
            /// <summary>
            /// Internetwork: UDP, TCP, etc [value = 2].
            /// </summary>
            AF_INET = 2,
            /// <summary>
            /// Arpanet imp addresses [value = 3].
            /// </summary>
            AF_IMPLINK = 3,
            /// <summary>
            /// Pup protocols: e.g. BSP [value = 4].
            /// </summary>
            AF_PUP = 4,
            /// <summary>
            /// Mit CHAOS protocols [value = 5].
            /// </summary>
            AF_CHAOS = 5,
            /// <summary>
            /// XEROX NS protocols [value = 6].
            /// </summary>
            AF_NS = 6,
            /// <summary>
            /// IPX protocols: IPX, SPX, etc [value = 6].
            /// </summary>
            AF_IPX = 6,
            /// <summary>
            /// ISO protocols [value = 7].
            /// </summary>
            AF_ISO = 7,
            /// <summary>
            /// OSI is ISO [value = 7].
            /// </summary>
            AF_OSI = 7,
            /// <summary>
            /// european computer manufacturers [value = 8].
            /// </summary>
            AF_ECMA = 8,
            /// <summary>
            /// datakit protocols [value = 9].
            /// </summary>
            AF_DATAKIT = 9,
            /// <summary>
            /// CCITT protocols, X.25 etc [value = 10].
            /// </summary>
            AF_CCITT = 10,
            /// <summary>
            /// IBM SNA [value = 11].
            /// </summary>
            AF_SNA = 11,
            /// <summary>
            /// DECnet [value = 12].
            /// </summary>
            AF_DECnet = 12,
            /// <summary>
            /// Direct data link interface [value = 13].
            /// </summary>
            AF_DLI = 13,
            /// <summary>
            /// LAT [value = 14].
            /// </summary>
            AF_LAT = 14,
            /// <summary>
            /// NSC Hyperchannel [value = 15].
            /// </summary>
            AF_HYLINK = 15,
            /// <summary>
            /// AppleTalk [value = 16].
            /// </summary>
            AF_APPLETALK = 16,
            /// <summary>
            /// NetBios-style addresses [value = 17].
            /// </summary>
            AF_NETBIOS = 17,
            /// <summary>
            /// VoiceView [value = 18].
            /// </summary>
            AF_VOICEVIEW = 18,
            /// <summary>
            /// Protocols from Firefox [value = 19].
            /// </summary>
            AF_FIREFOX = 19,
            /// <summary>
            /// Somebody is using this! [value = 20].
            /// </summary>
            AF_UNKNOWN1 = 20,
            /// <summary>
            /// Banyan [value = 21].
            /// </summary>
            AF_BAN = 21,
            /// <summary>
            /// Native ATM Services [value = 22].
            /// </summary>
            AF_ATM = 22,
            /// <summary>
            /// Internetwork Version 6 [value = 23].
            /// </summary>
            AF_INET6 = 23,
            /// <summary>
            /// Microsoft Wolfpack [value = 24].
            /// </summary>
            AF_CLUSTER = 24,
            /// <summary>
            /// IEEE 1284.4 WG AF [value = 25].
            /// </summary>
            AF_12844 = 25,
            /// <summary>
            /// IrDA [value = 26].
            /// </summary>
            AF_IRDA = 26,
            /// <summary>
            /// Network Designers OSI &amp; gateway enabled protocols [value = 28].
            /// </summary>
            AF_NETDES = 28,
            /// <summary>
            /// [value = 29].
            /// </summary>
            AF_TCNPROCESS = 29,
            /// <summary>
            /// [value = 30].
            /// </summary>
            AF_TCNMESSAGE = 30,
            /// <summary>
            /// [value = 31].
            /// </summary>
            AF_ICLFXBM = 31,
                BlueTooth = 32
        }

        [DllImport("Ws2_32.dll")]
        public static extern int WSAStartup(ushort Version, out WSAData Data);
        [DllImport("Ws2_32.dll")]
        public static extern SocketError WSAGetLastError();
        [DllImport("Ws2_32.dll")]
        public static extern SOCKET socket(AddressFamily af, SocketType type, ProtocolType protocol);
        [DllImport("Ws2_32.dll")]
        public static extern int send(SOCKET s, ref byte buf, int len, int flags);
        [DllImport("Ws2_32.dll")]
        public static extern int recv(SOCKET s, ref byte buf, int len, int flags);
        [DllImport("Ws2_32.dll")]
        public static extern SOCKET accept(SOCKET s, sockaddr_in addr, int addrsize);
        [DllImport("Ws2_32.dll")]
        public static extern int listen(SOCKET s, int backlog);
        [DllImport("Ws2_32.dll", CharSet = CharSet.Ansi)]
        public static extern uint inet_addr(string cp);
        [DllImport("Ws2_32.dll")]
        public static extern ushort htons(ushort hostshort);
        [DllImport("Ws2_32.dll")]
        public static extern ushort ntohs(ushort hostshort);
        [DllImport("Ws2_32.dll")]
        public static extern int connect(SOCKET s, ref sockaddr_in addr, int addrsize);
        [DllImport("Ws2_32.dll")]
        public static extern int closesocket(SOCKET s);
        [DllImport("Ws2_32.dll")]
        public static extern int getpeername(SOCKET s, ref sockaddr_in addr, ref int addrsize);
        [DllImport("Ws2_32.dll")]
        public static extern int bind(SOCKET s, ref sockaddr_in addr, int addrsize);
        //[DllImport("Ws2_32.dll")]
        //public static extern int select(int ndfs, fd_set* readfds, fd_set* writefds, fd_set* exceptfds, timeval* timeout); //TODO: figure out what timeval is
        [DllImport("Ws2_32.dll")]
        public static extern IntPtr inet_ntoa(in_addr _in);
    }
}
