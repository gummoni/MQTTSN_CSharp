using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// 切断要求
    /// </summary>
    public class Disconnect
    {
        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static Disconnect Parse(Payload payload)
        {
            if (2 != payload.Length) return null;
            return new Disconnect()
            {
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)2;
            var context = new Payload(length);
            context.Write((byte)MsgType.DISCONNECT);
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
