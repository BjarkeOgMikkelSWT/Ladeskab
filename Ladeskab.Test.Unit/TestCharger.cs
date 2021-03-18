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
        private UsbChageSimulator.UsbChargerSimulator _usbSimulator;
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

        [TestCase(0,true)]
        [TestCase(-6,true)]
        [TestCase(0.1, false)]
        [TestCase(5, false)]
        [TestCase(5.1, false)]
        [TestCase(500, false)]
        [TestCase(500.1, true)]
        public void StartChargeCallsStopChargeAtCorrectCurrents(double Current,bool expectedCall)
        {
            //Arange
            _usbCharger.CurrentValue.Returns(Current);

            //Action
            _uut.StartCharge();

            if(expectedCall)
            {
                _usbCharger.Received(1).StopCharge();
            }
            else
            {
                _usbCharger.DidNotReceive().StopCharge();
            }
        }

        [TestCase(0, false)]
        [TestCase(-6, false)]
        [TestCase(0.1, true)]
        [TestCase(5, true)]
        [TestCase(5.1, true)]
        [TestCase(500, true)]
        [TestCase(500.1, false)]
        public void StartChargeCallsStartChargeAtCorrectCurrents(double Current, bool expectedCall)
        {
            //Arange
            _usbCharger.CurrentValue.Returns(Current);

            //Action
            _uut.StartCharge();

            if (expectedCall)
            {
                _usbCharger.Received(1).StartCharge();
            }
            else
            {
                _usbCharger.DidNotReceive().StartCharge();
            }
        }

        [TestCase(0,false)]
        [TestCase(-6, false)]
        [TestCase(0.1, false)]
        [TestCase(5, false)]
        [TestCase(5.1, false)]
        [TestCase(500, false)]
        [TestCase(500.1, true)]
        public void StartChargeCallsDisplayWithChargingErrorAtCorrectCurrents(double Current, bool expected)
        {
            //Arange
            _usbCharger.CurrentValue.Returns(Current);

            //Action
            _uut.StartCharge();

            if(expected)
            {
                _display.Received(1).DisplayString("Charging Error");
            }
            else
            {
                _display.DidNotReceive().DisplayString("Charging Error");
            }
        }

        [TestCase(0, false)]
        [TestCase(-6, false)]
        [TestCase(0.1, true)]
        [TestCase(5, true)]
        [TestCase(5.1, false)]
        [TestCase(500, false)]
        [TestCase(500.1, false)]
        public void StartChargeCallsDisplayWithFullyChargedAtCorrectCurrents(double Current, bool expected)
        {
            //Arange
            _usbCharger.CurrentValue.Returns(Current);

            //Action
            _uut.StartCharge();

            if (expected)
            {
                _display.Received(1).DisplayString("Fully Charged");
            }
            else
            {
                _display.DidNotReceive().DisplayString("Fully Charged");
            }
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
