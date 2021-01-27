using BinarySerialization;

namespace ChaosLib.D3D.Structures
{
    public class CBinarySkeleton
    {
        [FieldOrder(0)]
        [FieldLength(4)]
        [FieldEncoding("ascii")]
        public string Magic;

        [FieldOrder(1)]
        public int Version;

        [FieldOrder(2)]
        public int LodCount;

        [FieldOrder(3)]
        public int PathLength;

        [FieldOrder(4)]
        [FieldLength(nameof(PathLength))]
        [FieldEncoding("ascii")]
        public string SkaPath;

        [FieldOrder(5)]
        public float MaxDistance;

        [FieldOrder(6)]
        public int DataLength;

        [FieldOrder(7)]
        [FieldCount(nameof(DataLength))]
        public CSkeletonBone[] Bones;
    }

    public class CSkeletonBone
    {
        [FieldOrder(8)]
        public int NameLength;

        [FieldOrder(9)]
        [FieldLength(nameof(NameLength))]
        [FieldEncoding("ascii")]
        public string Name;

        [FieldOrder(10)]
        public int ParentLength;

        [FieldOrder(11)]
        [FieldLength(nameof(ParentLength))]
        [FieldEncoding("ascii")]
        public string ParentID;

        [FieldOrder(12)]
        [FieldCount(12)]   // predefined
        public float[] AbsPlacement;

        [FieldOrder(13)]
        [FieldCount(3)]   // predefined
        public float[] vPosition;

        [FieldOrder(14)]
        [FieldCount(4)]   // predefined
        public float[] qRotation;

        [FieldOrder(15)]
        public float offsetLen;

        [FieldOrder(16)]
        public float boneLength;
    }
}
