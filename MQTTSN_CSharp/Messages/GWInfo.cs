namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// GWINFOメッセージは、SEARCHGWメッセージの応答メッセージになります。
    /// 送信元のGWIDのみ返します。
    /// </summary>
    /// </summary>
    public class GWInfo
    {
        public byte GwId { get; private set; }
        public string GwAdd { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static GWInfo Parse(MQTTSNPayload payload)
        {
            if (3 >= payload.Length) return null;
            return new GWInfo()
            {
                GwId = payload.ReadByte(),
                GwAdd = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway)
        {
            var length = (ushort)(3 + gateway.GwAdd.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.GWINFO);
            context.Write(gateway.GwId);
            context.Write(gateway.GwAdd);
            return context.Data;
        }

    }
}
