using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MQTTSN.Messages;

namespace MQTTSN
{
    public class Gateway
    {
        readonly List<Topic> Topics = new List<Topic>();
        readonly List<Node> Nodes = new List<Node>();
        public byte GwId { get; set; }
        //public string GwAdd { get; set; }
        public ushort Duration { get; set; }
        readonly UdpClient client;
        readonly IPEndPoint localEP;

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="client"></param>
        public Gateway(UdpClient client, IPEndPoint localEP)
        {
            this.client = client;
            this.localEP = localEP;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        public void Execute()
        {
            var session = new Session(client);
            session.Recv();
            Parse(session);
        }

        /// <summary>
        /// ブロードキャスト
        /// </summary>
        /// <param name="bytes"></param>
        public void Broadcast(byte[] bytes)
        {
            client.Send(bytes, bytes.Length, localEP);
        }

        /// <summary>
        /// 定期的にADVERISEメッセージを送信（送信間隔はDurationタイマーによる）
        /// </summary>
        public void Interval()
        {
            Broadcast(Advertise.Pack(this));
        }

        /// <summary>
        /// 接続シーケンス
        /// </summary>
        /// <param name="connect"></param>
        public void ConnectionSequence(Session session)
        {
            var connect = Connect.Parse(session.Payload);
            ReturnCode returnCode = ReturnCode.Accepted;
            try
            {
                var node = Nodes.FirstOrDefault(_ => _.ClientId == connect.ClientId);
                if (null == node)
                {
                    node = new Node();
                    Nodes.Add(node);
                }
                node.Connect = connect;

                if (connect.Will)
                {
                    var payload1 = session.Request(WillTopicReq.Pack(this));
                    if (payload1.MsgType != MsgType.WILLTOPIC) throw new SequenceException(ReturnCode.Rejected_Congestion);
                    node.WillTopic = WillTopic.Parse(payload1);

                    var payload2 = session.Request(WillMsgReq.Pack(this));
                    if (payload2.MsgType != MsgType.WILLMSG) throw new SequenceException(ReturnCode.Rejected_Congestion);
                    node.WillMsg = WillMsg.Parse(payload2);
                    Nodes.Add(node);
                }
            }
            catch (SequenceException se)
            {
                returnCode = se.ReturnCode;
            }
            finally
            {
                session.Notify(ConnAck.Pack(this, returnCode));
            }
        }

        /// <summary>
        /// 登録シーケンス
        /// </summary>
        /// <param name="session"></param>
        public void RegisterSequence(Session session)
        {
            var register = Register.Parse(session.Payload);
            var msgId = register.MsgId;
            var topicId = register.TopicId;
            var topicName = register.TopicName;

            var topics = Topics.Where(_ => _.TopicId == topicId || _.TopicName == topicName).ToArray();
            var length = topics.Length;
            Topic topic = null;
            if (0 == length)
            {
                topic = register.CreateTopic();
                var maxTopicId = (0 == Topics.Count) ? 0 : Topics.Max(_ => _.TopicId);
                topic.TopicId = topicId = (ushort)(1 + maxTopicId);
                Topics.Add(topic);
            }
            else if (1 == length)
            {
                topic = topics[0];
            }
            var returnCode = (null == topic) ? ReturnCode.Rejexted_InvalidTopicID : ReturnCode.Accepted;

            session.Notify(RegAck.Pack(this, topicId, msgId, returnCode));
        }

        /// <summary>
        /// 購読シーケンス
        /// </summary>
        /// <param name="session"></param>
        public void SubscribeSequence(Session session)
        {
            var subscribe = Subscribe.Parse(session.Payload);
            switch (subscribe.QoS)
            {
                case QoS.Level0:
                    return;

                case QoS.Level1:
                    {
                        var topic = Topics.FirstOrDefault(_ => _.TopicId == subscribe.TopicId);
                        if (null == topic)
                        {
                            session.Notify(SubAck.Pack(this, subscribe.Flags, subscribe.TopicId, subscribe.MsgId, ReturnCode.Rejexted_InvalidTopicID));
                        }
                        else
                        {
                            session.Notify(SubAck.Pack(this, subscribe.Flags, subscribe.TopicId, subscribe.MsgId, ReturnCode.Accepted));
                        }
                    }
                    break;

                case QoS.Level2:
                    var topic2 = Topics.FirstOrDefault(_ => _.TopicId == subscribe.TopicId || _.TopicName == subscribe.TopicName);
                    if (null == topic2)
                    {
                        session.Notify(SubAck.Pack(this, subscribe.Flags, subscribe.TopicId, subscribe.MsgId, ReturnCode.Rejexted_InvalidTopicID));
                    }
                    else
                    {
                        session.Notify(SubAck.Pack(this, subscribe.Flags, topic2.TopicId, subscribe.MsgId, ReturnCode.Accepted));
                    }
                    break;
            }
        }

        /// <summary>
        /// 通知処理
        /// </summary>
        /// <param name="session"></param>
        public void PublishSequence(Session session)
        {
            var publish = Publish.Parse(session.Payload);
            var topic = Topics.FirstOrDefault(_ => _.TopicId == publish.TopicId);
            topic.Data = publish.Data;

            //TODO:DUPフラグの対応
            //TODO:購読しているユーザに通知
        }

        /// <summary>
        /// 受信解析
        /// </summary>
        /// <param name="bytes"></param>
        public void Parse(Session session)
        {
            switch (session.Payload.MsgType)
            {
                case MsgType.CONNECT:
                    ConnectionSequence(session);
                    break;

                case MsgType.DISCONNECT:
                    break;

                case MsgType.PINGRESP:
                    break;

                case MsgType.PUBLISH:
                    PublishSequence(session);
                    break;

                case MsgType.PUBREL:
                    break;

                case MsgType.REGISTER:
                    RegisterSequence(session);
                    break;

                case MsgType.SEARCHGW:
                    //SEARCHGWメッセージを受け取ったらGWINFOメッセージを送信する
                    //Send(GWInfo.Pack(this), payload.RemoteEP);
                    break;

                case MsgType.SUBSCRIBE:
                    SubscribeSequence(session);
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
}
