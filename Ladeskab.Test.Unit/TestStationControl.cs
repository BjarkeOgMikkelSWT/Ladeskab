using System;
using System.Collections.Generic;
using System.Text;
using Charger;
using Display;
using EventLogger;
using LockSimulator;
using NSubstitute;
using NUnit.Framework;
using RFIDReaderSimulator;
using Door;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _station;
        private ILock _lock;
        private IRFIDReader _reader;
        private ICharger _charger;
        private IEventLogger _logger;
        private IDisplay _display;
        private IDoor _door;

        [SetUp]
        public void SetUp()
        {
            _lock = Substitute.For<ILock>();
            _logger = Substitute.For<IEventLogger>();
            _reader = Substitute.For<IRFIDReader>();
            _charger = Substitute.For<ICharger>();
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();

            _station = new StationControl(_lock, _logger, _reader, _charger, _display,_door);
        }

        [Test]
        public void ConstructorSetsUpEventReceiverForRFIDReader()
        {
            _reader.Received().RFIDReadEvent += Arg.Any<EventHandler<RFIDReaderEventArgs>>();
        }

        [Test]
        public void ConstructorSetsUpEventReceiverForDoor()
        {
            _door.Received().DoorChangedEvent += Arg.Any<EventHandler<DoorEventArg>>();
        }

        [Test]
        public void TestChargerIsConnectedIsCalledForStateAvailable()
        {
            //Default state of the station is available

            //Send event
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs());
            
            //Check that the expected functions were called
            _charger.Received(1).IsConnected();
        }

        [TestCase(42)]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(2000)]
        public void TestFunctionCalledForChargerIsConnectedReturnsTrue(int id)
        {
            //Default state of the station is available
            using var cLogger = new ConsoleOutput();
            _charger.IsConnected().Returns(true);
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() {RFID = id});

            _lock.Received(1).LockDoor();
            _charger.Received(1).StartCharge();
            _logger.Received(1).LogDoorLocked(id);
            _display.Received(1).DisplayString("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        [TestCase(42)]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(2000)]
        public void TestFunctionCalledForChargerIsConnectedReturnsFalse(int id)
        {
            //Default state of the station is available
            using var cLogger = new ConsoleOutput();
            _charger.IsConnected().Returns(false);
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() {RFID = id});

            _lock.Received(0).LockDoor();
            _charger.Received(0).StartCharge();
            _logger.Received(0).LogDoorLocked(id);
            _display.Received(1).DisplayString("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        [TestCase(42)]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(2000)]
        public void TestFunctionCalledForStateIsLockedAndLockIDIsEqualToUnlockID(int id)
        {
            //Change state to Locked
            _charger.IsConnected().Returns(true);
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() {RFID = id});

            using var cLogger = new ConsoleOutput();
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() {RFID = id});

            _charger.Received(1).StopCharge();
            _lock.Received(1).UnlockDoor();
            _logger.Received(1).LogDoorUnlocked(id);
            _display.Received(1).DisplayString("Tag din telefon ud af skabet og luk døren");
        }

        [TestCase(42)]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(2000)]
        public void TestFunctionCalledForStateIsLockedAndLockIDIsNotEqualToUnlockID(int id)
        {
            //Change state to Locked
            _charger.IsConnected().Returns(true);
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() { RFID = id });

            using var cLogger = new ConsoleOutput();
            _reader.RFIDReadEvent += Raise.EventWith(new RFIDReaderEventArgs() { RFID = id+1 });

            _charger.Received(0).StopCharge();
            _lock.Received(0).UnlockDoor();
            _logger.Received(0).LogDoorUnlocked(id);
            _display.Received(1).DisplayString("Forkert RFID tag");
        }


        [TestCase(DoorStateEnum.Closed,"Indlæs RFID")]
        [TestCase(DoorStateEnum.Open,"Tilslut telefon")]
        public void DoorChangedEventHandledCorrectly(DoorStateEnum doorStateIn,string expectedS)
        {
            _door.DoorChangedEvent += Raise.EventWith(new DoorEventArg { DoorState = doorStateIn });
            _display.Received(1).DisplayString(expectedS);
        }
    }
}
