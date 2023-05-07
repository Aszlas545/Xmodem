using GUI;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Xmodem
{
    internal class Reciever
    {
        SerialPort port;
        Utils utils = new Utils();

        public Reciever(string portName)
        {
            this.port = new SerialPort(portName);
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.Open();
        }

        public byte[] Recieve(bool useCRC)
        {
            var packetLenght = 0;
            if (useCRC) { packetLenght = 133; }
            else { packetLenght = 132; }
            var buffer = new byte[packetLenght];

            //---------------------------------------------------------------------------------------
            //nawiązanie połączniea
            //---------------------------------------------------------------------------------------

            for (var i = 0; i < 6; i++)
            {
                if (useCRC)
                {
                    port.WriteByte(Utils.C);
                    Console.WriteLine("wysyłam C");
                }
                else
                {
                    port.WriteByte(Utils.NAK);
                    Console.WriteLine("wysyłam NAK");
                }
                
                try
                {
                    var read = 0;
                    while (true)
                    {
                        read += port.Read(buffer, read, packetLenght - read);
                        if (read == packetLenght)
                        {
                            break;
                        }
                    }

                    if (buffer[0] == Utils.SOH)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("nie mam połącznia");
                }
            }

            var data = new List<byte>();

            //-------------------------------------------------------------------------------------------------------------
            //odbiór danych w segmentach 128 bajtowych i dodawany bajtów do listy z której potem zostaną zapisane do pliku
            //-------------------------------------------------------------------------------------------------------------

            while (true)
            {
                var message = GetMessageFromBuffer(buffer);
                if (buffer[0] == Utils.SOH)
                {
                    if (isPacketValid(buffer, useCRC) == false)
                    {
                        port.WriteByte(Utils.NAK);
                        Console.WriteLine("suma kontrolna sie nie zgadza, wysyłam NAK");
                    }
                    else
                    {
                        data.AddRange(message);
                        port.WriteByte(Utils.ACK);
                        Console.WriteLine("suma kontrolna się zgadza, wysyłam ACK");
                    }
                }
                else if (buffer[0] == Utils.EOT)
                {
                    port.WriteByte(Utils.ACK);
                    Console.WriteLine("odebrałem EOT, wysyłam ACK");
                    break;
                }

                var read = 0;
                while (true)
                {
                    read += port.Read(buffer, read, packetLenght - read);
                    if (read == packetLenght)
                    {
                        break;
                    }

                    if (buffer[0] == Utils.EOT)
                    {
                        break;
                    }
                }
            }

            var addedBytes = 0;
            for (var i = data.Count - 1; i >= 0; i--)
            {
                if (data[i] == 0x26)
                {
                    addedBytes++;
                }
                else
                {
                    break;
                }
            }
            return data.Take(data.Count - addedBytes).ToArray();
        }

        public byte[] GetMessageFromBuffer(byte[] packetBuffer)
        {
            byte[] message = new byte[128];
            for (int i = 0; i < message.Length; i++)
            {
                message[i] = packetBuffer[i+3];
            }
            return message;
        }

        public bool isPacketValid(byte[] packetBuffer, bool CRC)
        {
            byte[] message = GetMessageFromBuffer(packetBuffer);
            if (CRC)
            {
                Console.WriteLine("OdczytaneCRC: " + packetBuffer[131] + "," + packetBuffer[132] + "; ObliczoneCRC: " + utils.CountChecksumCRC(message)[0] + "," + utils.CountChecksumCRC(message)[1]);
                if (utils.CountChecksumCRC(message)[0] == packetBuffer[131] && utils.CountChecksumCRC(message)[1] == packetBuffer[132])
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("OdczytanyCheckSum: " + packetBuffer[131] + "; ObliczonyCheckSum: " + utils.CountChecksumAlgebraic(message)[0]);
                if (utils.CountChecksumAlgebraic(message)[0] == packetBuffer[131])
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
