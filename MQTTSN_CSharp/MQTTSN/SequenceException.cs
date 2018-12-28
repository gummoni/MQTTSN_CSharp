using System;

namespace MQTTSN
{
    public class SequenceException : Exception
    {
        public ReturnCode ReturnCode { get; }
        public SequenceException(ReturnCode returnCode)
        {
            ReturnCode = ReturnCode;
        }
    }
}
