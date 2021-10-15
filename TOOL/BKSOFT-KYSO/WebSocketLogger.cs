using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSockets.Common;

namespace BKSOFT_KYSO
{
    internal class WebSocketLogger : IWebSocketLogger
    {
        public void Information(Type type, string format, params object[] args)
        {
            Trace.TraceInformation(format, args);
        }

        public void Warning(Type type, string format, params object[] args)
        {
            Trace.TraceWarning(format, args);
        }

        public void Error(Type type, string format, params object[] args)
        {
            Trace.TraceError(format, args);
        }

        public void Error(Type type, Exception exception)
        {
            Error(type, "{0}", exception);
        }
    }
}
