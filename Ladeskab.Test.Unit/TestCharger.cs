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
        private Charger.Charger _uut;
        private IUsbCharger _usbCharger;
        private IDisplay _display;
        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _usbCharger = Substitute.For<IUsbCharger>();
            _uut = new Charger.Charger(_usbCharger,_display);
        }

        [Test]
        public void ConstructorSetupEvent()
        {
            _usbCharger.Received(1).CurrentValueEvent += Arg.Any<EventHandler<CurrentEventArgs>>();
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

        //Start Charge tests


        [Test]
        public void StartChargeCallsStartCharge()
        {
            //Action
            _uut.StartCharge();

            _usbCharger.Received(1).StartCharge();
        }


        [Test]
        public void CurrentChangedEventHandlerAtOverCurrent()
        {
            //Arange
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 501 });

            //Assert
            _usbCharger.Received(1).StopCharge();
            _display.Received(1).DisplayString("Charging Error");
            _display.Received(0).DisplayString("Fully Charged");
        }

        [TestCase(5.1)]
        [TestCase(500)]
        public void CurrentChangedEventHandlerAtNormalCurrent(double current)
        {
            //Arange
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });

            //Assert
            _usbCharger.Received(0).StopCharge();
            _display.Received(0).DisplayString("Charging Error");
            _display.Received(0).DisplayString("Fully Charged");
        }

        [TestCase(5)]
        [TestCase(0.1)]
        public void CurrentChangedEventHandlerAtLowCurrent(double current)
        {
            //Arange
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });

            //Assert
            _usbCharger.Received(0).StopCharge();
            _display.Received(0).DisplayString("Charging Error");
            _display.Received(1).DisplayString("Fully Charged");
        }

        [TestCase(0)]
        [TestCase(-6)]
        public void CurrentChangedEventHandlerAtNoCurrent(double current)
        {
            //Arange
            _usbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = current });

            //Assert
            _usbCharger.Received(1).StopCharge();
            _display.Received(0).DisplayString("Charging Error");
            _display.Received(0).DisplayString("Fully Charged");
        }
    }
}
