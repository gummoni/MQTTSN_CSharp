using System;
using System.Net;
using System.Net.Sockets;

namespace MQTTSN
{
    /// <summary>
    /// セッション
    /// </summary>
    public class Session
    {
        IPEndPoint remoteEP;
        readonly UdpClient client;
        public Payload Payload { get; private set; }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="client"></param>
        public Session(UdpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// 受信処理
        /// </summary>
        public Payload Recv()
        {
            Payload = new Payload(client.Receive(ref remoteEP));
            Console.WriteLine("---");
            foreach (var by in Payload.Data)
            {
                Console.Write(by.ToString("X2") + " ");
            }
            Console.WriteLine();
            Console.WriteLine("---");
            return Payload;
        }

        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="bytes"></param>
        public void Notify(byte[] bytes)
        {
            client.Send(bytes, bytes.Length, remoteEP);
        }

        /// <summary>
        /// 送受信処理
        /// </summary>
        /// <param name="bytes"></param>
        public Payload Request(byte[] bytes)
        {
            Notify(bytes);
            return Recv();
        }
    }
}
