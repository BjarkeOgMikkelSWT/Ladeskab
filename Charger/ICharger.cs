namespace Charger
{
    public interface ICharger
    {
        bool IsConnected();
        void StartCharge();
        void StopCharge();
    }
}