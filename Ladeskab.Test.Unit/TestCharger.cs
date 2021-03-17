using System;
using System.Collections.Generic;
using System.Text;

using NSubstitute;
using NUnit.Framework;
using Charger;
using UsbChageSimulator;
using Display;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestCharger
    {
        private ICharger _uut;
        private IUsbCharger _usbCharger;
        private UsbChageSimulator.UsbChargerSimulator _usbSimulator;
        private IDisplay _display;
        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _usbCharger = Substitute.For<IUsbCharger>();
            _uut = new Charger.Charger(_usbCharger,_display);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsConnectedReturnsCorrectConnectedStatus(bool expected)
        {
            //Arange
            _usbCharger.Connected.Returns(expected);

            Assert.That(_uut.IsConnected(), Is.EqualTo(expected));
        }

        [Test]
        public void StopChargeCallsStopCharge()
        {
            _uut.StopCharge();
            _usbCharger.Received(1).StopCharge();
        }

    }
}
