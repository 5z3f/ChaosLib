using System;
using NPoco;

using BinarySerialization;
using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    public class CNPC
    {
        public enum Flag : int
        {
            NPC_SHOPPER         = 1 << 0,
            NPC_FIRSTATTACK     = 1 << 1,
            NPC_ATTACK          = 1 << 2,
            NPC_MOVING          = 1 << 3,
            NPC_PEACEFUL        = 1 << 4,
            NPC_ZONEMOVER       = 1 << 5,
            NPC_CASTLE_GUARD    = 1 << 6,
            NPC_REFINER         = 1 << 7,
            NPC_QUEST           = 1 << 8,
            NPC_CASTLE_TOWER    = 1 << 9,
            NPC_MINERAL         = 1 << 10,
            NPC_CROPS           = 1 << 11,
            NPC_ENERGY          = 1 << 12,
            NPC_ETERNAL         = 1 << 13,
            NPC_LORD_SYMBOL     = 1 << 14,
            NPC_REMISSION       = 1 << 15,
            NPC_EVENT           = 1 << 16,
            NPC_GUARD           = 1 << 17,
            NPC_KEEPER          = 1 << 18,
            NPC_GUILD           = 1 << 19,
            NPC_MBOSS           = 1 << 20,
            NPC_BOSS            = 1 << 21,
            NPC_RESETSTAT       = 1 << 22,
            NPC_CHANGEWEAPON    = 1 << 23,
            NPC_WARCASTLE       = 1 << 24,
            NPC_DISPLAY_MAP     = 1 << 25,
            NPC_QUEST_COLLECT   = 1 << 26,
            NPC_PARTY_MOB       = 1 << 27,
            NPC_RAID            = 1 << 28,
            NPC_SUBCITY         = 1 << 29,
            NPC_CHAOCITY        = 1 << 30,
            NPC_HUNTERCITY      = 1 << 31
        }

        public enum Flag1 : int
        {
            NPC_TRADEAGENT      = 1 << 0,
            NPC_COLLSION        = 1 << 1,
            NPC_FACTORY         = 1 << 2,
            NPC_TRIGGER_CHOICE  = 1 << 3,
            NPC_TRIGGER_KILL    = 1 << 4,
            NPC_NOT_NPCPORTAL   = 1 << 5,
            NPC_DONT_ATTACK     = 1 << 6,
            NPC_AFFINITY        = 1 << 7,
            NPC_SHADOW          = 1 << 8,
            NPC_CRAFTING        = 1 << 9,
            NPC_TOTEM_ITEM      = 1 << 10,
        }

        [FieldOrder(0)]
        public int DataLength
        {
            get => Data.Length;
            set { }
        }

        [FieldOrder(1)]
        [FieldCount(nameof(DataLength))]
        public NPC[] Data;

        public const int DEF_SMC_LENGTH = 128;
        public const int DEF_ANI_LENGTH = 64;
    }

    public class NPC
    {
        [FieldOrder(2), Column("a_index")]
        public int ID { get; set; }

        [BinaryIgnore, Column("a_enable")]
        public int Enable { get; set; }

        [FieldOrder(3), Column("a_level")]
        public int Level { get; set; }

        [FieldOrder(4), Column("a_hp")]
        public int Health { get; set; }

        [FieldOrder(5), Column("a_mp")]
        public int Mana { get; set; }

        [FieldOrder(6), Column("a_flag")]
        public int Flag { get; set; }

        [FieldOrder(7), Column("a_flag1")]
        public int Flag1 { get; set; }

        [FieldOrder(8), Column("a_attackSpeed")]
        public int AttackSpeed { get; set; }

        [FieldOrder(9), Column("a_walk_speed")]
        public float WalkSpeed { get; set; }

        [FieldOrder(10), Column("a_run_speed")]
        public float RunSpeed { get; set; }

        [FieldOrder(11), Column("a_scale")]
        public float Scale { get; set; }

        [FieldOrder(12), Column("a_attack_area")]
        public float AttackArea { get; set; }

        [FieldOrder(13), Column("a_size")]
        public float Size { get; set; }

        [BinaryIgnore, Column("a_skillmaster")]
        public int SkillMaster { get; set; }

        [FieldOrder(14)]
        public byte SkillMasterBinary
        {
            get => SkillMaster == -1 ? byte.MaxValue : (byte)SkillMaster;
            set => SkillMaster = (value == byte.MaxValue) ? -1 : value;
        }

        [BinaryIgnore, Column("a_sskill_master")]
        public int SpecialSkillMaster { get; set; }

        [FieldOrder(15)]
        public byte SpecialSkillMasterBinary
        {
            get => SpecialSkillMaster == -1 ? byte.MaxValue : (byte)SpecialSkillMaster;
            set => SpecialSkillMaster = (value is byte.MaxValue) ? -1 : value;
        }

        // ---
        // Needed on file import/export stage, not referenced in database and completly unused
        // tmp.Format("a_skill%d_effect", j);
        // ---

        [FieldOrder(16), FieldCount(5)]
        public int[] EffectUnusedData { get; set; }

        [BinaryIgnore, Column("a_attackType")]
        public int AttackType { get; set; }

        [FieldOrder(17)]
        public byte AttackTypeBinary
        {
            get => AttackType == -1 ? byte.MaxValue : (byte)AttackType;
            set => AttackType = (value is byte.MaxValue) ? -1 : value;
        }

        [FieldOrder(18), Column("a_fireDelayCount"), SerializeAs(SerializedType.UInt1)]
        public int FireDelayCount { get; set; }

        [FieldOrder(19), Column("a_fireDelay0")]
        public float FireDelay0 { get; set; }

        [FieldOrder(20), Column("a_fireDelay1")]
        public float FireDelay1 { get; set; }

        [FieldOrder(21), Column("a_fireDelay2")]
        public float FireDelay2 { get; set; }

        [FieldOrder(22), Column("a_fireDelay3")]
        public float FireDelay3 { get; set; }

        [FieldOrder(23), Column("a_fireObject"), SerializeAs(SerializedType.UInt1)]
        public int FireObject { get; set; }

        [FieldOrder(24), Column("a_fireSpeed")]
        public float FireSpeed { get; set; }

        // SkillID* SkillLevel* is used only on client import/export stage
        // ----------------------------------------------------------------------
        // Space should not appear as the first character because it is a delimiter in these columns
        // You can fix the issue by executing these 4 commands in your database
        // ----------------------------------------------------------------------
        // UPDATE t_npc SET a_skill0 = RIGHT(a_skill0, LENGTH(a_skill0) - 1) WHERE a_skill0 like ' %';
        // UPDATE t_npc SET a_skill1 = RIGHT(a_skill1, LENGTH(a_skill1) - 1) WHERE a_skill1 like ' %';
        // UPDATE t_npc SET a_skill2 = RIGHT(a_skill2, LENGTH(a_skill2) - 1) WHERE a_skill2 like ' %';
        // UPDATE t_npc SET a_skill3 = RIGHT(a_skill3, LENGTH(a_skill3) - 1) WHERE a_skill3 like ' %';

        private int _skillId0, _skillId1 = -1;
        private byte _skillLevel0, _skillLevel1 = 0;
        private int _skillProb0, _skillProb1 = 0;

        [FieldOrder(25)]
        public int SkillID0
        {
            set => _skillId0 = value;
            get => _skillId0;
        }

        [FieldOrder(26)]
        public byte SkillLevel0
        {
            set => _skillLevel0 = value;
            get => _skillLevel0;
        }


        [FieldOrder(27)]
        public int SkillID1
        {
            set => _skillId1 = value;
            get => _skillId1;
        }

        [FieldOrder(28)]
        public byte SkillLevel1
        {
            set => _skillLevel1 = value;
            get => _skillLevel1;
        }

        [BinaryIgnore, Column("a_skill0")]
        public string Skill0
        {
            set
            {
                _skillId0 = (value != string.Empty) ? Convert.ToInt32(value.Split(' ')[0]) : -1;
                _skillLevel0 = (value != string.Empty) ? Convert.ToByte(value.Split(' ')[1]) : 0;
                _skillProb0 = (value != string.Empty) ? Convert.ToInt32(value.Split(' ')[2]) : 0;
            }
            get => _skillId0.ToString() + ' ' + _skillLevel0.ToString() + ' ' + _skillProb0.ToString();
        }

        [BinaryIgnore, Column("a_skill1")]
        public string Skill1
        {
            set
            {
                _skillId1 = (value != string.Empty) ? Convert.ToInt32(value.Split(' ')[0]) : -1;
                _skillLevel1 = (value != string.Empty) ? Convert.ToByte(value.Split(' ')[1]) : 0;
                _skillProb1 = (value != string.Empty) ? Convert.ToInt32(value.Split(' ')[2]) : 0;
            }
            get => _skillId1.ToString() + ' ' + _skillLevel1.ToString() + ' ' + _skillProb1.ToString();
        }

        [BinaryIgnore, Column("a_skill2")]
        public string Skill2 { get; set; }

        [BinaryIgnore, Column("a_skill3")]
        public string Skill3 { get; set; }

        [FieldOrder(29), Column("a_rvr_grade")]
        public int RVRGrade { get; set; }

        [FieldOrder(30), Column("a_rvr_value")]
        public int RVRValue { get; set; }

        [FieldOrder(31), Column("a_bound")]
        public float Bound { get; set; }

        [FieldOrder(32), FieldLength(128), FieldEncoding("ascii"), Column("a_file_smc"),
            SerializeAs(SerializedType.TerminatedString)]
        public string SMC;

        [FieldOrder(33), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_idle"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionIdle;

        [FieldOrder(34), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_walk"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionWalk;

        [FieldOrder(35), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_dam"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionDamage;

        [FieldOrder(36), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_attack"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionAttack;

        [FieldOrder(37), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_die"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionDie;

        [FieldOrder(38), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_run"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionRun;

        [FieldOrder(39), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_idle2"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionIdle2;

        [FieldOrder(40), FieldLength(64), FieldEncoding("ascii"), Column("a_motion_attack2"),
            SerializeAs(SerializedType.TerminatedString)]
        public string MotionAttack2;

        [FieldOrder(41), FieldLength(64), FieldEncoding("ascii"), Column("a_fireEffect0"),
            SerializeAs(SerializedType.TerminatedString)]
        public string FireEffect0;

        [FieldOrder(42), FieldLength(64), FieldEncoding("ascii"), Column("a_fireEffect1"),
            SerializeAs(SerializedType.TerminatedString)]
        public string FireEffect1;

        [FieldOrder(43), FieldLength(64), FieldEncoding("ascii"), Column("a_fireEffect2"),
            SerializeAs(SerializedType.TerminatedString)]
        public string FireEffect2;

        [BinaryIgnore, Column("a_family")]
        public short Family { get; set; }

        [BinaryIgnore, Column("a_state_flag")]
        public int StateFlag { get; set; }

        [BinaryIgnore, Column("a_prize")]
        public int Gold { get; set; }

        [BinaryIgnore, Column("a_sight")]
        public float Sight { get; set; }

        [BinaryIgnore, Column("a_move_area")]
        public short MoveArea { get; set; }

        [BinaryIgnore, Column("a_skill_point")]
        public int SkillPoint { get; set; }

        [BinaryIgnore, Column("a_str")]
        public int Strength { get; set; }

        [BinaryIgnore, Column("a_dex")]
        public int Dexterity { get; set; }

        [BinaryIgnore, Column("a_int")]
        public int Intelligence { get; set; }

        [BinaryIgnore, Column("a_con")]
        public int Constitution { get; set; }

        [BinaryIgnore, Column("a_attack")]
        public int Attack { get; set; }

        [BinaryIgnore, Column("a_magic")]
        public int MagicAttack { get; set; }

        [BinaryIgnore, Column("a_defense")]
        public int Defense { get; set; }

        [BinaryIgnore, Column("a_resist")]
        public int Resistance { get; set; }

        [BinaryIgnore, Column("a_attackLevel")]
        public int AttackLevel { get; set; }

        [BinaryIgnore, Column("a_defenseLevel")]
        public int DefenseLevel { get; set; }

        [BinaryIgnore, Column("a_recover_hp")]
        public int RecoverHP { get; set; }

        [BinaryIgnore, Column("a_recover_mp")]
        public int RecoverMP { get; set; }

        [BinaryIgnore, Column("a_exp")]
        public int Experience { get; set; }

        // -- itemdrop start

        [BinaryIgnore, Column("a_item_0")]
        public int DropItemID0 { get; set; }

        [BinaryIgnore, Column("a_item_1")]
        public int DropItemID1 { get; set; }

        [BinaryIgnore, Column("a_item_2")]
        public int DropItemID2 { get; set; }

        [BinaryIgnore, Column("a_item_3")]
        public int DropItemID3 { get; set; }

        [BinaryIgnore, Column("a_item_4")]
        public int DropItemID4 { get; set; }

        [BinaryIgnore, Column("a_item_5")]
        public int DropItemID5 { get; set; }

        [BinaryIgnore, Column("a_item_6")]
        public int DropItemID6 { get; set; }

        [BinaryIgnore, Column("a_item_7")]
        public int DropItemID7 { get; set; }

        [BinaryIgnore, Column("a_item_8")]
        public int DropItemID8 { get; set; }

        [BinaryIgnore, Column("a_item_9")]
        public int DropItemID9 { get; set; }

        [BinaryIgnore, Column("a_item_10")]
        public int DropItemID10 { get; set; }

        [BinaryIgnore, Column("a_item_11")]
        public int DropItemID11 { get; set; }

        [BinaryIgnore, Column("a_item_12")]
        public int DropItemID12 { get; set; }

        [BinaryIgnore, Column("a_item_13")]
        public int DropItemID13 { get; set; }

        [BinaryIgnore, Column("a_item_14")]
        public int DropItemID14 { get; set; }

        [BinaryIgnore, Column("a_item_15")]
        public int DropItemID15 { get; set; }

        [BinaryIgnore, Column("a_item_16")]
        public int DropItemID16 { get; set; }

        [BinaryIgnore, Column("a_item_17")]
        public int DropItemID17 { get; set; }

        [BinaryIgnore, Column("a_item_18")]
        public int DropItemID18 { get; set; }

        [BinaryIgnore, Column("a_item_19")]
        public int DropItemID19 { get; set; }

        [BinaryIgnore, Column("a_item_percent_0")]
        public int DropItemPercent0 { get; set; }

        [BinaryIgnore, Column("a_item_percent_1")]
        public int DropItemPercent1 { get; set; }

        [BinaryIgnore, Column("a_item_percent_2")]
        public int DropItemPercent2 { get; set; }

        [BinaryIgnore, Column("a_item_percent_3")]
        public int DropItemPercent3 { get; set; }

        [BinaryIgnore, Column("a_item_percent_4")]
        public int DropItemPercent4 { get; set; }

        [BinaryIgnore, Column("a_item_percent_5")]
        public int DropItemPercent5 { get; set; }

        [BinaryIgnore, Column("a_item_percent_6")]
        public int DropItemPercent6 { get; set; }

        [BinaryIgnore, Column("a_item_percent_7")]
        public int DropItemPercent7 { get; set; }

        [BinaryIgnore, Column("a_item_percent_8")]
        public int DropItemPercent8 { get; set; }

        [BinaryIgnore, Column("a_item_percent_9")]
        public int DropItemPercent9 { get; set; }

        [BinaryIgnore, Column("a_item_percent_10")]
        public int DropItemPercent10 { get; set; }

        [BinaryIgnore, Column("a_item_percent_11")]
        public int DropItemPercent11 { get; set; }

        [BinaryIgnore, Column("a_item_percent_12")]
        public int DropItemPercent12 { get; set; }

        [BinaryIgnore, Column("a_item_percent_13")]
        public int DropItemPercent13 { get; set; }

        [BinaryIgnore, Column("a_item_percent_14")]
        public int DropItemPercent14 { get; set; }

        [BinaryIgnore, Column("a_item_percent_15")]
        public int DropItemPercent15 { get; set; }

        [BinaryIgnore, Column("a_item_percent_16")]
        public int DropItemPercent16 { get; set; }

        [BinaryIgnore, Column("a_item_percent_17")]
        public int DropItemPercent17 { get; set; }

        [BinaryIgnore, Column("a_item_percent_18")]
        public int DropItemPercent18 { get; set; }

        [BinaryIgnore, Column("a_item_percent_19")]
        public int DropItemPercent19 { get; set; }

        [BinaryIgnore, Column("a_minplus")]
        public int MinPlus { get; set; }

        [BinaryIgnore, Column("a_maxplus")]
        public int MaxPlus { get; set; }

        [BinaryIgnore, Column("a_probplus")]
        public int ProbPlus { get; set; }

        // -- itemdrop end

        [BinaryIgnore, Column("a_product0")]
        public int Product0 { get; set; }

        [BinaryIgnore, Column("a_product1")]
        public int Product1 { get; set; }

        [BinaryIgnore, Column("a_product2")]
        public int Product2 { get; set; }

        [BinaryIgnore, Column("a_product3")]
        public int Product3 { get; set; }

        [BinaryIgnore, Column("a_product4")]
        public int Product4 { get; set; }

        [BinaryIgnore, Column("a_attribute")]
        public int Attribute { get; set; }

        [BinaryIgnore, Column("a_aitype")]
        public int AIType { get; set; }

        [BinaryIgnore, Column("a_aiflag")]
        public int AIFlag { get; set; }

        [BinaryIgnore, Column("a_aileader_flag")]
        public int AILeaderFlag { get; set; }

        [BinaryIgnore, Column("a_ai_summonHp")]
        public int AISummonHP { get; set; }

        [BinaryIgnore, Column("a_aileader_idx")]
        public int AILeaderID { get; set; }

        [BinaryIgnore, Column("a_aileader_count")]
        public int AILeaderCount { get; set; }

        [BinaryIgnore, Column("a_crafting_category")]
        public int CraftingCategory { get; set; }

        [BinaryIgnore, Column("a_productIndex")]
        public int ProductIndex { get; set; }

        [BinaryIgnore, Column("a_hit")]
        public int Hit { get; set; }

        [BinaryIgnore, Column("a_dodge")]
        public int Dodge { get; set; }

        [BinaryIgnore, Column("a_magicavoid")]
        public int MagicAvoid { get; set; }

        [BinaryIgnore, Column("a_job_attribute")]
        public int JobAttribute { get; set; }

        // ---
        // a_npc_choice*, a_npc_kill* have no references in published sources
        // ---

        [BinaryIgnore, Column("a_npc_choice_trigger_count")]
        public int ChoiceTriggerCount { get; set; }

        [BinaryIgnore, Column("a_npc_choice_trigger_ids")]
        public string ChoiceTriggerIDs { get; set; }

        [BinaryIgnore, Column("a_npc_kill_trigger_count")]
        public int KillTriggerCount { get; set; }

        [BinaryIgnore, Column("a_npc_kill_trigger_ids")]
        public string KillTriggerIDs { get; set; }


        [BinaryIgnore, Column("a_createprob")]
        public int CreateProb { get; set; }

        [BinaryIgnore, Column("a_socketprob_0")]
        public int SocketProb0 { get; set; }

        [BinaryIgnore, Column("a_socketprob_1")]
        public int SocketProb1 { get; set; }

        [BinaryIgnore, Column("a_socketprob_2")]
        public int SocketProb2 { get; set; }

        [BinaryIgnore, Column("a_socketprob_3")]
        public int SocketProb3 { get; set; }

        // -- jewel start

        [BinaryIgnore, Column("a_jewel_0")]
        public int Jewel0 { get; set; }

        [BinaryIgnore, Column("a_jewel_1")]
        public int Jewel1 { get; set; }

        [BinaryIgnore, Column("a_jewel_2")]
        public int Jewel2 { get; set; }

        [BinaryIgnore, Column("a_jewel_3")]
        public int Jewel3 { get; set; }

        [BinaryIgnore, Column("a_jewel_4")]
        public int Jewel4 { get; set; }

        [BinaryIgnore, Column("a_jewel_5")]
        public int Jewel5 { get; set; }

        [BinaryIgnore, Column("a_jewel_6")]
        public int Jewel6 { get; set; }

        [BinaryIgnore, Column("a_jewel_7")]
        public int Jewel7 { get; set; }

        [BinaryIgnore, Column("a_jewel_8")]
        public int Jewel8 { get; set; }

        [BinaryIgnore, Column("a_jewel_9")]
        public int Jewel9 { get; set; }

        [BinaryIgnore, Column("a_jewel_10")]
        public int Jewel10 { get; set; }

        [BinaryIgnore, Column("a_jewel_11")]
        public int Jewel11 { get; set; }

        [BinaryIgnore, Column("a_jewel_12")]
        public int Jewel12 { get; set; }

        [BinaryIgnore, Column("a_jewel_13")]
        public int Jewel13 { get; set; }

        [BinaryIgnore, Column("a_jewel_14")]
        public int Jewel14 { get; set; }

        [BinaryIgnore, Column("a_jewel_15")]
        public int Jewel15 { get; set; }

        [BinaryIgnore, Column("a_jewel_16")]
        public int Jewel16 { get; set; }

        [BinaryIgnore, Column("a_jewel_17")]
        public int Jewel17 { get; set; }

        [BinaryIgnore, Column("a_jewel_18")]
        public int Jewel18 { get; set; }

        [BinaryIgnore, Column("a_jewel_19")]
        public int Jewel19 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_0")]
        public int JewelPercent0 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_1")]
        public int JewelPercent1 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_2")]
        public int JewelPercent2 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_3")]
        public int JewelPercent3 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_4")]
        public int JewelPercent4 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_5")]
        public int JewelPercent5 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_6")]
        public int JewelPercent6 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_7")]
        public int JewelPercent7 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_8")]
        public int JewelPercent8 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_9")]
        public int JewelPercent9 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_10")]
        public int JewelPercent10 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_11")]
        public int JewelPercent11 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_12")]
        public int JewelPercent12 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_13")]
        public int JewelPercent13 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_14")]
        public int JewelPercent14 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_15")]
        public int JewelPercent15 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_16")]
        public int JewelPercent16 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_17")]
        public int JewelPercent17 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_18")]
        public int JewelPercent18 { get; set; }

        [BinaryIgnore, Column("a_jewel_percent_19")]
        public int JewelPercent19 { get; set; }
        // -- jewel end

        [BinaryIgnore, Column("a_zone_flag")]
        public long ZoneFlag { get; set; }

        [BinaryIgnore, Column("a_extra_flag")]
        public long ExtraFlag { get; set; }

        [BinaryIgnore, Column("a_lifetime")]
        public int Lifetime { get; set; }

        // -- language start

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

        [BinaryIgnore, Column("a_descr")]
        public string DescriptionKOR { get; set; }

        [BinaryIgnore, Column("a_descr_twn")]
        public string DescriptionTWN { get; set; }

        [BinaryIgnore, Column("a_descr_chn")]
        public string DescriptionCHN { get; set; }

        [BinaryIgnore, Column("a_descr_thai_eng")]
        public string DescriptionTHAIENG { get; set; }

        [BinaryIgnore, Column("a_descr_thai")]
        public string DescriptionTHAI { get; set; }

        [BinaryIgnore, Column("a_descr_jpn")]
        public string DescriptionJPN { get; set; }

        [BinaryIgnore, Column("a_descr_mal")]
        public string DescriptionMAL { get; set; }

        [BinaryIgnore, Column("a_descr_mal_eng")]
        public string DescriptionMALENG { get; set; }

        [BinaryIgnore, Column("a_descr_usa")]
        public string DescriptionUSA { get; set; }

        [BinaryIgnore, Column("a_descr_brz")]
        public string DescriptionBRZ { get; set; }

        [BinaryIgnore, Column("a_descr_hk")]
        public string DescriptionHK { get; set; }

        [BinaryIgnore, Column("a_descr_hk_eng")]
        public string DescriptionHKENG { get; set; }

        [BinaryIgnore, Column("a_descr_ger")]
        public string DescriptionGER { get; set; }

        [BinaryIgnore, Column("a_descr_spn")]
        public string DescriptionSPN { get; set; }

        [BinaryIgnore, Column("a_descr_frc")]
        public string DescriptionFRC { get; set; }

        [BinaryIgnore, Column("a_descr_pld")]
        public string DescriptionPLD { get; set; }

        [BinaryIgnore, Column("a_descr_rus")]
        public string DescriptionRUS { get; set; }

        [BinaryIgnore, Column("a_descr_tur")]
        public string DescriptionTUR { get; set; }

        [BinaryIgnore, Column("a_descr_spn2")]
        public string DescriptionSPN2 { get; set; }

        [BinaryIgnore, Column("a_descr_frc2")]
        public string DescriptionFRC2 { get; set; }

        [BinaryIgnore, Column("a_descr_ita")]
        public string DescriptionITA { get; set; }

        [BinaryIgnore, Column("a_descr_mex")]
        public string DescriptionMEX { get; set; }

        [BinaryIgnore, Column("a_descr_nld")]
        public string DescriptionNLD { get; set; }

        [BinaryIgnore, Column("a_descr_uk")]
        public string DescriptionUK { get; set; }

        [BinaryIgnore, Column("a_descr_dev")]
        public string DescriptionDEV { get; set; }

        [BinaryIgnore, Column("a_descr_long")]
        public string DescriptionLONG { get; set; }

        // -- language end


        public dynamic GetLocalized(Language lang, string type) => lang switch
        {
            Language.KOR            => type == "name" ? NameKOR : DescriptionKOR,
            Language.TWN            => type == "name" ? NameTWN : DescriptionTWN,
            Language.CHN            => type == "name" ? NameCHN : DescriptionCHN,
            Language.THAI           => type == "name" ? NameTHAI : DescriptionTHAI,
            Language.THAI_ENG       => type == "name" ? NameTHAIENG : DescriptionTHAIENG,
            Language.JPN            => type == "name" ? NameJPN : DescriptionJPN,
            Language.MAL            => type == "name" ? NameMAL : DescriptionMAL,
            Language.MAL_ENG        => type == "name" ? NameMALENG : DescriptionMALENG,
            Language.USA            => type == "name" ? NameUSA : DescriptionUSA,
            Language.BRZ            => type == "name" ? NameBRZ : DescriptionBRZ,
            Language.HK             => type == "name" ? NameHK : DescriptionHK,
            Language.HK_ENG         => type == "name" ? NameHKENG : DescriptionHKENG,
            Language.GER            => type == "name" ? NameGER : DescriptionGER,
            Language.SPN            => type == "name" ? NameSPN : DescriptionSPN,
            Language.FRC            => type == "name" ? NameFRC : DescriptionFRC,
            Language.PLD            => type == "name" ? NamePLD : DescriptionPLD,
            Language.RUS            => type == "name" ? NameRUS : DescriptionRUS,
            Language.TUR            => type == "name" ? NameTUR : DescriptionTUR,
            Language.SPN2           => type == "name" ? NameSPN2 : DescriptionSPN2,
            Language.FRC2           => type == "name" ? NameFRC2 : DescriptionFRC2,
            Language.ITA            => type == "name" ? NameITA : DescriptionITA,
            Language.MEX            => type == "name" ? NameMEX : DescriptionMEX,
            Language.NLD            => type == "name" ? NameNLD : DescriptionNLD,
            Language.UK             => type == "name" ? NameUK : DescriptionUK,
            Language.DEV            => type == "name" ? NameDEV : DescriptionDEV
        };

        public dynamic SetLocalized(Language lang, string type, string value) => lang switch
        {
            Language.KOR            => type == "name" ? NameKOR = value : DescriptionKOR = value,
            Language.TWN            => type == "name" ? NameTWN = value : DescriptionTWN = value,
            Language.CHN            => type == "name" ? NameCHN = value : DescriptionCHN = value,
            Language.THAI           => type == "name" ? NameTHAI = value : DescriptionTHAI = value,
            Language.THAI_ENG       => type == "name" ? NameTHAIENG = value : DescriptionTHAIENG = value,
            Language.JPN            => type == "name" ? NameJPN = value : DescriptionJPN = value,
            Language.MAL            => type == "name" ? NameMAL = value : DescriptionMAL = value,
            Language.MAL_ENG        => type == "name" ? NameMALENG = value : DescriptionMALENG = value,
            Language.USA            => type == "name" ? NameUSA = value : DescriptionUSA = value,
            Language.BRZ            => type == "name" ? NameBRZ = value : DescriptionBRZ = value,
            Language.HK             => type == "name" ? NameHK = value : DescriptionHK = value,
            Language.HK_ENG         => type == "name" ? NameHKENG = value : DescriptionHKENG = value,
            Language.GER            => type == "name" ? NameGER = value : DescriptionGER = value,
            Language.SPN            => type == "name" ? NameSPN = value : DescriptionSPN = value,
            Language.FRC            => type == "name" ? NameFRC = value : DescriptionFRC = value,
            Language.PLD            => type == "name" ? NamePLD = value : DescriptionPLD = value,
            Language.RUS            => type == "name" ? NameRUS = value : DescriptionRUS = value,
            Language.TUR            => type == "name" ? NameTUR = value : DescriptionTUR = value,
            Language.SPN2           => type == "name" ? NameSPN2 = value : DescriptionSPN2 = value,
            Language.FRC2           => type == "name" ? NameFRC2 = value : DescriptionFRC2 = value,
            Language.ITA            => type == "name" ? NameITA = value : DescriptionITA = value,
            Language.MEX            => type == "name" ? NameMEX = value : DescriptionMEX = value,
            Language.NLD            => type == "name" ? NameNLD = value : DescriptionNLD = value,
            Language.UK             => type == "name" ? NameUK = value : DescriptionUK = value,
            Language.DEV            => type == "name" ? NameDEV = value : DescriptionDEV = value
        };
    }
}
