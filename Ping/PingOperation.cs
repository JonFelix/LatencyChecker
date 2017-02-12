using System;

namespace Ping
{
    public class PingOperation
    {
        public string Ip;
        public readonly long[] ResponseTime = new long[1000];
        public TimeSpan Interval;
        public readonly string[] ResponseMessage = new string[1000];
        public readonly DateTime[] ResponseTimestamp = new DateTime[1000];
        public DateTime LastOperationTime;
        public int Cursor = -1;
        public string HostName = "";    
        public string OriginalHost = "";

        public PingOperation(string hostname, string ip, TimeSpan interval)
        {
            Ip = ip;
            Interval = interval;
            HostName = hostname;
            OriginalHost = Ip;
        }

        public void PushResponses()
        {
            for(var i = 1; i < ResponseMessage.Length; i++)
            {
                ResponseMessage[i - 1] = ResponseMessage[i];
                ResponseTime[i - 1] = ResponseTime[i];
                ResponseTimestamp[i - 1] = ResponseTimestamp[i];
            }
        }
    }
}
