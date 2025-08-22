using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_RoleBaseAttr : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Description;
	public string Description {get {return m_Description;}}
[SerializeField]
	private int m_CharModelID;
	public int CharModelID {get {return m_CharModelID;}}
[SerializeField]
	private int m_Width;
	public int Width {get {return m_Width;}}
[SerializeField]
	private int m_IsImportantNpc;
	public int IsImportantNpc {get {return m_IsImportantNpc;}}
[SerializeField]
	private int m_IsLoadLowNpc;
	public int IsLoadLowNpc {get {return m_IsLoadLowNpc;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 6)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_RoleBaseAttr");
			if(datas.Length != 6)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Description = datas[1];
			m_CharModelID = TableUtils.ParseInt(datas[2]);
			m_Width = TableUtils.ParseInt(datas[3]);
			m_IsImportantNpc = TableUtils.ParseInt(datas[4]);
			m_IsLoadLowNpc = TableUtils.ParseInt(datas[5]);
			return true;
#else
			return true;
#endif
		}
	}
}
