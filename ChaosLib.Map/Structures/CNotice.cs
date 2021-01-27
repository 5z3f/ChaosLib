using System.Collections.Generic;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CNotice
    {
        [FieldOrder(0)]
        public int DataLength;

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public List<CNoticeData> Notice;
    }

    public class CNoticeData
    {
        [FieldOrder(2)]
        public int ID;

        [FieldOrder(3)]
        public int Enabled;

        [FieldOrder(4)]
        public int TitleLength;

        [FieldOrder(5)]
        [FieldLength(nameof(TitleLength))]
        [FieldEncoding("ascii")]
        public string Title;

        [FieldOrder(6)]
        public int MessageLength;

        [FieldOrder(7)]
        [FieldLength(nameof(MessageLength))]
        [FieldEncoding("ascii")]
        public string Message;

        [FieldOrder(8)]
        public int TimeStartLength;

        [FieldOrder(9)]
        [FieldLength(nameof(TimeStartLength))]
        [FieldEncoding("ascii")]
        public string TimeStart;

        [FieldOrder(10)]
        public int TimeEndLength;

        [FieldOrder(11)]
        [FieldLength(nameof(TimeEndLength))]
        [FieldEncoding("ascii")]
        public string TimeEnd;

        [FieldOrder(12)]
        public int Cycle;

        [FieldOrder(13)]
        [FieldCount(4)]
        public byte[] ColorRGBA;
    }
}
