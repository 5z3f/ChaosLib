using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{

    // Currently only client side load/save

    public class CMap
    {
        [FieldOrder(0)]
        public int ZoneCount
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(ZoneCount))]
        public Map[] Data;
    }

    [TableName("t_mapinfo")]
    public class Map
    {
        [FieldOrder(2), Column("a_zone_index")]
        public int ZoneID { get; set; }

        [FieldOrder(3), Column("a_ylayer")]
        public byte LayerY { get; set; }

        [FieldOrder(4), Column("a_map_left")]
        public int Left { get; set; }

        [FieldOrder(5), Column("a_map_top")]
        public int Top { get; set; }

        [FieldOrder(6), Column("a_map_right")]
        public int Right { get; set; }

        [FieldOrder(7), Column("a_map_bottom")]
        public int Bottom { get; set; }

        [FieldOrder(8), Column("a_map_rate")]
        public float Ratio { get; set; }

        [FieldOrder(9), Column("a_map_offset_x")]
        public int OffsetX { get; set; } //

        [FieldOrder(10), Column("a_map_offset_z")]
        public int OffsetZ { get; set; } //

        [FieldOrder(11), Column("a_detailmap_count")]
        public byte DetailCount
        {
            get => (byte)BinaryDetail.Length;
            set { }
        }

        [FieldOrder(12)]
        [FieldCount(nameof(DetailCount))]
        public Detail[] BinaryDetail;


        public class Detail
        {
            [FieldOrder(13)]
            public int Left { get; set; }

            [FieldOrder(14)]
            public int Top { get; set; }

            [FieldOrder(15)]
            public int Right { get; set; }

            [FieldOrder(16)]
            public int Bottom { get; set; }

            [FieldOrder(17)]
            public int LeftW { get; set; }

            [FieldOrder(18)]
            public int TopW { get; set; }

            [FieldOrder(19)]
            public int RightW { get; set; }

            [FieldOrder(20)]
            public int BottomW { get; set; }

            [FieldOrder(21)]
            public float X { get; set; }

            [FieldOrder(22)]
            public float Z { get; set; }

            [FieldOrder(23)]
            public float Ratio { get; set; }

        }

        [FieldOrder(24), Column("a_dungeon_count")]
        public byte DungeonCount
        {
            get => (byte)BinarySubZone.Length;
            set { }
        }

        [FieldOrder(25)]
        [FieldCount(nameof(DungeonCount))]
        public Dungeon[] BinarySubZone;

        public class Dungeon
        {
            [FieldOrder(26)]
            public int ID { get; set; }

            [FieldOrder(27)]
            public float X { get; set; }

            [FieldOrder(28)]
            public float Z { get; set; }

            [FieldOrder(29)]
            public byte Type { get; set; }
        }

        [FieldOrder(30)]
        public int NPCCount
        {
            get => (byte)BinaryNPC.Length;
            set { }
        }

        [FieldOrder(31)]
        [FieldCount(nameof(NPCCount))]
        public MapNPC[] BinaryNPC;

        public class MapNPC
        {
            [FieldOrder(32)]
            public int ID { get; set; }

            [FieldOrder(33)]
            public int LayerY { get; set; } //

            [FieldOrder(34)]
            public float X { get; set; }

            [FieldOrder(35)]
            public float Z { get; set; }
        }
    }
}