using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acwl.hook
{
    public class Packet
    {
        public DateTime Captured { get; set; }
        public byte[] Data { get; set; }
    }
}
