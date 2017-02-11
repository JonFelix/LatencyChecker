using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ping
{
    public class PingOperation
    {
        public string IP;
        public long[] ResponseTime = new long[1000];
        public TimeSpan Interval;
        public string[] ResponseMessage = new string[1000];
        public DateTime[] ResponseTimestamp = new DateTime[1000];
        public DateTime LastOperationTime;
        public int Cursor = -1;

        public PingOperation(string ip, TimeSpan interval)
        {
            IP = ip;
            Interval = interval;
        }

        public void PushResponses()
        {
            for(int i = 1; i < ResponseMessage.Length; i++)
            {
                ResponseMessage[i - 1] = ResponseMessage[i];
                ResponseTime[i - 1] = ResponseTime[i];
                ResponseTimestamp[i - 1] = ResponseTimestamp[i];
            }
        }
    }
}
