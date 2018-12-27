using System.Collections.Generic;


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

    public class MQTTSNGateway
    {
        readonly List<Topic> Topics = new List<Topic>();
        public byte GwId { get; set; }
        public string GwAdd { get; set; }
        public ushort Duration { get; set; }
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
    }

    public class MQTTSNMessage
    {
        ///// <summary>
        ///// フォワーダを返してメッセージ送信
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="ctrl"></param>
        ///// <param name="wirelessNodeId"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static byte[] ForwarderEncapsulation(MQTTSNClient client, byte ctrl, string wirelessNodeId, string message)
        //{
        //    var length = (ushort)(3 + wirelessNodeId.Length + message.Length);
        //    var context = new MQTTSNParseContext(length);
        //    context.Write((byte)MsgType.EncapsulatedMessage);
        //    context.Write(ctrl);
        //    context.Write(wirelessNodeId);
        //    context.Write(message);
        //    return context.Data;
        //}

        //6.1 Gateway Advertisement and Discovery
        //int ConnectionList;

        public void GatewayAdvertisement()
        {
            //***GATEWAT***
            //・定期的にADVERISEメッセージを送信（送信間隔はDurationタイマーによる）
            //・切断検知したらWill処理を行う。
            //・SEARCHGWメッセージを受け取ったらGWINFOメッセージを送信する
        }

        public void ClientAdvertisement()
        {
            //***CLIENT***
            //・ADVERISEメッセージを受信したらGWINFOメッセージ？を送信する
            //・受信したAdvertisementはリストに登録して、生存タイマーをセットする。
            //・もし生存タイマーがタイムアウトしたら切断した認識する。
            //・切断したらすべての機能を停止して、SEARCHGWメッセージを送信して、接続確立を待つ。
        }

        public void ClientConnection()
        {
            //***CLIENT***
            // CONNECT(Flags, ProtcolID, KeepAlive, ClientId)送信
            // WILLTOPICREQ受信
            // WILLTOPIC(Flags, WillTipic)送信
            // WILLMSGREQ受信
            // WILLMSG(WillMessage)送信
            // CONNACK(Return code)

            //CleanSession=true, Will=true ... GWはすべての購読を削除し、クライアントに関するWillデータを削除します。そして、新しいWillトピックとWillメッセージの入力を促し始めます。

        }

        //6.1 Gateway Advertisement and Discovery
        //6.2 Client’s Connection Setup
        //6.3 Clean session        //6.4 Procedure for updating the Will data
        //6.5 Topic Name Registration Procedure
        //6.6 Client’s Publish Procedure
        //6.7 Pre-defined topic ids and short topic names
        //6.8 PUBLISH with QoS Level -1
        //6.9 Client’s Topic Subscribe/Un-subscribe Procedure
        //6.10 Gateway’s Publish Procedure
        //6.11 Keep Alive and PING Procedure        //6.12 Client’s Disconnect Procedure        //6.13 Client’s Retransmission Procedure
        //6.14 Support of sleeping clients
    }
}
