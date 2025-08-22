using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_CopyScene : ITableBase 
	{
[SerializeField]
	private int[] m_CopySceneBossDropID = new int[3];
	public int GetCopySceneBossDropIDbyIndex(int i) {if(i < 0 || i >= 3) return -1; return m_CopySceneBossDropID[i];}
	public int getCopySceneBossDropIDCount(){return 3;}
[SerializeField]
	private int[] m_CopySceneBossDataID = new int[3];
	public int GetCopySceneBossDataIDbyIndex(int i) {if(i < 0 || i >= 3) return -1; return m_CopySceneBossDataID[i];}
	public int getCopySceneBossDataIDCount(){return 3;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_IsOpen;
	public int IsOpen {get {return m_IsOpen;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private int m_LifeTime;
	public int LifeTime {get {return m_LifeTime;}}
[SerializeField]
	private int m_OpenID;
	public int OpenID {get {return m_OpenID;}}
[SerializeField]
	private int m_ResetID;
	public int ResetID {get {return m_ResetID;}}
[SerializeField]
	private int m_RuleID;
	public int RuleID {get {return m_RuleID;}}
[SerializeField]
	private int m_ActivenessIndex;
	public int ActivenessIndex {get {return m_ActivenessIndex;}}
[SerializeField]
	private int m_EnterMode;
	public int EnterMode {get {return m_EnterMode;}}
[SerializeField]
	private int m_EnterSubMod;
	public int EnterSubMod {get {return m_EnterSubMod;}}
[SerializeField]
	private int m_IsShowStatisticsData;
	public int IsShowStatisticsData {get {return m_IsShowStatisticsData;}}
[SerializeField]
	private int m_IsShowLethalDamageInfo;
	public int IsShowLethalDamageInfo {get {return m_IsShowLethalDamageInfo;}}
[SerializeField]
	private int m_IsCanEnterSendOpenPak;
	public int IsCanEnterSendOpenPak {get {return m_IsCanEnterSendOpenPak;}}
[SerializeField]
	private int m_StatisticsID;
	public int StatisticsID {get {return m_StatisticsID;}}
[SerializeField]
	private int m_LastBossNpcID;
	public int LastBossNpcID {get {return m_LastBossNpcID;}}
[SerializeField]
	private int m_UnlockFunctionID;
	public int UnlockFunctionID {get {return m_UnlockFunctionID;}}
[SerializeField]
	private int m_IsAchievementCopyscene;
	public int IsAchievementCopyscene {get {return m_IsAchievementCopyscene;}}
[SerializeField]
	private int m_ClearTeamTarget;
	public int ClearTeamTarget {get {return m_ClearTeamTarget;}}
[SerializeField]
	private int m_OpenNavigation;
	public int OpenNavigation {get {return m_OpenNavigation;}}
[SerializeField]
	private int m_CopySceneEnterCountId;
	public int CopySceneEnterCountId {get {return m_CopySceneEnterCountId;}}
[SerializeField]
	private int m_IsMarryAutoConfirmEnter;
	public int IsMarryAutoConfirmEnter {get {return m_IsMarryAutoConfirmEnter;}}
[SerializeField]
	private int m_DifficultySelect;
	public int DifficultySelect {get {return m_DifficultySelect;}}
[SerializeField]
	private int m_AFK;
	public int AFK {get {return m_AFK;}}
[SerializeField]
	private int m_OffsetTimeForShow;
	public int OffsetTimeForShow {get {return m_OffsetTimeForShow;}}
[SerializeField]
	private int m_IsShowBOSSFight;
	public int IsShowBOSSFight {get {return m_IsShowBOSSFight;}}
[SerializeField]
	private int m_CopySceneBossCount;
	public int CopySceneBossCount {get {return m_CopySceneBossCount;}}
[SerializeField]
	private int m_ShowChallengeDropInfo;
	public int ShowChallengeDropInfo {get {return m_ShowChallengeDropInfo;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 33)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_CopyScene");
			if(datas.Length != 33)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_IsOpen = TableUtils.ParseInt(datas[1]);
			m_Name = datas[2];
			m_LifeTime = TableUtils.ParseInt(datas[3]);
			m_OpenID = TableUtils.ParseInt(datas[4]);
			m_ResetID = TableUtils.ParseInt(datas[5]);
			m_RuleID = TableUtils.ParseInt(datas[6]);
			m_ActivenessIndex = TableUtils.ParseInt(datas[7]);
			m_EnterMode = TableUtils.ParseInt(datas[8]);
			m_EnterSubMod = TableUtils.ParseInt(datas[9]);
			m_IsShowStatisticsData = TableUtils.ParseInt(datas[10]);
			m_IsShowLethalDamageInfo = TableUtils.ParseInt(datas[11]);
			m_IsCanEnterSendOpenPak = TableUtils.ParseInt(datas[12]);
			m_StatisticsID = TableUtils.ParseInt(datas[13]);
			m_LastBossNpcID = TableUtils.ParseInt(datas[14]);
			m_UnlockFunctionID = TableUtils.ParseInt(datas[15]);
			m_IsAchievementCopyscene = TableUtils.ParseInt(datas[16]);
			m_ClearTeamTarget = TableUtils.ParseInt(datas[17]);
			m_OpenNavigation = TableUtils.ParseInt(datas[18]);
			m_CopySceneEnterCountId = TableUtils.ParseInt(datas[19]);
			m_IsMarryAutoConfirmEnter = TableUtils.ParseInt(datas[20]);
			m_DifficultySelect = TableUtils.ParseInt(datas[21]);
			m_AFK = TableUtils.ParseInt(datas[22]);
			m_OffsetTimeForShow = TableUtils.ParseInt(datas[23]);
			m_IsShowBOSSFight = TableUtils.ParseInt(datas[24]);
			m_CopySceneBossDropID[0] = TableUtils.ParseInt(datas[25]);
			m_CopySceneBossDropID[1] = TableUtils.ParseInt(datas[26]);
			m_CopySceneBossDropID[2] = TableUtils.ParseInt(datas[27]);
			m_CopySceneBossDataID[0] = TableUtils.ParseInt(datas[28]);
			m_CopySceneBossDataID[1] = TableUtils.ParseInt(datas[29]);
			m_CopySceneBossDataID[2] = TableUtils.ParseInt(datas[30]);
			m_CopySceneBossCount = TableUtils.ParseInt(datas[31]);
			m_ShowChallengeDropInfo = TableUtils.ParseInt(datas[32]);
			return true;
#else
			return true;
#endif
		}
	}
}
