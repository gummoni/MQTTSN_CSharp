namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 切断要求
    /// </summary>
    public class Disconnect
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Disconnect Parse(MQTTSNPayload payload)
        {
            if (2 != payload.Length) return null;
            return new Disconnect()
            {
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)2;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.DISCONNECT);
            return context.Data;
        }

    }
}
