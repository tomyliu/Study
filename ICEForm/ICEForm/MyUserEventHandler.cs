using CfApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEForm
{
    public class MyUserEventHandler : UserEventHandler
    {
        protected log4net.ILog Entrylog = log4net.LogManager.GetLogger("ICELog");

        private ProgData _ProgramData;
        public ProgData ProgramData
        {
            set { _ProgramData = value; }
            get { return _ProgramData; }
        }

        // Delegate Evemt Definition  
        public delegate void FEIBUserEventMessageHandler(string Msg);
        public event FEIBUserEventMessageHandler FEIBSetUserMessage;
        public void SetUserMessage(string _Msg)
        {
            if (FEIBSetUserMessage != null)
            {
                FEIBSetUserMessage(_Msg);
            }
            else
            {
                Entrylog.Error("Not Registered Event");
            }
        }

        public override void OnUserEvent(UserEvent userEvent)
        {
            try
            {
                string msg = null;
                var type = userEvent.GetType();
                if (type == UserEventTypes.AUTHORIZATION_FAILURE)
                {
                    msg = string.Format("login failed: return code = {0}: {1}", userEvent.GetRetCode(),
                        userEvent.GetRetCodeString());

                    FEIBSetUserMessage(msg);

                }

                if (_ProgramData.mDebug)
                    PrintAuthStatus(userEvent.GetSession().GetUserInfo());
            }
            catch (Exception ex)
            {
                Entrylog.Error(ex);
            }
        }
        void PrintAuthStatus(UserInfo user)
        {
            string msg = null;
            var state = user.GetState();
            switch (state)
            {
                case UserStates.NOT_AUTHENTICATED:
                    msg = ("Authorization status = NOT_AUTHENTICATED");
                    FEIBSetUserMessage(msg);
                    break;
                case UserStates.AUTHENTICATED:
                    msg = ("Authorization status = AUTHENTICATED");
                    FEIBSetUserMessage(msg);
                    break;
                case UserStates.PARTIALLY_AUTHENTICATED:
                    msg = ("Authorization status = PARTIALLY_AUTHENTICATED");
                    FEIBSetUserMessage(msg);
                    break;
            }
        }
    }
}
