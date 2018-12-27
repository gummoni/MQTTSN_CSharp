namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// Willメッセージ送信要求するためにGWによって送信される
    /// </summary>
    public class WillMsgReq
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillMsgReq Parse(MQTTSNPayload payload)
        {
            if (2 != payload.Length) return null;
            return new WillMsgReq();
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
            context.Write((byte)MsgType.WILLMSGREQ);
            return context.Data;
        }

    }
}
