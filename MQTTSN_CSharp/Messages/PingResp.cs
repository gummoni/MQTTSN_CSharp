namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 生存確認の応答
    /// </summary>
    public class PingResp
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PingResp Parse(MQTTSNPayload payload)
        {
            if (2 != payload.Length) return null;
            return new PingResp()
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
            context.Write((byte)MsgType.PINGRESP);
            return context.Data;
        }

    }
}
