namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// SUBSCRIBEメッセージの応答
    /// </summary>
    public class SubAck
    {
        public MQTTSNMessageFlags Flags { get; private set; }
        public ushort TopicId { get; private set; }
        public ushort MsgId { get; private set; }
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static SubAck Parse(MQTTSNPayload payload)
        {
            if (8 != payload.Length) return null;
            return new SubAck()
            {
                Flags = new MQTTSNMessageFlags(payload.ReadByte()),
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
        public static byte[] Pack(MQTTSNGateway gateway, Topic topic, ushort msgId, ReturnCode returnCode)
        {
            var length = (ushort)8;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.SUBACK);
            context.Write(topic.Flags.Value);
            context.Write(topic.TopicId);
            context.Write(msgId);
            context.Write((byte)returnCode);
            return context.Data;
        }

    }
}
