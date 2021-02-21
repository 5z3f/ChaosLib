using BinarySerialization;
using System.IO;
using ChaosLib.Map.Structures;
using System.Dynamic;
using System.Drawing;
using System.Text;
using System;
using System.Numerics;

namespace ChaosLib.Map.Classes
{
    class CImport
    {
        public object Binary(ContentType ct, string fp, dynamic additional)
        {
            var fs = File.OpenRead(fp);
            var bs = new BinarySerializer();

            if (ct is ContentType.ServerAttributeMap)
            {
                int height = additional.Height;
                int width = additional.Width;

                dynamic AttributeMap = additional.IsNew ? new ushort[height, width] : new byte[height, width];

                using (BinaryReader br = new BinaryReader(fs))
                {
                    for (int w = 0; w < width; w++)
                        for (int h = 0; h < height; h++)
                            if (additional.IsNew) AttributeMap[h, w] = br.ReadUInt16();
                            else AttributeMap[h, w] = br.ReadByte();
                }

                dynamic data = new ExpandoObject();
                var colorizedMergedAttributes = CAttributeMap.ColorizeAttributes(AttributeMap, width, height);

                data.AttributeMap = AttributeMap;

                if (additional.SeparateLayers)
                {
                    var colorizedAttributes = CAttributeMap.ColorizeAttributesAndKeepLayers(AttributeMap, width, height);

                    data.Layers = new ExpandoObject();
                    data.Layers.MATT_WALKABLE = colorizedAttributes[0];
                    data.Layers.MATT_UNWALKABLE = colorizedAttributes[1];
                    data.Layers.MATT_PEACE = colorizedAttributes[2];
                    data.Layers.MATT_FREEPKZONE = colorizedAttributes[3];
                    data.Layers.MATT_WAR = colorizedAttributes[4];
                    data.Layers.MATT_STAIR_UP = colorizedAttributes[5];
                    data.Layers.MATT_STAIR_DOWN = colorizedAttributes[6];
                    data.Layers.MATT_PRODUCT_PUBLIC = colorizedAttributes[7];
                    data.Layers.MATT_PRODUCT_PRIVATE = colorizedAttributes[8];
                    data.Layers.Merged = CUtils.CreateBitmap(width, height, colorizedMergedAttributes);
                }
                else
                    data.Layers.Merged = CUtils.CreateBitmap(width, height, colorizedMergedAttributes);


                return data;
            }
            else if (ct is ContentType.ServerHeightMap)
            {
                int height = additional.height;
                int width = additional.width;

                var HeightMap = new ushort[height, width];

                using (BinaryReader br = new BinaryReader(fs))
                {
                    BigEndianReader ber = new BigEndianReader(br);
                    for (int w = 0; w < width; w++)
                        for (int h = 0; h < height; h++)
                            HeightMap[h, w] = ber.ReadUInt16();
                }

                dynamic data = new ExpandoObject();
                data.HeightMap = HeightMap;

                return data;
            }

            return ct switch
            {
                ContentType.Action => bs.Deserialize<CAction>(fs),
                ContentType.NPC => bs.Deserialize<CNPC>(fs),
                ContentType.Title => bs.Deserialize<CTitle>(fs),
                ContentType.MonsterCombo => bs.Deserialize<CMonsterCombo>(fs),
                ContentType.LevelGuide => bs.Deserialize<CLevelGuide>(fs),
                ContentType.ItemExchange => bs.Deserialize<CItemExchange>(fs),
                ContentType.Help => bs.Deserialize<CHelp>(fs),
                ContentType.Map => bs.Deserialize<CMap>(fs),
                ContentType.ArmorPreview => bs.Deserialize<CArmorPreview>(fs),
                ContentType.WorldTerrain => ReadWorldTerrain(fs, colorizeAttributes: additional ?? false)
            };
        }

