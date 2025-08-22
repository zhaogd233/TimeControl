using System;
using Core.Table;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Games.Table
{
    [Serializable]
    public class Tab_SceneResource : ITableBase
    {
        [SerializeField] private int m_SceneID;

        [SerializeField] private string m_Name;

        [SerializeField] private string m_ResName;

        [SerializeField] private string m_ScenePolesInfo;

        public int SceneID => m_SceneID;
        public string Name => m_Name;
        public string ResName => m_ResName;
        public string ScenePolesInfo => m_ScenePolesInfo;
        public int Id => m_SceneID;

        public bool LoadData(string strLine)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            var datas = strLine.Split('\t');
            if (datas.Length != 4)
                Debug.Assert(false, "var count not match talble cols Tab_SceneResource");
            if (datas.Length != 4)
                throw new Exception("var count not match talble  cols");
            m_SceneID = TableUtils.ParseInt(datas[0]);
            m_Name = datas[1];
            m_ResName = datas[2];
            m_ScenePolesInfo = datas[3];
            return true;
#else
			return true;
#endif
        }
    }
}