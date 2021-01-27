﻿using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;

using Newtonsoft.Json;
using BinarySerialization;

using ChaosLib.D3D.Interfaces;
using ChaosLib.D3D.Structures;

using SharpGLTF.Schema2;
using SharpGLTF.Geometry;
using SharpGLTF.Materials;
using SharpGLTF.Geometry.VertexTypes;

namespace ChaosLib.D3D.Classes
{
    using VERTEX = VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty>;

    class CExport : IExport
    {
        public dynamic BinaryFile(AssetType at, dynamic bm, string fp)
        {
            FileStream fs = File.Create(fp);
            BinaryWriter bw = new BinaryWriter(fs);
            BinarySerializer bs = new BinarySerializer();

            var data = at switch
            {
                AssetType.Mesh => WriteMeshV17(bw, bm),
                AssetType.MeshSE => WriteMeshV110(bw, bm),
                AssetType.Animation => bs.Serialize(fs, bm),
                AssetType.Skeleton => bs.Serialize(fs, bm),

                _ => null,
            };

            bw.Close();
            fs.Close();

            return data;
        }

        private void MakeInformationHeader(string modelName, string fileType, StringBuilder sb)
        {
            sb.Append("# ------- "
              + $"\n# Model: {modelName}"
              + "\n# "
              + $"\n# {fileType} Generated by ChaosLib.D3D"
              + "\n# Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString()
              + "\n# -------"
              + "\n\n");
        }

        public dynamic ASCII(AssetType at, dynamic dataObject, string fp)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            StringBuilder sb = new StringBuilder();

            var data = at switch
            {
                AssetType.Skeleton => SE1_ASCII(at, dataObject, sb),
                AssetType.Mesh => SE1_ASCII(at, dataObject, sb),
                AssetType.Animation => SE1_ASCII(at, dataObject, sb),
                AssetType.Animset => SE1_ASCII(at, dataObject, sb),

                _ => null,
            };

            File.WriteAllText(fp, at is AssetType.Animset 
                ? sb.ToString().Replace("{savedir}", Path.GetDirectoryName(fp)) : sb.ToString());

            return true;
        }

        public StringBuilder SE1_ASCII(AssetType at, dynamic dataObject, StringBuilder sb)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            if (at is AssetType.Skeleton)
            {
                sb.Append("SE_SKELETON 0.1;\n\n");
                sb.Append($"BONES {dataObject.Bones.Length}\n{{\n");

                float[,] matrix = new float[3, 4]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };

                // var sortedBones = CUtils.sortBones(dataObject.Bones); ~ test

                foreach (var bone in dataObject.Bones)
                {
                    sb.Append($"\tNAME \"{bone.Name}\";\n");
                    sb.Append($"\tPARENT \"{bone.ParentID}\";\n");
                    sb.Append($"\tLENGTH {bone.boneLength.ToString("0.000000")};\n");
                    sb.Append("\t{\n");


                    float[,] q2m = CUtils.Q2M(new Quaternion(new Vector3(bone.qRotation[1], bone.qRotation[2], bone.qRotation[3]), bone.qRotation[0]));

                    matrix[0, 0] = q2m[0, 0];
                    matrix[0, 1] = q2m[0, 1];
                    matrix[0, 2] = q2m[0, 2];

                    matrix[1, 0] = q2m[1, 0];
                    matrix[1, 1] = q2m[1, 1];
                    matrix[1, 2] = q2m[1, 2];

                    matrix[2, 0] = q2m[2, 0];
                    matrix[2, 1] = q2m[2, 1];
                    matrix[2, 2] = q2m[2, 2];

                    matrix[0, 3] = bone.vPosition[0];
                    matrix[1, 3] = bone.vPosition[1];
                    matrix[2, 3] = bone.vPosition[2];

                    // yikes
                    string asciiMatrix = JsonConvert.SerializeObject(matrix).Replace("]", "").Replace("[", " ");
                    sb.Append($"\t\t{asciiMatrix};\n");
                    sb.Append("\t}\n");
                }

