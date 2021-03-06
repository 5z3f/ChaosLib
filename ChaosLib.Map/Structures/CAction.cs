﻿using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CAction
    {
        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public Action[] Data;
    }


    [TableName("t_action")]
    public class Action
    {
        [FieldOrder(2), Column("a_index")]
        public int ID { get; set; }

        [FieldOrder(3), Column("a_type")]
        public byte Type { get; set; }

        [BinaryIgnore]
        public string a_client_ani1 { get; set; }

        [BinaryIgnore]
        public string a_client_ani2 { get; set; }

        [BinaryIgnore]
        public string a_client_ani3 { get; set; }

        [BinaryIgnore]
        public string a_client_ani4 { get; set; }

        [BinaryIgnore]
        public string a_client_ani5 { get; set; }

        [BinaryIgnore]
        public string a_client_ani6 { get; set; }

        [FieldOrder(4), Column("a_job")]
        public int Job { get; set; }

        [FieldOrder(5), Column("a_iconid")]
        public int IconID { get; set; }

        [FieldOrder(6), Column("a_iconrow")]
        public int IconROW { get; set; }

        [FieldOrder(7), Column("a_iconcol")]
        public int IconCOL { get; set; }


        // language start
        [BinaryIgnore, Column("a_name")]
        public string NameKOR { get; set; }

        [BinaryIgnore, Column("a_name_twn")]
        public string NameTWN { get; set; }

        [BinaryIgnore, Column("a_name_chn")]
        public string NameCHN { get; set; }

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

        [BinaryIgnore, Column("a_client_description")]
        public string DescriptionKOR { get; set; }

        [BinaryIgnore, Column("a_client_description_twn")]
        public string DescriptionTWN { get; set; }

        [BinaryIgnore, Column("a_client_description_chn")]
        public string DescriptionCHN { get; set; }

        [BinaryIgnore, Column("a_client_description_thai_eng")]
        public string DescriptionTHAIENG { get; set; }

        [BinaryIgnore, Column("a_client_description_thai")]
        public string DescriptionTHAI { get; set; }

        [BinaryIgnore, Column("a_client_description_jpn")]
        public string DescriptionJPN { get; set; }

        [BinaryIgnore, Column("a_client_description_mal")]
        public string DescriptionMAL { get; set; }

        [BinaryIgnore, Column("a_client_description_mal_eng")]
        public string DescriptionMALENG { get; set; }

        [BinaryIgnore, Column("a_client_description_usa")]
        public string DescriptionUSA { get; set; }

        [BinaryIgnore, Column("a_client_description_brz")]
        public string DescriptionBRZ { get; set; }

        [BinaryIgnore, Column("a_client_description_hk")]
        public string DescriptionHK { get; set; }

        [BinaryIgnore, Column("a_client_description_hk_eng")]
        public string DescriptionHKENG { get; set; }

        [BinaryIgnore, Column("a_client_description_ger")]
        public string DescriptionGER { get; set; }

        [BinaryIgnore, Column("a_client_description_spn")]
        public string DescriptionSPN { get; set; }

        [BinaryIgnore, Column("a_client_description_frc")]
        public string DescriptionFRC { get; set; }

        [BinaryIgnore, Column("a_client_description_pld")]
        public string DescriptionPLD { get; set; }

        [BinaryIgnore, Column("a_client_description_rus")]
        public string DescriptionRUS { get; set; }

        [BinaryIgnore, Column("a_client_description_tur")]
        public string DescriptionTUR { get; set; }

        [BinaryIgnore, Column("a_client_description_spn2")]
        public string DescriptionSPN2 { get; set; }

        [BinaryIgnore, Column("a_client_description_frc2")]
        public string DescriptionFRC2 { get; set; }

        [BinaryIgnore, Column("a_client_description_ita")]
        public string DescriptionITA { get; set; }

        [BinaryIgnore, Column("a_client_description_mex")]
        public string DescriptionMEX { get; set; }

        [BinaryIgnore, Column("a_client_description_nld")]
        public string DescriptionNLD { get; set; }

        [BinaryIgnore, Column("a_client_description_uk")]
        public string DescriptionUK { get; set; }

        [BinaryIgnore, Column("a_client_description_dev")]
        public string DescriptionDEV { get; set; }
        // language end

        // [Reference(ReferenceType.OneToOne)]
        // public CLanguageDescriptionColumn Description { get; set; }

        // [Reference(ReferenceType.OneToOne)]
        // public CLanguageNameColumn Name { get; set; }
    }
}
