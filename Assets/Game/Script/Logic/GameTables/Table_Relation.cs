using System;
using Core.Table;
using UnityEngine;


namespace Games.Table
{
	[Serializable]
	public class Tab_Relation : ITableBase 
	{
[SerializeField]
	private int[] m_Relation = new int[36];
	public int GetRelationbyIndex(int i) {if(i < 0 || i >= 36) return -1; return m_Relation[i];}
	public int getRelationCount(){return 36;}
[SerializeField]
	private int m_Id;
	public int Id {get {return m_Id;}}
[SerializeField]
	private string m_DESC;
	public string DESC {get {return m_DESC;}}
		public bool LoadData(string strLine)
		{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
			string[] datas = strLine.Split('\t');
			if(datas.Length != 38)
				System.Diagnostics.Debug.Assert(false,"var count not match talble cols Tab_Relation");
			if(datas.Length != 38)
				throw new Exception("var count not match talble  cols");
			m_Id = TableUtils.ParseInt(datas[0]);
			m_DESC = datas[1];
			m_Relation[0] = TableUtils.ParseInt(datas[2]);
			m_Relation[1] = TableUtils.ParseInt(datas[3]);
			m_Relation[2] = TableUtils.ParseInt(datas[4]);
			m_Relation[3] = TableUtils.ParseInt(datas[5]);
			m_Relation[4] = TableUtils.ParseInt(datas[6]);
			m_Relation[5] = TableUtils.ParseInt(datas[7]);
			m_Relation[6] = TableUtils.ParseInt(datas[8]);
			m_Relation[7] = TableUtils.ParseInt(datas[9]);
			m_Relation[8] = TableUtils.ParseInt(datas[10]);
			m_Relation[9] = TableUtils.ParseInt(datas[11]);
			m_Relation[10] = TableUtils.ParseInt(datas[12]);
			m_Relation[11] = TableUtils.ParseInt(datas[13]);
			m_Relation[12] = TableUtils.ParseInt(datas[14]);
			m_Relation[13] = TableUtils.ParseInt(datas[15]);
			m_Relation[14] = TableUtils.ParseInt(datas[16]);
			m_Relation[15] = TableUtils.ParseInt(datas[17]);
			m_Relation[16] = TableUtils.ParseInt(datas[18]);
			m_Relation[17] = TableUtils.ParseInt(datas[19]);
			m_Relation[18] = TableUtils.ParseInt(datas[20]);
			m_Relation[19] = TableUtils.ParseInt(datas[21]);
			m_Relation[20] = TableUtils.ParseInt(datas[22]);
			m_Relation[21] = TableUtils.ParseInt(datas[23]);
			m_Relation[22] = TableUtils.ParseInt(datas[24]);
			m_Relation[23] = TableUtils.ParseInt(datas[25]);
			m_Relation[24] = TableUtils.ParseInt(datas[26]);
			m_Relation[25] = TableUtils.ParseInt(datas[27]);
			m_Relation[26] = TableUtils.ParseInt(datas[28]);
			m_Relation[27] = TableUtils.ParseInt(datas[29]);
			m_Relation[28] = TableUtils.ParseInt(datas[30]);
			m_Relation[29] = TableUtils.ParseInt(datas[31]);
			m_Relation[30] = TableUtils.ParseInt(datas[32]);
			m_Relation[31] = TableUtils.ParseInt(datas[33]);
			m_Relation[32] = TableUtils.ParseInt(datas[34]);
			m_Relation[33] = TableUtils.ParseInt(datas[35]);
			m_Relation[34] = TableUtils.ParseInt(datas[36]);
			m_Relation[35] = TableUtils.ParseInt(datas[37]);
			return true;
#else
			return true;
#endif
		}
	}
}
