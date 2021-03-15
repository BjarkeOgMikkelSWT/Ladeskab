using System;
using System.IO;

namespace EventLogger
{
    public class EventLogger : IEventLogger
    {
        private readonly string _logFilename;
        public EventLogger(string logFilename)
        {

            if (logFilename == null)
            {
                logFilename = "logfile.txt";
            }

            _logFilename = logFilename;

            //Create new file
            using (var tmp = new StreamWriter(_logFilename, true))
            {
                tmp.WriteLine("Start of Log file created on " + DateTime.Now);
            }
        }
        public void LogDoorLocked(int id)
        {
            using (var sw = new StreamWriter(_logFilename, true))
            {
                sw.WriteLine(DateTime.Now + " Door locked with ID: " + id);
            }
        }

        public void LogDoorUnlocked(int id)
        {
            using (var sw = new StreamWriter(_logFilename, true))
            {
                sw.WriteLine(DateTime.Now + " Door unlocked with ID: " + id);
            }
        }
    }
}
