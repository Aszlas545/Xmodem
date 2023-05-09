using GUI;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Xmodem
{
    //--------------------------------------------------------------------------------------------------------------------------
    //Klasa Sender odpowiada za wysyłanie wiadomości do drugiej strony za pomocą portu szeregowego
    //--------------------------------------------------------------------------------------------------------------------------
    internal class Sender
    {
        //--------------------------------------------------------------------------------------------------------------------------
        //Pola klasy:
        //port - reprezentuja port szeregowy i słóży on do odczytywania wiadomości i przesyłania komunikatów do nadawcy
        //utils - klasa która zawiera stałe bajty jak SOH, ACK itd, oraz metody tworzące wiadomości oraz obliczające CRC i checksum
        //incremented - ile SUB bajtów (0x26) zostało dopisanych 
        //--------------------------------------------------------------------------------------------------------------------------
        Utils utils = new Utils();
        SerialPort port;
        int incremented;

        //--------------------------------------------------------------------------------------------------------------------------
        //Konstruktor klasy:
        //Ustawia on nazwe portu podaną przez użytkownika, oraz timeouty ustawione na stało
        //następnie otwiera port aby był on gotowy do komunikacji
        //--------------------------------------------------------------------------------------------------------------------------
        public Sender(string portName)
        {
            this.port = new SerialPort(portName);
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.Open();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //SendData:
        //Główna funkcjonalność klasy, przyjmuje ona parametr data który jest wiadomością (plikiem który przesyłamy)
        //useCRC który określa z którego typu protokołu czekamy
        //Nadawca czeka na znak od odbiorcy NAK (dla protokołu Xmodem) lub C (dla protokołu XmodemCRC)
        //Dopisuje bajty aby ich liczba była podzielna przez 128 (wymóg protokołu)
        //Oblicza pakiety o długości 132 lub 133 (dla CRC) pisząc je w formacie:
        // HEADER, NUMER PAKIETU, DOPEŁNIENIE, 128B WIADOMOŚCI, CHECKSUM LUB CRC
        //Następnie zaczyna wysyłać pakiety, jeżeli otrzyma ACK to wysyła następny pakiet jeżeli NAK to ten sam pakiet
        //Powtarza operacje aż skończą mu sie pakiety, wysyła EOT i czeka na ACK
        //--------------------------------------------------------------------------------------------------------------------------
        public void SendData(byte[] data, bool useCRC)
        {

            //---------------------------------------------------------------------------------------
            //Dopisanie bjatów aby ich ilość była podzielna przez 128 i podział na pakiety
            //---------------------------------------------------------------------------------------

            Console.WriteLine("Rozmiar danych przed dopisaniem SUB bajtów: " +  data.Length);
            data = FillTo128(data);
            Console.WriteLine("Rozmiar danych po dopisaniu SUB bajtów: " + data.Length);
            List<byte[]> packets = DivideToPackets(data, useCRC);

            //---------------------------------------------------------------------------------------
            //Czekanie na C lub NAK od odbiorcy
            //---------------------------------------------------------------------------------------

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

            //---------------------------------------------------------------------------------------
            //Wysyłanie odpowiednich pakietów w zależności od odpowiedzi odbiorcy na dany pakiet
            //---------------------------------------------------------------------------------------

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

            //---------------------------------------------------------------------------------------
            //Wysłanie EOT po wysłanie wszystkich pakttów i oczekiwanie na odpowiedź ACK
            //---------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------------------------------------
        //FillTo128:
        //data - wiadomość
        //Metoda dopisuje SUB bajty 0x26 aby ich ilość była wielokrotnością 128
        //--------------------------------------------------------------------------------------------------------------------------
        public byte[] FillTo128(byte[] data)
        {
            incremented = 128 - data.Length % 128;
            byte[] addedBytes = Enumerable.Repeat((byte)0x26, incremented).ToArray();
            return data.Concat(addedBytes).ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //ReadWithoutTimeout:
        //Metoda czyta z portu bez limitu, pomaga to zacząć konwersacje bo możemy najpierw uruchomić nadawce i nie ma możliwości,
        //że odbiorca zostanie uruchomiony za późno
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //ReadWithoutTimeout:
        //data - wiadomość
        //CRC - który rodzaj protokołu używamy
        //Metoda tworzy pakiety i zwraca je w postaci Listy tablic bajtów, gdzie każda tablica to jeden pakiet
        //pakiety są tworzone w foramcie: HEADER, NUMER PAKIETU, DOPEŁNIENIE, 128B WIADOMOŚCI, CHECKSUM LUB CRC
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //Dispose:
        //Metoda wywoływana po zakończeniu operacji SendData aby zamknąć port, aby nie blokować go dla innych transmisji
        //--------------------------------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
