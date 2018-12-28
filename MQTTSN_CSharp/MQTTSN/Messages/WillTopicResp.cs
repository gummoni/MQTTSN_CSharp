using System.Linq;

namespace MQTTSN.Messages
{
    /// <summary>
    /// WILLTOPICUPD応答メッセージ
    /// </summary>
    public class WillTopicResp
    {
        /// <summary>
        /// リターンコード
        /// </summary>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// パケット作成
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static WillTopicResp Parse(Payload payload)
        {
            if (3 != payload.Length) return null;
            return new WillTopicResp()
            {
                ReturnCode = (ReturnCode)payload.ReadByte(),
            };
        }

        /// <summary>
        /// パケット解析
        /// </summary>
        /// <param name="gateway"></param>
        /// <returns></returns>
        public static byte[] Pack(Gateway gateway, ReturnCode returnCode)
        {
            var length = (ushort)3;
            var context = new Payload(length);
            context.Write((byte)MsgType.WILLTOPICRESP);
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
