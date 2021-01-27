using System;
using System.IO;
using System.Text;
using BinarySerialization;

using ChaosLib.D3D.Interfaces;
using ChaosLib.D3D.Structures;

namespace ChaosLib.D3D.Classes
{
    class CImport : IImport
    {
        public dynamic BinaryFile(AssetType at, string fp)
        {
            var fs = File.OpenRead(fp);
            var bs = new BinarySerializer();

            return at switch
            {
                AssetType.Mesh => BinaryMesh(fs),
                AssetType.MeshSE => BinaryMesh(fs),
                AssetType.Animation => bs.Deserialize<CBinaryAnimation>(fs),
                AssetType.AnimationEffect => bs.Deserialize<CBinaryAnimationEffect>(fs),
                AssetType.Skeleton => bs.Deserialize<CBinarySkeleton>(fs),

                _ => null,
            };
        }

        // code from sketch
        #region BINARY_DIRTY_CODE
        public CBinaryMesh BinaryMesh(FileStream fs)
        {
            CBinaryMesh cBinaryMesh = new CBinaryMesh();
            using (BinaryReader br = new BinaryReader(fs))
            {
                CMeshHeader cHeader = new CMeshHeader
                {
                    FileTypeMagic = Encoding.ASCII.GetString(br.ReadBytes(4)),
                    FileVersion = br.ReadInt32()
                };

                cBinaryMesh.Header = cHeader;

                if (cHeader.FileTypeMagic != "MESH")
                    throw new ArgumentException($"Header does not match 'MESH' pattern, invalid binary mesh file", nameof(cHeader.FileTypeMagic));

                if (cHeader.FileVersion is not CBinaryMesh.MESH_NEW_VER_LC
                    && cHeader.FileVersion is not CBinaryMesh.MESH_OLD_VER_LC
                    && cHeader.FileVersion is not CBinaryMesh.MESH_VER11_SE110
                    && cHeader.FileVersion is not CBinaryMesh.MESH_VER12_SE110)
                    throw new ArgumentException($"This binary mesh file is not supported by chaoslib.d3d", nameof(cHeader.FileTypeMagic));


                if (cHeader.FileVersion is CBinaryMesh.MESH_NEW_VER_LC or CBinaryMesh.MESH_OLD_VER_LC)
                {
                    int msize = br.ReadInt32();
                    if (msize == 0) throw new ArgumentException($"Mesh chunk is empty, invalid binary mesh file", nameof(msize));
                }

                var mdata = cHeader.FileVersion switch
                {
                    CBinaryMesh.MESH_NEW_VER_LC => ReadMeshV17(br, cBinaryMesh),
                    CBinaryMesh.MESH_OLD_VER_LC => ReadMeshV16(br, cBinaryMesh),
                    CBinaryMesh.MESH_VER12_SE110 => ReadMeshV110(br, cBinaryMesh),
                    CBinaryMesh.MESH_VER11_SE110 => ReadMeshV110(br, cBinaryMesh),

                    _ => null
                };

                return mdata;
            }
        }
        public CBinaryMesh ReadMeshV17(BinaryReader br, CBinaryMesh bm)
        {
            int meshCount = (int)bm.MeshBitwise(br.ReadUInt32());
            bm.Mesh = new cMesh[meshCount];
            for (int i = 0; i < meshCount; i++)
            {
                cMesh cMesh = new cMesh
                {
                    VertexMapCount = bm.MeshBitwise(br.ReadUInt32()),
                    WeightMapCount = bm.MeshBitwise(br.ReadUInt32()),
                    UVMapCount = bm.MeshBitwise(br.ReadUInt32()),
                    NormalCount = bm.MeshBitwise(br.ReadUInt32()),
                    SurfaceCount = bm.MeshBitwise(br.ReadUInt32()),
                    MorphMapCount = bm.MeshBitwise(br.ReadUInt32()),
                    SkaPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                    MaxDistance = br.ReadSingle(),
                    Flags = bm.MeshBitwise(br.ReadUInt32())
                };

                // vertices
                cMesh.Vertices = new Vertex3f[cMesh.NormalCount];
                for (int j = 0; j < cMesh.NormalCount; j++)
                    cMesh.Vertices[j] = new Vertex3f { 
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle() 
                    };

                // normals
                cMesh.Normals = new Vertex3f[cMesh.NormalCount];
                for (int j = 0; j < cMesh.NormalCount; j++)
                    cMesh.Normals[j] = new Vertex3f { 
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle()
                    };

                // uvmaps
                cMesh.UVMaps = new UVMap[cMesh.UVMapCount];
                for (int j = 0; j < cMesh.UVMapCount; j++)
                {
                    UVMap UVMap = new UVMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        UV = new UVCoord[cMesh.NormalCount]
                    };

                    for (int k = 0; k < cMesh.NormalCount; k++)
                        UVMap.UV[k] = new UVCoord { 
                            U = br.ReadSingle(), V = br.ReadSingle() 
                        };

                    cMesh.UVMaps[j] = UVMap;
                }

                // surfaces
                cMesh.Surfaces = new Surface[cMesh.SurfaceCount];
                for (int j = 0; j < cMesh.SurfaceCount; j++)
                {
                    Surface Surface = new Surface
                    {
                        FirstVertex = bm.MeshBitwise(br.ReadUInt32()),
                        VerticeCount = bm.MeshBitwise(br.ReadUInt32()),
                        TriangleCount = bm.MeshBitwise(br.ReadUInt32())
                    };

                    Surface.Triangles = new Triangle[Surface.TriangleCount];
                    for (int k = 0; k < Surface.TriangleCount; k++)
                        Surface.Triangles[k] = new Triangle { 
                            v0 = br.ReadUInt16(), v1 = br.ReadUInt16(), v2 = br.ReadUInt16() 
                        };

                    Surface.Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));
                    Surface.Flags = bm.MeshBitwise(br.ReadUInt32());

