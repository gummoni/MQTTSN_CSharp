namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// WILLTOPICUPD応答メッセージ
    /// </summary>
    public class ForwarderEncapsulation
    {
        public byte Ctrl { get; set; }
        public string WirelessNodeId { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static ForwarderEncapsulation Parse(MQTTSNPayload payload)
        {
            if (3 >= payload.Length) return null;
            return new ForwarderEncapsulation()
            {
                Ctrl = payload.ReadByte(),
                WirelessNodeId = "",
                Message = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client, byte ctrl, string wirelessNodeId, string message)
        {
            var length = (ushort)(3 + wirelessNodeId.Length + message.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.EncapsulatedMessage);
            context.Write(ctrl);
            context.Write(wirelessNodeId);
            context.Write(message);
            return context.Data;
        }
    }
}
