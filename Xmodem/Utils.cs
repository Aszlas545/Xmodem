using System;

namespace GUI
{
    //--------------------------------------------------------------------------------------------------------------------------
    //Klasa Utils zawiera stałe wartości bajtów używanych w protokole i dodatkowe funkcje takie jak obliczanie CRC i checksum
    //--------------------------------------------------------------------------------------------------------------------------
    internal class Utils
    {
        //Stałe wartości bajtów 
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

        //--------------------------------------------------------------------------------------------------------------------------
        //checkMessage:
        //b - określony w protokole bajt
        //Metoda zwraca reprezentacje bajtu w postaci stringa
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //MakekMessage:
        //header - określony w protokole bajt (zawsze SOH)
        //number - numer pakietu
        //complement - dopełnienie numeru pakietu
        //message128 - 128 bajtów wiadomości
        //checksum - wartość checksuma lub CRC (dlatego jest tablicą, a nie bajtem [CRC ma 2 bajty])
        //CRC - czy używamy CRC czy nie (wpływa to na długość pakietu)
        //Metoda tworzy pakiet umieszczając podane w wywołaniu parametry na odpowiednich indeksach pakietu
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //CountChecksumAlgebraic:
        //data - wiadomość
        //dodaje do siebie wartość lizbową bajtów i zwraca ich reszte z dzielenia przez 256 (zakres bajta)
        //zwraca tablice o długości 1 z obliczoną wartością
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //CountChecksumCRC:
        //data - wiadomość
        /*
        11010011101110 000 <--- 14 bitów danych + 3 wyzerowane bity
        1011               <--- 4-bitowy dzielnik CRC
        01100011101110 000 <--- wynik operacji XOR
         1011
        00111011101110 000
          1011
        00010111101110 000
           1011
        00000001101110 000
               1011
        00000000110110 000
                1011
        00000000011010 000
                 1011
        00000000001100 000
                  1011
        00000000000111 000 
                   101 1
        00000000000010 100
                    10 11 
        ------------------
        00000000000000 010 <--- CRC
        */
        //do ciągu danych dodaje się 3 wyzerowane bity,
        //w linii poniżej wpisuje się 4-bitowy dzielnik CRC,
        //jeżeli nad najstarszą pozycją dzielnika jest wartość 0, to przesuwa się dzielnik w prawo o jedną pozycję, aż do napotkania 1,
        //wykonuje się operację XOR pomiędzy bitami dzielnika i odpowiednimi bitami ciągu danych, uwzględniając dopisane 3 bity
        //wynik zapisuje się w nowej linii – poniżej,
        //jeżeli liczba bitów danych jest większa lub równa 4, przechodzi się do kroku 2,
        //3 najmłodsze bity stanowią szukane CRC, czyli cykliczny kod nadmiarowy:
        //
        //Z tym, że nasz dzielnik jest 2 bajtowy (16 bitowy)
        //Zwraca tablice 2 bajtów
        //--------------------------------------------------------------------------------------------------------------------------
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
