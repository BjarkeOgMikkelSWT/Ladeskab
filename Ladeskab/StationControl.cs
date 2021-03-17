using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventLogger;
using Ladeskab;
using LockSimulator;
using RFIDReaderSimulator;

namespace Ladeskab
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;

        //private IChargeControl _charger;
        private int _oldId;
        private ILock _lock;
        private IEventLogger _logger;
        private IRFIDReader _reader;

        // Her mangler constructor
        public StationControl(ILock doorLock, IEventLogger logger, IRFIDReader reader)
        {
            _lock = doorLock;
            _logger = logger;
            _reader = reader;
            _reader.RFIDReadEvent += OnRFIDReadEvent;
            
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RFIDDetected(int id)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    //if (_charger.Connected)
                {
                    _lock.LockDoor();
                    //_charger.StartCharge();
                    _oldId = id;
                    _logger.LogDoorLocked(_oldId);

                    Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                    _state = LadeskabState.Locked;
                }
                    //else
                {
                    Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (CheckID(id))
                    {
                        //_charger.StopCharge();
                        _lock.UnlockDoor();
                        _logger.LogDoorUnlocked(id);

                        Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
            }
        }

        private bool CheckID(int id)
        {
            return id == _oldId;
        }

        // Her mangler de andre trigger handlere
        private void OnRFIDReadEvent(object sender, RFIDReaderEventArgs eventArgs)
        {
            RFIDDetected(eventArgs.RFID);
        }
    }
}
