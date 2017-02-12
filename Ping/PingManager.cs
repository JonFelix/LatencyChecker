using System.Net.NetworkInformation;
using System.Text;

namespace Ping
{
    public class PingManager
    {
        private readonly Engine _host;


        public PingManager(Engine host)
        {
            _host = host;
        }



        public PingReply SendPing(string ip)
        {
            var pingSender = new System.Net.NetworkInformation.Ping();
            var options = new PingOptions {DontFragment = true};

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.

            // Create a buffer of 32 bytes of data to be transmitted.
            const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            var buffer = Encoding.ASCII.GetBytes(data);
            const int timeout = 120;
            var reply = pingSender.Send(ip, timeout, buffer, options);
            _host.Log("Pinging " + reply.Address + ": " + reply.Status + "("+ reply.RoundtripTime + "ms)");
             
            return reply;
        }

    }
}
