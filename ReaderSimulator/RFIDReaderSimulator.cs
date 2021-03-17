using System;

namespace RFIDReaderSimulator
{
    public class RFIDReaderSimulator : IRFIDReader
    {
        public event EventHandler<RFIDReaderEventArgs> RFIDReadEvent;

        public void SimulateRFIDScan(int id)
        {
            OnRFIDRead(id);
        }

        private void OnRFIDRead(int id)
        {
            RFIDReadEvent?.Invoke(this, new RFIDReaderEventArgs() {RFID = id});
        }
    }
}
