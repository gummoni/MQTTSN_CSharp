using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// 退会する
    /// </summary>
    public class Unsubscribe
    {

        public MessageFlags Flags { get; private set; }
        //public bool DUP => Flags.DUP;
        //public QoS QoS => Flags.QoS;
        //public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        public TopicIdType TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// メッセージID
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// トピックID
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Unsubscribe Parse(Payload payload)
        {
            if (7 != payload.Length) return null;
            return new Unsubscribe()
            {
                Flags = new MessageFlags(payload.ReadByte()),
                MsgId = payload.ReadUShort(),
                TopicId = payload.ReadUShort(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client, ushort msgId, ushort topicId)
        {
            var length = (ushort)7;
            var context = new Payload(length);
            context.Write((byte)MsgType.UNSUBSCRIBE);
            context.Write(client.Flags.Value);
            context.Write(msgId);
            context.Write(topicId);
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
