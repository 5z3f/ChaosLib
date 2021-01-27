using System.Collections.Generic;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CMonsterCombo
    {
        [FieldOrder(0)]
        public int DataLength;

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public List<CMonsterComboData> MonsterCombo;
    }

    public class CMonsterComboData
    {
        [FieldOrder(2)]
        public int ID;

        [FieldOrder(3)]
        public int Gold;

        [FieldOrder(4)]
        [FieldCount(3)]
        public int[] Icon;

        [FieldOrder(5)]
        public byte Skill; // Whether there is a mission penalty >> this is a boolean but client is reading it this way

        [FieldOrder(6)]
        public int Points;
    }
}