        public dynamic ReadWorldTerrain(FileStream fs, bool colorizeAttributes = false)
        {
            CWorldTerrain wt = new CWorldTerrain();
            using (BinaryReader br = new BinaryReader(fs))
            {
                CTerrainHeader h = new CTerrainHeader
                {
                    FileTypeMagic = Encoding.ASCII.GetString(br.ReadBytes(4)),
                };

                wt.Header = h;

                if (h.FileTypeMagic is not "TERR" and not "TRAR")
                    throw new ArgumentException($"Header does not match 'TERR' / 'TRAR' pattern, invalid world terrain file", nameof(h.FileTypeMagic));

                if (h.FileTypeMagic is "TERR")
                {
                    // file is old
                }
                else
                {
                    h.TerrainCount = br.ReadInt32();
                    wt.EOTAorVersionMagic = Encoding.ASCII.GetString(br.ReadBytes(4)); // TRVR

                    if (wt.EOTAorVersionMagic is "EOTA")
                        throw new ArgumentException($"Terrain file is empty", nameof(wt.EOTAorVersionMagic));
                    else if (wt.EOTAorVersionMagic is not "TRVR")
                        throw new ArgumentException($"Header does not match 'TRVR' pattern, invalid world terrain file", nameof(wt.EOTAorVersionMagic));

                    h.FileVersion = br.ReadInt32();

                    if (h.FileVersion >= 19 || h.FileVersion is 17 or 18) 
                        ReadTerrainV17(br, wt, h.FileVersion, colorizeAttributes);
                   // else if (h.FileVersion == 16) ReadTerrainV16(br, wt);
                    else 
                        throw new ArgumentException($"This world terrain file is not supported by chaoslib.map", nameof(h.FileVersion));
                }
            }

            return wt;
        }


        public void ReadTerrainV17(BinaryReader br, CWorldTerrain wt, int FileVersion, bool colorizeAttributes)
        {
            wt.GlobalDataMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.GlobalDataMagic is not "TRGD")
                throw new ArgumentException($"Header does not match 'TRGD' pattern, invalid world terrain file", nameof(wt.GlobalDataMagic));

            wt.HeightMapWidth = br.ReadInt32();
            wt.HeightMapHeight = br.ReadInt32();

            wt.TopMapWidth = br.ReadInt32();
            wt.TopMapHeight = br.ReadInt32();
            wt.ShadowMapSizeAspect = br.ReadInt32();
            wt.ShadingMapSizeAspect = br.ReadInt32();
            wt.AttributeMapSizeAspect = Convert.ToBoolean(wt.Header.FileVersion >= 19) ? br.ReadInt32() : 0;

            wt.FirstTopMapLOD = br.ReadInt32();
            wt.LayerCount = br.ReadInt32();
            wt.ShadowMapCount = br.ReadInt32();
            wt.DistFactor = br.ReadSingle();

            wt.Stretch = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            wt.MetricSize = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            // -- height map
            wt.HeightMapMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.HeightMapMagic is not "TRHM")
                throw new ArgumentException($"Global data chunk does not match 'TRHM' pattern, invalid world terrain file", nameof(wt.HeightMapMagic));

            wt.HeightMap = new ushort[wt.HeightMapWidth, wt.HeightMapHeight];

            for (int w = 0; w < wt.HeightMapWidth; w++)
                for (int h = 0; h < wt.HeightMapHeight; h++)
                    wt.HeightMap[w, h] = br.ReadUInt16();

