using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mozu.Api.ToolKit.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception ex = null, object properties = null)
        {
            logger.LogDebug(CreateMessage(message, ex), properties);
        }

        public static void Error(this ILogger logger, string message,Exception ex, object properties = null)
        {
            logger.LogDebug(CreateMessage(message, ex), properties);
        }

        public static void Warn(this ILogger logger, string message, Exception ex = null, object properties = null)
        {
            logger.LogDebug(CreateMessage(message, ex), properties);
        }

        public static void Info(this ILogger logger, string message, Exception ex = null, object properties = null)
        {
            logger.LogDebug(CreateMessage(message, ex), properties);
        }
        
        //Revisit this
        private static string CreateMessage(string message,Exception ex)
        {
            var mozuCorrId = (ex != null && ex.GetType() == typeof(ApiException)
                ? "Mozu CorrId " + ((ApiException)ex).CorrelationId
                : string.Empty);
            if (Trace.CorrelationManager.ActivityId != Guid.Empty)
                message = string.Format("{0} {1} {2}", Trace.CorrelationManager.ActivityId, mozuCorrId, message);

            return message;
        }
    }
}
