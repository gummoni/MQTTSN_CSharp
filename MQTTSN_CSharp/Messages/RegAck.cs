namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// メッセージの受信及び処理に対する確認として送信される
    /// </summary>
    public class RegAck
    {
        public ushort TopicId { get; private set; }
        public ushort MsgId { get; private set; }
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static RegAck Parse(MQTTSNPayload payload)
        {
            if (7 != payload.Length) return null;
            return new RegAck()
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
            var length = (ushort)7;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.REGACK);
            context.Write(topicId);
            context.Write(msgId);
            context.Write((byte)returnCode);
            return context.Data;
        }

    }
}