            // -- edge map
            wt.EdgeMapMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.EdgeMapMagic is not "TREM")
                throw new ArgumentException($"Global data chunk does not match 'TREM' pattern, invalid world terrain file", nameof(wt.EdgeMapMagic));

            wt.EdgeMap = br.ReadBytes(wt.EdgeMapSize);

            if (Encoding.ASCII.GetString(br.ReadBytes(4)) is not "DFNM")
                throw new ArgumentException($"Global data chunk does not match 'DFNM' pattern, invalid world terrain file");

            wt.EdgeMapFileName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));


            // -- shadow map
            wt.ShadowMapMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.ShadowMapMagic is not "TRSM")
                throw new ArgumentException($"Global data chunk does not match 'TRSM' pattern, invalid world terrain file", nameof(wt.ShadowMapMagic));

            wt.Shadow = new CTerrainShadow[wt.ShadowMapCount];
            for (int i = 0; i < wt.ShadowMapCount; i++)
            {
                CTerrainShadow ts = new CTerrainShadow
                {
                    ShadowTimes = br.ReadUInt32(),
                    BlurRadius = br.ReadSingle(),
                    ObjectColor = br.ReadBytes(4)
                };

                if (Encoding.ASCII.GetString(br.ReadBytes(4)) is not "DFNM")
                    throw new ArgumentException($"Global data chunk does not match 'DFNM' pattern, invalid world terrain file");

                ts.ShadowMapFileName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                if (i + 1 == wt.ShadowMapCount)
                    ts.ShadowTimes = br.ReadUInt32();

                wt.Shadow[i] = ts;
            }

            wt.ShadowOverbright = br.ReadSingle();

            // -- top map
            wt.TopMapMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.TopMapMagic is not "TRTM")
                throw new ArgumentException($"Global data chunk does not match 'TRTM' pattern, invalid world terrain file", nameof(wt.TopMapMagic));

            if (Encoding.ASCII.GetString(br.ReadBytes(4)) is not "DFNM")
                throw new ArgumentException($"Global data chunk does not match 'DFNM' pattern, invalid world terrain file");

            wt.TopMapFileName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

            // -- layers
            wt.Layer = new CTerrainLayer[wt.LayerCount];
            for (int i = 0; i < wt.LayerCount; i++)
            {
                CTerrainLayer tl = new CTerrainLayer
                {
                    LayerTextureMagic = Encoding.ASCII.GetString(br.ReadBytes(4))
                };

                if (tl.LayerTextureMagic is not "TRLT")
                    throw new ArgumentException($"Global data chunk does not match 'TRLT' pattern, invalid world terrain file", nameof(tl.LayerMagic));

                if (Encoding.ASCII.GetString(br.ReadBytes(4)) is not "DFNM")
                    throw new ArgumentException($"Global data chunk does not match 'DFNM' pattern, invalid world terrain file");

                tl.LayerFileName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

                tl.LayerVersionMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (tl.LayerVersionMagic is not "TRLV")
                    throw new ArgumentException($"Global data chunk does not match 'TRLV' pattern, invalid world terrain file", nameof(tl.LayerVersion));

                tl.LayerVersion = br.ReadInt32();

                dynamic LayerData = null;
                if (tl.LayerVersion is 15)
                    LayerData = ReadTerrainLayerV15(br, tl, wt.HeightMapWidth, wt.HeightMapHeight);

                wt.Layer[i] = LayerData;
            }

            if (FileVersion >= 19)
            {
                wt.AttributeMapMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (wt.AttributeMapMagic is not "TRAM")
                    throw new ArgumentException($"Global data chunk does not match 'TRAM' pattern, invalid world terrain file", nameof(wt.AttributeMapMagic));

                wt.AttributeMap = (FileVersion is 19) ? new byte[wt.AttributeMapWidth, wt.AttributeMapHeight] 
                    : new ushort[wt.AttributeMapWidth, wt.AttributeMapHeight];

                for (int w = 0; w < wt.AttributeMapWidth; w++)
                    for (int h = 0; h < wt.AttributeMapHeight; h++)
                        if(FileVersion is 19)   wt.AttributeMap[h, w] = br.ReadByte();
                        else                    wt.AttributeMap[h, w] = br.ReadUInt16();

                if (colorizeAttributes)
                {
                    dynamic attributeData = new ExpandoObject();
                    var colorizedAttributes = CAttributeMap.ColorizeAttributesAndKeepLayers(wt.AttributeMap, wt.AttributeMapWidth, wt.AttributeMapHeight);
                    var colorizedMergedAttributes = CAttributeMap.ColorizeAttributes(wt.AttributeMap, wt.AttributeMapWidth, wt.AttributeMapHeight);

                    attributeData.Layers = new ExpandoObject();
                    attributeData.Layers.MATT_WALKABLE = colorizedAttributes[0];
                    attributeData.Layers.MATT_UNWALKABLE = colorizedAttributes[1];
                    attributeData.Layers.MATT_PEACE = colorizedAttributes[2];
                    attributeData.Layers.MATT_FREEPKZONE = colorizedAttributes[3];
                    attributeData.Layers.MATT_WAR = colorizedAttributes[4];
                    attributeData.Layers.MATT_STAIR_UP = colorizedAttributes[5];
                    attributeData.Layers.MATT_STAIR_DOWN = colorizedAttributes[6];
                    attributeData.Layers.MATT_PRODUCT_PUBLIC = colorizedAttributes[7];
                    attributeData.Layers.MATT_PRODUCT_PRIVATE = colorizedAttributes[8];

                    attributeData.Layers.Merged = CUtils.CreateBitmap(wt.AttributeMapWidth, wt.AttributeMapHeight, colorizedMergedAttributes);

                    wt.AttributeBitmap = attributeData;
                }
                else
                    wt.AttributeBitmap = null;
            }

            wt.DataEndMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (wt.DataEndMagic is not "TRDE")
                throw new ArgumentException($"Global data chunk does not match 'TRDE' pattern, invalid world terrain file", nameof(wt.DataEndMagic));
        }

        private CTerrainLayer ReadTerrainLayerV15(BinaryReader br, CTerrainLayer tl, int w, int h)
        {
            tl.LayerDataMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (tl.LayerDataMagic is not "TRLG")
                throw new ArgumentException($"Layer data chunk does not match 'TRLG' pattern, invalid world terrain file", nameof(tl.LayerDataMagic));

            tl.LayerName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

            tl.IsLayerVisible = br.ReadInt32(); // boolean
            tl.LayerType = br.ReadInt32(); // TerrainLayerType
            tl.Multiply = br.ReadInt32();
            tl.Flags = br.ReadInt32();
            tl.MaskStretch = br.ReadInt32();
            tl.SoundIndex = br.ReadInt32();

            tl.LayerMaskWidth = (w - 1) << tl.MaskStretch;
            tl.LayerMaskHeight = (h - 1) << tl.MaskStretch;
            tl.LayerMaskSize = tl.LayerMaskWidth * tl.LayerMaskHeight;

            // layer transformation
            tl.OffsetX = br.ReadSingle();
            tl.OffsetY = br.ReadSingle();

            tl.RotateX = br.ReadSingle();
            tl.RotateY = br.ReadSingle();

            tl.StretchX = br.ReadSingle();
            tl.StretchY = br.ReadSingle();

            // layer distribution
            tl.IsAutoRegenerated = br.ReadInt32(); // boolean

            tl.Coverage = br.ReadSingle();
            tl.CoverageNoise = br.ReadSingle();
            tl.CoverageRandom = br.ReadSingle();

            // -- altitude
            tl.ApplyMinAltitude = br.ReadInt32(); // boolean
            tl.ApplyMaxAltitude = br.ReadInt32(); // boolean

            tl.MinAltitude = br.ReadSingle();
            tl.MaxAltitude = br.ReadSingle();

            tl.MinAltitudeFade = br.ReadSingle();
            tl.MaxAltitudeFade = br.ReadSingle();

            tl.MinAltitudeNoise = br.ReadSingle();
            tl.MaxAltitudeNoise = br.ReadSingle();

            tl.MinAltitudeRandom = br.ReadSingle();
            tl.MaxAltitudeRandom = br.ReadSingle();

            // -- slope
            tl.ApplyMinSlope = br.ReadInt32(); // boolean
            tl.ApplyMaxSlope = br.ReadInt32(); // boolean

            tl.MinSlope = br.ReadSingle();
            tl.MaxSlope = br.ReadSingle();

            tl.MinSlopeFade = br.ReadSingle();
            tl.MaxSlopeFade = br.ReadSingle();

            tl.MinSlopeNoise = br.ReadSingle();
            tl.MaxSlopeNoise = br.ReadSingle();

            tl.MinSlopeRandom = br.ReadSingle();
            tl.MaxSlopeRandom = br.ReadSingle();

            tl.LayerMaskMagic = Encoding.ASCII.GetString(br.ReadBytes(4));
            if (tl.LayerMaskMagic is not "TRLM")
                throw new ArgumentException($"Layer data chunk does not match 'TRLM' pattern, invalid world terrain file", nameof(tl.LayerMaskMagic));

            tl.LayerMask = br.ReadBytes(tl.LayerMaskSize);

            if (Encoding.ASCII.GetString(br.ReadBytes(4)) is not "DFNM")
                throw new ArgumentException($"Global data chunk does not match 'DFNM' pattern, invalid world terrain file");

            tl.LayerAlphaMaskTextureName = Encoding.ASCII.GetString(br.ReadBytes(br.ReadInt32()));

            return tl;
        }

        public string MakeLanguageQuery(ContentType ct, string columnName, bool json = false)
        {
            string query = string.Empty;

            if (json) query += ", CONCAT('{'";
            for (int i = 0; i < System.Enum.GetValues(typeof(Language)).Length; i++)
            {
                // help1 does not have  language
                if (i is (int)Language.CHN && ct is ContentType.Help)
                    continue;

                // monster combo does not have chn & thai_eng language locales
                if (i is (int)Language.CHN or (int)Language.THAI_ENG && ct is ContentType.MonsterCombo)
                    continue;

                string language = (i == 0) ? "kor" : System.Enum.GetName(typeof(Language), i).ToLower();

                string localized = $"{columnName}_" + language;
                string value = (i == 0) ? columnName : localized;

                query += json 
                    ? ", '\"" + language + "\":', '\"', " + value + ", '\", '" 
                    : $", {value}";
            }
            if (ct is ContentType.NPC && columnName is "a_descr") query += ", a_descr_long";
            if (json) query += ", '}') as " + columnName;

            return query;
        }

        public string MakeFetchQuery(ContentType ct)
        {
            string query = ct switch
            {
                ContentType.Action =>
                    "SELECT a_index, a_type, a_job,"
                        + " a_client_ani1, a_client_ani2, a_client_ani3, a_client_ani4, a_client_ani5, a_client_ani6,"
                        + " a_iconid, a_iconrow, a_iconcol" 
                        + MakeLanguageQuery(ct, "a_name") 
                        + MakeLanguageQuery(ct, "a_client_description") 
                        + " FROM t_action ORDER BY a_index;",

                ContentType.Title =>
                    "SELECT a_index, a_enable, a_effect_name, a_attack, a_damage, a_time, a_bgcolor, a_color,"
                        + " a_option_index0, a_option_index1, a_option_index2, a_option_index3, a_option_index4,"
                        + " a_option_level0, a_option_level1, a_option_level2, a_option_level3, a_option_level4,"
                        + " a_item_index, a_flag, a_castle_num, a_name, a_describe FROM t_title ORDER BY a_index;",

                ContentType.MonsterCombo =>
                    "SELECT a_index, a_enable, a_nas, a_smc_file, a_texture_id, a_texture_col, a_texture_row,"
                        + " a_point"
                        + MakeLanguageQuery(ct, "a_name")
                        + " FROM t_missioncase ORDER BY a_index;",

                ContentType.ItemExchange =>
                    "SELECT a_index, a_enable, a_npc_index, result_itemIndex, result_itemCount, source_itemIndex0,"
                        + " source_itemCount0, source_itemIndex1, source_itemCount1, source_itemIndex2, source_itemCount2,"
                        + " source_itemIndex3, source_itemCount3, source_itemIndex4, source_itemCount4, a_name, a_desc"
                        + " FROM t_item_exchange ORDER BY a_index;",

                ContentType.Help =>
                    "SELECT a_index, a_subNum, a_subLevel, a_file, a_uv_x, a_uv_y, a_width, a_height"
                        + MakeLanguageQuery(ct, "a_name")
                        + MakeLanguageQuery(ct, "a_desc")
                        + " FROM t_help1 ORDER BY a_index;",

                ContentType.NPC =>
                    "SELECT a_index, a_enable, a_family, a_skillmaster,"
                        + " a_flag, a_flag1, a_state_flag, a_level, a_exp, a_prize,"
                        + " a_sight, a_size, a_move_area, a_attack_area, a_skill_point,"
                        + " a_sskill_master, a_str, a_dex, a_int, a_con, a_attack, a_magic,"
                        + " a_defense, a_resist, a_attacklevel, a_defenselevel, a_hp, a_mp,"
                        + " a_attackType, a_attackSpeed, a_recover_hp, a_recover_mp, a_walk_speed,"
                        + " a_run_speed, a_skill0, a_skill1, a_skill2, a_skill3,"
                        + " a_item_0, a_item_1, a_item_2, a_item_3, a_item_4, a_item_5, a_item_6,"
                        + " a_item_7, a_item_8, a_item_9, a_item_10, a_item_11, a_item_12, a_item_13,"
                        + " a_item_14, a_item_15, a_item_16, a_item_17, a_item_18, a_item_19,"
                        + " a_item_percent_0, a_item_percent_1, a_item_percent_2, a_item_percent_3,"
                        + " a_item_percent_4, a_item_percent_5, a_item_percent_6, a_item_percent_7,"
                        + " a_item_percent_8, a_item_percent_9, a_item_percent_10, a_item_percent_11,"
                        + " a_item_percent_12, a_item_percent_13, a_item_percent_14, a_item_percent_15,"
                        + " a_item_percent_16, a_item_percent_17, a_item_percent_18, a_item_percent_19,"
                        + " a_minplus, a_maxplus, a_probplus, a_product0, a_product1, a_product2, a_product3,"
                        + " a_product4, a_file_smc, a_motion_walk, a_motion_idle, a_motion_dam, a_motion_attack,"
                        + " a_motion_die, a_motion_run, a_motion_idle2, a_motion_attack2, a_scale, a_attribute,"
                        + " a_fireDelayCount, a_fireDelay0, a_fireDelay1, a_fireDelay2, a_fireDelay3, a_fireEffect0,"
                        + " a_fireEffect1, a_fireEffect2, a_fireObject, a_fireSpeed, a_aitype, a_aiflag, a_aileader_flag,"
                        + " a_ai_summonHp, a_aileader_idx, a_aileader_count, a_crafting_category, a_productIndex, a_hit,"
                        + " a_dodge, a_magicavoid, a_job_attribute, a_npc_choice_trigger_count, a_npc_choice_trigger_ids,"
                        + " a_npc_kill_trigger_count, a_npc_kill_trigger_ids, a_createprob, a_socketprob_0, a_socketprob_1,"
                        + " a_socketprob_2, a_jewel_0, a_jewel_1, a_jewel_2, a_jewel_3, a_jewel_4, a_jewel_5, a_jewel_6,"
                        + " a_jewel_7, a_jewel_8, a_jewel_9, a_jewel_10, a_jewel_11, a_jewel_12, a_jewel_13, a_jewel_14,"
                        + " a_jewel_15, a_jewel_16, a_jewel_17, a_jewel_18, a_jewel_19, a_jewel_percent_0, a_jewel_percent_1,"
                        + " a_jewel_percent_2, a_jewel_percent_3, a_jewel_percent_4, a_jewel_percent_5, a_jewel_percent_6,"
                        + " a_jewel_percent_7, a_jewel_percent_8, a_jewel_percent_9, a_jewel_percent_10, a_jewel_percent_11,"
                        + " a_jewel_percent_12, a_jewel_percent_13, a_jewel_percent_14, a_jewel_percent_15, a_jewel_percent_16,"
                        + " a_jewel_percent_17, a_jewel_percent_18, a_jewel_percent_19, a_zone_flag, a_extra_flag, a_rvr_value, a_rvr_grade,"
                        + " a_bound, a_lifetime"
                        + MakeLanguageQuery(ct, "a_name")
                        + MakeLanguageQuery(ct, "a_descr")
                        + " FROM t_npc ORDER BY a_index;",

                _ => string.Empty
            };

            return query;
        }

        public object Database(ContentType ct)
        {
            string query = MakeFetchQuery(ct);

            return ct switch
            {
                ContentType.Action => CMySQL.QueryToClass<CAction>(new CAction(), query),
                ContentType.NPC => CMySQL.QueryToClass<NPC>(new CNPC(), query),
                ContentType.Title => CMySQL.QueryToClass<Title>(new CTitle(), query),
                ContentType.MonsterCombo => CMySQL.QueryToClass<MonsterCombo>(new CMonsterCombo(), query),
                ContentType.ItemExchange => CMySQL.QueryToClass<ItemExchange>(new CItemExchange(), query),
                ContentType.Help => CMySQL.QueryToClass<Help>(new CHelp(), query)
            };
        }
    }
}
