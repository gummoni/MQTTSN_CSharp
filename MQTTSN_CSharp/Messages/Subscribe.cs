namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 特定のトピック購読する
    /// </summary>
    public class Subscribe
    {
        public MQTTSNMessageFlags Flags { get; private set; }
        public ushort MsgId { get; private set; }
        public ushort TopicId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Subscribe Parse(MQTTSNPayload payload)
        {
            if (7 != payload.Length) return null;
            return new Subscribe()
            {
                Flags = new MQTTSNMessageFlags(payload.ReadByte()),
                MsgId = payload.ReadUShort(),
                TopicId = payload.ReadUShort(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client, ushort msgId, ushort topicId)
        {
            var length = (ushort)7;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.SUBSCRIBE);
            context.Write(client.Flags.Value);
            context.Write(msgId);
            context.Write(topicId);
            return context.Data;
        }
    }
}
