using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// メッセージの受信及び処理に対する確認として送信される
    /// </summary>
    public class RegAck
    {
        /// <summary>
        /// PUBLISHメッセージでトピックIDとして使用されている値
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// 対応するREGISTERメッセージに含まれているものと同じ値
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
        public static RegAck Parse(Payload payload)
        {
            if (7 != payload.Length) return null;
            return new RegAck()
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
            var length = (ushort)7;
            var context = new Payload(length);
            context.Write((byte)MsgType.REGACK);
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
