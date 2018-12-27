namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// 生存確認
    /// </summary>
    public class PingReq
    {
        public string ClientId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PingReq Parse(MQTTSNPayload payload)
        {
            if (2 > payload.Length) return null;
            return new PingReq()
            {
                ClientId = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway, string clientId = "")
        {
            var length = (ushort)(2 + clientId.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.PINGREQ);
            context.Write(clientId);
            return context.Data;
        }
    }
}
