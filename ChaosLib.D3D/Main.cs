using ChaosLib.D3D.Classes;

namespace ChaosLib.D3D
{
    // ------------------------------------------------------------
    // ChaosLib - Last Chaos Library (Alpha, under construction)
    // Written in .NET 5 | C# 9.0
    // ------------------------------------------------------------
    // ChaosLib.D3D        3D Utilites
    // ChaosLib.MAP        Game Data Mapper
    // ------------------------------------------------------------

    /// <summary>
    /// Allowed data types
    /// </summary>
    public enum AssetDataType
    {
        /// <summary>Binary Asset</summary>
        Binary,

        /// <summary>Serious SKA ASCII file</summary>
        ASCII,

        /// <summary>Wavefront 3D Object</summary>
        OBJ,

        /// <summary>gltf test</summary>
        GLTF
    }

    /// <summary>
    /// Allowed file types
    /// </summary>
    public enum AssetType
    {
        /// <summary>Mesh</summary>
        Mesh,

        /// <summary>SE Mesh</summary>
        MeshSE,

        /// <summary>Skeleton</summary>
        Skeleton,

        /// <summary>Animation</summary>
        Animation,

        /// <summary>Animation Effect</summary>
        AnimationEffect,

        /// <summary>ASCII Animation List</summary>
        Animset,

        // <summary>Texture</summary>
        Texture
    }

    public class ChaosAsset
    {
        readonly CImport cImport = new CImport();
        readonly CExport cExport = new CExport();

        public dynamic Import(AssetType at, string fileDir, AssetDataType adt) => adt switch
        {
            AssetDataType.Binary => cImport.BinaryFile(at, fileDir),
            //FileDataType.ASCII => cImport.ASCII(fileType, fileDir, dataType),

            _ => null,
        };

        public dynamic Export(AssetType at, object dataObject, string fp, AssetDataType adt) => adt switch
        {
            AssetDataType.Binary => cExport.BinaryFile(at, dataObject, fp),
            AssetDataType.ASCII => cExport.ASCII(at, dataObject, fp),
            AssetDataType.OBJ => cExport.OBJ(dataObject, fp),
            AssetDataType.GLTF => cExport.glTF(dataObject, fp),

            _ => null,
        };
    }
}
