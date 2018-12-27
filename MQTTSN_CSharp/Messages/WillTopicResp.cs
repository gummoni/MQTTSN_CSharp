namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// WILLTOPICUPD応答メッセージ
    /// </summary>
    public class WillTopicResp
    {
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicResp Parse(MQTTSNPayload payload)
        {
            if (3 != payload.Length) return null;
            return new WillTopicResp()
            {
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway, ReturnCode returnCode)
        {
            var length = (ushort)3;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLTOPICRESP);
            context.Write((byte)returnCode);
            return context.Data;
        }
    }
}
