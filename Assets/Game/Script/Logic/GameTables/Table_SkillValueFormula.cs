using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_SkillValueFormula : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Des;
	public string Des {get {return m_Des;}}
[SerializeField]
	private int m_LevelValueID;
	public int LevelValueID {get {return m_LevelValueID;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 3)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_SkillValueFormula");
			if(datas.Length != 3)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Des = datas[1];
			m_LevelValueID = TableUtils.ParseInt(datas[2]);
			return true;
#else
			return true;
#endif
		}
	}
}
