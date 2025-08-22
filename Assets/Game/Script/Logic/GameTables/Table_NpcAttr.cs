using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_NpcAttr : ITableBase 
	{
[SerializeField]
	private int[] m_FieldEffectId = new int[2];
	public int GetFieldEffectIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_FieldEffectId[i];}
	public int getFieldEffectIdCount(){return 2;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Description;
	public string Description {get {return m_Description;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private string m_Title;
	public string Title {get {return m_Title;}}
[SerializeField]
	private int m_NpcType;
	public int NpcType {get {return m_NpcType;}}
[SerializeField]
	private int m_Level;
	public int Level {get {return m_Level;}}
[SerializeField]
	private int m_ForceId;
	public int ForceId {get {return m_ForceId;}}
[SerializeField]
	private float m_AlertRadius;
	public float AlertRadius {get {return m_AlertRadius;}}
[SerializeField]
	private int m_CorpseTime;
	public int CorpseTime {get {return m_CorpseTime;}}
[SerializeField]
	private int m_DialogID;
	public int DialogID {get {return m_DialogID;}}
[SerializeField]
	private float m_DialogRadius;
	public float DialogRadius {get {return m_DialogRadius;}}
[SerializeField]
	private int m_DialogStop;
	public int DialogStop {get {return m_DialogStop;}}
[SerializeField]
	private int m_AttackFlyDistance;
	public int AttackFlyDistance {get {return m_AttackFlyDistance;}}
[SerializeField]
	private int m_BelongType;
	public int BelongType {get {return m_BelongType;}}
[SerializeField]
	private int m_CombatNPC;
	public int CombatNPC {get {return m_CombatNPC;}}
[SerializeField]
	private int m_NpcPaoPaoId;
	public int NpcPaoPaoId {get {return m_NpcPaoPaoId;}}
[SerializeField]
	private int m_NpcPaoPaoShowType;
	public int NpcPaoPaoShowType {get {return m_NpcPaoPaoShowType;}}
[SerializeField]
	private int m_IsCanBeClientSel;
	public int IsCanBeClientSel {get {return m_IsCanBeClientSel;}}
[SerializeField]
	private int m_IsDisableFaceto;
	public int IsDisableFaceto {get {return m_IsDisableFaceto;}}
[SerializeField]
	private int m_IsCanCatch;
	public int IsCanCatch {get {return m_IsCanCatch;}}
[SerializeField]
	private int m_BornAnimId;
	public int BornAnimId {get {return m_BornAnimId;}}
[SerializeField]
	private int m_RelibeAnimId;
	public int RelibeAnimId {get {return m_RelibeAnimId;}}
[SerializeField]
	private int m_DropResetCountId;
	public int DropResetCountId {get {return m_DropResetCountId;}}
[SerializeField]
	private int m_ItemDropType;
	public int ItemDropType {get {return m_ItemDropType;}}
[SerializeField]
	private int m_ShowNameBordType;
	public int ShowNameBordType {get {return m_ShowNameBordType;}}
[SerializeField]
	private int m_IsDissolution;
	public int IsDissolution {get {return m_IsDissolution;}}
[SerializeField]
	private float m_DissolutionBronTime;
	public float DissolutionBronTime {get {return m_DissolutionBronTime;}}
[SerializeField]
	private string m_DissolutionColor;
	public string DissolutionColor {get {return m_DissolutionColor;}}
[SerializeField]
	private int m_DissolutionEffectID;
	public int DissolutionEffectID {get {return m_DissolutionEffectID;}}
[SerializeField]
	private int m_ActivenessID;
	public int ActivenessID {get {return m_ActivenessID;}}
[SerializeField]
	private int m_BackHomeResetFaceDir;
	public int BackHomeResetFaceDir {get {return m_BackHomeResetFaceDir;}}
[SerializeField]
	private float m_HitEffctScaleRate;
	public float HitEffctScaleRate {get {return m_HitEffctScaleRate;}}
[SerializeField]
	private int m_NpcFunctionalMark;
	public int NpcFunctionalMark {get {return m_NpcFunctionalMark;}}
[SerializeField]
	private int m_CanPlayCameraRock;
	public int CanPlayCameraRock {get {return m_CanPlayCameraRock;}}
[SerializeField]
	private int m_IsMustShowHp;
	public int IsMustShowHp {get {return m_IsMustShowHp;}}
[SerializeField]
	private int m_LODDistance;
	public int LODDistance {get {return m_LODDistance;}}
[SerializeField]
	private int m_PetID;
	public int PetID {get {return m_PetID;}}
[SerializeField]
	private int m_IsHideHeadHpBar;
	public int IsHideHeadHpBar {get {return m_IsHideHeadHpBar;}}
[SerializeField]
	private int m_NpcPaoPaoTalkID;
	public int NpcPaoPaoTalkID {get {return m_NpcPaoPaoTalkID;}}
[SerializeField]
	private int m_DmgStatisticIndex;
	public int DmgStatisticIndex {get {return m_DmgStatisticIndex;}}
[SerializeField]
	private int m_StatisticBossCount;
	public int StatisticBossCount {get {return m_StatisticBossCount;}}
[SerializeField]
	private int m_IsBornFaceTo;
	public int IsBornFaceTo {get {return m_IsBornFaceTo;}}
[SerializeField]
	private int m_ShowBelong;
	public int ShowBelong {get {return m_ShowBelong;}}
[SerializeField]
	private int m_IsPrecentHp;
	public int IsPrecentHp {get {return m_IsPrecentHp;}}
[SerializeField]
	private int m_Rewardinterface;
	public int Rewardinterface {get {return m_Rewardinterface;}}
[SerializeField]
	private float m_EnemySelectScale;
	public float EnemySelectScale {get {return m_EnemySelectScale;}}
[SerializeField]
	private int m_CatchCutePet;
	public int CatchCutePet {get {return m_CatchCutePet;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 49)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_NpcAttr");
			if(datas.Length != 49)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Description = datas[1];
			m_Name = datas[2];
			m_Title = datas[3];
			m_NpcType = TableUtils.ParseInt(datas[4]);
			m_Level = TableUtils.ParseInt(datas[5]);
			m_ForceId = TableUtils.ParseInt(datas[6]);
			m_AlertRadius = TableUtils.ParseFloat(datas[7]);
			m_CorpseTime = TableUtils.ParseInt(datas[8]);
			m_DialogID = TableUtils.ParseInt(datas[9]);
			m_DialogRadius = TableUtils.ParseFloat(datas[10]);
			m_DialogStop = TableUtils.ParseInt(datas[11]);
			m_AttackFlyDistance = TableUtils.ParseInt(datas[12]);
			m_BelongType = TableUtils.ParseInt(datas[13]);
			m_CombatNPC = TableUtils.ParseInt(datas[14]);
			m_NpcPaoPaoId = TableUtils.ParseInt(datas[15]);
			m_NpcPaoPaoShowType = TableUtils.ParseInt(datas[16]);
			m_IsCanBeClientSel = TableUtils.ParseInt(datas[17]);
			m_IsDisableFaceto = TableUtils.ParseInt(datas[18]);
			m_IsCanCatch = TableUtils.ParseInt(datas[19]);
			m_BornAnimId = TableUtils.ParseInt(datas[20]);
			m_RelibeAnimId = TableUtils.ParseInt(datas[21]);
			m_DropResetCountId = TableUtils.ParseInt(datas[22]);
			m_ItemDropType = TableUtils.ParseInt(datas[23]);
			m_ShowNameBordType = TableUtils.ParseInt(datas[24]);
			m_IsDissolution = TableUtils.ParseInt(datas[25]);
			m_DissolutionBronTime = TableUtils.ParseFloat(datas[26]);
			m_DissolutionColor = datas[27];
			m_DissolutionEffectID = TableUtils.ParseInt(datas[28]);
			m_ActivenessID = TableUtils.ParseInt(datas[29]);
			m_BackHomeResetFaceDir = TableUtils.ParseInt(datas[30]);
			m_HitEffctScaleRate = TableUtils.ParseFloat(datas[31]);
			m_NpcFunctionalMark = TableUtils.ParseInt(datas[32]);
			m_CanPlayCameraRock = TableUtils.ParseInt(datas[33]);
			m_IsMustShowHp = TableUtils.ParseInt(datas[34]);
			m_LODDistance = TableUtils.ParseInt(datas[35]);
			m_PetID = TableUtils.ParseInt(datas[36]);
			m_IsHideHeadHpBar = TableUtils.ParseInt(datas[37]);
			m_FieldEffectId[0] = TableUtils.ParseInt(datas[38]);
			m_FieldEffectId[1] = TableUtils.ParseInt(datas[39]);
			m_NpcPaoPaoTalkID = TableUtils.ParseInt(datas[40]);
			m_DmgStatisticIndex = TableUtils.ParseInt(datas[41]);
			m_StatisticBossCount = TableUtils.ParseInt(datas[42]);
			m_IsBornFaceTo = TableUtils.ParseInt(datas[43]);
			m_ShowBelong = TableUtils.ParseInt(datas[44]);
			m_IsPrecentHp = TableUtils.ParseInt(datas[45]);
			m_Rewardinterface = TableUtils.ParseInt(datas[46]);
			m_EnemySelectScale = TableUtils.ParseFloat(datas[47]);
			m_CatchCutePet = TableUtils.ParseInt(datas[48]);
			return true;
#else
			return true;
#endif
		}
	}
}
