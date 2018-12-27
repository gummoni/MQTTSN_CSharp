namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// Willトピックアップロード
    /// </summary>
    public class WillTopicUpd
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicUpd Parse(MQTTSNPayload payload)
        {
            if (3 >= payload.Length) return null;
            return new WillTopicUpd()
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
            var length = (ushort)(3 + client.WillTopic.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLTOPICUPD);
            context.Write(client.Flags.Value);
            context.Write(client.WillTopic);
            return context.Data;
        }

    }
}
