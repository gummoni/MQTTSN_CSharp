using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// PUBLISH応答確認メッセージ（QoS=2）
    /// </summary>
    public class PubRel
    {
        /// <summary>
        /// PUBLISHメッセージと同じメッセージID
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PubRel Parse(Payload payload)
        {
            if (4 != payload.Length) return null;
            return new PubRel()
            {
                MsgId = payload.ReadUShort(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client, ushort msgId)
        {
            var length = (ushort)4;
            var context = new Payload(length);
            context.Write((byte)MsgType.PUBREL);
            context.Write(msgId);
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
