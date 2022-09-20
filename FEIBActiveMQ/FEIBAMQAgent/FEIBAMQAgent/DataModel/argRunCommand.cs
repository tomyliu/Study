using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEIBAMQAgent.DataModel
{
    public class argRunCommand
    {
        #region Request
        public string Cmd { get; set; }
        public string Args { get; set; }
        public string RunPath { get; set; }
        #endregion
        #region Response
        public string StarndardOutput { get; set; }
        public string StarndardError { get; set; }
        public string ReturnCode { get; set; }
        #endregion
    }
}
