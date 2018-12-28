using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// CONNECTメッセージは、接続確立するためクライアントから送信されます。
    /// </summary>
    public class Connect
    {
        const byte ProtcolID = 0x00;

        /// <summary>
        /// フラグ
        /// </summary>
        public MessageFlags Flags { get; private set; }
        //public bool DUP => Flags.DUP;
        //public QoS QoS => Flags.QoS;
        //public bool Retain => Flags.Retain;
        public bool Will => Flags.Will;
        public bool CleanSession => Flags.CleanSession;
        //public TopicId TopicIdType => Flags.TopicIdType;

        /// <summary>
        /// プロトコル名およびプロトコルバージョンに対応
        /// </summary>
        public byte ProtcolId { get; private set; }

        /// <summary>
        /// キープアライブタイマー値
        /// </summary>
        public ushort Duration { get; private set; }

        /// <summary>
        /// クライアントID（１～２３文字）
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Connect Parse(Payload payload)
        {
            if (6 >= payload.Length) return null;
            return new Connect()
            {
                 Flags = new MessageFlags(payload.ReadByte()),
                 ProtcolId = payload.ReadByte(),
                 Duration = payload.ReadUShort(),
                 ClientId = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)(6 + client.ClientId.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.CONNECT);
            context.Write(client.Flags.Value);
            context.Write(ProtcolID);
            context.Write(client.Duration);
            context.Write(client.ClientId);
            return context.Data;
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var type = GetType();
            var props = type.GetProperties();
            var args = props.Select(_ => $"{_.Name}={_.GetValue(this)}");
            return $"{type.Name}({string.Join(", ", args)})";
        }
    }
}
