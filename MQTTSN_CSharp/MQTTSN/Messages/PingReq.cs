using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// 生存確認
    /// </summary>
    public class PingReq
    {
        /// <summary>
        /// 指定クライアントID（オプション）
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static PingReq Parse(Payload payload)
        {
            if (2 > payload.Length) return null;
            return new PingReq()
            {
                ClientId = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, string clientId = "")
        {
            var length = (ushort)(2 + clientId.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.PINGREQ);
            context.Write(clientId);
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
