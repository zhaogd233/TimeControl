using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_Bullet : ITableBase 
	{
[SerializeField]
	private float[] m_Param = new float[6];
	public float GetParambyIndex(int i) {if(i < 0 || i >= 6) return -1; return m_Param[i];}
	public int getParamCount(){return 6;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_Name;
	public string Name {get {return m_Name;}}
[SerializeField]
	private int m_EffectID;
	public int EffectID {get {return m_EffectID;}}
[SerializeField]
	private float m_Speed;
	public float Speed {get {return m_Speed;}}
[SerializeField]
	private int m_ArriveEffectID;
	public int ArriveEffectID {get {return m_ArriveEffectID;}}
[SerializeField]
	private int m_ArriveColorID;
	public int ArriveColorID {get {return m_ArriveColorID;}}
[SerializeField]
	private int m_IsReverse;
	public int IsReverse {get {return m_IsReverse;}}
[SerializeField]
	private float m_Distance;
	public float Distance {get {return m_Distance;}}
[SerializeField]
	private int m_HitPoint;
	public int HitPoint {get {return m_HitPoint;}}
[SerializeField]
	private int m_BulletType;
	public int BulletType {get {return m_BulletType;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 16)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_Bullet");
			if(datas.Length != 16)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Name = datas[1];
			m_EffectID = TableUtils.ParseInt(datas[2]);
			m_Speed = TableUtils.ParseFloat(datas[3]);
			m_ArriveEffectID = TableUtils.ParseInt(datas[4]);
			m_ArriveColorID = TableUtils.ParseInt(datas[5]);
			m_IsReverse = TableUtils.ParseInt(datas[6]);
			m_Distance = TableUtils.ParseFloat(datas[7]);
			m_HitPoint = TableUtils.ParseInt(datas[8]);
			m_BulletType = TableUtils.ParseInt(datas[9]);
			m_Param[0] = TableUtils.ParseFloat(datas[10]);
			m_Param[1] = TableUtils.ParseFloat(datas[11]);
			m_Param[2] = TableUtils.ParseFloat(datas[12]);
			m_Param[3] = TableUtils.ParseFloat(datas[13]);
			m_Param[4] = TableUtils.ParseFloat(datas[14]);
			m_Param[5] = TableUtils.ParseFloat(datas[15]);
			return true;
#else
			return true;
#endif
		}
	}
}
