using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// SEARCHGWメッセージは、クライアントがGW検索する時にブロードキャストされます。
    /// ブロードキャストする範囲は制限します。
    /// </summary>
    public class SearchGW
    {
        /// <summary>
        /// ブロードキャストする半径
        /// </summary>
        public byte Radius { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SearchGW Parse(Payload context)
        {
            if (3 != context.Data.Length) return null;
            return new SearchGW()
            {
                Radius = context.ReadByte()
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static byte[] Pack(Client client)
        {
            var length = (ushort)3;
            var context = new Payload(length);
            context.Write((byte)MsgType.SEARCHGW);
            context.Write(client.Radius);
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
