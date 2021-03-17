using System;
using UsbChageSimulator;

namespace Charger
{
    public class Charger : ICharger
    {
        private readonly IUsbCharger _usbCharger;
        private readonly IDisplay _display;

        public Charger(IUsbCharger usbCharger)
        {
            _usbCharger = usbCharger;
            usbCharger.CurrentValueEvent += HandleCurrentChangedEvent;
        }

        private void HandleCurrentChangedEvent(object sender, CurrentEventArgs e)
        {
            double Current = e.Current;
            if (Current > 500)
            {
                StopCharge();
                _Display.DisplayString("Charging Error");
            }
            else if (Current > 5 && Current <= 500)
            {
                //Do nothing
            }
            else if (Current > 0 && Current <= 5)
            {
                _Display.DisplayString("Fully Charged");
            }
            else if (Current <= 0)
            {
                //Do nothing
                StopCharge();
            }
        }

        public bool IsConnected()
        {
            return _usbCharger.Connected;
        }

        public void StartCharge()
        {
            double Current = MesureCurrent();

            if (Current > 500)
            {
                StopCharge();
                _Display.DisplayString("Charging Error");
            }
            else if (Current > 5 && Current <= 500)
            {
                _usbCharger.StartCharge();
            }
            else if (Current > 0 && Current <= 5)
            {
                _Display.DisplayString("Fully Charged");
            }
            else if (Current <= 0)
            {
                StopCharge();
            }
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }

        private double MesureCurrent()
        {
            return _usbCharger.CurrentValue;
        }
    }
}
