namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// WILLTOPICREQメッセージの応答
    /// </summary>
    public class WillTopic
    {
        public MQTTSNMessageFlags Flags { get; private set; }
        public string Will_Topic { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopic Parse(MQTTSNPayload payload)
        {
            if (3 >= payload.Length) return null;
            return new WillTopic()
            {
                Flags = new MQTTSNMessageFlags(payload.ReadByte()),
                Will_Topic = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)3;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.WILLTOPIC);
            context.Write(client.Flags.Value);    //QoS & Retain
            context.Write(client.WillTopic);
            return context.Data;
        }
    }
}
