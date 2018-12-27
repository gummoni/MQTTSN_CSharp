using System.Collections.Generic;
using WindowsFormsApp1.Messages;

namespace WindowsFormsApp1
{
    public class Topic
    {
        public ushort TopicId { get; set; }
        public string TopicName { get; set; }
        public string Data { get; set; }
        public MQTTSNMessageFlags Flags { get; set; }
    }

    public class ActiveGateway
    {
        public ushort Duration { get; set; }
        public byte GwId { get; set; }
    }

    public enum MQTTSNState
    {
        Offline,
        Online,
    }

    //6.1 Gateway Advertisement and Discovery
    //6.2 Client’s Connection Setup
    //6.3 Clean session
    //6.4 Procedure for updating the Will data
    //6.5 Topic Name Registration Procedure
    //6.6 Client’s Publish Procedure
    //6.7 Pre-defined topic ids and short topic names
    //6.8 PUBLISH with QoS Level -1
    //6.9 Client’s Topic Subscribe/Un-subscribe Procedure
    //6.10 Gateway’s Publish Procedure
    //6.11 Keep Alive and PING Procedure
    //6.12 Client’s Disconnect Procedure
    //6.13 Client’s Retransmission Procedure
    //6.14 Support of sleeping clients

    public class MQTTSNGateway
    {
        readonly List<Topic> Topics = new List<Topic>();
        public byte GwId { get; set; }
        public string GwAdd { get; set; }
        public ushort Duration { get; set; }

        public void Send(byte[] data)
        {
        }

        /// <summary>
        /// 定期的にADVERISEメッセージを送信（送信間隔はDurationタイマーによる）
        /// </summary>
        public void IntervalCheck()
        {
            Send(Advertise.Pack(this));
            //・切断検知したらWill処理を行う。
        }

        /// <summary>
        /// 受信解析
        /// </summary>
        /// <param name="bytes"></param>
        public void Parse(byte[] bytes)
        {
            var payload = new MQTTSNPayload(bytes);
            switch (payload.MsgType)
            {
                case MsgType.CONNECT:
                    Send(ConnAck.Pack(this, ReturnCode.Accepted));
                    //***CLIENT***
                    // CONNECT(Flags, ProtcolID, KeepAlive, ClientId)送信
                    // WILLTOPICREQ受信
                    // WILLTOPIC(Flags, WillTipic)送信
                    // WILLMSGREQ受信
                    // WILLMSG(WillMessage)送信
                    // CONNACK(Return code)
                    break;

                case MsgType.DISCONNECT:
                    break;

                case MsgType.PINGRESP:
                    break;

                case MsgType.PUBLISH:
                    break;

                case MsgType.PUBREL:
                    break;

                case MsgType.REGISTER:
                    break;

                case MsgType.SEARCHGW:
                    //SEARCHGWメッセージを受け取ったらGWINFOメッセージを送信する
                    Send(GWInfo.Pack(this));
                    break;

                case MsgType.SUBSCRIBE:
                    break;

                case MsgType.UNSUBSCRIBE:
                    break;

                case MsgType.WILLMSG:
                    break;

                case MsgType.WILLMSGUPD:
                    break;

                case MsgType.WILLTOPIC:
                    break;

                case MsgType.WILLTOPICUPD:
                    break;

                case MsgType.EncapsulatedMessage:
                    break;
            }
        }
    }

    public class MQTTSNClient
    {
        readonly List<Topic> Topics = new List<Topic>();
        public byte Radius { get; set; }
        public MQTTSNMessageFlags Flags { get; set; }
        public ushort Duration { get; set; }
        public string ClientId { get; set; }
        public string WillTopic { get; set; }
        public string WillMsg { get; set; }
        public MQTTSNState State { get; set; } = MQTTSNState.Offline;

        public void Send(byte[] data)
        {
        }

        public void Interval()
        {
            //ホスト接続チェック。
            //もし生存タイマーがタイムアウトしたら切断した認識する。
        }

        /// <summary>
        /// 受信解析
        /// </summary>
        /// <param name="bytes"></param>
        public void Parse(byte[] bytes)
        {
            var payload = new MQTTSNPayload(bytes);

            switch (payload.MsgType)
            {
                case MsgType.ADVERTISE:
                    Send(GWInfo.Pack(null));    //?
                    break;

                case MsgType.GWINFO:
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
