using CfApiNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public class MyMessageEventHandler : MessageEventHandler
    {
        private ConcurrentDictionary<long, string> _RequestCommands;
        public ConcurrentDictionary<long, string> RequestCommands
        {
            set { _RequestCommands = value; }
            get { return _RequestCommands; }
        }
        private ProgData _ProgramData;
        public ProgData ProgramData
        {
            set { _ProgramData = value; }
            get { return _ProgramData; }
        }

        protected log4net.ILog Entrylog = log4net.LogManager.GetLogger("ICELog");

        private const string ETX = "<ETX>\r";


        #region Delegate Evemt Definition  
        public delegate void FEIBMessageEventHandler(string msg);
        public event FEIBMessageEventHandler FEIBGetMessage;

        public void GetMessage(string _msg)
        {
            if (FEIBGetMessage != null)
            {
                FEIBGetMessage(_msg);
            }
            else
            {
                Entrylog.Error("Not Registered Event");
            }
        }

        public delegate void FEIBMarketDataEventHandler(List<MarketDataItem> msg);
        public event FEIBMarketDataEventHandler FEIBGetMarketData;

        public void GetMarketData(List<MarketDataItem> _msg)
        {
            if (FEIBGetMarketData != null)
            {
                FEIBGetMarketData(_msg);
            }
            else
            {
                Entrylog.Error("Not Registered Event");
            }
        }
        #endregion

        public override void OnMessageEvent(MessageEvent messageEvent)
        {
            List<MarketDataItem> MarketDataList = new List<MarketDataItem>();

            MarketDataItem marketdata;

            try
            {
                var type = messageEvent.GetType();

                if ((type == MessageEvent.Types.STATUS) || (type == MessageEvent.Types.IMAGE_COMPLETE))
                {
                    var messageEventTag = messageEvent.GetTag();

                    string command;
                    _RequestCommands.TryRemove(messageEventTag, out command);

                    Console.WriteLine("Status code = {0} ({1}) for tag {2} ({3})", messageEvent.GetStatusCode(),
                        messageEvent.GetStatusString(), messageEventTag, command);
                }
                else if (ProgramData.mConflationIndicator)
                {
                    string tmpStr;
                    var isConflatable = messageEvent.IsConflatable();
                    switch (isConflatable)
                    {
                        case 0:
                            tmpStr = "not ";
                            break;
                        case 1:
                            tmpStr = "";
                            break;
                        default:
                            tmpStr = "not known if ";
                            break;
                    }
                    Console.WriteLine("This message is {0}conflatable", tmpStr);
                }

                StringBuilder sb = new StringBuilder();

                //Only add if each is present
                int perm = messageEvent.GetPermission();
                if (perm != 0)
                {
                    sb.AppendLine("PERMISSION(3)=" + perm);
                    marketdata = new MarketDataItem();
                    marketdata.ItemName = "PERMISSION";
                    marketdata.ItemType = 0;
                    marketdata.Code = "3";
                    marketdata.Value = perm.ToString();
                    MarketDataList.Add(marketdata);
                     
                }

                int src = messageEvent.GetSource();
                if (src != 0)
                {
                    sb.AppendLine("ENUM.SRC.ID(4)=" + src);
                    marketdata = new MarketDataItem();
                    marketdata.ItemName = "ENUM.SRC.ID";
                    marketdata.ItemType = 0;
                    marketdata.Code = "4";
                    marketdata.Value = src.ToString();
                    MarketDataList.Add(marketdata);
                }

                var symbol = messageEvent.GetSymbol();
                if (!string.IsNullOrEmpty(symbol))
                {
                    sb.AppendLine("SYMBOL.TICKER(5)=" + symbol);
                    marketdata = new MarketDataItem();
                    marketdata.ItemName = "SYMBOL.TICKER";
                    marketdata.ItemType = 0;
                    marketdata.Code = "5";
                    marketdata.Value = symbol.ToString();
                    MarketDataList.Add(marketdata);
                }

                for (ulong i = 0; i < messageEvent.GetNumberofAlternateIndexes(); i++)
                {
                    sb.AppendFormat("{0}({1})={2}\n", messageEvent.GetAlternateIndexTokenName(i),
                                messageEvent.GetAlternateIndexTokenNumber(i),
                                messageEvent.GetAlternateIndexValue(i));
                    marketdata = new MarketDataItem();
                    marketdata.ItemName = messageEvent.GetAlternateIndexTokenName(i);
                    marketdata.Code = messageEvent.GetAlternateIndexTokenNumber(i).ToString();
                    marketdata.Value = messageEvent.GetAlternateIndexValue(i);
                    MarketDataList.Add(marketdata);
                }

                if (type == MessageEvent.Types.REFRESH)
                {
                    sb.AppendLine("REFRESH(24)=1");
                    marketdata = new MarketDataItem();
                    marketdata.ItemName = "REFRESH";
                    marketdata.ItemType = 0;
                    marketdata.Code = "24";
                    marketdata.Value = "1";
                    MarketDataList.Add(marketdata);
                }

                var reader = messageEvent.GetReader();

                int tokenNumber;
                string tokenName;

                while (reader.Next() != MessageReader.END_OF_MESSAGE)
                {
                    tokenNumber = reader.GetTokenNumber();
                    tokenName = reader.GetTokenName();
                    switch (reader.GetValueType())
                    {
                        case ValueTypes.INT64:
                            var integer = reader.GetValueAsInteger();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, integer);
                            marketdata = new MarketDataItem();
                            marketdata.ItemName = tokenName;
                            marketdata.ItemType = 1;    // 資料
                            marketdata.Code = tokenNumber.ToString() ;
                            marketdata.Value = integer.ToString();
                            MarketDataList.Add(marketdata);

                            break;
                        case ValueTypes.STRING:
                            var str = reader.GetValueAsString();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, str);
                            marketdata = new MarketDataItem();
                            marketdata.ItemType = 1;    // 資料
                            marketdata.ItemName = tokenName;
                            marketdata.Code = tokenNumber.ToString();
                            marketdata.Value = str;
                            MarketDataList.Add(marketdata);
                            break;
                        case ValueTypes.DOUBLE:
                            var doubleValue = reader.GetValueAsDouble();
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, doubleValue);
                            marketdata = new MarketDataItem();
                            marketdata.ItemName = tokenName;
                            marketdata.ItemType = 1;    // 資料
                            marketdata.Code = tokenNumber.ToString();
                            marketdata.Value = doubleValue.ToString();
                            MarketDataList.Add(marketdata);
                            break;
                        case ValueTypes.DATETIME:
                            var dateTime = reader.GetValueAsDateTime();
                            var date = dateTime.GetDate();
                            var time = dateTime.GetTime();
                            var dateTimeFormat = string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6} UTC",
                                date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second,
                                (time.Millisecond * 1000) + time.Microsecond);
                            doubleValue = reader.GetValueAsDouble();
                            sb.AppendFormat("{0}({1})={2}({3})\n", tokenName, tokenNumber, dateTimeFormat, doubleValue);
                            marketdata = new MarketDataItem();
                            marketdata.ItemType = 1;    // 資料
                            marketdata.ItemName = tokenName;
                            marketdata.Code = tokenNumber.ToString();
                            marketdata.Value = String.Format("{0}({1})", dateTimeFormat, doubleValue);
                            MarketDataList.Add(marketdata);
                            break;
                        case ValueTypes.UNKNOWN:
                        default:
                            Console.WriteLine("OnMessageEvent, Unknown value type");
                            sb.AppendFormat("{0}({1})={2}\n", tokenName, tokenNumber, "UNKNOWN value type, cannot decode");
                            break;
                    }
                }
                sb.AppendLine(ETX);

                if (!ProgramData.mQuiet)
                    Entrylog.Info(sb.ToString());

                GetMessage(sb.ToString());
                GetMarketData(MarketDataList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnMessageEvent exception: " + ex);
                Entrylog.Error(ex + Environment.NewLine);
            }
        }
    }
}
