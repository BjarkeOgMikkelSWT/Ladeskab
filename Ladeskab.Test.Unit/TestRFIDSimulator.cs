using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RFIDReaderSimulator;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestRFIDSimulator
    {

        private RFIDReaderSimulator.RFIDReaderSimulator _uut;

        [SetUp]
        public void SetUp()
        {
            _uut = new RFIDReaderSimulator.RFIDReaderSimulator();
        }

        [TestCase(42)]
        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(2000)]
        public void EventReceiverGetsCorrectID(int id)
        {
            var ID = int.MinValue;
            _uut.RFIDReadEvent += (o, args) => ID = args.RFID;

            _uut.SimulateRFIDScan(id);

            Assert.That(ID, Is.EqualTo(id));
        }
    }
}
