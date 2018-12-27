namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// Willメッセージアップロード
    /// </summary>
    public class WillMsgUpd
    {
        public string WillMsg { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillMsgUpd Parse(MQTTSNPayload payload)
        {
            if (2 >= payload.Length) return null;
            return new WillMsgUpd()
            {
                WillMsg = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)(2 + client.WillMsg.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLMSGUPD);
            context.Write(client.WillMsg);
            return context.Data;
        }

    }
}
