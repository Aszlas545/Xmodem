using System.IO.Ports;

namespace Xmodem
{
    internal static class PortExtension
    {
        public static void WriteByte(this SerialPort port, byte data)
        {
            var buffer = new byte[1];
            buffer[0] = data;
            port.Write(buffer, 0, 1);
        }
    }
}
