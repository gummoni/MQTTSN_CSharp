using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// WILLTOPICREQメッセージの応答
    /// </summary>
    public class WillTopic
    {
        public MessageFlags Flags { get; private set; }
        public bool DUP => Flags.DUP;
        public QoS QoS => Flags.QoS;
        public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        //public TopicId TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// Willトピック名
        /// </summary>
        public string Will_Topic { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopic Parse(Payload payload)
        {
            if (3 >= payload.Length) return null;
            return new WillTopic()
            {
                Flags = new MessageFlags(payload.ReadByte()),
                Will_Topic = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)3;
            var context = new Payload(length);
            context.Write((byte)MsgType.WILLTOPIC);
            context.Write(client.Flags.Value);
            context.Write(client.WillTopic);
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
