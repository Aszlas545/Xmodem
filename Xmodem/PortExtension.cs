using System.IO.Ports;

namespace Xmodem
{
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    //Klasa: PortExtension rozszerzenie klasy portu szeregowego dodając metoda piszącą jeden bajt przydatną do wysyłania wszystkich możliwych bajtów poza SOH
    //--------------------------------------------------------------------------------------------------------------------------------------------------------
    internal static class PortExtension
    {
        //--------------------------------------------------------------------------------------------------------------------------------
        //WriteByte:
        //Metoda pisząca jeden bajt do danego portu
        //--------------------------------------------------------------------------------------------------------------------------------
        public static void WriteByte(this SerialPort port, byte data)
        {
            var buffer = new byte[1];
            buffer[0] = data;
            port.Write(buffer, 0, 1);
        }
    }
}
