using System;
using System.IO;
using System.Threading;
using EventLogger;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class TestEventLogger
    {
        private IEventLogger _logger;
        private const string StandardFileName = "testLog.txt";
        private const string DoorLockedText = "Door locked with ID: ";
        private const string DoorUnlockedText = "Door unlocked with ID: ";

        [SetUp]
        public void SetUp()
        {
            _logger = new EventLogger.EventLogger(StandardFileName);
        }

        [Test]
        public void NewlyCreatedFileGetsCorrectHeader()
        {
            Assert.That(File.ReadAllText(StandardFileName), Does.Contain("Start of Log file created on " + DateTime.Now));
        }

        [Test]
        public void NewlyCreatedFileOnlyContainsCorrectHeader()
        {
            Assert.That(File.ReadAllLines(StandardFileName).Length, Is.EqualTo(1));
        }

        [TestCase("testLogfile.txt")]
        [TestCase("testLogFile2.txt")]
        public void TestFileCreatedWithSpecifiedName(string filename)
        {
            var unused = new EventLogger.EventLogger(filename);
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
            Assert.That(File.ReadAllText(StandardFileName), Does.Contain(DoorLockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorLockedCreatesOneEntry(int id)
        {
            _logger.LogDoorLocked(id);
            Assert.That(File.ReadAllLines(StandardFileName).Length, Is.EqualTo(2));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorUnlockedEntry(int id)
        {
            _logger.LogDoorUnlocked(id);
            Assert.That(File.ReadAllText(StandardFileName), Does.Contain(DoorUnlockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorUnlockedCreatesOneEntry(int id)
        {
            _logger.LogDoorUnlocked(id);
            Assert.That(File.ReadAllLines(StandardFileName).Length, Is.EqualTo(2));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorLockedThenUnlockedEntryCorrectOrder(int id)
        {
            _logger.LogDoorLocked(id);
            _logger.LogDoorUnlocked(id);


            var temp = File.ReadAllLines(StandardFileName);

            Assert.That(temp[1], Does.Contain(DoorLockedText + id));
            Assert.That(temp[2], Does.Contain(DoorUnlockedText + id));
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestLogDoorUnlockedThenLockedEntryCorrectOrder(int id)
        {

            _logger.LogDoorUnlocked(id);
            _logger.LogDoorLocked(id);

            var temp = File.ReadAllLines(StandardFileName);

            Assert.That(temp[1], Does.Contain(DoorUnlockedText + id));
            Assert.That(temp[2], Does.Contain(DoorLockedText + id));
        }

        [TestCase(new [] {1,2,3,4,5,6,7,8,9,10})]
        [TestCase(new [] {-15,0,-21,68,43,23,11,1,2,3,61})]
        [TestCase(new [] {-15, 0, -21, 68, 43, 23, 11, 1, 2, 3, 61})]
        [TestCase(new [] {-1,0,1})]
        [TestCase(new [] { 1,2,3,4,1,2,3,4,5,6,1,2,3})]
        public void TestMultipleIDsCorrectNumberOfEntries(int[] ids)
        {
            foreach (var id in ids)
            {
                _logger.LogDoorLocked(id);
                _logger.LogDoorUnlocked(id);
            }

            Assert.That(File.ReadAllLines(StandardFileName).Length, Is.EqualTo((ids.Length * 2) + 1));
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(new[] { -15, 0, -21, 68, 43, 23, 11, 1, 2, 3, 61 })]
        [TestCase(new[] { -15, 0, -21, 68, 43, 23, 11, 1, 2, 3, 61 })]
        [TestCase(new[] { -1, 0, 1 })]
        [TestCase(new[] { 1, 2, 3, 4, 1, 2, 3, 4, 5, 6, 1, 2, 3 })]
        public void TestMultipleIDsCorrectSequenceOfEntries(int[] ids)
        {
            foreach (var id in ids)
            {
                _logger.LogDoorLocked(id);
                _logger.LogDoorUnlocked(id);
            }

            var temp = File.ReadAllLines(StandardFileName);
            var i = 1;
            foreach (var id in ids)
            {
                Assert.That(temp[i++], Does.Contain(DoorLockedText + id));
                Assert.That(temp[i++], Does.Contain(DoorUnlockedText + id));
            }
            
        }

        [TestCase(42)]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(2000)]
        public void TestTimeUpdatedCorrectly(int id)
        {
            _logger.LogDoorLocked(id);
            Thread.Sleep(1000);
            _logger.LogDoorUnlocked(id);
            
            var temp = File.ReadAllLines(StandardFileName);
            var date1 = DateTime.Parse(temp[1].Substring(0, 19));
            var date2 = DateTime.Parse(temp[2].Substring(0, 19));

            Assert.That(date1.CompareTo(date2), Is.LessThan(0));
        }



        [TearDown]
        public void TearDown()
        {
            if (File.Exists(StandardFileName))
            {
                File.Delete(StandardFileName);
            }
        }
    }
}
