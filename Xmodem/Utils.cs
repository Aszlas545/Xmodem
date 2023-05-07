using System;

namespace GUI
{
    internal class Utils
    {
        private const byte sOH = 0x01;
        private const byte eOT = 0x04;
        private const byte aCK = 0x06;
        private const byte nAK = 0x15;
        private const byte cAN = 0x18;
        private const byte c = 0x43;

        public static byte SOH => sOH;

        public static byte EOT => eOT;

        public static byte ACK => aCK;

        public static byte NAK => nAK;

        public static byte CAN => cAN;

        public static byte C => c;

        public string checkMessage(byte b)
        {
            switch (b)
            {
                case sOH:
                    {
                        return "SOH";
                    }
                case eOT:
                    {
                        return "EOT";
                    }
                case aCK:
                    {
                        return "ACK";
                    }
                case nAK:
                    {
                        return "NAK";
                    }
                case cAN:
                    {
                        return "CAN";
                    }
                case c:
                    {
                        return "C";
                    }
            }
            return "nie ma przypisanego znaku";
        }

        public byte[] MakeMessage(byte header, byte number, byte complement, byte[] message128, byte[] checksum, bool CRC)
        {
            byte[] data;

            if (CRC == true)
            {
                data = new byte[133];
            }
            else
            {
                data = new byte[132];
            }

            data[0] = header;
            data[1] = number;
            data[2] = complement;

            for (int i = 0; i < message128.Length; i++)
            {
                data[i + 3] = message128[i];
            }

            for (int i = 0; i < checksum.Length; i++)
            {
                data[131 + i] = checksum[i];
            }
            return data;
        }

        public byte[] CountChecksumAlgebraic(byte[] data)
        {
            byte[] checksum = new byte[1];
            var sum = 0;
            foreach (var b in data)
            {
                sum += b;
                sum %= 256;
            }
            checksum[0] = (byte)sum;

            return checksum;
        }

        public byte[] CountChecksumCRC(byte[] data)
        {
            byte[] checksum = new byte[2];
            var crc = 0;
            byte b;

            for (var i = 0; i < 128; i++)
            {
                crc ^= data[i] << 8;
                b = 8;
                do
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = crc << 1 ^ 0x1021;
                    }
                    else
                    {
                        crc = crc << 1;
                    }
                } while (--b != 0);
            }

            crc = (ushort)crc;
            byte[] checksum2 = BitConverter.GetBytes(crc);
            checksum[0] = checksum2[0];
            checksum[1] = checksum2[1];

            return checksum;
        }
    }
}
