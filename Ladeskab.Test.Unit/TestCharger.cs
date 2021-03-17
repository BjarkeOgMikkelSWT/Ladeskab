using System;
using System.Collections.Generic;
using System.Text;

using NSubstitute;
using NUnit.Framework;
using Charger;
using UsbChageSimulator;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestCharger
    {
        private ICharger _uut;
        private IUsbCharger _usbCharger;    
        [SetUp]
        public void Setup()
        {
            _usbCharger = Substitute.For<IUsbCharger>();
            _uut = new Charger.Charger(_usbCharger);
        }



    }
}
