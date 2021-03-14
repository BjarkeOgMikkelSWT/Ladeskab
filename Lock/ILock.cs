namespace LockSimulator
{
    public interface ILock
    {
        void LockDoor();
        void UnlockDoor();

        ELockState LockState { get; }
    };
}