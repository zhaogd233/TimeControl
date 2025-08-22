using System;
using Core.Table;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Games.Table
{
    [Serializable]
    public class Tab_SceneClass : ITableBase
    {
        [SerializeField] private int[] m_ReliveType = new int[9];

        [SerializeField] private int[] m_Poem = new int[2];

        [SerializeField] private int m_Id;

        [SerializeField] private string m_Name;

        [SerializeField] private int m_SceneResource;

        [SerializeField] private int m_SceneRuleID;

        [SerializeField] private int m_Type;

        [SerializeField] private int m_IsOriginalWorldLoad;

        [SerializeField] private int m_IsBigWorldLoad;

        [SerializeField] private int m_Length;

        [SerializeField] private int m_Width;

        [SerializeField] private string m_SceneMapTexture;

        [SerializeField] private int m_BGMusicDay;

        [SerializeField] private int m_BGMusicNight;

        [SerializeField] private int m_CopySceneID;

        [SerializeField] private int m_PlayersMaxA;

        [SerializeField] private int m_PlayersMaxB;

        [SerializeField] private float m_Entryx;

        [SerializeField] private float m_Entryy;

        [SerializeField] private float m_Entryz;

        [SerializeField] private int m_EnterSceneEffect;

        [SerializeField] private int m_LeaveSceneEffect;

        [SerializeField] private int m_WMEnterLevelLimit;

        [SerializeField] private int m_EnterNoticeDicID;

        [SerializeField] private int m_ChangeTab;

        [SerializeField] private float m_SeamlessOffSetX;

        [SerializeField] private float m_SeamlessOffsetZ;

        [SerializeField] private int m_AreaType;

        [SerializeField] private int m_RecommendLevel;

        [SerializeField] private float m_LefePixelX;

        [SerializeField] private float m_LefePixelY;

        [SerializeField] private float m_RigehtPixelX;

        [SerializeField] private float m_RigehtPixelY;

        [SerializeField] private float m_WalkAreaStartX;

        [SerializeField] private float m_WalkAreaStartY;

        [SerializeField] private float m_WalkAreaEndX;

        [SerializeField] private float m_WalkAreaEndY;

        [SerializeField] private int m_CameraSceneRule;

        [SerializeField] private int m_IsUsingClientWeather;

        [SerializeField] private int m_ClientWeatherType;

        [SerializeField] private int m_BlockID;

        [SerializeField] private int m_SourceSceneClass;

        [SerializeField] private int m_ColorCorrection;

        [SerializeField] private int m_PlayerSwitchTargetDistanceA;

        [SerializeField] private int m_PlayerSwitchTargetDistanceB;

        [SerializeField] private int m_OpenAutoCombat;

        [SerializeField] private int m_OpenAutoCombatTips;

        [SerializeField] private int m_CancelAutoCombat;

        [SerializeField] private int m_ShowExpTip;

        [SerializeField] private int m_CanShaderSnow;

        [SerializeField] private string m_MapNameIcon;

        [SerializeField] private float m_OfferX;

        [SerializeField] private float m_OfferY;

        [SerializeField] private string m_LoadingPath;

        [SerializeField] private float m_WorldMapEntryx;

        [SerializeField] private float m_WorldMapEntryy;

        [SerializeField] private float m_WorldMapEntryz;

        [SerializeField] private int m_OpenLucency;

        [SerializeField] private int m_OpenShowPlayerTactics;

        [SerializeField] private int m_BGBattleMusicDay;

        [SerializeField] private int m_BGBattleMusicNight;

        [SerializeField] private int m_CombatAngleIgnoreTarget;

        [SerializeField] private int m_OpenPlayerViewFilter;

        [SerializeField] private int m_PerformanceLucency;

        [SerializeField] private int m_PKOptimize;

        [SerializeField] private int m_OpenMood;

        [SerializeField] private float m_JumpSyncInterval;

        [SerializeField] private int m_IsCrossBigWorldScene;

        [SerializeField] private float m_scenefogstart;

        [SerializeField] private float m_scenefogend;

        [SerializeField] private float m_scenefogdensity;

        [SerializeField] private string m_scenefogcolor;

        [SerializeField] private int m_IsUpdateTaskUIInit;

        public string Name => m_Name;
        public int SceneResource => m_SceneResource;
        public int SceneRuleID => m_SceneRuleID;
        public int Type => m_Type;
        public int IsOriginalWorldLoad => m_IsOriginalWorldLoad;
        public int IsBigWorldLoad => m_IsBigWorldLoad;
        public int Length => m_Length;
        public int Width => m_Width;
        public string SceneMapTexture => m_SceneMapTexture;
        public int BGMusicDay => m_BGMusicDay;
        public int BGMusicNight => m_BGMusicNight;
        public int CopySceneID => m_CopySceneID;
        public int PlayersMaxA => m_PlayersMaxA;
        public int PlayersMaxB => m_PlayersMaxB;
        public float Entryx => m_Entryx;
        public float Entryy => m_Entryy;
        public float Entryz => m_Entryz;
        public int EnterSceneEffect => m_EnterSceneEffect;
        public int LeaveSceneEffect => m_LeaveSceneEffect;
        public int WMEnterLevelLimit => m_WMEnterLevelLimit;
        public int EnterNoticeDicID => m_EnterNoticeDicID;
        public int ChangeTab => m_ChangeTab;
        public float SeamlessOffSetX => m_SeamlessOffSetX;
        public float SeamlessOffsetZ => m_SeamlessOffsetZ;
        public int AreaType => m_AreaType;
        public int RecommendLevel => m_RecommendLevel;
        public float LefePixelX => m_LefePixelX;
        public float LefePixelY => m_LefePixelY;
        public float RigehtPixelX => m_RigehtPixelX;
        public float RigehtPixelY => m_RigehtPixelY;
        public float WalkAreaStartX => m_WalkAreaStartX;
        public float WalkAreaStartY => m_WalkAreaStartY;
        public float WalkAreaEndX => m_WalkAreaEndX;
        public float WalkAreaEndY => m_WalkAreaEndY;
        public int CameraSceneRule => m_CameraSceneRule;
        public int IsUsingClientWeather => m_IsUsingClientWeather;
        public int ClientWeatherType => m_ClientWeatherType;
        public int BlockID => m_BlockID;
        public int SourceSceneClass => m_SourceSceneClass;
        public int ColorCorrection => m_ColorCorrection;
        public int PlayerSwitchTargetDistanceA => m_PlayerSwitchTargetDistanceA;
        public int PlayerSwitchTargetDistanceB => m_PlayerSwitchTargetDistanceB;
        public int OpenAutoCombat => m_OpenAutoCombat;
        public int OpenAutoCombatTips => m_OpenAutoCombatTips;
        public int CancelAutoCombat => m_CancelAutoCombat;
        public int ShowExpTip => m_ShowExpTip;
        public int CanShaderSnow => m_CanShaderSnow;
        public string MapNameIcon => m_MapNameIcon;
        public float OfferX => m_OfferX;
        public float OfferY => m_OfferY;
        public string LoadingPath => m_LoadingPath;
        public float WorldMapEntryx => m_WorldMapEntryx;
        public float WorldMapEntryy => m_WorldMapEntryy;
        public float WorldMapEntryz => m_WorldMapEntryz;
        public int OpenLucency => m_OpenLucency;
        public int OpenShowPlayerTactics => m_OpenShowPlayerTactics;
        public int BGBattleMusicDay => m_BGBattleMusicDay;
        public int BGBattleMusicNight => m_BGBattleMusicNight;
        public int CombatAngleIgnoreTarget => m_CombatAngleIgnoreTarget;
        public int OpenPlayerViewFilter => m_OpenPlayerViewFilter;
        public int PerformanceLucency => m_PerformanceLucency;
        public int PKOptimize => m_PKOptimize;
        public int OpenMood => m_OpenMood;
        public float JumpSyncInterval => m_JumpSyncInterval;
        public int IsCrossBigWorldScene => m_IsCrossBigWorldScene;
        public float scenefogstart => m_scenefogstart;
        public float scenefogend => m_scenefogend;
        public float scenefogdensity => m_scenefogdensity;
        public string scenefogcolor => m_scenefogcolor;
        public int IsUpdateTaskUIInit => m_IsUpdateTaskUIInit;
        public int Id => m_Id;

        public bool LoadData(string strLine)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            var datas = strLine.Split('\t');
            if (datas.Length != 82)
                Debug.Assert(false, "var count not match talble cols Tab_SceneClass");
            if (datas.Length != 82)
                throw new Exception("var count not match talble  cols");
            m_Id = TableUtils.ParseInt(datas[0]);
            m_Name = datas[1];
            m_SceneResource = TableUtils.ParseInt(datas[2]);
            m_SceneRuleID = TableUtils.ParseInt(datas[3]);
            m_Type = TableUtils.ParseInt(datas[4]);
            m_IsOriginalWorldLoad = TableUtils.ParseInt(datas[5]);
            m_IsBigWorldLoad = TableUtils.ParseInt(datas[6]);
            m_Length = TableUtils.ParseInt(datas[7]);
            m_Width = TableUtils.ParseInt(datas[8]);
            m_SceneMapTexture = datas[9];
            m_BGMusicDay = TableUtils.ParseInt(datas[10]);
            m_BGMusicNight = TableUtils.ParseInt(datas[11]);
            m_CopySceneID = TableUtils.ParseInt(datas[12]);
            m_PlayersMaxA = TableUtils.ParseInt(datas[13]);
            m_PlayersMaxB = TableUtils.ParseInt(datas[14]);
            m_ReliveType[0] = TableUtils.ParseInt(datas[15]);
            m_ReliveType[1] = TableUtils.ParseInt(datas[16]);
            m_ReliveType[2] = TableUtils.ParseInt(datas[17]);
            m_ReliveType[3] = TableUtils.ParseInt(datas[18]);
            m_ReliveType[4] = TableUtils.ParseInt(datas[19]);
            m_ReliveType[5] = TableUtils.ParseInt(datas[20]);
            m_ReliveType[6] = TableUtils.ParseInt(datas[21]);
            m_ReliveType[7] = TableUtils.ParseInt(datas[22]);
            m_ReliveType[8] = TableUtils.ParseInt(datas[23]);
            m_Entryx = TableUtils.ParseFloat(datas[24]);
            m_Entryy = TableUtils.ParseFloat(datas[25]);
            m_Entryz = TableUtils.ParseFloat(datas[26]);
            m_EnterSceneEffect = TableUtils.ParseInt(datas[27]);
            m_LeaveSceneEffect = TableUtils.ParseInt(datas[28]);
            m_WMEnterLevelLimit = TableUtils.ParseInt(datas[29]);
            m_EnterNoticeDicID = TableUtils.ParseInt(datas[30]);
            m_ChangeTab = TableUtils.ParseInt(datas[31]);
            m_SeamlessOffSetX = TableUtils.ParseFloat(datas[32]);
            m_SeamlessOffsetZ = TableUtils.ParseFloat(datas[33]);
            m_AreaType = TableUtils.ParseInt(datas[34]);
            m_RecommendLevel = TableUtils.ParseInt(datas[35]);
            m_LefePixelX = TableUtils.ParseFloat(datas[36]);
            m_LefePixelY = TableUtils.ParseFloat(datas[37]);
            m_RigehtPixelX = TableUtils.ParseFloat(datas[38]);
            m_RigehtPixelY = TableUtils.ParseFloat(datas[39]);
            m_WalkAreaStartX = TableUtils.ParseFloat(datas[40]);
            m_WalkAreaStartY = TableUtils.ParseFloat(datas[41]);
            m_WalkAreaEndX = TableUtils.ParseFloat(datas[42]);
            m_WalkAreaEndY = TableUtils.ParseFloat(datas[43]);
            m_CameraSceneRule = TableUtils.ParseInt(datas[44]);
            m_IsUsingClientWeather = TableUtils.ParseInt(datas[45]);
            m_ClientWeatherType = TableUtils.ParseInt(datas[46]);
            m_BlockID = TableUtils.ParseInt(datas[47]);
            m_SourceSceneClass = TableUtils.ParseInt(datas[48]);
            m_ColorCorrection = TableUtils.ParseInt(datas[49]);
            m_PlayerSwitchTargetDistanceA = TableUtils.ParseInt(datas[50]);
            m_PlayerSwitchTargetDistanceB = TableUtils.ParseInt(datas[51]);
            m_OpenAutoCombat = TableUtils.ParseInt(datas[52]);
            m_OpenAutoCombatTips = TableUtils.ParseInt(datas[53]);
            m_CancelAutoCombat = TableUtils.ParseInt(datas[54]);
            m_ShowExpTip = TableUtils.ParseInt(datas[55]);
            m_CanShaderSnow = TableUtils.ParseInt(datas[56]);
            m_Poem[0] = TableUtils.ParseInt(datas[57]);
            m_Poem[1] = TableUtils.ParseInt(datas[58]);
            m_MapNameIcon = datas[59];
            m_OfferX = TableUtils.ParseFloat(datas[60]);
            m_OfferY = TableUtils.ParseFloat(datas[61]);
            m_LoadingPath = datas[62];
            m_WorldMapEntryx = TableUtils.ParseFloat(datas[63]);
            m_WorldMapEntryy = TableUtils.ParseFloat(datas[64]);
            m_WorldMapEntryz = TableUtils.ParseFloat(datas[65]);
            m_OpenLucency = TableUtils.ParseInt(datas[66]);
            m_OpenShowPlayerTactics = TableUtils.ParseInt(datas[67]);
            m_BGBattleMusicDay = TableUtils.ParseInt(datas[68]);
            m_BGBattleMusicNight = TableUtils.ParseInt(datas[69]);
            m_CombatAngleIgnoreTarget = TableUtils.ParseInt(datas[70]);
            m_OpenPlayerViewFilter = TableUtils.ParseInt(datas[71]);
            m_PerformanceLucency = TableUtils.ParseInt(datas[72]);
            m_PKOptimize = TableUtils.ParseInt(datas[73]);
            m_OpenMood = TableUtils.ParseInt(datas[74]);
            m_JumpSyncInterval = TableUtils.ParseFloat(datas[75]);
            m_IsCrossBigWorldScene = TableUtils.ParseInt(datas[76]);
            m_scenefogstart = TableUtils.ParseFloat(datas[77]);
            m_scenefogend = TableUtils.ParseFloat(datas[78]);
            m_scenefogdensity = TableUtils.ParseFloat(datas[79]);
            m_scenefogcolor = datas[80];
            m_IsUpdateTaskUIInit = TableUtils.ParseInt(datas[81]);
            return true;
#else
			return true;
#endif
        }

        public int GetReliveTypebyIndex(int i)
        {
            if (i < 0 || i >= 9) return -1;
            return m_ReliveType[i];
        }

        public int getReliveTypeCount()
        {
            return 9;
        }

        public int GetPoembyIndex(int i)
        {
            if (i < 0 || i >= 2) return -1;
            return m_Poem[i];
        }

        public int getPoemCount()
        {
            return 2;
        }
    }
}