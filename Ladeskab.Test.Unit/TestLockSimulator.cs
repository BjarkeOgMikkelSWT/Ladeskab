using LockSimulator;
using NUnit.Framework;

namespace Ladeskab.Test.Unit
{
    [TestFixture]
    public class LockTests
    {
        private ILock _lock;
        [SetUp]
        public void Setup()
        {
            _lock = new Lock();
        }

        [Test]
        public void TestLockInitialState_EqualDoorOpen()
        {
            Assert.That(_lock.LockState, Is.EqualTo(ELockState.LockOpen));
        }

        [Test]
        public void TestLockDoor_StateIsDoorClosed()
        {
            _lock.LockDoor();
            Assert.That(_lock.LockState, Is.EqualTo(ELockState.LockClosed));
        }

        [Test]
        public void TestUnlockDoor_StateIsDoorOpen()
        {
            _lock.UnlockDoor();
            Assert.That(_lock.LockState, Is.EqualTo(ELockState.LockOpen));
        }

        [Test]
        public void TestLockThenUnlockDoor_StateIsDoorOpen()
        {
            _lock.LockDoor();
            _lock.UnlockDoor();
            Assert.That(_lock.LockState, Is.EqualTo(ELockState.LockOpen));
        }


    }
}