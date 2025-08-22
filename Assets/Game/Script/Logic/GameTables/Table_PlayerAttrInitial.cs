using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_PlayerAttrInitial : ITableBase 
	{
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private int m_Profession;
	public int Profession {get {return m_Profession;}}
[SerializeField]
	private int m_Level;
	public int Level {get {return m_Level;}}
[SerializeField]
	private int m_STR;
	public int STR {get {return m_STR;}}
[SerializeField]
	private int m_AGI;
	public int AGI {get {return m_AGI;}}
[SerializeField]
	private int m_VIT;
	public int VIT {get {return m_VIT;}}
[SerializeField]
	private int m_SPI;
	public int SPI {get {return m_SPI;}}
[SerializeField]
	private int m_WLP;
	public int WLP {get {return m_WLP;}}
[SerializeField]
	private int m_HPMax;
	public int HPMax {get {return m_HPMax;}}
[SerializeField]
	private int m_MPMax;
	public int MPMax {get {return m_MPMax;}}
[SerializeField]
	private int m_BaseAllAttr;
	public int BaseAllAttr {get {return m_BaseAllAttr;}}
[SerializeField]
	private int m_PhysicalAttack;
	public int PhysicalAttack {get {return m_PhysicalAttack;}}
[SerializeField]
	private int m_MagicalAttack;
	public int MagicalAttack {get {return m_MagicalAttack;}}
[SerializeField]
	private int m_PhysicalDefence;
	public int PhysicalDefence {get {return m_PhysicalDefence;}}
[SerializeField]
	private int m_MagicalDefence;
	public int MagicalDefence {get {return m_MagicalDefence;}}
[SerializeField]
	private int m_Hit;
	public int Hit {get {return m_Hit;}}
[SerializeField]
	private int m_Dodge;
	public int Dodge {get {return m_Dodge;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 17)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_PlayerAttrInitial");
			if(datas.Length != 17)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_Profession = TableUtils.ParseInt(datas[1]);
			m_Level = TableUtils.ParseInt(datas[2]);
			m_STR = TableUtils.ParseInt(datas[3]);
			m_AGI = TableUtils.ParseInt(datas[4]);
			m_VIT = TableUtils.ParseInt(datas[5]);
			m_SPI = TableUtils.ParseInt(datas[6]);
			m_WLP = TableUtils.ParseInt(datas[7]);
			m_HPMax = TableUtils.ParseInt(datas[8]);
			m_MPMax = TableUtils.ParseInt(datas[9]);
			m_BaseAllAttr = TableUtils.ParseInt(datas[10]);
			m_PhysicalAttack = TableUtils.ParseInt(datas[11]);
			m_MagicalAttack = TableUtils.ParseInt(datas[12]);
			m_PhysicalDefence = TableUtils.ParseInt(datas[13]);
			m_MagicalDefence = TableUtils.ParseInt(datas[14]);
			m_Hit = TableUtils.ParseInt(datas[15]);
			m_Dodge = TableUtils.ParseInt(datas[16]);
			return true;
#else
			return true;
#endif
		}
	}
}
