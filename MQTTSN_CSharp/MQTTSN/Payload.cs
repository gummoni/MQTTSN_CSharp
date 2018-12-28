using System;
using System.Text;

namespace MQTTSN
{
    //http://emqtt.io/docs/v2/mqtt-sn.html
    //http://www.mqtt.org/new/wp-content/uploads/2009/06/MQTT-SN_spec_v1.2.pdf
    //https://qiita.com/TomoakiYAMAGUCHI/items/26ea7861aafbaae80ca1

    /// <summary>
    /// MQTTメッセージペイロード
    /// </summary>
    public class Payload
    {
        int Index;

        public byte[] Data { get; private set; }
        public int Length { get; }
        public MsgType MsgType { get; }

        /// <summary>
        /// 送信側
        /// </summary>
        /// <param name="length"></param>
        public Payload(ushort length)
        {
            Data = new byte[length];
            Length = length;
            Index = 0;

            if (0xff < length)
            {
                Write(0x01);
                Write(length);
            }
            else
            {
                Write((byte)length);
            }
        }

        /// <summary>
        /// 受信側
        /// </summary>
        /// <param name="data"></param>
        public Payload(byte[] data)
        {
            Data = data;
            Index = 0;

            var d0 = ReadByte();
            if (0x01 == d0)
            {
                Length = ReadUShort();
            }
            else
            {
                Length = d0;
            }
            MsgType = (MsgType)ReadByte();
        }

        /// <summary>
        /// 1Byte読込み
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return Data[Index++];
        }

        /// <summary>
        /// 2Byte読込み
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            var d1 = (ushort)Data[Index++];
            var d2 = (ushort)Data[Index++];
            return (ushort)((d1 << 8) | d2);
        }

        /// <summary>
        /// 文字列読込み（最後まで）
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var result = Encoding.UTF8.GetString(Data, Index, Length - Index);
            Index = Length;
            return result;
        }

        /// <summary>
        /// 1Byte書込み
        /// </summary>
        /// <param name="value"></param>
        public void Write(byte value)
        {
            Data[Index++] = value;
        }

        /// <summary>
        /// 2Byte書込み
        /// </summary>
        /// <param name="value"></param>
        public void Write(ushort value)
        {
            Data[Index++] = (byte)(value >> 8);
            Data[Index++] = (byte)(value & 0xff);
        }

        /// <summary>
        /// 文字列書込み
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var len = bytes.Length;
            Buffer.BlockCopy(bytes, 0, Data, Index, len);
            Index += len;
        }
    }
}
