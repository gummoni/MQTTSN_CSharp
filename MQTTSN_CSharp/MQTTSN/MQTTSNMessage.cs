using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MQTTSN.Messages;

namespace MQTTSN
{
    //5.3.1  ClientId    // 1-23charactor
    //5.3.2  Data        // MQTT_PUBLISH message
    //5.3.3  Duration    // 2byte long (max 18hour) 単位:sec
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


    //6.1 Gateway Advertisement and Discovery
    //　★ケース１　ADVERTISEメッセージシーケンス
    //　　１－１．Gateway[ADVERTISE]をブロードキャスト
    //　　１－２．Client[GWINFO]を返信。（Durationタイマーセット、Offline→Onlineへ）
    //
    //　★ケース２　定周期送信(Duration)
    //　  [Gateway]
    //　　  ２－１．Durationタイムアウト後、Gatewayは定期的にケース１を実行。
    //　　  ２－２．Client側は受信したらDurationタイマーリセット。(他のメッセージも含む)
    //　  [Client]
    //　　  ２－３．Client側はDurationタイムアウト後、Gatewayと切断したと判断。（Online→Offlineへ）
    //
    //  ★ケース３．SEARCHGWメッセージシーケンス
    //　　３－１．Client[SEARCHGW]送信。
    //　　３－２．Gateway[GWINFO]を返信。（Durationタイマーセット、Offline→Onlineへ）

    //6.2 Client’s Connection Setup
    // Client[CONNECT]送信
    // Gateway[WILLTOPICRE]送信
    // Client[WILLTOPIC]送信
    // Gateway[WILLMSGREQ]送信
    // Client[WILLMSG]送信
    // Gateway[CONNACK]送信

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


    public class Node
    {
        public Connect Connect { get; set; }
        public WillTopic WillTopic { get; set; }
        public WillMsg WillMsg { get; set; }
        public MessageFlags Flags { get; set; }

        public string ClientId => Connect.ClientId;
    }

    public class Topic
    {
        //TODO:TopicIDはユニークにつける
        public ushort TopicId { get; set; }
        public string TopicName { get; set; }
        public string Data { get; set; }
        public List<Node> Subscribes = new List<Node>();
    }

    public class ActiveGateway
    {
        public ushort Duration { get; set; }
        public byte GwId { get; set; }
    }

    public enum CommunicationState
    {
        Offline,
        Online,
    }

}
