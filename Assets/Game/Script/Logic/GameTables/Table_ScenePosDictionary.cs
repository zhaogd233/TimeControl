using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_ScenePosDictionary : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
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
	private string m_ParamADescribe;
	public string ParamADescribe {get {return m_ParamADescribe;}}
[SerializeField]
	private float m_ParamA;
	public float ParamA {get {return m_ParamA;}}
[SerializeField]
	private string m_ParamBDescribe;
	public string ParamBDescribe {get {return m_ParamBDescribe;}}
[SerializeField]
	private float m_ParamB;
	public float ParamB {get {return m_ParamB;}}
[SerializeField]
	private string m_ParamCDescribe;
	public string ParamCDescribe {get {return m_ParamCDescribe;}}
[SerializeField]
	private float m_ParamC;
	public float ParamC {get {return m_ParamC;}}
[SerializeField]
	private string m_ParamDDescribe;
	public string ParamDDescribe {get {return m_ParamDDescribe;}}
[SerializeField]
	private float m_ParamD;
	public float ParamD {get {return m_ParamD;}}
[SerializeField]
	private string m_ParamEDescribe;
	public string ParamEDescribe {get {return m_ParamEDescribe;}}
[SerializeField]
	private float m_ParamE;
	public float ParamE {get {return m_ParamE;}}
[SerializeField]
	private int m_ClusterId;
	public int ClusterId {get {return m_ClusterId;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 15)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_ScenePosDictionary");
			if(datas.Length != 15)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_PosX = TableUtils.ParseFloat(datas[1]);
			m_PosY = TableUtils.ParseFloat(datas[2]);
			m_PosZ = TableUtils.ParseFloat(datas[3]);
			m_ParamADescribe = datas[4];
			m_ParamA = TableUtils.ParseFloat(datas[5]);
			m_ParamBDescribe = datas[6];
			m_ParamB = TableUtils.ParseFloat(datas[7]);
			m_ParamCDescribe = datas[8];
			m_ParamC = TableUtils.ParseFloat(datas[9]);
			m_ParamDDescribe = datas[10];
			m_ParamD = TableUtils.ParseFloat(datas[11]);
			m_ParamEDescribe = datas[12];
			m_ParamE = TableUtils.ParseFloat(datas[13]);
			m_ClusterId = TableUtils.ParseInt(datas[14]);
			return true;
#else
			return true;
#endif
		}
	}
}
