using System.Collections.Generic;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class COption
    {
        [FieldOrder(0)]
        public int DataLength;

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public List<COptionData> Actions;
    }

    public class COptionData
    {
        public const int DEF_OPTION_MAX_LEVEL = 36;
        public const int DEF_RAREOPTION_MAX = 10;

        [FieldOrder(2)]
        public int ID;

        [FieldOrder(3)]
        public int Type;

        [FieldOrder(4)]
        [FieldCount(DEF_OPTION_MAX_LEVEL)]
        public int[] Value;
    }
}
