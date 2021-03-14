using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EventLogger;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestEventLogger
    {
        private IEventLogger _logger;
        private static readonly string _standardFileName = "testLog.txt";
        private static readonly string _doorLockedText = "Door locked with ID: ";
        private static readonly string _doorUnlockedText = "Door unlocked with ID: ";

        [SetUp]
        public void SetUp()
        {
            _logger = new EventLogger.EventLogger(_standardFileName);
        }

        [TestCase("testLogfile.txt")]
        [TestCase("testLogFile2.txt")]
        public void TestFileCreated(string filename)
        {
            var tempLogger = new EventLogger.EventLogger(filename);
            Assert.That(File.Exists(filename), Is.True);
            File.Delete(filename);
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorLockedEntry(int id)
        {
            _logger.LogDoorLocked(id);
            Assert.That(File.ReadAllText(_standardFileName), Does.Contain(_doorLockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorUnlockedEntry(int id)
        {
            _logger.LogDoorUnlocked(id);
            Assert.That(File.ReadAllText(_standardFileName), Does.Contain(_doorUnlockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorLockedThenUnlockedEntry(int id)
        {
            _logger.LogDoorLocked(id);
            _logger.LogDoorUnlocked(id);


            var temp = File.ReadAllLines(_standardFileName);

            Assert.That(temp[1], Does.Contain(_doorLockedText + id));
            Assert.That(temp[2], Does.Contain(_doorUnlockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorUnockedThenLockedEntry(int id)
        {

            _logger.LogDoorUnlocked(id);
            _logger.LogDoorLocked(id);

            var temp = File.ReadAllLines(_standardFileName);

            Assert.That(temp[1], Does.Contain(_doorUnlockedText + id));
            Assert.That(temp[2], Does.Contain(_doorLockedText + id));
        }

        [Test]
        public void NewlyCreatedFileGetsCorrectHeader()
        {
            Assert.That(File.ReadAllText(_standardFileName), Does.Contain("Start of Log file created on " + DateTime.Now));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_standardFileName))
            {
                File.Delete(_standardFileName);
            }
        }
    }
}
