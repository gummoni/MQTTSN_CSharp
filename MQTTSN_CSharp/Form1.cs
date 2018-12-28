using MQTTSN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        readonly UdpClient client;
        readonly IPEndPoint localEP;
        readonly Gateway gateway;

        public Form1()
        {
            InitializeComponent();

            localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1883);
            client = new UdpClient(localEP);
            gateway = new Gateway(client, localEP);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            gateway.Execute();
            timer1.Enabled = true;
        }
    }
}
