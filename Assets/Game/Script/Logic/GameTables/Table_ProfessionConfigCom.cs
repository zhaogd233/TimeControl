using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_ProfessionConfigCom : ITableBase 
	{
[SerializeField]
	private int[] m_DataId = new int[2];
	public int GetDataIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_DataId[i];}
	public int getDataIdCount(){return 2;}
[SerializeField]
	private int[] m_CharAttr = new int[5];
	public int GetCharAttrbyIndex(int i) {if(i < 0 || i >= 5) return -1; return m_CharAttr[i];}
	public int getCharAttrCount(){return 5;}
[SerializeField]
	private int[] m_JumpFirstId = new int[2];
	public int GetJumpFirstIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFirstId[i];}
	public int getJumpFirstIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpSecondId = new int[2];
	public int GetJumpSecondIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpSecondId[i];}
	public int getJumpSecondIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpThirdId = new int[2];
	public int GetJumpThirdIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpThirdId[i];}
	public int getJumpThirdIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpFourId = new int[2];
	public int GetJumpFourIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFourId[i];}
	public int getJumpFourIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpSecondGroundId = new int[2];
	public int GetJumpSecondGroundIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpSecondGroundId[i];}
	public int getJumpSecondGroundIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpThirdGroundId = new int[2];
	public int GetJumpThirdGroundIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpThirdGroundId[i];}
	public int getJumpThirdGroundIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpFourGroundId = new int[2];
	public int GetJumpFourGroundIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFourGroundId[i];}
	public int getJumpFourGroundIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpSecondWaterId = new int[2];
	public int GetJumpSecondWaterIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpSecondWaterId[i];}
	public int getJumpSecondWaterIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpThirdWaterId = new int[2];
	public int GetJumpThirdWaterIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpThirdWaterId[i];}
	public int getJumpThirdWaterIdCount(){return 2;}
[SerializeField]
	private int[] m_JumpFourWaterId = new int[2];
	public int GetJumpFourWaterIdbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFourWaterId[i];}
	public int getJumpFourWaterIdCount(){return 2;}
[SerializeField]
	private int[] m_LoadingJumpUp = new int[2];
	public int GetLoadingJumpUpbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_LoadingJumpUp[i];}
	public int getLoadingJumpUpCount(){return 2;}
[SerializeField]
	private int[] m_LoadingJumpDown = new int[2];
	public int GetLoadingJumpDownbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_LoadingJumpDown[i];}
	public int getLoadingJumpDownCount(){return 2;}
[SerializeField]
	private int[] m_JumpFairy = new int[2];
	public int GetJumpFairybyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFairy[i];}
	public int getJumpFairyCount(){return 2;}
