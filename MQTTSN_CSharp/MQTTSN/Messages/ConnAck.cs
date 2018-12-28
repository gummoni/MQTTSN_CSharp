using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// クライアントからの接続要求に応じて返信します。
    /// </summary>
    public class ConnAck
    {
        /// <summary>
        /// リターンコード
        /// </summary>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static ConnAck Parse(Payload payload)
        {
            if (3 != payload.Length) return null;
            return new ConnAck()
            {
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, ReturnCode code)
        {
            var length = (ushort)3;
            var context = new Payload(length);
            context.Write((byte)MsgType.CONNACK);
            context.Write((byte)code);
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
