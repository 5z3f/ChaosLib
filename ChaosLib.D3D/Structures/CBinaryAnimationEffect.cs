using BinarySerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosLib.D3D.Structures
{
    public class CBinaryAnimationEffect
    {
        [FieldOrder(0)]
        [FieldLength(4)]
        [FieldEncoding("ascii")]
        public string Magic;

        [FieldOrder(1)]
        public int DataLength;

        [FieldOrder(2)]
        [FieldCount(nameof(DataLength))]
        public CAnimationEffect[] AnimationEffects;
    }

    public class CAnimationEffect
    {
        [FieldOrder(3)]
        [FieldLength(4)]
        [FieldEncoding("ascii")]
        public string Magic;

        [FieldOrder(4)]
        public byte Version;

        [FieldOrder(5)]
        public int NameLength;

        [FieldOrder(6)]
        [FieldLength(nameof(NameLength))]
        [FieldEncoding("ascii")]
        public string Name;

        [FieldOrder(7)]
        public int DataLength;

        [FieldOrder(8)]
        [FieldCount(nameof(DataLength))]
        public CAnimationEffectData[] AnimationEffectData;

    }

    public class CAnimationEffectData
    {
        [FieldOrder(9)]
        public int NameLength;

        [FieldOrder(10)]
        [FieldLength(nameof(NameLength))]
        [FieldEncoding("ascii")]
        public string EffectGroupName;

        [FieldOrder(11)]
        public float StartTime;

        [FieldOrder(12)]
        public uint Flags;
    }

    public enum EffectFlag : ulong
    {
       REF_NOTHING = 0UL,
       REF_SYNCANIMLENGTH = (1UL << 0),   // effect group cannot take longer than animation length
       REF_SYNCANIMLOOP = (1UL << 1),     // restart effect at anim loop
       REF_SYNCANIMSPEED = (1UL << 2)     // synchronize effect with animation speed
    }
}
