namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 退会する
    /// </summary>
    public class Unsubscribe
    {
        public ushort MsgId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Unsubscribe Parse(MQTTSNPayload payload)
        {
            if (7 != payload.Length) return null;
            return new Unsubscribe()
            {
                MsgId = payload.ReadUShort(),
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
            context.Write((byte)MsgType.UNSUBSCRIBE);
            context.Write(client.Flags.Value);
            context.Write(msgId);
            context.Write(topicId);
            return context.Data;
        }

    }
}
