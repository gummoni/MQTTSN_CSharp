namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// SEARCHGWメッセージは、クライアントがGW検索する時にブロードキャストされます。
    /// ブロードキャストする範囲は制限します。
    /// </summary>
    public class SearchGW
    {
        public byte Radius { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SearchGW Parse(MQTTSNPayload context)
        {
            if (3 != context.Data.Length) return null;
            return new SearchGW()
            {
                Radius = context.ReadByte()
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)3;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.SEARCHGW);
            context.Write(client.Radius);
            return context.Data;
        }
    }
}
