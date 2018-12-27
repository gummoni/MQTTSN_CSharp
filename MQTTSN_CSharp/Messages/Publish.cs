namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 特定のトピックデータを全体に送信
    /// </summary>
    public class Publish
    {
        public MQTTSNMessageFlags Flags { get; private set; }
        public ushort TopicId { get; private set; }
        public ushort MsgId { get; private set; }
        public string Data { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Publish Parse(MQTTSNPayload payload)
        {
            if (7 >= payload.Length) return null;
            return new Publish()
            {
                Flags = new MQTTSNMessageFlags(payload.ReadByte()),
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadUShort(),
                Data = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client, Topic topic, ushort msgId)
        {
            var length = (ushort)(7 + topic.Data.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.PUBLISH);
            context.Write(client.Flags.Value);
            context.Write(topic.TopicId);
            context.Write(msgId);
            context.Write(topic.Data);
            return context.Data;
        }
    }
}
