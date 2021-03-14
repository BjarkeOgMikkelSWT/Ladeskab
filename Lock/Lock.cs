namespace LockSimulator
{
    public class Lock : ILock
    {
        public Lock()
        {
            LockState = ELockState.LockOpen;
        }

        public void LockDoor()
        {
            LockState = ELockState.LockClosed;
        }

        public void UnlockDoor()
        {
            LockState = ELockState.LockOpen;
        }

        public ELockState LockState { get; private set; }
    }
}
