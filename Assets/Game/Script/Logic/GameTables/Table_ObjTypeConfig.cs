using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_ObjTypeConfig : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private int m_Capacity;
	public int Capacity {get {return m_Capacity;}}
[SerializeField]
	private int m_ExpireTime;
	public int ExpireTime {get {return m_ExpireTime;}}
[SerializeField]
	private int m_ReleaseInterval;
	public int ReleaseInterval {get {return m_ReleaseInterval;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 5)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_ObjTypeConfig");
			if(datas.Length != 5)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Name = datas[1];
			m_Capacity = TableUtils.ParseInt(datas[2]);
			m_ExpireTime = TableUtils.ParseInt(datas[3]);
			m_ReleaseInterval = TableUtils.ParseInt(datas[4]);
			return true;
#else
			return true;
#endif
		}
	}
}
