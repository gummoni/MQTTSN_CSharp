namespace WindowsFormsApp1.Messages
{
    public class Connect
    {
        const byte ProtcolID = 0x00;

        public MQTTSNMessageFlags Flags { get; private set; }
        public byte ProtcolId { get; private set; }
        public ushort Duration { get; private set; }
        public string ClientId { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Connect Parse(MQTTSNPayload payload)
        {
            if (6 >= payload.Length) return null;
            return new Connect()
            {
                 Flags = new MQTTSNMessageFlags(payload.ReadByte()),
                 ProtcolId = payload.ReadByte(),
                 Duration = payload.ReadUShort(),
                 ClientId = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <returns></returns>
        public static byte[] Pack(MQTTSNClient client)
        {
            var length = (ushort)(6 + client.ClientId.Length);
            var context = new MQTTSNPayload(length);
            context.Write((byte)MsgType.CONNECT);
            context.Write(client.Flags.Value);
            context.Write(ProtcolID);
            context.Write(client.Duration);
            context.Write(client.ClientId);
            return context.Data;
        }
    }
}
