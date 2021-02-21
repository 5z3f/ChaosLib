using BinarySerialization;
using NPoco;

using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CMonsterCombo
    {
        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public MonsterCombo[] Data;
    }

    [TableName("t_missioncase")]
    public class MonsterCombo
    {
        // ---
        // Database: a_smc_file seems to be unused
        // Client: penalty boolean is unused
        // ---

        [FieldOrder(2), Column("a_index")]
        public int ID { get; set; }

        [BinaryIgnore, Column("a_enable")]
        public int Enable { get; set; }

        [FieldOrder(3), Column("a_nas")]
        public int Gold { get; set; }

        [BinaryIgnore, Column("a_smc_file")]
        public string SMCFile { get; set; }

        [FieldOrder(4), Column("a_texture_id")]
        public int IconID { get; set; }

        [FieldOrder(5), Column("a_texture_row")]
        public int IconROW { get; set; }

        [FieldOrder(6), Column("a_texture_col")]
        public int IconCOL { get; set; }

        [FieldOrder(7), SerializeAs(SerializedType.Int1)]
        public bool Penalty
        {
            get => false;
            set { }
        }

        [FieldOrder(8), Column("a_point")]
        public int Points { get; set; }

        // -- language start

        [BinaryIgnore, Column("a_name")]
        public string NameKOR { get; set; }

        [BinaryIgnore, Column("a_name_twn")]
        public string NameTWN { get; set; }

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

        // -- language end
    }

}
