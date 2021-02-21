using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CItemExchange
    {
        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        public int DataMax
        {
            get => Data.Length; // unlimited
            set { }
        }

        [FieldOrder(2), FieldCount(nameof(DataLength))]
        public ItemExchange[] Data;
    }

    public class ItemExchange
    {
        [FieldOrder(3), Column("a_index")]
        public int ID { get; set; }

        [BinaryIgnore, Column("a_enable")]
        public int Enable { get; set; }

        [FieldOrder(4), Column("a_npc_index")]
        public int NPCID { get; set; }

        [FieldOrder(5), Column("result_itemIndex")]
        public int ItemID { get; set; }

        [FieldOrder(6), Column("result_itemCount")]
        public int ItemCount { get; set; }

        [FieldOrder(7), Column("source_itemIndex0")]
        public int SourceItemID0 { get; set; }

        [FieldOrder(8), Column("source_itemCount0")]
        public int SourceItemCount0 { get; set; }

        [FieldOrder(9), Column("source_itemIndex1")]
        public int SourceItemID1 { get; set; }

        [FieldOrder(10), Column("source_itemCount1")]
        public int SourceItemCount1 { get; set; }

        [FieldOrder(11), Column("source_itemIndex2")]
        public int SourceItemID2 { get; set; }

        [FieldOrder(12), Column("source_itemCount2")]
        public int SourceItemCount2 { get; set; }

        [FieldOrder(13), Column("source_itemIndex3")]
        public int SourceItemID3 { get; set; }

        [FieldOrder(14), Column("source_itemCount3")]
        public int SourceItemCount3 { get; set; }

        [FieldOrder(15), Column("source_itemIndex4")]
        public int SourceItemID4 { get; set; }

        [FieldOrder(16), Column("source_itemCount4")]
        public int SourceItemCount4 { get; set; }

        [BinaryIgnore, Column("a_name")]
        public string Name { get; set; }

        [BinaryIgnore, Column("a_desc")]
        public string Description { get; set; }
    }
}
