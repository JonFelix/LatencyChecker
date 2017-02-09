using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ping
{
    public class PingManager
    {
        Engine _host;


        public PingManager(Engine host)
        {
            _host = host;
        }



        public PingReply SendPing(string ip)
        {
            System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(ip, timeout, buffer, options);
            _host.Log("Pinging " + reply.Address + ": " + reply.Status + "("+ reply.RoundtripTime + "ms)");
            return reply;
        }

    }
}
