using System.Linq;

namespace MQTTSN.Messages
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
        public static ForwarderEncapsulation Parse(Payload payload)
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
        public static byte[] Pack(Client client, byte ctrl, string wirelessNodeId, string message)
        {
            var length = (ushort)(3 + wirelessNodeId.Length + message.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.EncapsulatedMessage);
            context.Write(ctrl);
            context.Write(wirelessNodeId);
            context.Write(message);
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
