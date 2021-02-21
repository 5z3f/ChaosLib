using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CLevelGuide
    {
        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public LevelGuide[] Data;

    }

    public class LevelGuide
    {
        [FieldOrder(2)]
        public int Level { get; set; }

        [FieldOrder(3)]
        public int StringID { get; set; }
    }
}
