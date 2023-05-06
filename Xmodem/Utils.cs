using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    internal class Utils
    {
        const byte SOH = 0x01;
        const byte EOT = 0x04;
        const byte ACK = 0x06;
        const byte NAK = 0x15;
        const byte CAN = 0x18;
        const byte C = 0x43;
        const byte INC = 0x35;

        
        public string checkMessage(byte b)
        {
            switch (b)
            {
                case SOH:
                    {
                        return "SOH";
                    }
                case EOT:
                    {
                        return "EOT";
                    }
                case ACK:
                    {
                        return "ACK";
                    }
                    
            }
            return "nie ma przypisanego znaku";
        }
    }
}
