using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CArmorPreview
    {
        private const int JOB_COUNT = 9;
        private const int DEF_ARMOR_ROW = 5;
        private const int DEF_ARMOR_COL = 8;
        private const int DEF_DEFAULT_COL = DEF_ARMOR_COL + 2; // DEF_FACE = 8, DEF_HAIR = 9

        [FieldOrder(0)]
        public int DataLength
        {
            get => JOB_COUNT;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public DefaultWear[] DefaultArmor;

        public class DefaultWear
        {
            [FieldOrder(2)]
            public int Helmet { get; set; }

            [FieldOrder(3)]
            public int Jacket { get; set; }

            [FieldOrder(4)]
            public int Weapon { get; set; }

            [FieldOrder(5)]
            public int Pants { get; set; }

            [FieldOrder(6)]
            public int Shield { get; set; }

            [FieldOrder(7)]
            public int Gloves { get; set; }

            [FieldOrder(8)]
            public int Boots { get; set; }

            [FieldOrder(9)]
            public int Accesory { get; set; }

            [FieldOrder(10)]
            public int Face { get; set; }

            [FieldOrder(11)]
            public int Hair { get; set; }
        }

        [FieldOrder(12)]
        [FieldCount(JOB_COUNT)]
        public SetWear[] PreviewArmor;

        public class SetWear
        {
            [FieldOrder(13)]
            public int ArmorPartCount
            {
                get => 5; // 5 = preview set limitation
                set { }
            }

            [FieldOrder(14)]
            [FieldCount(nameof(ArmorPartCount))]
            public ArmorSet[] Set;
        }

        public class ArmorSet
        {
            [FieldOrder(15)]
            public int Helmet { get; set; }

            [FieldOrder(16)]
            public int Jacket { get; set; }

            [FieldOrder(17)]
            public int Weapon { get; set; }

            [FieldOrder(18)]
            public int Pants { get; set; }

            [FieldOrder(19)]
            public int Shield { get; set; }

            [FieldOrder(20)]
            public int Gloves { get; set; }

            [FieldOrder(21)]
            public int Boots { get; set; }

            [FieldOrder(22)]
            public int Accesory { get; set; }
        }
    }
}