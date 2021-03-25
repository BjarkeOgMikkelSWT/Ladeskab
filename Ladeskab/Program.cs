using System;
using Door;

namespace Ladeskab
{ 
    public class Program
    {
        static void Main(string[] args)
        {
            
            var display = new Display.Display();
            var door = new Door.DoorSimulator();
            var logger = new EventLogger.EventLogger("logfile.txt");
            var myLock = new LockSimulator.Lock();
            var rfid = new RFIDReaderSimulator.RFIDReaderSimulator();
            var usbChargeSimulator = new UsbChageSimulator.UsbChargerSimulator();

            var charger = new Charger.Charger(usbChargeSimulator, display);

            var station = new StationControl(myLock, logger, rfid, charger, display, door);

            var finish = false;

            Console.WriteLine("Velkommen til ladeskabet!");
            Console.WriteLine("I dette ladeskab er der fem kommandoer:");
            Console.WriteLine("E for exit");
            Console.WriteLine("O for åben døren");
            Console.WriteLine("C for luk døren");
            Console.WriteLine("T for tilslut telefon");
            Console.WriteLine("F for fjern telefon");
            Console.WriteLine("R for rfid");

            do
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        door.SimulateDoorChange(DoorStateEnum.Open);
                        break;

                    case 'C':
                        door.SimulateDoorChange(DoorStateEnum.Closed);
                        break;

                    case 'T':
                        usbChargeSimulator.SimulateConnected(true);
                        break;

                    case 'F':
                        usbChargeSimulator.SimulateConnected(false);
                        break;

                    case 'R':
                        Console.WriteLine("Indtast RFID id: ");
                        var idString = Console.ReadLine();

                        var id = Convert.ToInt32(idString);
                        rfid.SimulateRFIDScan(id);
                        break;

                    default:
                        break;
                }


            } while(!finish);
        }
    }
}
