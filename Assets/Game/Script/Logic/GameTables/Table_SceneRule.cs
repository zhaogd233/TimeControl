using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_SceneRule : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_IsCanPK;
	public int IsCanPK {get {return m_IsCanPK;}}
[SerializeField]
	private int m_PKValueDecTime;
	public int PKValueDecTime {get {return m_PKValueDecTime;}}
[SerializeField]
	private int m_IsIncPKValue;
	public int IsIncPKValue {get {return m_IsIncPKValue;}}
[SerializeField]
	private int m_IgnorePKValueWeekDay;
	public int IgnorePKValueWeekDay {get {return m_IgnorePKValueWeekDay;}}
[SerializeField]
	private int m_IgnorePKValueBeginTimeDay;
	public int IgnorePKValueBeginTimeDay {get {return m_IgnorePKValueBeginTimeDay;}}
[SerializeField]
	private int m_IgnorePKValueEndTimeDay;
	public int IgnorePKValueEndTimeDay {get {return m_IgnorePKValueEndTimeDay;}}
[SerializeField]
	private int m_IsCanUseXp;
	public int IsCanUseXp {get {return m_IsCanUseXp;}}
[SerializeField]
	private int m_IsCanFly;
	public int IsCanFly {get {return m_IsCanFly;}}
[SerializeField]
	private int m_IsCanLand;
	public int IsCanLand {get {return m_IsCanLand;}}
[SerializeField]
	private int m_IsCanMount;
	public int IsCanMount {get {return m_IsCanMount;}}
[SerializeField]
	private int m_IsCanUseRecover;
	public int IsCanUseRecover {get {return m_IsCanUseRecover;}}
[SerializeField]
	private int m_IsCanUseRelieveSkill;
	public int IsCanUseRelieveSkill {get {return m_IsCanUseRelieveSkill;}}
[SerializeField]
	private int m_JumpLimit;
	public int JumpLimit {get {return m_JumpLimit;}}
[SerializeField]
	private int m_IsCanUseBackToGuildSceneSkill;
	public int IsCanUseBackToGuildSceneSkill {get {return m_IsCanUseBackToGuildSceneSkill;}}
[SerializeField]
	private int m_IsCanUseBackToGuildWildSceneSkill;
	public int IsCanUseBackToGuildWildSceneSkill {get {return m_IsCanUseBackToGuildWildSceneSkill;}}
[SerializeField]
	private int m_IsShowMTargetUI;
	public int IsShowMTargetUI {get {return m_IsShowMTargetUI;}}
[SerializeField]
	private int m_IsCanTakePhoto;
	public int IsCanTakePhoto {get {return m_IsCanTakePhoto;}}
[SerializeField]
	private int m_IsCanUsePet;
	public int IsCanUsePet {get {return m_IsCanUsePet;}}
[SerializeField]
	private int m_IsJumpCD;
	public int IsJumpCD {get {return m_IsJumpCD;}}
[SerializeField]
	private int m_IsCanGuildDeclareWar;
	public int IsCanGuildDeclareWar {get {return m_IsCanGuildDeclareWar;}}
[SerializeField]
	private int m_IsCanChangePkMode;
	public int IsCanChangePkMode {get {return m_IsCanChangePkMode;}}
[SerializeField]
	private int m_IsCanPvPDrop;
	public int IsCanPvPDrop {get {return m_IsCanPvPDrop;}}
[SerializeField]
	private int m_IsCanRideBattleHorse;
	public int IsCanRideBattleHorse {get {return m_IsCanRideBattleHorse;}}
[SerializeField]
	private int m_IsTeamOnlyOriginalMember;
	public int IsTeamOnlyOriginalMember {get {return m_IsTeamOnlyOriginalMember;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 25)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_SceneRule");
			if(datas.Length != 25)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_IsCanPK = TableUtils.ParseInt(datas[1]);
			m_PKValueDecTime = TableUtils.ParseInt(datas[2]);
			m_IsIncPKValue = TableUtils.ParseInt(datas[3]);
			m_IgnorePKValueWeekDay = TableUtils.ParseInt(datas[4]);
			m_IgnorePKValueBeginTimeDay = TableUtils.ParseInt(datas[5]);
			m_IgnorePKValueEndTimeDay = TableUtils.ParseInt(datas[6]);
			m_IsCanUseXp = TableUtils.ParseInt(datas[7]);
			m_IsCanFly = TableUtils.ParseInt(datas[8]);
			m_IsCanLand = TableUtils.ParseInt(datas[9]);
			m_IsCanMount = TableUtils.ParseInt(datas[10]);
			m_IsCanUseRecover = TableUtils.ParseInt(datas[11]);
			m_IsCanUseRelieveSkill = TableUtils.ParseInt(datas[12]);
			m_JumpLimit = TableUtils.ParseInt(datas[13]);
			m_IsCanUseBackToGuildSceneSkill = TableUtils.ParseInt(datas[14]);
			m_IsCanUseBackToGuildWildSceneSkill = TableUtils.ParseInt(datas[15]);
			m_IsShowMTargetUI = TableUtils.ParseInt(datas[16]);
			m_IsCanTakePhoto = TableUtils.ParseInt(datas[17]);
			m_IsCanUsePet = TableUtils.ParseInt(datas[18]);
			m_IsJumpCD = TableUtils.ParseInt(datas[19]);
			m_IsCanGuildDeclareWar = TableUtils.ParseInt(datas[20]);
			m_IsCanChangePkMode = TableUtils.ParseInt(datas[21]);
			m_IsCanPvPDrop = TableUtils.ParseInt(datas[22]);
			m_IsCanRideBattleHorse = TableUtils.ParseInt(datas[23]);
			m_IsTeamOnlyOriginalMember = TableUtils.ParseInt(datas[24]);
			return true;
#else
			return true;
#endif
		}
	}
}
