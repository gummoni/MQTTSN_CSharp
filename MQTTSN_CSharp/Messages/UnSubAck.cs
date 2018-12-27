namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// Unsubscribeメッセージの応答
    /// </summary>
    public class UnSubAck
    {
        public ushort MsgId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static UnSubAck Parse(MQTTSNPayload payload)
        {
            if (4 != payload.Length) return null;
            return new UnSubAck()
            {
                MsgId = payload.ReadUShort(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway, ushort msgId)
        {
            var length = (ushort)4;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.UNSUBACK);
            context.Write(msgId);
            return context.Data;
        }

    }
}
