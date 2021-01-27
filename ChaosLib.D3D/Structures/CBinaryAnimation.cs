using BinarySerialization;

namespace ChaosLib.D3D.Structures
{
    public class CBinaryAnimation
    {
        [FieldOrder(0)]
        [FieldLength(4)]
        [FieldEncoding("ascii")]
        public string Magic;

        [FieldOrder(1)]
        public int Version;

        [FieldOrder(2)]
        public int DataLength;

        [FieldOrder(3)]
        [FieldCount(nameof(DataLength))]
        public CAnimation[] Animations;
    }

    public class CAnimation
    {
        [FieldOrder(4)]
        public int PathLength;

        [FieldOrder(5)]
        [FieldLength(nameof(PathLength))]
        [FieldEncoding("ascii")]
        public string SkaPath;

        [FieldOrder(6)]
        public int NameLength;

        [FieldOrder(7)]
        [FieldLength(nameof(NameLength))]
        [FieldEncoding("ascii")]
        public string Name;

        [FieldOrder(8)]
        public float FPS;

        [FieldOrder(9)]
        public int TotalFrames;

        [FieldOrder(10)]
        public float Threshold;

        [FieldOrder(11)]
        public int IsCompressed;

        [FieldOrder(12)]
        public int IsCustomSpeed;

        [FieldOrder(13)]
        public int BoneDataLength;

        [FieldOrder(14)]
        [FieldCount(nameof(BoneDataLength))]
        public CAnimationBone[] Bones;

        [FieldOrder(34)]
        public int MorphDataLength;

        [FieldOrder(35)]
        [FieldCount(nameof(MorphDataLength))]
        public float[] MorphFactors;
    }

    public class CAnimationBone
    {
        [FieldOrder(15)]
        public int NameLength;

        [FieldOrder(16)]
        [FieldLength(nameof(NameLength))]
        [FieldEncoding("ascii")]
        public string Name;

        [FieldOrder(17)]
        [FieldCount(12)] // predefined in client
        public float[] DefaultPosition;

        [FieldOrder(18)]
        public int AnimationPositionLength;

        [FieldOrder(19)]
        [FieldCount(nameof(AnimationPositionLength))]
        public CAnimationPosition[] Positions;

        [FieldOrder(25)]
        public int RotationPositionLength;

        [FieldOrder(26)]
        [FieldCount(nameof(RotationPositionLength))]
        public CAnimationRotation[] Rotations;

        [FieldOrder(33)]
        public float OffsetLength;
    }

    public class CAnimationPosition
    {
        [FieldOrder(20)]
        public ushort Frame;

        [FieldOrder(21)]
        public short Flags;

        [FieldOrder(22)]
        public float X;

        [FieldOrder(23)]
        public float Y;

        [FieldOrder(24)]
        public float Z;
    }

    public class CAnimationRotation
    {
        [FieldOrder(27)]
        public ushort Frame;

        [FieldOrder(28)]
        public short Flags;

        [FieldOrder(29)]
        public float W;

        [FieldOrder(30)]
        public float X;

        [FieldOrder(31)]
        public float Y;

        [FieldOrder(32)]
        public float Z;
    }

    // testflag - do not use
    enum LWBONE
    {
        LWBONEF_ACTIVE = (1 << 0),
        LWBONEF_LIMITED_RANGE = (1 << 1),
        LWBONEF_SCALE_STRENGTH = (1 << 2),
        LWBONEF_WEIGHT_MAP_ONLY = (1 << 3),
        LWBONEF_WEIGHT_NORM = (1 << 4),
        LWBONEF_JOINT_COMP = (1 << 5),
        LWBONEF_JOINT_COMP_PAR = (1 << 6),
        LWBONEF_MUSCLE_FLEX = (1 << 7),
        LWBONEF_MUSCLE_FLEX_PAR = (1 << 8)
    }
}
