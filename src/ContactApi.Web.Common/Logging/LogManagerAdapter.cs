using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ContactApi.Web.Common.Logging
{
    public class LogManagerAdapter : ILogManager
    {
        public ILog GetLog(Type typeOfRequestedLog)
        {
            var log = LogManager.GetLogger(typeOfRequestedLog);
            return log;
        }
    }
}
