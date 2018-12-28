using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// WillMsgReqの応答
    /// </summary>
    public class WillMsg
    {
        /// <summary>
        /// WILLメッセージ
        /// </summary>
        public string Will_Msg { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillMsg Parse(Payload payload)
        {
            if (2 >= payload.Length) return null;
            return new WillMsg()
            {
                Will_Msg = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)(2 + client.WillMsg.Length);
            var context = new Payload(length);
            context.Write((byte)MsgType.WILLMSG);
            context.Write(client.WillMsg);
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
