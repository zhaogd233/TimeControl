using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_CoolDownTimeInfo : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_CDTime;
	public int CDTime {get {return m_CDTime;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 2)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_CoolDownTimeInfo");
			if(datas.Length != 2)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_CDTime = TableUtils.ParseInt(datas[1]);
			return true;
#else
			return true;
#endif
		}
	}
}
