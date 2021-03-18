using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charger;
using Display;
using EventLogger;
using Ladeskab;
using LockSimulator;
using RFIDReaderSimulator;
using Door;

namespace Ladeskab
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            //DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;

        //private IChargeControl _charger;
        private int _oldId;
        private readonly ILock _lock;
        private readonly IEventLogger _logger;
        private IRFIDReader _reader;
        private readonly ICharger _charger;
        private readonly IDisplay _display;
        private IDoor _door;

        // Her mangler constructor
        public StationControl(ILock doorLock, IEventLogger logger, IRFIDReader reader, ICharger charger, IDisplay display,IDoor door)
        {
            _lock = doorLock;
            _logger = logger;
            _charger = charger;
            _reader = reader;
            _display = display;
            _door = door;
            _reader.RFIDReadEvent += OnRFIDReadEvent;
            _door.DoorChangedEvent += OnDoorChangedEvent;
            _state = _lock.LockState == ELockState.LockOpen ? LadeskabState.Available : LadeskabState.Locked;

        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RFIDDetected(int id)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected())
                    {
                        _lock.LockDoor();
                        _charger.StartCharge();
                        _oldId = id;
                        _logger.LogDoorLocked(_oldId);
                        _display.DisplayString("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.DisplayString("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                //case LadeskabState.DoorOpen:
                    // Ignore
                    //break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (CheckID(id))
                    {
                        _charger.StopCharge();
                        _lock.UnlockDoor();
                        _logger.LogDoorUnlocked(id);

                        _display.DisplayString("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.DisplayString("Forkert RFID tag");
                    }

                    break;
            }
        }

        private bool CheckID(int id)
        {
            return id == _oldId;
        }

        private void DoorStateChangedHandler(DoorStateEnum doorstate)
        {
            if(doorstate == DoorStateEnum.Open)
            {
                _display.DisplayString("Tilslut telefon");
            }
            else
            {
                _display.DisplayString("Indlæs RFID");
            }
        }

        // Her mangler de andre trigger handlere
        private void OnRFIDReadEvent(object sender, RFIDReaderEventArgs eventArgs)
        {
            RFIDDetected(eventArgs.RFID);
        }

        private void OnDoorChangedEvent(object sender, DoorEventArg eventArgs)
        {
            DoorStateChangedHandler(eventArgs.DoorState);
        }
    }
}
