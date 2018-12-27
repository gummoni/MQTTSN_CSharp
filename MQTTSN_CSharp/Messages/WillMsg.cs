namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// WillMsgReqの応答
    /// </summary>
    public class WillMsg
    {
        public string Will_Msg { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillMsg Parse(MQTTSNPayload payload)
        {
            if (2 >= payload.Length) return null;
            return new WillMsg()
            {
                Will_Msg = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)(2 + client.WillMsg.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLMSG);
            context.Write(client.WillMsg);
            return context.Data;
        }
    }
}
