using System;

namespace Door
{
    public enum DoorStateEnum
    {
        Open,
        Closed
    }
    public class DoorEventArg : EventArgs
    {
        public DoorStateEnum DoorState { set; get; }
    }
    public interface IDoor
    {
        event EventHandler<DoorEventArg> DoorChangedEvent;
    }
}