                    uint weightMapIndicesCount = bm.MeshBitwise(br.ReadUInt32());
                    if (weightMapIndicesCount > 0)
                    {
                        Surface.WeightMapIndices = new byte[weightMapIndicesCount];
                        for (int k = 0; k < weightMapIndicesCount; k++)
                            Surface.WeightMapIndices[k] = br.ReadByte();
                    }

                    // shader
                    bool shaderExists = Convert.ToBoolean(bm.MeshBitwise(br.ReadUInt32()));
                    if (shaderExists)
                    {
                        Shader Shader = new Shader
                        {
                            ColorCount = bm.MeshBitwise(br.ReadUInt32()),
                            FloatCount = bm.MeshBitwise(br.ReadUInt32()),
                            TextureCount = bm.MeshBitwise(br.ReadUInt32()),
                            TextureCoordsCount = bm.MeshBitwise(br.ReadUInt32()),
                            Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()))
                        };

                        Shader.TexIDs = new string[Shader.TextureCount];
                        for (int k = 0; k < Shader.TextureCount; k++)
                            Shader.TexIDs[k] = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                        Shader.Flags = bm.MeshBitwise(br.ReadUInt32());

                        Shader.Colors = new uint[Shader.ColorCount];
                        for (int k = 0; k < Shader.ColorCount; k++)
                            Shader.Colors[k] = bm.MeshBitwise(br.ReadUInt32());

                        Shader.Floats = new float[Shader.FloatCount];
                        for (int k = 0; k < Shader.FloatCount; k++)
                            Shader.Floats[k] = br.ReadSingle();

                        Shader.TexCoords = new uint[Shader.TextureCoordsCount];
                        for (int k = 0; k < Shader.TextureCoordsCount; k++)
                            Shader.TexCoords[k] = bm.MeshBitwise(br.ReadUInt32());