                sb.Append("}\n\n");
                sb.Append("SE_SKELETON_END;");
            }
            else if (at is AssetType.Mesh)
            {
                // multiple mesh seems to not be supported so lets do it like that for now
                var md = dataObject.Mesh[0];

                sb.Append("SE_MESH 0.1;\n\n");
                sb.Append($"VERTICES {md.VertexMapCount}\n{{\n");

                for (int i = 0; i < md.SurfaceCount; i++)
                {
                    for (uint j = md.Surfaces[i].FirstVertex; j < md.Surfaces[i].FirstVertex + md.Surfaces[i].VerticeCount; ++j)
                    {
                        sb.Append(
                            $"\t{md.Vertices[j].X}," +
                            $"{md.Vertices[j].Y}," +
                            $"{md.Vertices[j].Z};\n");
                    }
                }

                sb.Append("}\n\n");
                sb.Append($"NORMALS {md.VertexMapCount}\n{{\n");

                for (int i = 0; i < md.SurfaceCount; i++)
                {
                    for (uint j = md.Surfaces[i].FirstVertex; j < md.Surfaces[i].FirstVertex + md.Surfaces[i].VerticeCount; ++j)
                    {
                        sb.Append(
                            $"\t{md.Normals[j].X}," +
                            $"{md.Normals[j].Y}," +
                            $"{md.Normals[j].Z};\n");

                    }
                }

                sb.Append("}\n\n");
                sb.Append($"UVMAPS {md.UVMapCount}\n{{\n");

                for (int i = 0; i < md.UVMapCount; i++)
                {
                    sb.Append($"\t{{\n\t\tNAME \"{md.UVMaps[i].Name}\";\n");
                    sb.Append($"\t\tTEXCOORDS {md.VertexMapCount}\n\t\t{{\n");

                    for (int j = 0; j < md.VertexMapCount; j++)
                        sb.Append(
                            $"\t\t\t{md.UVMaps[i].UV[j].U}," +
                            $"{md.UVMaps[i].UV[j].V};\n");

                    sb.Append("\t\t}\n\t}\n}\n");
                }

                sb.Append($"SURFACES {md.SurfaceCount}\n{{\n");

                for (int i = 0; i < md.SurfaceCount; i++)
                {
                    sb.Append($"\t{{\n\t\tNAME \"{md.Surfaces[i].Name}\";\n");
                    sb.Append($"\t\tTRIANGLE_SET {md.Surfaces[i].TriangleCount}\n\t\t{{\n");

                    for (int j = 0; j < md.Surfaces[i].TriangleCount; j++)
                    {
                        int fw = (int)md.Surfaces[i].FirstVertex;
                        sb.Append(
                            $"\t\t\t{md.Surfaces[i].Triangles[j].v0 + fw }," +
                            $"{md.Surfaces[i].Triangles[j].v1 + fw}," +
                            $"{md.Surfaces[i].Triangles[j].v2 + fw};\n");
                    }

                    sb.Append("\t\t}\n\t}\n");
                }

                sb.Append($"\n}}\nWEIGHTS {md.WeightMapCount}\n{{\n");

                for (int i = 0; i < md.WeightMapCount; i++)
                {
                    sb.Append($"\t{{\n\t\tNAME \"{md.WeightMaps[i].Name}\";\n");
                    sb.Append($"\t\tWEIGHT_SET {md.WeightMaps[i].VertexWeightCount}\n\t\t{{\n");

                    for (int j = 0; j < md.WeightMaps[i].VertexWeightCount; j++)
                        sb.Append(
                            $"\t\t\t{{ {md.WeightMaps[i].VertexMapWeights[j].Index};" +
                            $" {md.WeightMaps[i].VertexMapWeights[j].Weight}; }}\n");

                    sb.Append("\t\t}\n\t}\n");
                }

                sb.Append("}\nMORPHS 0\n{\n}\n\n"); // skip
                sb.Append("SE_MESH_END;");
            }
            else if (at is AssetType.Animation)
            {
                var ani = dataObject;

                float[,] matrix = new float[3, 4]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };

                sb.Append("SE_ANIM 0.1;\n\n");
                sb.Append($"SEC_PER_FRAME {ani.FPS};\n");
                sb.Append($"FRAMES {ani.TotalFrames};\n");
                sb.Append($"ANIM_ID \"{ani.Name}\";\n\n");
                sb.Append($"BONEENVELOPES {ani.Bones.Length}\n{{\n");

                foreach (var bone in ani.Bones)
                {
                    sb.Append($"\tNAME \"{bone.Name}\"\n");

                    string asciiDefaultPosition = string.Empty;

                    for (int i = 0; i < bone.DefaultPosition.Length; i++)
                    {
                        asciiDefaultPosition += $"{bone.DefaultPosition[i]:g2},";
                        if ((i > 1) && (i % 4) == 3)
                            asciiDefaultPosition += " ";
                    }

                    sb.Append($"\tDEFAULT_POSE {{{asciiDefaultPosition.Remove(asciiDefaultPosition.Length - 2)};}}\n");
                    sb.Append("\t{\n");

                    var sorted = CUtils.SortByFrame(ani.TotalFrames, bone.Positions, bone.Rotations);
                    for (int i = 0; i < ani.TotalFrames; i++)
                    {
                        if (sorted.rot[i] != null)
                        {
                            float[,] q2m = CUtils.Q2M(new Quaternion(new Vector3(sorted.rot[i].X, sorted.rot[i].Y, sorted.rot[i].Z), sorted.rot[i].W));

                            matrix[0, 0] = q2m[0, 0];
                            matrix[0, 1] = q2m[0, 1];
                            matrix[0, 2] = q2m[0, 2];

                            matrix[1, 0] = q2m[1, 0];
                            matrix[1, 1] = q2m[1, 1];
                            matrix[1, 2] = q2m[1, 2];

                            matrix[2, 0] = q2m[2, 0];
                            matrix[2, 1] = q2m[2, 1];
                            matrix[2, 2] = q2m[2, 2];
                        }

                        if (sorted.pos[i] != null)
                        {
                            matrix[0, 3] = sorted.pos[i].X;
                            matrix[1, 3] = sorted.pos[i].Y;
                            matrix[2, 3] = sorted.pos[i].Z;
                        }

                        string am = JsonConvert.SerializeObject(matrix).Replace("]", "").Replace("[", " "); // wesmart
                        sb.Append($"\t{am};\n");
                    }

                    sb.Append("\t}\n");
                }

                sb.Append("}\n\nMORPHENVELOPES 0\n"); // lc does not use that so lets skip for now
                sb.Append("{\n}\nSE_ANIM_END;");
            }
            else if (at is AssetType.Animset)
            {
                var aniData = dataObject;

                sb.Append("ANIMSETLIST\n{\n");
                for (int i = 0; i < aniData.Animations.Length; i++)
                {
                    var a = aniData.Animations[i];

                    bool isCompressed = Convert.ToBoolean(a.IsCompressed);
                    bool isCustomSpeed = Convert.ToBoolean(a.IsCustomSpeed);
                    string c = isCompressed ? "TRUE" : "FALSE";

                    sb.Append($"\tTRESHOLD {a.Threshold.ToString()};\n");
                    sb.Append($"\tCOMPRESION {c};\n");
                    if (isCustomSpeed) sb.Append($"\tANIMSPEED {a.FPS.ToString()};\n");
                    sb.Append($"\t#INCLUDE \"{{savedir}}\\{aniData.Animations[i].Name}\"\n");
                }

                sb.Append("}\n");
            }
            return sb;
        }

        public dynamic OBJ(dynamic dataObject, string fp)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            StringBuilder sb = new StringBuilder();
            StringBuilder sbmtl = new StringBuilder();

            var meshData = dataObject.Mesh[0];

            MakeInformationHeader(Path.GetFileNameWithoutExtension(meshData.SkaPath), "OBJ", sb);
            MakeInformationHeader(Path.GetFileNameWithoutExtension(meshData.SkaPath), "MTL", sbmtl);

            sb.Append(string.Format("mtllib ./{0}.mtl\n", Path.GetFileNameWithoutExtension(fp)));
            for (int i = 0; i < meshData.SurfaceCount; i++)
            {
                sb.Append($"\no {meshData.Surfaces[i].Name}\n");
                sbmtl.Append($"\nnewmtl surface_{i}\n");
                sbmtl.Append($"map_Kd default.tga\n");
                //sbmtl.Append("Kd 1 1 1\n");
                //sbmtl.Append("Ks 1 1 1\n");

                sb.Append($"usemtl surface_{i}\n");

                for (uint j = meshData.Surfaces[i].FirstVertex; j < meshData.Surfaces[i].FirstVertex + meshData.Surfaces[i].VerticeCount; ++j)
                {
                    float vx = meshData.Vertices[j].X;
                    float vy = meshData.Vertices[j].Y;
                    float vz = meshData.Vertices[j].Z;

                    float nx = meshData.Normals[j].X;
                    float ny = meshData.Normals[j].Y;
                    float nz = meshData.Normals[j].Z;

                    float uvu = meshData.UVMaps[0].UV[j].U;
                    float uvv = meshData.UVMaps[0].UV[j].V;

                    sb.Append($"v {-vx} {vy} {vz}\n");
                    sb.Append($"vn {nx} {ny} {nz}\n");
                    sb.Append($"vt {uvu} {1.0 - uvv}\n");
                }

                for (int j = 0; j < meshData.Surfaces[i].TriangleCount; j++)
                {
                    int fw = (int)meshData.Surfaces[i].FirstVertex + 1;

                    int v0 = meshData.Surfaces[i].Triangles[j].v0 + fw;
                    int v1 = meshData.Surfaces[i].Triangles[j].v1 + fw;
                    int v2 = meshData.Surfaces[i].Triangles[j].v2 + fw;

                    sb.Append(string.Format("f" +
                        " {0}/{0}/{0}" +
                        " {1}/{1}/{1}" +
                        " {2}/{2}/{2}\n", v0, v1, v2));
                }
            }

            File.WriteAllText(fp, sb.ToString());
            File.WriteAllText(fp.Remove(fp.Length - 4) + ".mtl", sbmtl.ToString());

            return true;
        }

        public dynamic glTF(dynamic dataObject, string fp)
        {
            // broken

            var meshData = dataObject.Mesh[0];
            ModelRoot model = ModelRoot.CreateModel();

            for (int i = 0; i < meshData.SurfaceCount; i++)
            {
                var mesh = VERTEX.CreateCompatibleMesh();

                MaterialBuilder material = new MaterialBuilder(meshData.Surfaces[i].Name);
                var primitive = mesh.UsePrimitive(material);

                List<VERTEX> vertices = new List<VERTEX>();

                for (uint j = meshData.Surfaces[i].FirstVertex; j < meshData.Surfaces[i].FirstVertex + meshData.Surfaces[i].VerticeCount; ++j)
                {
                    vertices.Add(new VERTEX()
                        .WithGeometry(new Vector3(meshData.Vertices[j].X, meshData.Vertices[j].Y, meshData.Vertices[j].Z))
                        .WithMaterial(new Vector2(meshData.UVMaps[0].UV[j].U, meshData.UVMaps[0].UV[j].V)));
                    // add normals
                }


                for (int j = 0; j < meshData.Surfaces[i].TriangleCount; j++) // += 3  && [v0 - 0] [v1 - 1] [v2 - 2] || fw + 1
                {
                    int fw = (int)meshData.Surfaces[i].FirstVertex;

                    int v0i = meshData.Surfaces[i].Triangles[j].v0 + fw;
                    int v1i = meshData.Surfaces[i].Triangles[j].v1 + fw;
                    int v2i = meshData.Surfaces[i].Triangles[j].v2 + fw;

                    VERTEX v0 = vertices[(int)meshData.Surfaces[i].Triangles[v0i].v0];
                    VERTEX v1 = vertices[(int)meshData.Surfaces[i].Triangles[v1i].v1];
                    VERTEX v2 = vertices[(int)meshData.Surfaces[i].Triangles[v2i].v2];

                    primitive.AddTriangle(v0, v1, v2);

                    /*
                    int v0value = v0;
                    int v1value = v1;
                    int v2value = v2;



                    var v0g = new VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>((
                        new Vector3(v[v0value].X, v[v0value].Y, v[v0value].Z),
                        new Vector3(vn[v0value].X, vn[v0value].Y, vn[v0value].Z)),
                        new Vector2(uv[v0value].X, uv[v0value].Y));


                    var v1g = new VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>((
                        new Vector3(v[v1value].X, v[v1value].Y, v[v1value].Z),
                        new Vector3(vn[v1value].X, vn[v1value].Y, vn[v1value].Z)),
                        new Vector2(uv[v1value].X, uv[v1value].Y));


                    var v2g = new VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>((
                        new Vector3(v[v2value].X, v[v2value].Y, v[v2value].Z),
                        new Vector3(vn[v2value].X, vn[v2value].Y, vn[v2value].Z)),
                        new Vector2(uv[v2value].X, uv[v2value].Y));

                    //mesh.UsePrimitive(material).AddTriangle(v0g, v1g, v2g);

                    var ver0 = new Vector3(v[v0value].X, v[v0value].Y, v[v0value].Z);
                    var ver1 = new Vector3(v[v1value].X, v[v1value].Y, v[v1value].Z);
                    var ver2 = new Vector3(v[v2value].X, v[v2value].Y, v[v2value].Z);

                    mesh.UsePrimitive(material).AddTriangle(ver0, ver1, ver2);
                    */
                }

                model.CreateMeshes(mesh);
            }

            // create a scene, a node, and assign the first mesh (the terrain)
            model.UseScene("Default")
                .CreateNode().WithMesh(model.LogicalMeshes[0]);

            // save the model as GLTF
            model.SaveGLTF(Environment.CurrentDirectory + $"/test_compra.gltf");

            return true;
        }

        // code from sketch
        #region BINARY_DIRTY_CODE
        private static void BWString(BinaryWriter bw, string str)
        {
            bw.Write(str.Length);
            if (str.Length > 0) bw.Write(Encoding.ASCII.GetBytes(str));
        }
        public dynamic WriteMeshV110(BinaryWriter bw, CBinaryMesh bm)
        {
            bw.Write(Encoding.ASCII.GetBytes(bm.Header.FileTypeMagic));
            bw.Write(CBinaryMesh.MESH_VER12_SE110); // latest ver

            bw.Write(bm.Mesh.Length);
            for (uint i = 0; i < bm.Mesh.Length; i++)
            {
                BWString(bw, bm.Mesh[i].SkaPath);

                bw.Write(bm.Mesh[i].MaxDistance);
                bw.Write(bm.Mesh[i].Flags);

                // vertices
                bw.Write(bm.Mesh[i].VertexMapCount);
                for (int j = 0; j < bm.Mesh[i].VertexMapCount; j++)
                {
                    bw.Write(bm.Mesh[i].Vertices[j].X);
                    bw.Write(bm.Mesh[i].Vertices[j].Y);
                    bw.Write(bm.Mesh[i].Vertices[j].Z);
                    bw.Write(bm.Mesh[i].Vertices[j].dummy);
                }

                // normals
                for (int j = 0; j < bm.Mesh[i].VertexMapCount; j++)
                {
                    bw.Write(bm.Mesh[i].Normals[j].X);
                    bw.Write(bm.Mesh[i].Normals[j].Y);
                    bw.Write(bm.Mesh[i].Normals[j].Z);
                    bw.Write(bm.Mesh[i].Normals[j].dummy);
                }

                // uvmaps
                bw.Write(bm.Mesh[i].UVMapCount);
                for (int j = 0; j < bm.Mesh[i].UVMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].UVMaps[j].Name);

                    for (int k = 0; k < bm.Mesh[i].VertexMapCount; k++)
                    {
                        bw.Write(bm.Mesh[i].UVMaps[j].UV[k].U);
                        bw.Write(bm.Mesh[i].UVMaps[j].UV[k].V);
                    }
                }

                // surfaces
                bw.Write(bm.Mesh[i].SurfaceCount);
                for (int j = 0; j < bm.Mesh[i].SurfaceCount; j++)
                {
                    BWString(bw, bm.Mesh[i].Surfaces[j].Name);

                    bw.Write(bm.Mesh[i].Surfaces[j].FirstVertex);
                    bw.Write(bm.Mesh[i].Surfaces[j].VerticeCount);
                    bw.Write(bm.Mesh[i].Surfaces[j].TriangleCount);

                    for (int k = 0; k < bm.Mesh[i].Surfaces[j].TriangleCount; k++)
                    {
                        if (bm.Mesh[i].Surfaces[j].Triangles32 != null)
                        {
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles32[k].v0);
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles32[k].v1);
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles32[k].v2);
                        }
                        else
                        {
                            bw.Write((uint)bm.Mesh[i].Surfaces[j].Triangles[k].v0);
                            bw.Write((uint)bm.Mesh[i].Surfaces[j].Triangles[k].v1);
                            bw.Write((uint)bm.Mesh[i].Surfaces[j].Triangles[k].v2);
                        }

                    }

                    if (bm.Mesh[i].Surfaces[j].Shader != null)
                    {
                        bw.Write(1); // shaderExists = true
                        bw.Write(bm.Mesh[i].Surfaces[j].Shader.TextureCount);
                        bw.Write(bm.Mesh[i].Surfaces[j].Shader.TextureCoordsCount);
                        bw.Write(bm.Mesh[i].Surfaces[j].Shader.ColorCount);
                        bw.Write(bm.Mesh[i].Surfaces[j].Shader.FloatCount);

                        BWString(bw, bm.Mesh[i].Surfaces[j].Shader.Name);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.TextureCount; k++)
                            BWString(bw, bm.Mesh[i].Surfaces[j].Shader.TexIDs[k]);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.TextureCoordsCount; k++)
                            bw.Write(bm.Mesh[i].Surfaces[j].Shader.TexCoords[k]);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.ColorCount; k++)
                            bw.Write(bm.Mesh[i].Surfaces[j].Shader.Colors[k]);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.FloatCount; k++)
                            bw.Write(bm.Mesh[i].Surfaces[j].Shader.Floats[k]);

                        bw.Write(bm.Mesh[i].Surfaces[j].Shader.Flags);
                    }
                    else
                        bw.Write(0); // shader does not exist
                }

                // weight maps
                bw.Write(bm.Mesh[i].WeightMapCount);
                for (int j = 0; j < bm.Mesh[i].WeightMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].WeightMaps[j].Name);

                    bw.Write(bm.Mesh[i].WeightMaps[j].VertexWeightCount);
                    for (int k = 0; k < bm.Mesh[i].WeightMaps[j].VertexWeightCount; k++)
                    {
                        bw.Write(bm.Mesh[i].WeightMaps[j].VertexMapWeights[k].Index);
                        bw.Write(bm.Mesh[i].WeightMaps[j].VertexMapWeights[k].Weight);
                    }
                }
                
                // morph maps
                bw.Write(bm.Mesh[i].MorphMapCount);
                for (int j = 0; j < bm.Mesh[i].MorphMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].MorphMaps[j].ID);
                    bw.Write(bm.Mesh[i].MorphMaps[j].Relative);

                    bw.Write((uint)bm.Mesh[i].MorphMaps[j].MeshVertexMorphs.Length);
                    for (int k = 0; k < bm.Mesh[i].MorphMaps[j].MeshVertexMorphs.Length; k++)
                    {
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].VertexID);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].X);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].Y);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].Z);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NX);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NY);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NZ);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].dummy);
                    }
                }
                
            }

            return bw;
        }
        public dynamic WriteMeshV17(BinaryWriter bw, CBinaryMesh bm)
        {
            bm.ubChecker = CBinaryMesh.MESH_NEW_VER_LC; // RESET

            bm.Header.FileVersion = CBinaryMesh.MESH_NEW_VER_LC;
            bw.Write(Encoding.ASCII.GetBytes(bm.Header.FileTypeMagic));
            bw.Write(CBinaryMesh.MESH_NEW_VER_LC);

            // mesh size
            long bwSizePos = bw.BaseStream.Length;
            bw.Write(0);

            bw.Write(bm.MeshBitwise(unchecked((uint)bm.Mesh.Length)));
            for (uint i = 0; i < bm.Mesh.Length; i++)
            {
                bw.Write(bm.MeshBitwise(bm.Mesh[i].VertexMapCount));
                bw.Write(bm.MeshBitwise(bm.Mesh[i].WeightMapCount));
                bw.Write(bm.MeshBitwise(bm.Mesh[i].UVMapCount));
                bw.Write(bm.MeshBitwise(bm.Mesh[i].NormalCount));
                bw.Write(bm.MeshBitwise(bm.Mesh[i].SurfaceCount));
                bw.Write(bm.MeshBitwise(bm.Mesh[i].MorphMapCount));

                BWString(bw, bm.Mesh[i].SkaPath);

                bw.Write(bm.Mesh[i].MaxDistance);
                bw.Write(bm.MeshBitwise(bm.Mesh[i].Flags));

                // vertices
                for (int j = 0; j < bm.Mesh[i].NormalCount; j++)
                {
                    bw.Write(bm.Mesh[i].Vertices[j].X);
                    bw.Write(bm.Mesh[i].Vertices[j].Y);
                    bw.Write(bm.Mesh[i].Vertices[j].Z);
                }

                // normals
                for (int j = 0; j < bm.Mesh[i].NormalCount; j++)
                {
                    bw.Write(bm.Mesh[i].Normals[j].X);
                    bw.Write(bm.Mesh[i].Normals[j].Y);
                    bw.Write(bm.Mesh[i].Normals[j].Z);
                }

                // uvmaps
                for (int j = 0; j < bm.Mesh[i].UVMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].UVMaps[j].Name);

                    for (int k = 0; k < bm.Mesh[i].NormalCount; k++)
                    {
                        bw.Write(bm.Mesh[i].UVMaps[j].UV[k].U);
                        bw.Write(bm.Mesh[i].UVMaps[j].UV[k].V);
                    }
                }

                // surfaces
                for (int j = 0; j < bm.Mesh[i].SurfaceCount; j++)
                {
                    bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].FirstVertex));
                    bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].VerticeCount));
                    bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].TriangleCount));

                    for (int k = 0; k < bm.Mesh[i].Surfaces[j].TriangleCount; k++)
                    {
                        if (bm.Mesh[i].Surfaces[j].Triangles32 != null)
                        {
                            bw.Write((ushort)bm.Mesh[i].Surfaces[j].Triangles32[k].v0);
                            bw.Write((ushort)bm.Mesh[i].Surfaces[j].Triangles32[k].v1);
                            bw.Write((ushort)bm.Mesh[i].Surfaces[j].Triangles32[k].v2);
                        }
                        else
                        {
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles[k].v0);
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles[k].v1);
                            bw.Write(bm.Mesh[i].Surfaces[j].Triangles[k].v2);
                        }

                    }

                    BWString(bw, bm.Mesh[i].Surfaces[j].Name);
                    bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Flags));

                    bw.Write(bm.MeshBitwise(unchecked((uint)bm.Mesh[i].Surfaces[j].WeightMapIndices.Length)));
                    bw.Write(bm.Mesh[i].Surfaces[j].WeightMapIndices);

                    if (bm.Mesh[i].Surfaces[j].Shader != null)
                    {
                        bw.Write(bm.MeshBitwise(1)); // shaderExists = true
                        bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.ColorCount));
                        bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.FloatCount));
                        bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.TextureCount));
                        bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.TextureCoordsCount));

                        BWString(bw, bm.Mesh[i].Surfaces[j].Shader.Name);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.TextureCount; k++)
                            BWString(bw, bm.Mesh[i].Surfaces[j].Shader.TexIDs[k]);

                        bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.Flags));

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.ColorCount; k++)
                            bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.Colors[k]));

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.FloatCount; k++)
                            bw.Write(bm.Mesh[i].Surfaces[j].Shader.Floats[k]);

                        for (int k = 0; k < bm.Mesh[i].Surfaces[j].Shader.TextureCoordsCount; k++)
                            bw.Write(bm.MeshBitwise(bm.Mesh[i].Surfaces[j].Shader.TexCoords[k]));
                    }
                    else
                        bw.Write(bm.MeshBitwise(0)); // shader does not exist
                }

                // weight maps
                for (int j = 0; j < bm.Mesh[i].WeightMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].WeightMaps[j].Name);

                    bw.Write(bm.MeshBitwise(bm.Mesh[i].WeightMaps[j].VertexWeightCount));
                    for (int k = 0; k < bm.Mesh[i].WeightMaps[j].VertexWeightCount; k++)
                    {
                        bw.Write(bm.Mesh[i].WeightMaps[j].VertexMapWeights[k].Index);
                        bw.Write(bm.Mesh[i].WeightMaps[j].VertexMapWeights[k].Weight);
                    }
                }

                // morph maps
                for (int j = 0; j < bm.Mesh[i].MorphMapCount; j++)
                {
                    BWString(bw, bm.Mesh[i].MorphMaps[j].ID);
                    bw.Write(bm.MeshBitwise(bm.Mesh[i].MorphMaps[j].Relative));

                    bw.Write(bm.MeshBitwise((uint)bm.Mesh[i].MorphMaps[j].MeshVertexMorphs.Length));
                    for (int k = 0; k < bm.Mesh[i].MorphMaps[j].MeshVertexMorphs.Length; k++)
                    {
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].VertexID);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].X);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].Y);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].Z);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NX);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NY);
                        bw.Write(bm.Mesh[i].MorphMaps[j].MeshVertexMorphs[k].NZ);
                    }
                }


                for (int j = 0; j < bm.Mesh[i].VertexMapCount; j++)
                {
                    bw.Write(bm.Mesh[i].VertexWeights[j].Indices);
                    bw.Write(bm.Mesh[i].VertexWeights[j].Weights);
                }
            }

            long bwCurrentPos = bw.BaseStream.Position - 8; // - Header
            bw.BaseStream.Position = bwSizePos;
            bw.Write((int)bwCurrentPos);

            return bw;
        }
        #endregion
    }
}
