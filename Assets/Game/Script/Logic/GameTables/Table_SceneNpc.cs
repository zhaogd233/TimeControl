using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_SceneNpc : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_SceneID;
	public int SceneID {get {return m_SceneID;}}
[SerializeField]
	private int m_DataID;
	public int DataID {get {return m_DataID;}}
[SerializeField]
	private float m_PosX;
	public float PosX {get {return m_PosX;}}
[SerializeField]
	private float m_PosY;
	public float PosY {get {return m_PosY;}}
[SerializeField]
	private float m_PosZ;
	public float PosZ {get {return m_PosZ;}}
[SerializeField]
	private string m_SpriteName;
	public string SpriteName {get {return m_SpriteName;}}
[SerializeField]
	private int m_DescName;
	public int DescName {get {return m_DescName;}}
[SerializeField]
	private int m_DescNameDirection;
	public int DescNameDirection {get {return m_DescNameDirection;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 9)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_SceneNpc");
			if(datas.Length != 9)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_SceneID = TableUtils.ParseInt(datas[1]);
			m_DataID = TableUtils.ParseInt(datas[2]);
			m_PosX = TableUtils.ParseFloat(datas[3]);
			m_PosY = TableUtils.ParseFloat(datas[4]);
			m_PosZ = TableUtils.ParseFloat(datas[5]);
			m_SpriteName = datas[6];
			m_DescName = TableUtils.ParseInt(datas[7]);
			m_DescNameDirection = TableUtils.ParseInt(datas[8]);
			return true;
#else
			return true;
#endif
		}
	}
}
