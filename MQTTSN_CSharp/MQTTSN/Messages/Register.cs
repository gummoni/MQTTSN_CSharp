using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// クライアントがGWへ送信。（トピック名のトピックID値を取得するため）
    /// </summary>
    public class Register
    {
        /// <summary>
        /// クライアントから送信された場合、0x0000とコーディングされており、関係ありません。
        /// </summary>
        public ushort TopicId { get; private set; }

        /// <summary>
        /// 対応するREGACKメッセージを識別するためにコーディングする必要があります。
        /// </summary>
        public ushort MsgId { get; private set; }

        /// <summary>
        /// トピック名
        /// </summary>
        public string TopicName { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Register Parse(Payload payload)
        {
            if (6 >= payload.Length) return null;
            return new Register()
            {
                TopicId = payload.ReadUShort(),
                MsgId = payload.ReadUShort(),
                TopicName = payload.ReadString(),
            };
        }

        /// <summary>
        /// トピック生成
        /// </summary>
        /// <returns></returns>
        public Topic CreateTopic()
        {
            return new Topic()
            {
                TopicId = TopicId,
                TopicName = TopicName,
                Data = null,
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client, ushort msgId, ushort topicId, string topicName)
        {
            var length = (ushort)(6 + topicName.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.REGISTER);
            context.Write(topicId);
            context.Write(msgId);
            context.Write(topicName);
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
