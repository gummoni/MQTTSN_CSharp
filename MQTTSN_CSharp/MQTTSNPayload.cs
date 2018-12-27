using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //http://emqtt.io/docs/v2/mqtt-sn.html
    //http://www.mqtt.org/new/wp-content/uploads/2009/06/MQTT-SN_spec_v1.2.pdf
    //https://qiita.com/TomoakiYAMAGUCHI/items/26ea7861aafbaae80ca1

    /// <summary>
    /// MQTTメッセージペイロード
    /// </summary>
    public class MQTTSNPayload
    {
        int Index;

        public byte[] Data { get; private set; }
        public int Length { get; }
        public MsgType MsgType { get; }

        /// <summary>
        /// 送信側
        /// </summary>
        /// <param name="length"></param>
        public MQTTSNPayload(ushort length)
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
        public MQTTSNPayload(byte[] data)
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

    /// <summary>
    /// MQTT-SNメッセージフラグ
    /// </summary>
    public class MQTTSNMessageFlags
    {
        public byte Value { get; set; }
        public bool DUP
        {
            get => 0 != (Value & 0x80);
            set => Value = (value) ? (byte)(Value | 0x80) : (byte)(Value & 0x7f);
        }
        public QoS QoS
        {
            get => (QoS)((Value >> 5) & 0x03);
            set => Value = (byte)((Value & 0x90) | (byte)value << 5);
        }
        public bool Retain
        {
            get => 0 != (Value & 0x10);
            set => Value = (value) ? (byte)(Value | 0x10) : (byte)(Value & 0xef);
        }
        public bool Will
        {
            get => 0 != (Value & 0x08);
            set => Value = (value) ? (byte)(Value | 0x08) : (byte)(Value & 0xf7);
        }
        public bool CleanSession
        {
            get => 0 != (Value & 0x04);
            set => Value = (value) ? (byte)(Value | 0x04) : (byte)(Value & 0xfb);
        }
        public TopicId TopicIdType
        {
            get => (TopicId)(Value & 0x03);
            set => Value = (byte)((Value & 0xfc) | (byte)value);
        }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="flags"></param>
        public MQTTSNMessageFlags(byte flags)
        {
            Value = flags;
        }
    }

    public enum MsgType : byte
    {
        ADVERTISE = 0x00,
        SEARCHGW = 0x01,
        GWINFO = 0x02,
        CONNECT = 0x04,
        CONNACK = 0x05,
        WILLTOPICREQ = 0x06,
        WILLTOPIC = 0x07,
        WILLMSGREQ = 0x08,
        WILLMSG = 0x09,
        REGISTER = 0x0A,
        REGACK = 0x0B,
        PUBLISH = 0x0C,
        PUBACK = 0x0D,
        PUBCOMP = 0x0E,
        PUBREC = 0x0F,
        PUBREL = 0x10,
        SUBSCRIBE = 0x12,
        SUBACK = 0x13,
        UNSUBSCRIBE = 0x14,
        UNSUBACK = 0x15,
        PINGREQ = 0x16,
        PINGRESP = 0x17,
        DISCONNECT = 0x18,
        WILLTOPICUPD = 0x1A,
        WILLTOPICRESP = 0x1B,
        WILLMSGUPD = 0x1C,
        WILLMSGRESP = 0x1D,
        EncapsulatedMessage = 0xFE,
    }

    public enum QoS
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
    }

    //see section3 and 6.7 
    public enum TopicId
    {
        Normal = 0,
        Predefined = 1,
        Short = 2,
    }

    public enum ReturnCode
    {
        Accepted = 0x00,
        Rejected_Congestion = 0x01,
        Rejexted_InvalidTopicID = 0x02,
        Rejected_NotSupported = 0x03,
    }

}
