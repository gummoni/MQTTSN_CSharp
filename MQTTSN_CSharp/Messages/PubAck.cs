namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// PUBLISH応答確認メッセージ（QoS=1、２）
    /// </summary>
    public class PubAck
    {
        public ushort TopicId { get; private set; }
        public ushort MsgId { get; private set; }
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PubAck Parse(MQTTSNPayload payload)
        {
            if (6 != payload.Length) return null;
            return new PubAck()
            {
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
        public static byte[] Pack(MQTTSNGateway gateway, ushort topicId, ushort msgId, ReturnCode returnCode)
        {
            var length = (ushort)6;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.PUBACK);
            context.Write(topicId);
            context.Write(msgId);
            context.Write((byte)returnCode);
            return context.Data;
        }

    }
}
