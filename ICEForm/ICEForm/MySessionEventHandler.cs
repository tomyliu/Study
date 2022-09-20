using CfApiNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public class MySessionEventHandler : SessionEventHandler
    {
        protected log4net.ILog Entrylog = log4net.LogManager.GetLogger("ICELog");
        public const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
         
        private bool _SessionStatus;
        /// <summary>
        /// Session連線狀態
        /// </summary>
        public bool SessionStatus
        {
            get
            {
                return _SessionStatus;
            }
        }

        private SessionEvent _sessionEvent;

        /// <summary>
        /// 取得目前Event
        /// </summary>
        public SessionEvent CurrentsessionEvent
        {
            get
            {
                return _sessionEvent;
            }
        }

        // Delegate Evemt Definition  
        public delegate void FEIBSessionEventHandler(bool status, SessionEvent sevent);
        public event FEIBSessionEventHandler FEIBSetSession;

        public void SetSessionStatus(bool _status, SessionEvent _sevent)
        {
            if (FEIBSetSession != null)
            {
                _SessionStatus = _status;
                _sessionEvent = _sevent;
                FEIBSetSession(_status, _sevent);
            }
            else
            {
                Entrylog.Error("Not Registered Event");
            }
        }

        public delegate void FEIBSessionEventMessageHandler(string Msg);
        public event FEIBSessionEventMessageHandler FEIBSetSessionMessage;
        public void SetSessionMessage(string _Msg)
        {
            if (FEIBSetSessionMessage != null)
            {
                FEIBSetSessionMessage(_Msg );
            }
            else
            {
                Entrylog.Error("Not Registered Event");
            }
        }

        public MySessionEventHandler()
        {
            _SessionStatus = false;
        }

        public override void OnSessionEvent(SessionEvent sessionEvent)
        {
            string Msg = null;
            try
            {
                var type = sessionEvent.GetType();
                SetSessionStatus(true, sessionEvent);
                if (type == SessionEventTypes.CFAPI_SESSION_ESTABLISHED)
                {
                    Msg = "Session established " + System.DateTime.Now.ToString(TimeFormat);
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                  
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_UNAVAILABLE)
                {
                    Msg = "session unavailable";
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);

                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECOVERY)
                {
                    Msg = "SESSION_RECOVERY";
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                }
                else if (type == SessionEventTypes.CFAPI_CDD_LOADED)
                {
                    Msg = string.Format("CFAPI_CDD_LOADED; version={0}", sessionEvent.GetCddVersion());
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECOVERY_SOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Msg = string.Format("CFAPI_SESSION_RECOVERY_SOURCES  sourceID = {0}", sourceID);
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);

                }
                else if (type == SessionEventTypes.CFAPI_SESSION_AVAILABLE_ALLSOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Msg = string.Format("CFAPI_SESSION_AVAILABLE_ALLSOURCES  sourceID ={0}", sourceID);
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                }
                else if (type == SessionEventTypes.CFAPI_SESSION_AVAILABLE_SOURCES)
                {
                    int sourceID = sessionEvent.GetSourceID();
                    Msg = string.Format("CFAPI_SESSION_AVAILABLE_SOURCES  sourceID ={0}", sourceID);
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);

                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECEIVE_QUEUE_ABOVE_THRESHOLD)
                {
                    Msg = string.Format("SESSION_RECEIVE_QUEUE_ABOVE_THRESHOLD ({0})", sessionEvent.GetQueueDepth());
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);

                }
                else if (type == SessionEventTypes.CFAPI_SESSION_RECEIVE_QUEUE_BELOW_THRESHOLD)
                {
                    Msg = string.Format("SESSION_RECEIVE_QUEUE_BELOW_THRESHOLD ({0})", sessionEvent.GetQueueDepth());
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                }
                else
                {
                    Entrylog.Info("Unknown session state");
                    Msg = "Unknown session state";
                    Entrylog.Info(Msg);
                    SetSessionMessage(Msg);
                }
            }
            catch (Exception ex)
            {
                Entrylog.Error(ex);
                SetSessionMessage("Exception=" + ex.Message.ToString());
            }

        }
 
      
     
    }
}
