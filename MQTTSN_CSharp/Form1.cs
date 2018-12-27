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
        UdpClient client;
        IPEndPoint localEP;

        public Form1()
        {
            InitializeComponent();

            localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1883);
            client = new UdpClient(localEP);
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
            IPEndPoint remoteEP = null;
            var bytes = client.Receive(ref remoteEP);
            if (0 < bytes?.Length)
            {
                Console.WriteLine($"IP={remoteEP.Address}, port={remoteEP.Port}");
                Console.WriteLine("---");
                foreach (var by in bytes)
                {
                    Console.Write(by.ToString("X2") + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("---");
                var parser = new MQTTParser(bytes);
            }

            timer1.Enabled = true;
        }
    }

    //http://mqtt.org/new/wp-content/uploads/2009/06/MQTT-SN_spec_v1.2.pdf


    //5.2.1
    public class MQTTSNPacket
    {
        public MsgType MessageType { get; set; }    // 1[byte]
        public byte[] Body { get; set; }            // n[byte]

        readonly MQTTSNPayload context;

        public MQTTSNPacket(byte[] data)
        {
            context = new MQTTSNPayload(data);
        }
    }


    public class Body
    {
        //5.3.1  ClientId    // 1-23charactor
        //5.3.2  Data        // MQTT_PUBLISH message
        //5.3.3  Duration    // 2byte long (max 18hour)
        //5.3.4  Flags       // 
        //5.3.5  GwAdd
        //5.3.6  GwId
        //5.3.7  MsgId
        //5.3.8  ProtcolId
        //5.3.9  Radius
        //5.3.10 ReturnCode
        //5.3.11 TopicId
        //5.3.12 TopicName
        //5.3.13 WillMsg
        //5.3.14 WillTopic

    }

}
