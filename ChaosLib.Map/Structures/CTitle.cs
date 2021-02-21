using System;
using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CTitle
    {
        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public Title[] Data;
    }

    [TableName("t_title")]
    public class Title
    {
        [FieldOrder(2), Column("a_index")]
        public int ID { get; set; }

        [FieldOrder(3), Column("a_enable")]
        public byte Enable { get; set; }

        [FieldOrder(4), FieldLength(64), FieldEncoding("ascii"), Column("a_effect_name"),
            SerializeAs(SerializedType.TerminatedString)]
        public string EffectIdle;

        [FieldOrder(5), FieldLength(64), FieldEncoding("ascii"), Column("a_attack"),
            SerializeAs(SerializedType.TerminatedString)]
        public string EffectAttack;

        [FieldOrder(6), FieldLength(64), FieldEncoding("ascii"), Column("a_damage"),
            SerializeAs(SerializedType.TerminatedString)]
        public string EffectDamage;

        [FieldOrder(7)]
        public uint ForegroundColorBinary
        {
            get => ForegroundColor != string.Empty ? Convert.ToUInt32(ForegroundColor, 16) : 0;
            set => ForegroundColor = value.ToString("X4");
        }

        [FieldOrder(8)]
        public uint BackgroundColorBinary
        {
            get => BackgroundColor != string.Empty ? Convert.ToUInt32(BackgroundColor, 16) : 0;
            set => BackgroundColor = value.ToString("X4");
        }

        [BinaryIgnore, Column("a_color")]
        public string ForegroundColor { get; set; }

        [BinaryIgnore, Column("a_bgcolor")]
        public string BackgroundColor { get; set; }

        [FieldOrder(9), Column("a_option_index0")]
        public int OptionID0 { get; set; }

        [FieldOrder(10), Column("a_option_index1")]
        public int OptionID1 { get; set; }

        [FieldOrder(11), Column("a_option_index2")]
        public int OptionID2 { get; set; }

        [FieldOrder(12), Column("a_option_index3")]
        public int OptionID3 { get; set; }

        [FieldOrder(13), Column("a_option_index4")]
        public int OptionID4 { get; set; }

        [FieldOrder(14), Column("a_option_level0")]
        public byte OptionLevel0 { get; set; }

        [FieldOrder(15), Column("a_option_level1")]
        public byte OptionLevel1 { get; set; }

        [FieldOrder(16), Column("a_option_level2")]
        public byte OptionLevel2 { get; set; }

        [FieldOrder(17), Column("a_option_level3")]
        public byte OptionLevel3 { get; set; }

        [FieldOrder(18), Column("a_option_level4")]
        public byte OptionLevel4 { get; set; }

        [FieldOrder(19), Column("a_item_index")]
        public int ItemID { get; set; }

        [BinaryIgnore, Column("a_time")]
        public int Time { get; set; }

        [BinaryIgnore, Column("a_flag")]
        public int Flag { get; set; }

        [BinaryIgnore, Column("a_castle_num")]
        public int CastleZoneID { get; set; }
    }
}