[SerializeField]
	private int[] m_JumpFairyOne = new int[2];
	public int GetJumpFairyOnebyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_JumpFairyOne[i];}
	public int getJumpFairyOneCount(){return 2;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private int m_AttackType;
	public int AttackType {get {return m_AttackType;}}
[SerializeField]
	private int m_XinShouMisId;
	public int XinShouMisId {get {return m_XinShouMisId;}}
[SerializeField]
	private int m_DefaultSceneClassId;
	public int DefaultSceneClassId {get {return m_DefaultSceneClassId;}}
[SerializeField]
	private float m_LockSkillTargetDistance;
	public float LockSkillTargetDistance {get {return m_LockSkillTargetDistance;}}
[SerializeField]
	private int m_Race;
	public int Race {get {return m_Race;}}
[SerializeField]
	private int m_CancelSelectTargetDistance;
	public int CancelSelectTargetDistance {get {return m_CancelSelectTargetDistance;}}
[SerializeField]
	private int m_SwitchTargetDistanceA;
	public int SwitchTargetDistanceA {get {return m_SwitchTargetDistanceA;}}
[SerializeField]
	private int m_SwitchTargetDistanceB;
	public int SwitchTargetDistanceB {get {return m_SwitchTargetDistanceB;}}
[SerializeField]
	private int m_SwitchTargetAngle;
	public int SwitchTargetAngle {get {return m_SwitchTargetAngle;}}
[SerializeField]
	private int m_XiuZhenIncreaseSceneId;
	public int XiuZhenIncreaseSceneId {get {return m_XiuZhenIncreaseSceneId;}}
[SerializeField]
	private float m_XiuZhenIncreaseScenePosX;
	public float XiuZhenIncreaseScenePosX {get {return m_XiuZhenIncreaseScenePosX;}}
[SerializeField]
	private float m_XiuZhenIncreaseScenePosY;
	public float XiuZhenIncreaseScenePosY {get {return m_XiuZhenIncreaseScenePosY;}}
[SerializeField]
	private float m_XiuZhenIncreaseScenePosZ;
	public float XiuZhenIncreaseScenePosZ {get {return m_XiuZhenIncreaseScenePosZ;}}
[SerializeField]
	private int m_FlyingValue;
	public int FlyingValue {get {return m_FlyingValue;}}
[SerializeField]
	private int m_EntryBeforeFlyingValue;
	public int EntryBeforeFlyingValue {get {return m_EntryBeforeFlyingValue;}}
[SerializeField]
	private float m_JumpMinCameraScale;
	public float JumpMinCameraScale {get {return m_JumpMinCameraScale;}}
[SerializeField]
	private float m_JumpCameraScale;
	public float JumpCameraScale {get {return m_JumpCameraScale;}}
[SerializeField]
	private int m_RedressLimit;
	public int RedressLimit {get {return m_RedressLimit;}}
[SerializeField]
	private int m_JumpDropNormal;
	public int JumpDropNormal {get {return m_JumpDropNormal;}}
[SerializeField]
	private int m_JumpDropWater;
	public int JumpDropWater {get {return m_JumpDropWater;}}
[SerializeField]
	private int m_JumpSprintID;
	public int JumpSprintID {get {return m_JumpSprintID;}}
[SerializeField]
	private int m_JumpBig;
	public int JumpBig {get {return m_JumpBig;}}
[SerializeField]
	private int m_FirstGemstone;
	public int FirstGemstone {get {return m_FirstGemstone;}}
[SerializeField]
	private int m_Mis5835GemReward;
	public int Mis5835GemReward {get {return m_Mis5835GemReward;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 61)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_ProfessionConfigCom");
			if(datas.Length != 61)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Name = datas[1];
			m_DataId[0] = TableUtils.ParseInt(datas[2]);
			m_DataId[1] = TableUtils.ParseInt(datas[3]);
			m_AttackType = TableUtils.ParseInt(datas[4]);
			m_XinShouMisId = TableUtils.ParseInt(datas[5]);
			m_DefaultSceneClassId = TableUtils.ParseInt(datas[6]);
			m_LockSkillTargetDistance = TableUtils.ParseFloat(datas[7]);
			m_Race = TableUtils.ParseInt(datas[8]);
			m_CancelSelectTargetDistance = TableUtils.ParseInt(datas[9]);
			m_SwitchTargetDistanceA = TableUtils.ParseInt(datas[10]);
			m_SwitchTargetDistanceB = TableUtils.ParseInt(datas[11]);
			m_SwitchTargetAngle = TableUtils.ParseInt(datas[12]);
			m_XiuZhenIncreaseSceneId = TableUtils.ParseInt(datas[13]);
			m_XiuZhenIncreaseScenePosX = TableUtils.ParseFloat(datas[14]);
			m_XiuZhenIncreaseScenePosY = TableUtils.ParseFloat(datas[15]);
			m_XiuZhenIncreaseScenePosZ = TableUtils.ParseFloat(datas[16]);
			m_FlyingValue = TableUtils.ParseInt(datas[17]);
			m_EntryBeforeFlyingValue = TableUtils.ParseInt(datas[18]);
			m_CharAttr[0] = TableUtils.ParseInt(datas[19]);
			m_CharAttr[1] = TableUtils.ParseInt(datas[20]);
			m_CharAttr[2] = TableUtils.ParseInt(datas[21]);
			m_CharAttr[3] = TableUtils.ParseInt(datas[22]);
			m_CharAttr[4] = TableUtils.ParseInt(datas[23]);
			m_JumpMinCameraScale = TableUtils.ParseFloat(datas[24]);
			m_JumpCameraScale = TableUtils.ParseFloat(datas[25]);
			m_JumpFirstId[0] = TableUtils.ParseInt(datas[26]);
			m_JumpFirstId[1] = TableUtils.ParseInt(datas[27]);
			m_JumpSecondId[0] = TableUtils.ParseInt(datas[28]);
			m_JumpSecondId[1] = TableUtils.ParseInt(datas[29]);
			m_JumpThirdId[0] = TableUtils.ParseInt(datas[30]);
			m_JumpThirdId[1] = TableUtils.ParseInt(datas[31]);
			m_JumpFourId[0] = TableUtils.ParseInt(datas[32]);
			m_JumpFourId[1] = TableUtils.ParseInt(datas[33]);
			m_JumpSecondGroundId[0] = TableUtils.ParseInt(datas[34]);
			m_JumpSecondGroundId[1] = TableUtils.ParseInt(datas[35]);
			m_JumpThirdGroundId[0] = TableUtils.ParseInt(datas[36]);
			m_JumpThirdGroundId[1] = TableUtils.ParseInt(datas[37]);
			m_JumpFourGroundId[0] = TableUtils.ParseInt(datas[38]);
			m_JumpFourGroundId[1] = TableUtils.ParseInt(datas[39]);
			m_JumpSecondWaterId[0] = TableUtils.ParseInt(datas[40]);
			m_JumpSecondWaterId[1] = TableUtils.ParseInt(datas[41]);
			m_JumpThirdWaterId[0] = TableUtils.ParseInt(datas[42]);
			m_JumpThirdWaterId[1] = TableUtils.ParseInt(datas[43]);
			m_JumpFourWaterId[0] = TableUtils.ParseInt(datas[44]);
			m_JumpFourWaterId[1] = TableUtils.ParseInt(datas[45]);
			m_RedressLimit = TableUtils.ParseInt(datas[46]);
			m_LoadingJumpUp[0] = TableUtils.ParseInt(datas[47]);
			m_LoadingJumpUp[1] = TableUtils.ParseInt(datas[48]);
			m_LoadingJumpDown[0] = TableUtils.ParseInt(datas[49]);
			m_LoadingJumpDown[1] = TableUtils.ParseInt(datas[50]);
			m_JumpFairy[0] = TableUtils.ParseInt(datas[51]);
			m_JumpFairy[1] = TableUtils.ParseInt(datas[52]);
			m_JumpFairyOne[0] = TableUtils.ParseInt(datas[53]);
			m_JumpFairyOne[1] = TableUtils.ParseInt(datas[54]);
			m_JumpDropNormal = TableUtils.ParseInt(datas[55]);
			m_JumpDropWater = TableUtils.ParseInt(datas[56]);
			m_JumpSprintID = TableUtils.ParseInt(datas[57]);
			m_JumpBig = TableUtils.ParseInt(datas[58]);
			m_FirstGemstone = TableUtils.ParseInt(datas[59]);
			m_Mis5835GemReward = TableUtils.ParseInt(datas[60]);
			return true;
#else
			return true;
#endif
		}
	}
}
