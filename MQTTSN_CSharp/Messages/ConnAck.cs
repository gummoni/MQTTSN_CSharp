namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// クライアントからの接続要求に応じて返信します。
    /// </summary>
    public class ConnAck
    {
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static ConnAck Parse(MQTTSNPayload payload)
        {
            if (3 != payload.Length) return null;
            return new ConnAck()
            {
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway, ReturnCode code)
        {
            var length = (ushort)3;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.CONNACK);
            context.Write((byte)code);
            return context.Data;
        }

    }
}
