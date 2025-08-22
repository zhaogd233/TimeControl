using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_SkillXmlBase : ITableBase 
	{
[SerializeField]
	private int[] m_SkillBreakTime = new int[5];
	public int GetSkillBreakTimebyIndex(int i) {if(i < 0 || i >= 5) return -1; return m_SkillBreakTime[i];}
	public int getSkillBreakTimeCount(){return 5;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Des;
	public string Des {get {return m_Des;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private int m_PerformMatchID;
	public int PerformMatchID {get {return m_PerformMatchID;}}
[SerializeField]
	private int m_BreakLevel;
	public int BreakLevel {get {return m_BreakLevel;}}
[SerializeField]
	private int m_NeedCheckHaveSkill;
	public int NeedCheckHaveSkill {get {return m_NeedCheckHaveSkill;}}
[SerializeField]
	private int m_IsStaticSkill;
	public int IsStaticSkill {get {return m_IsStaticSkill;}}
[SerializeField]
	private int m_SkillContinueTime;
	public int SkillContinueTime {get {return m_SkillContinueTime;}}
[SerializeField]
	private int m_InterruptTime;
	public int InterruptTime {get {return m_InterruptTime;}}
[SerializeField]
	private int m_IsTurnToTarget;
	public int IsTurnToTarget {get {return m_IsTurnToTarget;}}
[SerializeField]
	private float m_Radius;
	public float Radius {get {return m_Radius;}}
[SerializeField]
	private int m_SkillSubClass;
	public int SkillSubClass {get {return m_SkillSubClass;}}
[SerializeField]
	private int m_SkillTag;
	public int SkillTag {get {return m_SkillTag;}}
[SerializeField]
	private int m_CDTimeId;
	public int CDTimeId {get {return m_CDTimeId;}}
[SerializeField]
	private int m_TargetType;
	public int TargetType {get {return m_TargetType;}}
[SerializeField]
	private int m_IsSubSkill;
	public int IsSubSkill {get {return m_IsSubSkill;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 21)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_SkillXmlBase");
			if(datas.Length != 21)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Des = datas[1];
			m_Name = datas[2];
			m_PerformMatchID = TableUtils.ParseInt(datas[3]);
			m_BreakLevel = TableUtils.ParseInt(datas[4]);
			m_NeedCheckHaveSkill = TableUtils.ParseInt(datas[5]);
			m_IsStaticSkill = TableUtils.ParseInt(datas[6]);
			m_SkillContinueTime = TableUtils.ParseInt(datas[7]);
			m_InterruptTime = TableUtils.ParseInt(datas[8]);
			m_SkillBreakTime[0] = TableUtils.ParseInt(datas[9]);
			m_SkillBreakTime[1] = TableUtils.ParseInt(datas[10]);
			m_SkillBreakTime[2] = TableUtils.ParseInt(datas[11]);
			m_SkillBreakTime[3] = TableUtils.ParseInt(datas[12]);
			m_SkillBreakTime[4] = TableUtils.ParseInt(datas[13]);
			m_IsTurnToTarget = TableUtils.ParseInt(datas[14]);
			m_Radius = TableUtils.ParseFloat(datas[15]);
			m_SkillSubClass = TableUtils.ParseInt(datas[16]);
			m_SkillTag = TableUtils.ParseInt(datas[17]);
			m_CDTimeId = TableUtils.ParseInt(datas[18]);
			m_TargetType = TableUtils.ParseInt(datas[19]);
			m_IsSubSkill = TableUtils.ParseInt(datas[20]);
			return true;
#else
			return true;
#endif
		}
	}
}
