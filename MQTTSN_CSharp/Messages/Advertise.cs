namespace WindowsFormsApp1.Messages
{
    /// <summary>
    /// ADVERTISEメッセージは、その存在を知らせるためにゲートウェイによって定期的にブロードキャストされます。 
    /// 次のブロードキャスト時刻までの間隔は、このメッセージのDurationフィールドに示されています。 
    /// </summary>
    public class Advertise
    {
        public byte GwId { get; private set; }
        public ushort Duration { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Advertise Parse(MQTTSNPayload payload)
        {
            if (5 != payload.Data.Length) return null;
            return new Advertise()
            {
                GwId = payload.ReadByte(),
                Duration = payload.ReadUShort(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNGateway gateway)
        {
            var length = (ushort)5;
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.ADVERTISE);
            context.Write(gateway.GwId);
            context.Write(gateway.Duration);
            return context.Data;
        }
    }
}
