using System;
using System.Collections.Generic;
using System.Text;

namespace Door
{
    public class DoorSimulator : IDoor
    {
        public event EventHandler<DoorEventArg> DoorChangedEvent;

        public void SimulateDoorChange(DoorStateEnum DoorState)
        {
            OnDoorStateChange(DoorState);
        }

        private void OnDoorStateChange(DoorStateEnum DoorStateIn)
        {
            DoorChangedEvent?.Invoke(this, new DoorEventArg() { DoorState = DoorStateIn });
        }
    }
}
