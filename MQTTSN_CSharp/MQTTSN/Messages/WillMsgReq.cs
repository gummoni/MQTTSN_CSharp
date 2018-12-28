using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// Willメッセージ送信要求するためにGWによって送信される
    /// </summary>
    public class WillMsgReq
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillMsgReq Parse(Payload payload)
        {
            if (2 != payload.Length) return null;
            return new WillMsgReq();
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
            context.Write((byte)MsgType.WILLMSGREQ);
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
