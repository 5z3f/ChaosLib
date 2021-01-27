using System.Collections.Generic;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CAction
    {
        [FieldOrder(0)]
        public int DataLength;

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public List<CActionData> Action;
    }

    public class CActionData
    {
        [FieldOrder(2)]
        public int ID;

        [FieldOrder(3)]
        public byte Type;

        [FieldOrder(4)]
        public int Job;

        [FieldOrder(5)]
        [FieldCount(3)]
        public int[] Icon; // ID | ROW | COL
    }
}
