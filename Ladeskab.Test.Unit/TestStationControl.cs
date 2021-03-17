using System;
using System.Collections.Generic;
using System.Text;
using Charger;
using EventLogger;
using LockSimulator;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using RFIDReaderSimulator;

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

        [SetUp]
        public void SetUp()
        {
            _lock = Substitute.For<ILock>();
            _logger = Substitute.For<IEventLogger>();
            _reader = Substitute.For<IRFIDReader>();
            _charger = Substitute.For<ICharger>();

            _station = new StationControl(_lock, _logger, _reader, _charger);
        }

        [Test]
        public void ConstructorSetsUpEventReceiverForRFIDReader()
        {
            _reader.Received().RFIDReadEvent += Arg.Any<EventHandler<RFIDReaderEventArgs>>();
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

        [Test]
        public void TestFunctionCalledForChargerIsConnectedReturnsTrue()
        {
            _charger.IsConnected().Returns(true);
            _reader.RFIDReadEvent += Raise.EventWith()
        }

    }
}
