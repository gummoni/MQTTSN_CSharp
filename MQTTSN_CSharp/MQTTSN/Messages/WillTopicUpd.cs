using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// Willトピックアップロード
    /// </summary>
    public class WillTopicUpd
    {
        public MessageFlags Flags { get; private set; }
        //public bool DUP => Flags.DUP;
        public QoS QoS => Flags.QoS;
        public bool Retain => Flags.Retain;
        //public bool Will => Flags.Will;
        //public bool CleanSession => Flags.CleanSession;
        //public TopicIdType TopicIdType => Flags.TopicIdType;

        public string WillTopic { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicUpd Parse(Payload payload)
        {
            if (3 >= payload.Length) return null;
            return new WillTopicUpd()
            {
                Flags = new MessageFlags(payload.ReadByte()),
                WillTopic = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)(3 + client.WillTopic.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.WILLTOPICUPD);
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
