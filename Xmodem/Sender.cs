using GUI;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Xmodem
{
    internal class Sender
    {
        Utils utils = new Utils();
        SerialPort port;
        int incremented;


        public Sender(string portName)
        {
            this.port = new SerialPort(portName);
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.Open();
        }

        public void SendData(byte[] data, bool useCRC)
        {
            Console.WriteLine("Rozmiar danych przed dopisaniem SUB bajtów: " +  data.Length);
            data = FillTo128(data);
            Console.WriteLine("Rozmiar danych po dopisaniu SUB bajtów: " + data.Length);
            List<byte[]> packets = DivideToPackets(data, useCRC);
            while (true)
            {
                var response = ReadWithoutTimeout();
                if (response == Utils.NAK)
                {
                    Console.WriteLine("otrzymałem NAK, zaczynam");
                    break;
                }

                if (response == Utils.C)
                {
                    if (useCRC)
                    {
                        Console.WriteLine("otrzymałem C, zaczynam");
                        break;
                    }
                    else 
                    {
                        Console.WriteLine("konflikt CRC");
                        return;
                    }
                }
            }
            
            var packetNumber = 0;
            while (true)
            {
                if (packetNumber == packets.Count())
                {
                    break;
                }
                var packet = packets[packetNumber];
                port.Write(packet, 0, packet.Length);
                if (useCRC)
                {
                    Console.WriteLine("Wysyłam pakiet: " + utils.checkMessage(packet[0]) + "; " + packet[1] + "; " + packet[2] + "; 128 message bytes; " + packet[131] + "," + packet[132]);
                }
                else
                {
                    Console.WriteLine("Wysyłam pakiet: " + utils.checkMessage(packet[0]) + "; " + packet[1] + "; " + packet[2] + "; 128 message bytes; " + packet[131]);
                }
                
                var response = port.ReadByte();
                
                if (response == Utils.ACK)
                {
                    packetNumber++;
                }

                if (response == Utils.NAK)
                {
                    Console.WriteLine("ponownie wysyłam pakiet " + packet[1]);
                }
            }

            while (true)
            {
                port.WriteByte(Utils.EOT);
                Console.WriteLine("Wysyłam EOT");
                var response = port.ReadByte();
                if (response == Utils.ACK)
                {
                    Console.WriteLine("otrzymałem ACK, kończe");
                    return;
                }
            }
        }

        public byte[] FillTo128(byte[] data)
        {
            incremented = 128 - data.Length % 128;
            byte[] addedBytes = Enumerable.Repeat((byte)0x26, incremented).ToArray();
            return data.Concat(addedBytes).ToArray();
        }

        private byte ReadWithoutTimeout()
        {
            try
            {
                var value = port.ReadByte();
                return (byte)value;
            }
            catch (Exception e)
            {
                Console.WriteLine("Czekam na połączenie");
            }
            return 0;
        }

        private List<byte[]> DivideToPackets(byte[] data, bool CRC)
        {
            List<byte[]> packets = new List<byte[]>();
            List<byte[]> messages = new List<byte[]>();
            for (int i = 0; i < data.Length/128; i++)
            {
                byte[] message = new byte[128];
                for (int j = 0; j < 128; j++)
                {
                    message[j] = data[i * 128 + j];
                }
                messages.Add(message);
            }
            for (int i = 0; i < messages.Count; i++)
            {
                byte[] packet;
                if (CRC)
                {
                    packet = utils.MakeMessage(Utils.SOH, Convert.ToByte(i + 1), Convert.ToByte(255 - (i + 1)), messages[i], utils.CountChecksumCRC(messages[i]), CRC);
                }
                else
                {
                    packet = utils.MakeMessage(Utils.SOH, Convert.ToByte(i + 1), Convert.ToByte(255 - (i + 1)), messages[i], utils.CountChecksumAlgebraic(messages[i]), CRC);
                }
                packets.Add(packet);
            }
            Console.WriteLine("ilość pakietów: " + packets.Count);
            return packets;
        }

        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
