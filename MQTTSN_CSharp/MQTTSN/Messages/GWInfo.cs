using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// GWINFOメッセージは、SEARCHGWメッセージの応答メッセージになります。
    /// 送信元のGWIDのみ返します。
    /// </summary>
    /// </summary>
    public class GWInfo
    {
        /// <summary>
        /// ゲートウェイID
        /// </summary>
        public byte GwId { get; private set; }

        /// <summary>
        /// 指定されたゲートウェイアドレス（オプション）
        /// メッセージがクライアントから送信された場合のみ含まれます。
        /// </summary>
        public string GwAdd { get; private set; }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static GWInfo Parse(Payload payload)
        {
            if (3 > payload.Length) return null;
            return new GWInfo()
            {
                GwId = payload.ReadByte(),
                GwAdd = payload.ReadString(),
            };
        }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway)
        {
            //var length = (ushort)(3 + gateway.GwAdd.Length);
            var length = (ushort)3;
            var context = new Payload(length);
            context.Write((byte)MsgType.GWINFO);
            context.Write(gateway.GwId);
            //context.Write(gateway.GwAdd);
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
