using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace MyPingLibrary
{
    public class PingSender
    {
        //variable to store ping reply
        private PingReply reply;

        //instance of Ping for sending requests
        private Ping pingSender;

        //instance of PingStatistics for working with statistics
        private PingStatistics Statistics;

        //buffer for ping request
        private byte[] _buffer;
        private byte[] Buffer
        {
            get
            {
                return _buffer;
            }
            set
            {
                _buffer = value ?? new byte[32];
            }
        }

        //address for ping request
        private string _address = "";
        private string Address
        {
            get
            {
                return _address;
            }
            set
            {
                value = value.Replace("http://", "");
                value = value.Replace("https://", "");
                value = value.Replace("ftp://", "");
                int slashIndex = value.IndexOf('/');
                if (slashIndex >= 0)
                {
                    value = value.Remove(slashIndex);
                }
                if (_address != value)
                {
                    Statistics.ClearStatistics();
                    Statistics.Address = value;
                }
                _address = value;
            }
        }

        //options for ping request
        private PingOptions Options { get; set; }

        public PingSender()
        {
            Statistics = new PingStatistics();
            pingSender = new Ping();
            Options = new PingOptions();
            reply = null;
        }

        public string GetPingStatistics()
        {
            string result = Statistics.GetStatistics();
            return result;
        }

        public async Task<string> GetPingStatisticsAsync()
        {
            string result = await Statistics.GetStatisticsAsync();
            return result;
        }

        public string SendRequest(string address, int timeout = 1000,
            byte[] buffer = null, int Ttl = 128)
        {
            if (String.IsNullOrWhiteSpace(address))
            {
                address = "google.com";
            }
            Address = address;
            Options.Ttl = Ttl;
            Buffer = buffer;
            try
            {
                Statistics.RegisterPingAttempt();
                reply = pingSender.Send(Address, timeout, Buffer, Options);
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
                return $"Во время операции \"ping {Address}\" произошла ошибка:" +
                    $" \n{e.Message}\n";
            }
            if (reply.Status == IPStatus.Success)
            {
                PingResult pingResult = new PingResult(Address, reply);
                Statistics.AddPingResult(pingResult);
                Thread.Sleep(1000 - (int)reply.RoundtripTime);
                return $"{pingResult}";
            }
            else if (reply.Status == IPStatus.TimedOut)
            {
                Thread.Sleep(1000);
                return $"Время передачи превысило {timeout} мс.\n\n";
            }
            Thread.Sleep(1000);
            return $"Во время операции \"ping {Address}\" произошла ошибка.\n\n";
        }

        public async Task<string> SendRequestAsync(string address, int timeout = 1000,
            byte[] buffer = null, int Ttl = 128)
        {
            return await Task.Run(() =>
            {
                string result = SendRequest(address, timeout,
            buffer, Ttl);
                return result;
            });
        }

        public void ClearStatistics()
        {
            Statistics.ClearStatistics();
        }
    }
}
