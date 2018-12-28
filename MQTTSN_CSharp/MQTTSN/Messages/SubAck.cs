using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// SUBSCRIBEメッセージの応答
    /// </summary>
    public class SubAck
    {
        public MessageFlags Flags { get; private set; }
        //public bool DUP => Flags.DUP;
        public QoS QoS => Flags.QoS;
        //public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        //public TopicIdType TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// 購読するトピックID
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// SUBSCRIBEメッセージと同じメッセージID
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// リターンコード
        /// </summary>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static SubAck Parse(Payload payload)
        {
            if (8 != payload.Length) return null;
            return new SubAck()
            {
                Flags = new MessageFlags(payload.ReadByte()),
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadUShort(),
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, MessageFlags flags, ushort topicId, ushort msgId, ReturnCode returnCode)
        {
            var length = (ushort)8;
            var context = new Payload(length);
            context.Write((byte)MsgType.SUBACK);
            context.Write(flags.Value);
            context.Write(topicId);
            context.Write(msgId);
            context.Write((byte)returnCode);
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
