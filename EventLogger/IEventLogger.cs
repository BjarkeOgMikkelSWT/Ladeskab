using System;
using System.Collections.Generic;
using System.Text;

namespace EventLogger
{
    public interface IEventLogger
    {
        void LogDoorLocked(int id);
        void LogDoorUnlocked(int id);
    }
}
