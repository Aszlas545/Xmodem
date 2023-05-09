using GUI;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Xmodem
{
    //--------------------------------------------------------------------------------------------------------------------------
    //Klasa Reciever odpowiada za odbieranie wiadomości od drugiej strony za pomocą portu szeregowego
    //--------------------------------------------------------------------------------------------------------------------------
    internal class Reciever
    {
        //--------------------------------------------------------------------------------------------------------------------------
        //Pola klasy:
        //port - reprezentuja port szeregowy i słóży on do odczytywania wiadomości i przesyłania komunikatów do nadawcy
        //utils - klasa która zawiera stałe bajty jak SOH, ACK itd, oraz metody tworzące wiadomości oraz obliczające CRC i checksum
        //--------------------------------------------------------------------------------------------------------------------------
        SerialPort port;
        Utils utils = new Utils();

        //--------------------------------------------------------------------------------------------------------------------------
        //Konstruktor klasy:
        //Ustawia on nazwe portu podaną przez użytkownika, oraz timeouty ustawione na stało
        //następnie otwiera port aby był on gotowy do komunikacji
        //--------------------------------------------------------------------------------------------------------------------------
        public Reciever(string portName)
        {
            this.port = new SerialPort(portName);
            port.ReadTimeout = 1000;
            port.WriteTimeout = 1000;
            port.Open();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //Recieve:
        //Główna funkcjonalność klasy, przyjmuje ona parametr useCRC który określa z którego typu protokołu czekamy
        //Odbiorca zaczyna dialog wysyłając NAK (dla protokołu Xmodem) lub C (dla protokołu XmodemCRC)
        //Następnie zaczyna odbierać pakiety o długości 132 lub 133 bajtów
        //Oblicza CRC lub checksum dla wiadomości i odpowiada znakiem ACK jeśli się zgadza z tą odebraną 
        //lub NAK jeżeli się różnią. Izoluje 128 bajtów wiadomości i dodaje je do listy bajtów.
        //Proces powtarza się aż odebrane zostanie EOT, po tym odbiorca odsyła ACK i usuwa bajty 26 (SUB) i zwraca tablice bajtów.
        //--------------------------------------------------------------------------------------------------------------------------
        public byte[] Recieve(bool useCRC)
        {

            //---------------------------------------------------------------------------------------
            //w zależności od użycia CRC oblicza długość pakietów i tworzy bufor o takiej długości
            //---------------------------------------------------------------------------------------

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

            //-------------------------------------------------------------------------------------------------------------
            //Usunięcie bajtów o wartości 26 (SUB bytes) i zwrócenie bajtów wiadomości
            //-------------------------------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------------------------------------
        //GetMessageFromBuffer:
        //packetBuffer - cały pakiet przechowywany w bufforze
        //Metoda izoluje 128 bajtów wiadomości odcinając header, numer pakietu i dopełnienie.
        //--------------------------------------------------------------------------------------------------------------------------
        public byte[] GetMessageFromBuffer(byte[] packetBuffer)
        {
            byte[] message = new byte[128];
            for (int i = 0; i < message.Length; i++)
            {
                message[i] = packetBuffer[i+3];
            }
            return message;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //isPacketValid:
        //packetBuffer - wiadomość
        //CRC - czy używamy CRC czy zwykłego Xmodem
        //Metoda izoluje 128 bajtów wiadomości odcinając header, numer pakietu i dopełnienie.
        //--------------------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------------------
        //Dispose:
        //Metoda wywoływana po zakończeniu operacji Recieve aby zamknąć port, aby nie blokować go dla innych transmisji
        //--------------------------------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
