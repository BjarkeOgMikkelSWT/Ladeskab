using System;
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
            double Current = e.Current;
            if (Current > 500)
            {
                StopCharge();
                _display.DisplayString("Charging Error");
            }
            else if (Current > 5 && Current <= 500)
            {
                //Do nothing
            }
            else if (Current > 0 && Current <= 5)
            {
                _display.DisplayString("Fully Charged");
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
                _display.DisplayString("Charging Error");
            }
            else if (Current > 5 && Current <= 500)
            {
                _usbCharger.StartCharge();
            }
            else if (Current > 0 && Current <= 5)
            {
                _usbCharger.StartCharge();
                _display.DisplayString("Fully Charged");
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
