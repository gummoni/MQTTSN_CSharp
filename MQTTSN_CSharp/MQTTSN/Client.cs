using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MQTTSN.Messages;

namespace MQTTSN
{
    public class Client
    {
        readonly List<Topic> Topics = new List<Topic>();
        public byte Radius { get; set; }
        public MessageFlags Flags { get; set; }
        public ushort Duration { get; set; }
        public string ClientId { get; set; }
        public string WillTopic { get; set; }
        public string WillMsg { get; set; }
        public CommunicationState State { get; set; } = CommunicationState.Offline;
        readonly UdpClient client;
        readonly IPEndPoint localEP;

        /// <summary>
        /// ブロードキャスト
        /// </summary>
        /// <param name="bytes"></param>
        public void Broadcast(byte[] bytes)
        {
            client.Send(bytes, bytes.Length, localEP);
        }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="localEP"></param>
        public Client(UdpClient client, IPEndPoint localEP)
        {
            this.client = client;
            this.localEP = localEP;
        }


        public void RestartInvervalTimer()
        {
        }

        public void Interval()
        {
            if (State == CommunicationState.Offline)
            {
                //定期的にホスト接続要求
                Broadcast(SearchGW.Pack(this));
            }
            else
            {
                //切断検知
                State = CommunicationState.Offline;
            }
        }

        /// <summary>
        /// GW検索
        /// </summary>
        public void SearchGateway()
        {
        }

        /// <summary>
        /// 接続要求
        /// </summary>
        public void Connect()
        {
        }

        /// <summary>
        /// 切断要求
        /// </summary>
        public void Disconnect()
        {
        }

        /// <summary>
        /// 登録要求
        /// </summary>
        public void Register()
        {
        }

        /// <summary>
        /// 購読要求
        /// </summary>
        public void Subscribe()
        {
        }

        /// <summary>
        /// 解除要求
        /// </summary>
        public void Ubsubscribe()
        {
        }

        /// <summary>
        /// 受信解析
        /// </summary>
        /// <param name="bytes"></param>
        public void Parse(Payload payload)
        {
            switch (payload.MsgType)
            {
                case MsgType.ADVERTISE:
                    //State = MQTTSNState.Online;
                    RestartInvervalTimer();
                    break;

                case MsgType.GWINFO:
                    State = CommunicationState.Online;
                    RestartInvervalTimer();
                    break;

                case MsgType.CONNACK:
                    break;

                case MsgType.WILLTOPICREQ:
                    break;

                case MsgType.WILLMSGREQ:
                    break;

                case MsgType.REGACK:
                    break;

                case MsgType.PUBACK:
                    break;

                case MsgType.PUBCOMP:
                    break;

                case MsgType.PUBREC:
                    break;

                case MsgType.SUBACK:
                    break;

                case MsgType.UNSUBACK:
                    break;

                case MsgType.PINGREQ:
                    break;

                case MsgType.WILLTOPICRESP:
                    break;

                case MsgType.WILLMSGRESP:
                    break;

                case MsgType.EncapsulatedMessage:
                    break;
            }
        }
    }
}
