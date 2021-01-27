using ChaosLib.Map.Classes;

namespace ChaosLib.Map
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
    public enum ContentDataType
    {
        /// <summary>A original 16xx client binary file.</summary>
        Binary,

        /// <summary>A database table.</summary>
        Database,

        /// <summary>A json file. (1/5 gcc)</summary>
        JSON,

        /// <summary>A bson file. (under construction)</summary>
        BSON,

        /// <summary>A zero formatter file. (under construction)</summary>
        ZeroFormatter,

        /// <summary>A messagepack file. (under construction)</summary>
        MSGPACK
    }

    /// <summary>
    /// Allowed types of files that can be readed
    /// </summary>
    public enum ContentType
    {
        /// <summary>Affinity client content</summary>
        Affinity,

        /// <summary>Item client content</summary>
        Item,

        /// <summary>Cobmo client content</summary>
        MonsterCombo,

        /// <summary>Npc client content</summary>
        NPC,

        /// <summary></summary>
        Skill,

        /// <summary></summary>
        Quest,

        /// <summary></summary>
        Action,

        /// <summary>Notice client content</summary>
        Notice,

        /// <summary></summary>
        Option
    }

    public static class ChaosMap
    {
        public static dynamic Import(ContentType ct, string fileDir, ContentDataType cdt) => cdt switch
        {
            ContentDataType.Binary             => CImport.Binary(ct, fileDir),
            //ContentDataType.Database           => Import(contentType, fileDir, cdt),
            //ContentDataType.JSON               => Import(contentType, fileDir, cdt),
            //ContentDataType.BSON               => Import(contentType, fileDir, cdt),
            //ContentDataType.ZeroFormatter      => Import(contentType, fileDir, cdt),
            //ContentDataType.MSGPACK            => Import(contentType, fileDir, cdt),

            _ => null,
        };

        public static void Export(ContentType contentType, string fileDir, ContentDataType contentDataType)
        {

        }
    }
}
