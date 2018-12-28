using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// ADVERTISEメッセージは、その存在を知らせるためにゲートウェイによって定期的にブロードキャストされます。 
    /// 次のブロードキャスト時刻までの間隔は、このメッセージのDurationフィールドに示されています。 
    /// </summary>
    public class Advertise
    {
        /// <summary>
        /// ゲートウェイID
        /// </summary>
        public byte GwId { get; private set; }

        /// <summary>
        /// ADVERTISEメッセージがブロードキャストされるまでの時間間隔
        /// </summary>
        public ushort Duration { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Advertise Parse(Payload payload)
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
        public static byte[] Pack(Gateway gateway)
        {
            var length = (ushort)5;
            var context = new Payload(length);
            context.Write((byte)MsgType.ADVERTISE);
            context.Write(gateway.GwId);
            context.Write(gateway.Duration);
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
