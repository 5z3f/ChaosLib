using ChaosLib.D3D.Structures;
using System.IO;
using System.Text;

namespace ChaosLib.D3D.Interfaces
{
    public interface IExport
    {
        /// <summary>
        /// Start processing given asset
        /// </summary>
        /// <param name="at">Asset type</param>
        /// <param name="dataObject">Data object</param>
        /// <param name="fp">File path</param>
        dynamic BinaryFile(AssetType at, dynamic dataObject, string fp);

        /// <summary>
        /// Start converting given asset
        /// </summary>
        /// <param name="at">Asset type</param>
        /// <param name="dataObject">Data object</param>
        /// <param name="fp">File path</param>
        dynamic ASCII(AssetType at, dynamic dataObject, string fp);

        /// <summary>
        /// Convert data object to ascii and write it into string builder
        /// </summary>
        /// <param name="at">Asset type</param>
        /// <param name="dataObject">Data object</param>
        /// <param name="sb">String builder</param>
        StringBuilder SE1_ASCII(AssetType at, dynamic dataObject, StringBuilder sb);

        /// <summary>
        /// Write Wavefront OBJ
        /// </summary>
        /// <param name="dataObject">Data object</param>
        /// <param name="fp">Full file path</param>
        dynamic OBJ(dynamic dataObject, string fp);

        /// <summary>
        /// Write 1.10 mesh to binary stream
        /// </summary>
        /// <param name="bw">Binary writer</param>
        /// <param name="bm">Mesh data</param>
        dynamic WriteMeshV110(BinaryWriter bw, CBinaryMesh bm);


        /// <summary>
        /// Write mesh to binary stream
        /// </summary>
        /// <param name="bw">Binary writer</param>
        /// <param name="bm">Mesh data</param>
        dynamic WriteMeshV17(BinaryWriter bw, CBinaryMesh bm);
    }
}
