using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPingLibrary
{
    public class PingStatistics
    {
        //variable to store the total number of outgoing ping requests
        private int PingRequestsTotally;

        private int ReceivedPackages;

        private long TotalTime;

        private long MinimalTime;

        private long MaximalTime;

        //variable to store address of ping requests
        public string Address { get; set; }

        public PingStatistics()
        {
            ClearStatistics();
        }

        public void AddPingResult(PingResult pingResult)
        {
            ReceivedPackages++;
            TotalTime += pingResult.RoundtripTime;
            if (pingResult.RoundtripTime < MinimalTime)
            {
                MinimalTime = pingResult.RoundtripTime;
            }
            if (pingResult.RoundtripTime > MaximalTime)
            {
                MaximalTime = pingResult.RoundtripTime;
            }
        }

        public void RegisterPingAttempt()
        {
            PingRequestsTotally++;
        }

        public string GetStatistics()
        {
            int lostPackages = PingRequestsTotally - ReceivedPackages;
            double lostPackagesPercentage = ((double)lostPackages / PingRequestsTotally) * 100;
            lostPackagesPercentage = Math.Round(lostPackagesPercentage, 2);
            string result = $"Статистика Ping для {Address}:\n" +
                $"\tПакетов: отправлено - {PingRequestsTotally}, " +
                $"получено - {ReceivedPackages}, " +
                $"потеряно - {lostPackages}\n" +
                $"\t({lostPackagesPercentage.ToString(CultureInfo.GetCultureInfo("en-US"))}% потерь)\n\n";
            if (ReceivedPackages > 0)
            {
                double AvarageTime = (double)TotalTime / ReceivedPackages;
                AvarageTime = Math.Round(AvarageTime, 2);
                result += $"Время приема-передачи:\n" +
                    $"\tмаксимальное - {MaximalTime} мс, " +
                    $"минимальное - {MinimalTime} мс, " +
                    $"среднее - {AvarageTime.ToString(CultureInfo.GetCultureInfo("en-US"))} мс\n\n";
            }
            return result;
        }

        public async Task<string> GetStatisticsAsync()
        {
            return await Task.Run(() =>
            {
                string result = GetStatistics();
                return result;
            });
        }

        public void ClearStatistics()
        {
            PingRequestsTotally = 0;
            ReceivedPackages = 0;
            TotalTime = 0;
            MinimalTime = long.MaxValue;
            MaximalTime = -1;
        }
    }
}
