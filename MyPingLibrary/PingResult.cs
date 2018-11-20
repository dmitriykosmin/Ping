using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace MyPingLibrary
{
    public class PingResult
    {
        public string Address { get; private set; }
        public string IpAddress { get; private set; }
        public long RoundtripTime { get; private set; }
        public int TTL { get; private set; }
        public int BufferLength { get; private set; }

        public PingResult(string address, PingReply pingReply)
        {
            Address = address;
            IpAddress = pingReply.Address.ToString();
            RoundtripTime = pingReply.RoundtripTime;
            TTL = pingReply.Options.Ttl;
            BufferLength = pingReply.Buffer.Length;
        }

        public override string ToString()
        {
            string result = $" Ответ от {Address} [{IpAddress}]:\n";
            result += $" Время приема-передачи: {RoundtripTime} мс;\n";
            result += $" TTL: {TTL};";
            result += $" Число байт: {BufferLength}.\n\n";
            return result;
        }
    }
}
