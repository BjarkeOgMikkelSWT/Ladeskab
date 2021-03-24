using UsbChageSimulator;
using Display;

namespace Charger
{
    public class Charger : ICharger
    {
        private readonly IUsbCharger _usbCharger;
        private readonly IDisplay _display;

        public Charger(IUsbCharger usbCharger,IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            usbCharger.CurrentValueEvent += HandleCurrentChangedEvent;
        }

        private void HandleCurrentChangedEvent(object sender, CurrentEventArgs e)
        {
            double current = e.Current;
            if (current > 500)
            {
                StopCharge();
                _display.DisplayString("Charging Error");
            }
            else if (current > 5 && current <= 500)
            {
                //Do nothing
            }
            else if (current > 0 && current <= 5)
            {
                _display.DisplayString("Fully Charged");
            }
            else if (current <= 0)
            {
                StopCharge();
            }
        }

        public bool IsConnected()
        {
            return _usbCharger.Connected;
        }

        public void StartCharge()
        {
            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }
    }
}
