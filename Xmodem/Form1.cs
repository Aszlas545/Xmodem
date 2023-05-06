using GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xmodem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Utils utils = new Utils();

        private void SendXmodemButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine(utils.checkMessage(0x01));
            Console.WriteLine(utils.checkMessage(0x04));
            Console.WriteLine(utils.checkMessage(0x06));
            Console.WriteLine(utils.checkMessage(0x15));
            Console.WriteLine(utils.checkMessage(0x18));
            Console.WriteLine(utils.checkMessage(0x43));
            Console.WriteLine(utils.checkMessage(0x35));
        }

        private void RecieveXmodemButton_Click(object sender, EventArgs e)
        {

        }

        private void FileToSendButton_Click(object sender, EventArgs e)
        {

        }

        private void PathToSaveBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PortSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
