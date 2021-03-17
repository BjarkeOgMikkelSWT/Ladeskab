using System;
using System.Collections.Generic;
using System.Text;

namespace Display
{
    public class Display : IDisplay
    {
        public void DisplayString(string inputS)
        {
            Console.WriteLine(inputS);
        }
    }
}
