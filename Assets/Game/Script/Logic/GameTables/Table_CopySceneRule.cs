using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_CopySceneRule : ITableBase 
	{
[SerializeField]
	private int[] m_MinXiuZhenLev = new int[2];
	public int GetMinXiuZhenLevbyIndex(int i) {if(i < 0 || i >= 2) return -1; return m_MinXiuZhenLev[i];}
	public int getMinXiuZhenLevCount(){return 2;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_MinLv;
	public int MinLv {get {return m_MinLv;}}
[SerializeField]
	private int m_MaxLv;
	public int MaxLv {get {return m_MaxLv;}}
[SerializeField]
	private int m_MinMember;
	public int MinMember {get {return m_MinMember;}}
[SerializeField]
	private int m_BonusID;
	public int BonusID {get {return m_BonusID;}}
[SerializeField]
	private int m_MapShowLineLable;
	public int MapShowLineLable {get {return m_MapShowLineLable;}}
[SerializeField]
	private int m_RelationLimit;
	public int RelationLimit {get {return m_RelationLimit;}}
[SerializeField]
	private int m_ClientCountDownSinceLeftTime;
	public int ClientCountDownSinceLeftTime {get {return m_ClientCountDownSinceLeftTime;}}
[SerializeField]
	private int m_IsIgnoreHuiliu;
	public int IsIgnoreHuiliu {get {return m_IsIgnoreHuiliu;}}
[SerializeField]
	private int m_IsCloseAutoCombatTeamRelive;
	public int IsCloseAutoCombatTeamRelive {get {return m_IsCloseAutoCombatTeamRelive;}}
[SerializeField]
	private int m_IsShowAddFriend;
	public int IsShowAddFriend {get {return m_IsShowAddFriend;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 13)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_CopySceneRule");
			if(datas.Length != 13)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_MinLv = TableUtils.ParseInt(datas[1]);
			m_MaxLv = TableUtils.ParseInt(datas[2]);
			m_MinMember = TableUtils.ParseInt(datas[3]);
			m_MinXiuZhenLev[0] = TableUtils.ParseInt(datas[4]);
			m_MinXiuZhenLev[1] = TableUtils.ParseInt(datas[5]);
			m_BonusID = TableUtils.ParseInt(datas[6]);
			m_MapShowLineLable = TableUtils.ParseInt(datas[7]);
			m_RelationLimit = TableUtils.ParseInt(datas[8]);
			m_ClientCountDownSinceLeftTime = TableUtils.ParseInt(datas[9]);
			m_IsIgnoreHuiliu = TableUtils.ParseInt(datas[10]);
			m_IsCloseAutoCombatTeamRelive = TableUtils.ParseInt(datas[11]);
			m_IsShowAddFriend = TableUtils.ParseInt(datas[12]);
			return true;
#else
			return true;
#endif
		}
	}
}
