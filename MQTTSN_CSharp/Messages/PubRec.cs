namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// PUBLISH応答確認メッセージ（QoS=2）
    /// </summary>
    public class PubRec
    {
        public ushort MsgId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PubRec Parse(MQTTSNPayload payload)
        {
            if (4 != payload.Length) return null;
            return new PubRec()
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
            context.Write((byte)MsgType.PUBREC);
            context.Write(msgId);
            return context.Data;
        }

    }
}
