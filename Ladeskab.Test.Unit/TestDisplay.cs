using System;
using System.Collections.Generic;
using System.Text;


using NUnit.Framework;
using Display;
using NSubstitute;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestDisplay
    {
        private IDisplay _uut;
        [SetUp]
        public void SetUp()
        {
            _uut = new Display.Display();
        }

        [TestCase("String 1")]
        [TestCase("String with @3€$€${@£")]
        [TestCase("")]
        public void DisplayStringDisplaysOnConsole(string expected)
        {
            using var consoleOutput = new ConsoleOutput();

            _uut.DisplayString(expected);

            Assert.That(expected, Is.EqualTo(consoleOutput.GetOuput()));
        }

        [TestCase("String 1")]
        [TestCase("String with @3€$€${@£")]
        [TestCase("")]
        public void DisplayStringFailsWithWrongInputs(string expected)
        {
            using var consoleOutput = new ConsoleOutput();

            _uut.DisplayString(expected + "Kage");

            Assert.That(expected, Is.Not.EqualTo(consoleOutput.GetOuput()));
        }
    }
}
