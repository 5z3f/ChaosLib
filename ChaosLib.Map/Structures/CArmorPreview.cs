using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CArmorPreview
    {
        // -- not completed

        [FieldOrder(0)]
        public int DataLength
        {
            get => DefaultArmor.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public DefaultArmorPreview[] DefaultArmor;

        public class DefaultArmorPreview
        {
            [FieldOrder(2)]
            [FieldCount(10)] // DEF_DEFAULT_COL
            public int[] DefaultWearInfo { get; set; }
        }

        [FieldOrder(3)]
        [FieldCount(9)] // job
        public ArmorPreview[] Armor;

        public class ArmorPreview
        {
            [FieldOrder(4)]
            public int ArmorPartCount { get; set; }

            [FieldOrder(5)]
            [FieldCount(5 * 8)] // ROW * COL
            public int[] BinaryIDs;
        }
    }
}