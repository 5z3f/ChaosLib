using System.IO;
using ChaosLib.D3D.Structures;

namespace ChaosLib.D3D.Interfaces
{
    public interface IImport
    {
        /// <summary>
        /// Start processing given asset
        /// </summary>
        /// <param name="at">Asset Type</param>
        /// <param name="fp">Full file path</param>
        dynamic BinaryFile(AssetType at, string fp);

        /// <summary>
        /// Define which mesh version to read
        /// </summary>
        /// <param name="fs">File Stream</param>
        CBinaryMesh BinaryMesh(FileStream fs);

        /// <summary>
        /// Read Mesh V17
        /// </summary>
        /// <param name="br">Binary Reader</param>
        /// <param name="bm">Mesh data</param>
        CBinaryMesh ReadMeshV17(BinaryReader br, CBinaryMesh bm);

        /// <summary>
        /// Read Mesh V16
        /// </summary>
        /// <param name="br">Binary Reader</param>
        /// <param name="bm">Mesh data</param>
        CBinaryMesh ReadMeshV16(BinaryReader br, CBinaryMesh bm);

        /// <summary>
        /// Read Mesh v1.10
        /// </summary>
        /// <param name="br">Binary Reader</param>
        /// <param name="bm">Mesh data</param>
        CBinaryMesh ReadMeshV110(BinaryReader br, CBinaryMesh bm);
    }
}
