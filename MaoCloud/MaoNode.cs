using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaoCloud
{
    // Attention !!!
    // TODO - update this class for adapt to NodeBroadcastInfo's Builder Pattern - 2016.09.02 19:46
    class MaoNode
   {
        public MaoNode(string name, string ip)
        {
            this.name = name;
            this.ip = ip;
            this.seenCount = 1;
        }
        public void updateIP(string ip)
        {
            this.ip = name;
            // deal with Monitor link.
        }
        public void seeAgain()
        {
            seenCount++;
        }

        private string name;
        public string Name { get { return name; } }
        private string ip;//True IP, not from broadcast data
        public string IP { get { return ip; } }
        private int seenCount;// TODO - upgrade to lastSeen
        public int SeenCount { get { return seenCount; } }
    }
}
