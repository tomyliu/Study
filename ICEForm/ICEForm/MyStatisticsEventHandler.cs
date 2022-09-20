using CfApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public class MyStatisticsEventHandler : StatisticsEventHandler
    {
        protected log4net.ILog Entrylog = log4net.LogManager.GetLogger("ICELog");

        public override void OnStatisticsEvent(StatisticsEvent statisticsEvent)
        {
            try
            {
                Entrylog.Info(string.Format(
                    "MSGS_IN --> {0}; MSGS_OUT --> {1}; NET_MSGS_OUT --> {2}; DROP --> {3}; CSP_DROP --> {4}; PCT_FULL={5}/{6}; IN_MSGS/SEC(100ms)={7}/{8} IN_MSGS/SEC(1sec)={9}/{10}; OUT_MSGS/SEC(100ms)={11}/{12} OUT_MSGS/SEC(1sec)={13}/{14}; NET_OUT_MSGS/SEC(100ms)={15}/{16} NET_OUT_MSGS/SEC(1sec)={17}/{18}\n",
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_IN),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.MSGS_OUT),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_MSGS_OUT),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.DROP),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.CSP_DROP),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PCT_FULL),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_PCT_FULL),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.IN_MSGS_SEC),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_IN_MSGS_SEC),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.OUT_MSGS_SEC),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_OUT_MSGS_SEC),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC_100MS),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.NET_OUT_MSGS_SEC),
                    statisticsEvent.GetStat(StatisticsEvent.StatsTypes.PEAK_NET_OUT_MSGS_SEC)));

            }
            catch (Exception ex)
            {
                Entrylog.Error(ex);
            }
        }
    }
}
