using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// 特定のトピックデータを全体に送信
    /// </summary>
    public class Publish
    {
        public MessageFlags Flags { get; private set; }

        /// <summary>
        /// 始めてメッセージを送信するかどうかを示す
        /// </summary>
        public bool DUP => Flags.DUP;
        //public QoS QoS => Flags.QoS;
        //public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        //public TopicId TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// 公開先トピックID
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// QoS1,2の場合のみ関連するメッセージID。それ以外は0x0000でコーディング
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// 公開するメッセージ
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Publish Parse(Payload payload)
        {
            if (7 >= payload.Length) return null;
            return new Publish()
            {
                Flags = new MessageFlags(payload.ReadByte()),
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadUShort(),
                Data = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット生成（クライアントによる送信）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client, Topic topic, ushort msgId)
        {
            var length = (ushort)(7 + topic.Data.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.PUBLISH);
            context.Write(client.Flags.Value);
            context.Write(topic.TopicId);
            context.Write(msgId);
            context.Write(topic.Data);
            return context.Data;
        }

        /// <summary>
        /// パケット生成(ゲートウェイによる通知)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, Topic topic, Node node, ushort msgId)
        {
            var length = (ushort)(7 + topic.Data.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.PUBLISH);
            context.Write(node.Flags.Value);
            context.Write(topic.TopicId);
            context.Write(msgId);
            context.Write(topic.Data);
            return context.Data;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var type = GetType();
            var props = type.GetProperties();
            var args = props.Select(_ => $"{_.Name}={_.GetValue(this)}");
            return $"{type.Name}({string.Join(", ", args)})";
        }
    }
}
