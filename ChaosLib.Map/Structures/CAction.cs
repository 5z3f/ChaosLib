using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CAction
    {
        [FieldOrder(0)]
        public int DataLength;

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public List<Action> Action;
    }

    public class CLanguage
    {
        public string[] Name;
        public string[] Description;
    }

    public class CActionData
    {
        [FieldOrder(2)]
        public int ID;

        [FieldOrder(3)]
        public byte Type;

        [FieldOrder(4)]
        public int Job;

        [FieldOrder(5)]
        [FieldCount(3)]
        public int[] Icon; // ID | ROW | COL
    }

    [Table("t_action")]
    public class Action
    {
        [FieldOrder(2)]
        [Column("a_index")]
        public int ID;

        [FieldOrder(3)]
        [Column("a_type")]
        public byte Type;

        [Ignore]
        [Column("a_name")]
        public CLanguage Name;

        [Ignore]
        public string a_name_twn;

        [Ignore]
        public string a_name_chn;

        [Ignore]
        public string a_name_thai_eng;

        [Ignore]
        public string a_name_thai;

        [Ignore]
        public string a_client_description;

        [Ignore]
        public string a_client_description_twn;

        [Ignore]
        public string a_client_description_chn;

        [Ignore] 
        public string a_client_description_thai_eng;

        [Ignore]
        public string a_client_description_thai;

        [Ignore]
        public string a_client_ani1;

        [Ignore]
        public string a_client_ani2;

        [Ignore]
        public string a_client_ani3;

        [Ignore]
        public string a_client_ani4;

        [Ignore]
        public string a_client_ani5;

        [Ignore]
        public string a_client_ani6;

        [FieldOrder(4)]
        public int a_job;

        [FieldOrder(5)]
        public int a_iconid;

        [FieldOrder(6)]
        public int a_iconrow;

        [FieldOrder(7)]
        public int a_iconcol;

        [Ignore]
        public string a_name_jpn;

        [Ignore]
        public string a_client_description_jpn;

        [Ignore]
        public string a_name_mal;

        [Ignore]
        public string a_client_description_mal;

        [Ignore]
        public string a_name_mal_eng;

        [Ignore]
        public string a_client_description_mal_eng;

        [Ignore]
        public string a_name_usa;

        [Ignore]
        public string a_client_description_usa;

        [Ignore]
        public string a_name_brz;

        [Ignore]
        public string a_client_description_brz;

        [Ignore]
        public string a_name_hk;

        [Ignore]
        public string a_client_description_hk;

        [Ignore]
        public string a_client_description_hk_eng;

        [Ignore]
        public string a_name_hk_eng;

        [Ignore]
        public string a_name_ger;

        [Ignore]
        public string a_client_description_ger;

        [Ignore]
        public string a_name_spn;

        [Ignore]
        public string a_client_description_spn;

        [Ignore]
        public string a_name_frc;

        [Ignore]
        public string a_client_description_frc;

        [Ignore]
        public string a_name_pld;

        [Ignore]
        public string a_client_description_pld;

        [Ignore]
        public string a_name_rus;

        [Ignore]
        public string a_client_description_rus;

        [Ignore]
        public string a_name_tur;

        [Ignore]
        public string a_client_description_tur;

        [Ignore]
        public string a_name_spn2;

        [Ignore]
        public string a_client_description_spn2;

        [Ignore]
        public string a_name_frc2;

        [Ignore]
        public string a_client_description_frc2;

        [Ignore]
        public string a_name_ita;

        [Ignore]
        public string a_client_description_ita;

        [Ignore]
        public string a_name_mex;

        [Ignore]
        public string a_client_description_mex;

        [Ignore]
        public string a_name_nld;

        [Ignore]
        public string a_client_description_nld;

        [Ignore]
        public string a_name_uk;

        [Ignore]
        public string a_client_description_uk;

        [Ignore]
        public string a_name_dev;

        [Ignore]
        public string a_client_description_dev;
    }
}
