using System;
using System.Collections.Generic;
using System.Text;

namespace RFIDReaderSimulator
{
    public class RFIDReaderEventArgs : EventArgs
    {
        public int RFID { get; set; }
    }

    public interface IRFIDReader
    {
        event EventHandler<RFIDReaderEventArgs> RFIDReadEvent;
    }
}
