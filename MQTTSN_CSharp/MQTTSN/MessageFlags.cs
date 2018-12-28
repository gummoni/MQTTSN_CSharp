namespace MQTTSN
{
    /// <summary>
    /// MQTT-SNメッセージフラグ
    /// </summary>
    public class MessageFlags
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
        public TopicIdType TopicIdType
        {
            get => (TopicIdType)(Value & 0x03);
            set => Value = (byte)((Value & 0xfc) | (byte)value);
        }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="flags"></param>
        public MessageFlags(byte flags)
        {
            Value = flags;
        }
    }

    public enum QoS
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
    }

    //see section3 and 6.7 
    public enum TopicIdType
    {
        Normal = 0,
        Predefined = 1,
        Short = 2,
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

    public enum ReturnCode
    {
        Accepted = 0x00,
        Rejected_Congestion = 0x01,
        Rejexted_InvalidTopicID = 0x02,
        Rejected_NotSupported = 0x03,
    }
}
