using System;
using System.Diagnostics;

namespace DVLD_DataAccessLayer
{
    static internal class clsLogger
    {

        private static string SourceName = "DVLD";
        private static string LogType = "Application";

        public static void LogError(Exception ex)
        {
            string Message =
                $"Exception: {ex.Message}\n\n" +
                $"Type: {ex.GetType().FullName}\n\n" +
                $"StackTrace: {ex.StackTrace}";

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogType);
            }

            EventLog.WriteEntry(SourceName, Message, EventLogEntryType.Error);

        }
    }
}
