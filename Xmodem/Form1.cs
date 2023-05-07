using System;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace Xmodem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PortSelectionBox.DataSource = SerialPort.GetPortNames();
        }
        bool CRCbool;
        byte[] data = null;
        string pathToSave = String.Empty;

        private void SendXmodemButton_Click(object sender, EventArgs e)
        {
            if (PortSelectionBox.Text != String.Empty && data != null)
            {
                Sender XmodemSender = new Sender(PortSelectionBox.Text);
                try
                {
                    XmodemSender.SendData(data, CRCbool);
                    Comms.Text = "udało się wysłać plik";
                    XmodemSender.Dispose();
                }
                catch (Exception ex)
                {
                    Comms.Text = "nie udało się odebrać pliku";
                    XmodemSender.Dispose();
                }
            }
            else
            {
                Comms.Text = "Należy najpierw wybrać port i plik do wysłania";
            }
        }

        private void RecieveXmodemButton_Click(object sender, EventArgs e)
        {
            if (PortSelectionBox.Text != String.Empty && pathToSave != String.Empty)
            {
                Reciever XmodemReciever = new Reciever(PortSelectionBox.Text);
                try 
                {
                    data = XmodemReciever.Recieve(CRCbool);
                    File.WriteAllBytes(pathToSave, data);
                    Comms.Text = "udało się odebrać plik";
                    XmodemReciever.Dispose();
                }
                catch (Exception ex)
                {
                    XmodemReciever.Dispose();
                    Comms.Text = "nie udało się odebrać pliku";
                }
            }
            else
            {
                Comms.Text = "Należy najpierw wybrać port i plik do którego zostanie zapisana zawartość";
            }
        }

        private void FileToSendButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                data = File.ReadAllBytes(selectedFileName);
                Comms.Text = "Wybrano plik do wysłania, zawartość została zaczytana";
            }
            else
            {
                Comms.Text = "Przerwano okno dialogu wyboru pliku!!!";
            }
        }

        private void PathToSaveBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                pathToSave = selectedFileName;
                Comms.Text = "Wybrano plik do zapisania, można rozpoczynać transfer";
            }
            else
            {
                Comms.Text = "Przerwano okno dialogu wyboru pliku!!!";
            }
        }

        private void CRCUsage_CheckedChanged(object sender, EventArgs e)
        {
            CRCbool = !CRCbool;
            Comms.Text = "Zmieniono użycie CRC";
        }
    }
}
