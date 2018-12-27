namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// クライアントがGWへ送信。（トピック名のトピックID値を取得するため）
    /// </summary>
    public class Register
    {
        public ushort TopicId { get; private set; }
        public ushort MsgId { get; private set; }
        public string TopicName { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Register Parse(MQTTSNPayload payload)
        {
            if (6 >= payload.Length) return null;
            return new Register()
            {
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadByte(),
                TopicName = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client, ushort msgId, ushort topicId, string topicName)
        {
            var length = (ushort)(6 + topicName.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.REGISTER);
            context.Write(topicId);
            context.Write(msgId);
            context.Write(topicName);
            return context.Data;
        }
    }
}