                        Surface.Shader = Shader;
                    }

                    cMesh.Surfaces[j] = Surface;
                }

                // weight maps
                cMesh.WeightMaps = new WeightMap[cMesh.WeightMapCount];
                for (int j = 0; j < cMesh.WeightMapCount; j++)
                {
                    WeightMap WeightMap = new WeightMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        VertexWeightCount = bm.MeshBitwise(br.ReadUInt32())
                    };

                    WeightMap.VertexMapWeights = new VertexMapWeight[WeightMap.VertexWeightCount];
                    for (int k = 0; k < WeightMap.VertexWeightCount; k++)
                        WeightMap.VertexMapWeights[k] = new VertexMapWeight { 
                            Index = br.ReadUInt32(), Weight = br.ReadSingle() 
                        };

                    cMesh.WeightMaps[j] = WeightMap;
                }

                // morph maps
                cMesh.MorphMaps = new MorphMap[cMesh.MorphMapCount];
                for (int j = 0; j < cMesh.MorphMapCount; j++)
                {
                    cMesh.MorphMaps[j] = new MorphMap
                    {
                        ID = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        Relative = bm.MeshBitwise(br.ReadUInt32())
                    };

                    int morphSetCount = (int)bm.MeshBitwise(br.ReadUInt32());
                    cMesh.MorphMaps[j].MeshVertexMorphs = new MeshVertexMorph[morphSetCount];
                    for (int k = 0; k < morphSetCount; k++)
                    {
                        cMesh.MorphMaps[j].MeshVertexMorphs[k] = new MeshVertexMorph()
                        {
                            VertexID = br.ReadUInt32(),
                            X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle(),
                            NX = br.ReadSingle(), NY = br.ReadSingle(), NZ = br.ReadSingle()
                        };
                    }
                }

                // vertex veights
                cMesh.VertexWeights = new VertexWeight[cMesh.VertexMapCount];
                for (int j = 0; j < cMesh.VertexMapCount; j++)
                {
                    cMesh.VertexWeights[j] = new VertexWeight()
                    {
                        // 4 = max bones per vertex || engine limitation
                        Indices = new byte[4] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() },
                        Weights = new byte[4] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() }
                    };
                }


                bm.Mesh[i] = cMesh;
            }

            return bm;
        }
        public CBinaryMesh ReadMeshV16(BinaryReader br, CBinaryMesh bm)
        {
            int meshCount = (int)br.ReadUInt32();
            bm.Mesh = new cMesh[meshCount];
            for (int i = 0; i < meshCount; i++)
            {
                cMesh cMesh = new cMesh
                {
                    NormalCount = br.ReadUInt32(),
                    UVMapCount = br.ReadUInt32(),
                    SurfaceCount = br.ReadUInt32(),
                    WeightMapCount = br.ReadUInt32(),
                    MorphMapCount = br.ReadUInt32(),
                    VertexMapCount = br.ReadUInt32(),

                    SkaPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                    MaxDistance = br.ReadSingle(),
                    Flags = br.ReadUInt32(),

                };

                // vertices
                cMesh.Vertices = new Vertex3f[cMesh.NormalCount];
                for (int j = 0; j < cMesh.NormalCount; j++)
                    cMesh.Vertices[j] = new Vertex3f { 
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle() 
                    };

                // normals
                cMesh.Normals = new Vertex3f[cMesh.NormalCount];
                for (int j = 0; j < cMesh.NormalCount; j++)
                    cMesh.Normals[j] = new Vertex3f { 
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle() 
                    };

                // uvmaps
                cMesh.UVMaps = new UVMap[cMesh.UVMapCount];
                for (int j = 0; j < cMesh.UVMapCount; j++)
                {
                    UVMap UVMap = new UVMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        UV = new UVCoord[cMesh.NormalCount]
                    };

                    for (int k = 0; k < cMesh.NormalCount; k++)
                        UVMap.UV[k] = new UVCoord { 
                            U = br.ReadSingle(), V = br.ReadSingle() 
                        };

                    cMesh.UVMaps[j] = UVMap;
                }

                // surfaces
                cMesh.Surfaces = new Surface[cMesh.SurfaceCount];
                for (int j = 0; j < cMesh.SurfaceCount; j++)
                {
                    Surface Surface = new Surface
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        Flags = br.ReadUInt32(),
                        FirstVertex = br.ReadUInt32(),
                        VerticeCount = br.ReadUInt32(),
                        TriangleCount = br.ReadUInt32()
                    };

                    Surface.Triangles = new Triangle[Surface.TriangleCount];
                    for (int k = 0; k < Surface.TriangleCount; k++)
                        Surface.Triangles[k] = new Triangle { 
                            v0 = br.ReadUInt16(), v1 = br.ReadUInt16(), v2 = br.ReadUInt16() 
                        };


                    uint weightMapIndicesCount = br.ReadUInt32();
                    if (weightMapIndicesCount > 0)
                    {
                        Surface.WeightMapIndices = new byte[weightMapIndicesCount];
                        for (int k = 0; k < weightMapIndicesCount; k++)
                            Surface.WeightMapIndices[k] = br.ReadByte();
                    }

                    // shader
                    bool shaderExists = Convert.ToBoolean(br.ReadUInt32());
                    if (shaderExists)
                    {
                        Shader Shader = new Shader
                        {
                            TextureCount = br.ReadUInt32(),
                            TextureCoordsCount = br.ReadUInt32(),
                            ColorCount = br.ReadUInt32(),
                            FloatCount = br.ReadUInt32(),
                            Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()))
                        };


                        Shader.TexIDs = new string[Shader.TextureCount];
                        for (int k = 0; k < Shader.TextureCount; k++)
                            Shader.TexIDs[k] = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                        Shader.TexCoords = new uint[Shader.TextureCoordsCount];
                        for (int k = 0; k < Shader.TextureCoordsCount; k++)
                            Shader.TexCoords[k] = br.ReadUInt32();

                        Shader.Colors = new uint[Shader.ColorCount];
                        for (int k = 0; k < Shader.ColorCount; k++)
                            Shader.Colors[k] = br.ReadUInt32();

                        Shader.Floats = new float[Shader.FloatCount];
                        for (int k = 0; k < Shader.FloatCount; k++)
                            Shader.Floats[k] = br.ReadSingle();

                        Shader.Flags = br.ReadUInt32();

                        Surface.Shader = Shader;
                    }

                    cMesh.Surfaces[j] = Surface;
                }

                // weight maps
                cMesh.WeightMaps = new WeightMap[cMesh.WeightMapCount];
                for (int j = 0; j < cMesh.WeightMapCount; j++)
                {
                    WeightMap WeightMap = new WeightMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        VertexWeightCount = br.ReadUInt32()
                    };

                    WeightMap.VertexMapWeights = new VertexMapWeight[WeightMap.VertexWeightCount];
                    for (int k = 0; k < WeightMap.VertexWeightCount; k++)
                        WeightMap.VertexMapWeights[k] = new VertexMapWeight { 
                            Index = br.ReadUInt32(), Weight = br.ReadSingle() 
                        };

                    cMesh.WeightMaps[j] = WeightMap;
                }

                // morph maps
                cMesh.MorphMaps = new MorphMap[cMesh.MorphMapCount];
                for (int j = 0; j < cMesh.MorphMapCount; j++)
                {
                    cMesh.MorphMaps[j] = new MorphMap
                    {
                        ID = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        Relative = br.ReadUInt32()
                    };

                    int morphSetCount = (int)br.ReadUInt32();
                    cMesh.MorphMaps[j].MeshVertexMorphs = new MeshVertexMorph[morphSetCount];
                    for (int k = 0; k < morphSetCount; k++)
                    {
                        cMesh.MorphMaps[j].MeshVertexMorphs[k] = new MeshVertexMorph()
                        {
                            VertexID = br.ReadUInt32(),
                            X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle(),
                            NX = br.ReadSingle(), NY = br.ReadSingle(), NZ = br.ReadSingle()
                        };
                    }
                }

                // vertex veights
                cMesh.VertexWeights = new VertexWeight[cMesh.VertexMapCount];
                for (int j = 0; j < cMesh.VertexMapCount; j++)
                {
                    cMesh.VertexWeights[j] = new VertexWeight()
                    {
                        // 4 = max bones per vertex || engine limitation
                        Indices = new byte[4] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() },
                        Weights = new byte[4] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() }
                    };
                }


                bm.Mesh[i] = cMesh;
            }

            return bm;
        }
        public CBinaryMesh ReadMeshV110(BinaryReader br, CBinaryMesh bm)
        {
            uint meshCount = br.ReadUInt32();
            bm.Mesh = new cMesh[meshCount];
            for (uint i = 0; i < meshCount; i++)
            {
                cMesh cMesh = new cMesh
                {
                    SkaPath = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                    MaxDistance = br.ReadSingle(),
                    Flags = br.ReadUInt32()
                };

                // vertices
                cMesh.VertexMapCount = br.ReadUInt32();
                cMesh.Vertices = new Vertex3f[cMesh.VertexMapCount];
                for (int j = 0; j < cMesh.VertexMapCount; j++)
                    cMesh.Vertices[j] = new Vertex3f()
                    {
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle(),
                        dummy = br.ReadUInt32()
                    };

                // normals
                cMesh.Normals = new Vertex3f[cMesh.VertexMapCount];
                for (int j = 0; j < cMesh.VertexMapCount; j++)
                    cMesh.Normals[j] = new Vertex3f()
                    {
                        X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle(),
                        dummy = br.ReadUInt32()
                    };

                // uvmaps
                cMesh.UVMapCount = br.ReadUInt32();
                cMesh.UVMaps = new UVMap[cMesh.UVMapCount];
                for (int j = 0; j < cMesh.UVMapCount; j++)
                {
                    UVMap UVMap = new UVMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        UV = new UVCoord[cMesh.VertexMapCount]
                    };

                    for (int k = 0; k < cMesh.VertexMapCount; k++)
                        UVMap.UV[k] = new UVCoord {
                            U = br.ReadSingle(), V = br.ReadSingle()
                        };

                    cMesh.UVMaps[j] = UVMap;
                }

                // surfaces
                cMesh.SurfaceCount = br.ReadUInt32();
                cMesh.Surfaces = new Surface[cMesh.SurfaceCount];
                for (int j = 0; j < cMesh.SurfaceCount; j++)
                {
                    Surface Surface = new Surface
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        FirstVertex = br.ReadUInt32(),
                        VerticeCount = br.ReadUInt32(),
                        TriangleCount = br.ReadUInt32()
                    };

                    Surface.Triangles32 = new Triangle32[Surface.TriangleCount];
                    for (int k = 0; k < Surface.TriangleCount; k++)
                        Surface.Triangles32[k] = new Triangle32 {
                            v0 = br.ReadUInt32(), v1 = br.ReadUInt32(), v2 = br.ReadUInt32()
                        };

                    // shader
                    bool shaderExists = Convert.ToBoolean(br.ReadUInt32());
                    if (shaderExists)
                    {
                        Shader Shader = new Shader
                        {
                            TextureCount = br.ReadUInt32(),
                            TextureCoordsCount = br.ReadUInt32(),
                            ColorCount = br.ReadUInt32(),
                            FloatCount = br.ReadUInt32(),
                            Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()))
                        };


                        Shader.TexIDs = new string[Shader.TextureCount];
                        for (int k = 0; k < Shader.TextureCount; k++)
                            Shader.TexIDs[k] = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                        Shader.TexCoords = new uint[Shader.TextureCoordsCount];
                        for (int k = 0; k < Shader.TextureCoordsCount; k++)
                            Shader.TexCoords[k] = br.ReadUInt32();

                        Shader.Colors = new uint[Shader.ColorCount];
                        for (int k = 0; k < Shader.ColorCount; k++)
                            Shader.Colors[k] = br.ReadUInt32();

                        Shader.Floats = new float[Shader.FloatCount];
                        for (int k = 0; k < Shader.FloatCount; k++)
                            Shader.Floats[k] = br.ReadSingle();

                        Shader.Flags = bm.Header.FileVersion is CBinaryMesh.MESH_VER12_SE110 ? br.ReadUInt32() : 0;
                        Surface.Shader = Shader;
                    }

                    cMesh.Surfaces[j] = Surface;
                }

                // weight maps
                cMesh.WeightMapCount = br.ReadUInt32();
                cMesh.WeightMaps = new WeightMap[cMesh.WeightMapCount];
                for (int j = 0; j < cMesh.WeightMapCount; j++)
                {
                    WeightMap WeightMap = new WeightMap
                    {
                        Name = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        VertexWeightCount = br.ReadUInt32()
                    };

                    WeightMap.VertexMapWeights = new VertexMapWeight[WeightMap.VertexWeightCount];
                    for (int k = 0; k < WeightMap.VertexWeightCount; k++)
                        WeightMap.VertexMapWeights[k] = new VertexMapWeight {
                            Index = br.ReadUInt32(), Weight = br.ReadSingle()
                        };


                    cMesh.WeightMaps[j] = WeightMap;
                }

                // morph maps
                cMesh.MorphMaps = new MorphMap[cMesh.MorphMapCount];
                for (int j = 0; j < cMesh.MorphMapCount; j++)
                {
                    cMesh.MorphMaps[j] = new MorphMap
                    {
                        ID = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32())),
                        Relative = br.ReadUInt32()
                    };

                    int morphSetCount = (int)br.ReadUInt32();
                    cMesh.MorphMaps[j].MeshVertexMorphs = new MeshVertexMorph[morphSetCount];
                    for (int k = 0; k < morphSetCount; k++)
                    {
                        cMesh.MorphMaps[j].MeshVertexMorphs[k] = new MeshVertexMorph()
                        {
                            VertexID = br.ReadUInt32(),
                            X = br.ReadSingle(), Y = br.ReadSingle(), Z = br.ReadSingle(),
                            NX = br.ReadSingle(), NY = br.ReadSingle(), NZ = br.ReadSingle(),
                            dummy = br.ReadUInt32()
                        };
                    }
                }

                bm.Mesh[i] = cMesh;
            }

            return bm;
        }
        #endregion
    }
}
