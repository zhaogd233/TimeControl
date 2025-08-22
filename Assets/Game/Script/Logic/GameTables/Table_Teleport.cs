using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_Teleport : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_DESC;
	public string DESC {get {return m_DESC;}}
[SerializeField]
	private string m_TeleportName;
	public string TeleportName {get {return m_TeleportName;}}
[SerializeField]
	private int m_SrcSceneID;
	public int SrcSceneID {get {return m_SrcSceneID;}}
[SerializeField]
	private float m_SrcPosX;
	public float SrcPosX {get {return m_SrcPosX;}}
[SerializeField]
	private float m_SrcPosY;
	public float SrcPosY {get {return m_SrcPosY;}}
[SerializeField]
	private float m_SrcPosZ;
	public float SrcPosZ {get {return m_SrcPosZ;}}
[SerializeField]
	private int m_DstSceneID;
	public int DstSceneID {get {return m_DstSceneID;}}
[SerializeField]
	private int m_IsShow;
	public int IsShow {get {return m_IsShow;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 9)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_Teleport");
			if(datas.Length != 9)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_DESC = datas[1];
			m_TeleportName = datas[2];
			m_SrcSceneID = TableUtils.ParseInt(datas[3]);
			m_SrcPosX = TableUtils.ParseFloat(datas[4]);
			m_SrcPosY = TableUtils.ParseFloat(datas[5]);
			m_SrcPosZ = TableUtils.ParseFloat(datas[6]);
			m_DstSceneID = TableUtils.ParseInt(datas[7]);
			m_IsShow = TableUtils.ParseInt(datas[8]);
			return true;
#else
			return true;
#endif
		}
	}
}
