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
        public const byte SOH = 0x01;
        public const byte EOT = 0x04;
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;
        public const byte CAN = 0x18;
        public const byte C = 0x43;
        public const byte INC = 0x35;

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
                case NAK:
                    {
                        return "NAK";
                    }
                case CAN:
                    {
                        return "CAN";
                    }
                case C:
                    {
                        return "C";
                    }
                case INC:
                    {
                        return "INC";
                    }
            }
            return "nie ma przypisanego znaku";
        }
    }
}
