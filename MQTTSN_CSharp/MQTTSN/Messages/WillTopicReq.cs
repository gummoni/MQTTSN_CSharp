using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// Willトピック名の送信をクライアントに要求する
    /// </summary>
    public class WillTopicReq
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicReq Parse(Payload payload)
        {
            if (2 != payload.Length) return null;
            return new WillTopicReq();
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway)
        {
            var length = (ushort)2;
            var context = new Payload(length);
            context.Write((byte)MsgType.WILLTOPICREQ);
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
