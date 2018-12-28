using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// 特定のトピック購読する
    /// </summary>
    public class Subscribe
    {
        public MessageFlags Flags { get; private set; }
        public bool DUP => Flags.DUP;
        public QoS QoS => Flags.QoS;
        //public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        public TopicIdType TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// 対応するSUBACKメッセージを識別するために使用するユニークなID
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// トピック名、トピックID、短いトピック名を含みます。
        /// </summary>
        public ushort TopicId { get; private set; }

        public string TopicName { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Subscribe Parse(Payload payload)
        {
            if (7 >= payload.Length) return null;
            var result = new Subscribe();
            result.Flags = new MessageFlags(payload.ReadByte());
            result.MsgId = payload.ReadUShort();
            switch (result.Flags.TopicIdType)
            {
                case TopicIdType.Normal:
                    result.TopicName = payload.ReadString();
                    break;

                case TopicIdType.Predefined:
                    result.TopicId = payload.ReadUShort();
                    break;

                case TopicIdType.Short:
                    result.TopicId = payload.ReadUShort();
                    break;
            }
            return result;
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
            context.Write((byte)MsgType.SUBSCRIBE);
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
