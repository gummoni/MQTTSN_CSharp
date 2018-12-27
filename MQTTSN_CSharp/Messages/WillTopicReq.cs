namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// Willトピック名の送信をクライアントに要求する
    /// </summary>
    public class WillTopicReq
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicReq Parse(MQTTSNPayload payload)
        {
            if (2 != payload.Length) return null;
            return new WillTopicReq();
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway)
        {
            var length = (ushort)2;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLTOPICREQ);
            return context.Data;
        }
    }
}
