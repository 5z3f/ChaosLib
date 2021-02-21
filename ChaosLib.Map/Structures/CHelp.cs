using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CHelp
    {
        [BinaryIgnore]
        public const string FieldEncoding = "ascii";

        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        public int DataMax
        {
            get => Data.Length; // unlimited
            set { }
        }

        [FieldOrder(2)]
        [FieldCount(nameof(DataLength))]
        public Help[] Data;
    }

    public class Help
    {
        [FieldOrder(3), Column("a_index")]
        public int ID { get; set; }

        [FieldOrder(4)]
        public int NameLength
        {
            get => Name != null ? Name.Length : 0;
            set { }
        }

        [FieldOrder(5), FieldLength(nameof(NameLength)), FieldEncoding(CHelp.FieldEncoding)]
        public string Name { get; set; }

        [FieldOrder(6)]
        public int DescriptionLength
        {
            get => Description != null ? Description.Length : 0;
            set { }
        }

        [FieldOrder(7), FieldLength(nameof(DescriptionLength)), FieldEncoding(CHelp.FieldEncoding)]
        public string Description { get; set; }

        [FieldOrder(8), Column("a_subNum")]
        public int SubNum { get; set; }

        [FieldOrder(9), Column("a_subLevel")]
        public int SubLevel { get; set; }

        [FieldOrder(10)]
        public int HelpPictureLength
        {
            get => HelpPicture != null ? HelpPicture.Length : 0;
            set { }
        }

        [FieldOrder(11), FieldLength(nameof(HelpPictureLength)), FieldEncoding(CHelp.FieldEncoding), Column("a_file")]
        public string HelpPicture { get; set; }

        [FieldOrder(12), Column("a_uv_x")]
        public int UVX { get; set; }

        [FieldOrder(13), Column("a_uv_y")]
        public int UVY { get; set; }

        [FieldOrder(14), Column("a_width")]
        public int Width { get; set; }

        [FieldOrder(15), Column("a_height")]
        public int Height { get; set; }

        // -- language start

        [BinaryIgnore, Column("a_name")]
        public string NameKOR { get; set; }

        [BinaryIgnore, Column("a_name_twn")]
        public string NameTWN { get; set; }

        [BinaryIgnore, Column("a_name_thai_eng")]
        public string NameTHAIENG { get; set; }

        [BinaryIgnore, Column("a_name_thai")]
        public string NameTHAI { get; set; }

        [BinaryIgnore, Column("a_name_jpn")]
        public string NameJPN { get; set; }

        [BinaryIgnore, Column("a_name_mal")]
        public string NameMAL { get; set; }

        [BinaryIgnore, Column("a_name_mal_eng")]
        public string NameMALENG { get; set; }

        [BinaryIgnore, Column("a_name_usa")]
        public string NameUSA { get; set; }

        [BinaryIgnore, Column("a_name_brz")]
        public string NameBRZ { get; set; }

        [BinaryIgnore, Column("a_name_hk")]
        public string NameHK { get; set; }

        [BinaryIgnore, Column("a_name_hk_eng")]
        public string NameHKENG { get; set; }

        [BinaryIgnore, Column("a_name_ger")]
        public string NameGER { get; set; }

        [BinaryIgnore, Column("a_name_spn")]
        public string NameSPN { get; set; }

        [BinaryIgnore, Column("a_name_frc")]
        public string NameFRC { get; set; }

        [BinaryIgnore, Column("a_name_pld")]
        public string NamePLD { get; set; }

        [BinaryIgnore, Column("a_name_rus")]
        public string NameRUS { get; set; }

        [BinaryIgnore, Column("a_name_tur")]
        public string NameTUR { get; set; }

        [BinaryIgnore, Column("a_name_spn2")]
        public string NameSPN2 { get; set; }

        [BinaryIgnore, Column("a_name_frc2")]
        public string NameFRC2 { get; set; }

        [BinaryIgnore, Column("a_name_ita")]
        public string NameITA { get; set; }

        [BinaryIgnore, Column("a_name_mex")]
        public string NameMEX { get; set; }

        [BinaryIgnore, Column("a_name_nld")]
        public string NameNLD { get; set; }

        [BinaryIgnore, Column("a_name_uk")]
        public string NameUK { get; set; }

        [BinaryIgnore, Column("a_name_dev")]
        public string NameDEV { get; set; }

        [BinaryIgnore, Column("a_desc")]
        public string DescriptionKOR { get; set; }

        [BinaryIgnore, Column("a_desc_twn")]
        public string DescriptionTWN { get; set; }

        [BinaryIgnore, Column("a_desc_thai_eng")]
        public string DescriptionTHAIENG { get; set; }

        [BinaryIgnore, Column("a_desc_thai")]
        public string DescriptionTHAI { get; set; }

        [BinaryIgnore, Column("a_desc_jpn")]
        public string DescriptionJPN { get; set; }

        [BinaryIgnore, Column("a_desc_mal")]
        public string DescriptionMAL { get; set; }

        [BinaryIgnore, Column("a_desc_mal_eng")]
        public string DescriptionMALENG { get; set; }

        [BinaryIgnore, Column("a_desc_usa")]
        public string DescriptionUSA { get; set; }

        [BinaryIgnore, Column("a_desc_brz")]
        public string DescriptionBRZ { get; set; }

        [BinaryIgnore, Column("a_desc_hk")]
        public string DescriptionHK { get; set; }

        [BinaryIgnore, Column("a_desc_hk_eng")]
        public string DescriptionHKENG { get; set; }

        [BinaryIgnore, Column("a_desc_ger")]
        public string DescriptionGER { get; set; }

        [BinaryIgnore, Column("a_desc_spn")]
        public string DescriptionSPN { get; set; }

        [BinaryIgnore, Column("a_desc_frc")]
        public string DescriptionFRC { get; set; }

        [BinaryIgnore, Column("a_desc_pld")]
        public string DescriptionPLD { get; set; }

        [BinaryIgnore, Column("a_desc_rus")]
        public string DescriptionRUS { get; set; }

        [BinaryIgnore, Column("a_desc_tur")]
        public string DescriptionTUR { get; set; }

        [BinaryIgnore, Column("a_desc_spn2")]
        public string DescriptionSPN2 { get; set; }

        [BinaryIgnore, Column("a_desc_frc2")]
        public string DescriptionFRC2 { get; set; }

        [BinaryIgnore, Column("a_desc_ita")]
        public string DescriptionITA { get; set; }

        [BinaryIgnore, Column("a_desc_mex")]
        public string DescriptionMEX { get; set; }

        [BinaryIgnore, Column("a_desc_nld")]
        public string DescriptionNLD { get; set; }

        [BinaryIgnore, Column("a_desc_uk")]
        public string DescriptionUK { get; set; }

        [BinaryIgnore, Column("a_desc_dev")]
        public string DescriptionDEV { get; set; }

        // --- language end
    }
}
