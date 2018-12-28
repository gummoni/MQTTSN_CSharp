using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// PUBLISH応答確認メッセージ（QoS=1、２）
    /// </summary>
    public class PubAck
    {
        /// <summary>
        /// PUBLISHメッセージと同じトピックID
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// PUBLISHメッセージと同じメッセージID
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// リターンコード
        /// </summary>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PubAck Parse(Payload payload)
        {
            if (6 != payload.Length) return null;
            return new PubAck()
            {
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadUShort(),
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, ushort topicId, ushort msgId, ReturnCode returnCode)
        {
            var length = (ushort)6;
            var context = new Payload(length);
            context.Write((byte)MsgType.PUBACK);
            context.Write(topicId);
            context.Write(msgId);
            context.Write((byte)returnCode);
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
