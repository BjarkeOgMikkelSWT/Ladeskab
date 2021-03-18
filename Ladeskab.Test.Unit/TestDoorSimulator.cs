using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Door;


namespace Ladeskab.Test.Unit
{
    [TestFixture]
    class TestDoorSimulator
    {
        private DoorSimulator _uut;
        private DoorEventArg _reciecedDoorArg;

        [SetUp]
        public void SetUp()
        {
            _reciecedDoorArg = null;
            _uut = new DoorSimulator();
            _uut.DoorChangedEvent += (o, args) => _reciecedDoorArg = args;
        }

        [Test]
        public void DoorStateEventFired()
        {
            _uut.SimulateDoorChange(DoorStateEnum.Open);
            Assert.That(_reciecedDoorArg, Is.Not.Null);
        }
       
        [TestCase(DoorStateEnum.Closed)]
        [TestCase(DoorStateEnum.Open)]
        public void DoorStateIsStoredInArg(DoorStateEnum state)
        {
            _uut.SimulateDoorChange(state);
            Assert.That(state, Is.EqualTo(_reciecedDoorArg.DoorState));
        }

    }
}
