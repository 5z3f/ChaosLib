
// temp prototype dirty structure model

namespace ChaosLib.D3D.Structures
{
    public class CMeshHeader
    {
        public string FileTypeMagic;
        public int FileVersion;
    }

    public class CBinaryMesh
    {
        public CMeshHeader Header;
        public cMesh[] Mesh;

        public const int MESH_NEW_VER_LC = 17;
        public const int MESH_OLD_VER_LC = 16;

        public const int MESH_VER12_SE110 = 12;
        public const int MESH_VER11_SE110 = 11;

        private readonly byte[] CODE_MESH = { 13, 23, 19, 29, 5 };
        public byte ubChecker = (byte)17; // MESH_NEW_VER

        public uint MeshBitwise(uint i)
        {
            if (Header.FileVersion is MESH_NEW_VER_LC)
            {
                uint ulChecker = 0;

                ulChecker |= (uint)(ubChecker += CODE_MESH[0]) << 24;
                ulChecker |= (uint)(ubChecker += CODE_MESH[1]) << 16;
                ulChecker |= (uint)(ubChecker += CODE_MESH[2]) << 8;
                ulChecker |= (uint)(ubChecker += CODE_MESH[3]) << 0;

                ubChecker += CODE_MESH[4];
                return i ^ ulChecker;
            }
            else
                return i;
        }
    }

    public class cMesh
    {
        public uint VertexMapCount,
            WeightMapCount,
            UVMapCount,
            NormalCount,
            SurfaceCount,
            MorphMapCount,
            Flags;

        public string SkaPath;
        public float MaxDistance;

        public Vertex3f[] Vertices,
            Normals;

        public UVMap[] UVMaps;
        public Surface[] Surfaces;
        public WeightMap[] WeightMaps;
        public MorphMap[] MorphMaps;
        public VertexWeight[] VertexWeights;
    }

    public class Vertex3f // actually not 3 and not float anymore lol
    {
        public float X, Y, Z;
        public uint dummy; // SE 1.10

        public Vertex3f() { }
    }

    public class UVMap
    {
        public UVCoord[] UV;
        public string Name;
    }

    public class UVCoord
    {
        public float U, V;
        public UVCoord() { }
    }

    public class Surface
    {
        public uint FirstVertex,
            VerticeCount,
            TriangleCount,
            Flags;

        public string Name;

        public Shader Shader;

        public Triangle[] Triangles;
        public Triangle32[] Triangles32;

        public byte[] WeightMapIndices;
    }

    public class MorphMap
    {
        public string ID;
        public uint Relative;
        public uint morphSetsCount;

        public MeshVertexMorph[] MeshVertexMorphs;

        public MorphMap() { }
    }

    public class MeshVertexMorph
    {
        public uint VertexID;
        public float X, Y, Z, NX, NY, NZ;
        public uint dummy;

        public MeshVertexMorph() { }
    }

    public class VertexWeight
    {
        public byte[] Indices;
        public byte[] Weights;

        public VertexWeight() { }
    }

    public class WeightMap
    {
        public string Name;
        public uint VertexWeightCount;
        public VertexMapWeight[] VertexMapWeights;
    }

    public class VertexMapWeight
    {
        public uint Index;
        public float Weight;

        public VertexMapWeight() { }
    }

    public class Shader
    {
        public uint ColorCount,
            FloatCount,
            TextureCount,
            TextureCoordsCount,
            Flags;

        public string Name;

        public uint[] Colors, TexCoords;
        public float[] Floats;
        public string[] TexIDs;
    }

    public class Triangle
    {
        public ushort v0, v1, v2;

        public Triangle() { }
    }

    public class Triangle32 // SE 1.10
    {
        public uint v0, v1, v2;

        public Triangle32() { }
    }
}

