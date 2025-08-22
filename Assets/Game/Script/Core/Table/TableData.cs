#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Zeus.Framework.ConfigDB;
using System.Linq;

namespace Core.Table{

    /// <summary>
    /// 单条数据的接口
    /// </summary>
public interface ITableBase
{
    int Id { get; }
    bool LoadData(string strLine);
}

    /// <summary>
    /// 单表的接口
    /// </summary>
public interface ITable
{
    void Clear();
}

public class TableData<T> : IDataTable<T>,ITable where T : class, ITableBase, new()
{
    private Dictionary<int, T> m_tableDic = new Dictionary<int, T>();
    private HashSet<int> m_nullSet = new HashSet<int>();

    public bool Init(string tableString)
    {
        if (string.IsNullOrEmpty(tableString) == true)
            return true;
        string[] lineData = tableString.Split('\n');
        for (int i = 3; i < lineData.Length; i++)
        {
            string curLine = lineData[i];
            if (curLine.StartsWith("#"))
            {
                continue;
            }

            if (curLine.Length <= 0)
            {
                continue;
            }

            T newData = new T();
            if (!newData.LoadData(curLine))
            {
                return false;
            }

            m_tableDic[newData.Id] = newData;
        }
        return true;
    }

    public void InitTableObj(List<T> list)
    {
        if (list == null)
        {
            return;
        }
        m_tableDic.Clear();
        for (int i = 0; i < list.Count; ++i)
        {
            T data = list[i];
            if (null != data)
            {
                m_tableDic.Add(data.Id, data);
            }
        }
    }

    public void Clear()
    {
        m_tableDic.Clear();
        m_nullSet.Clear();
    }

    public void AddNullDataID(int id)
    {
        if (m_nullSet != null)
            m_nullSet.Add(id);
    }

    public bool IsNullData(int id)
    {
        return m_nullSet.Contains(id) == true;
    }

    public T GetDataByKV(string key, object value, List<T> result)
    {
        var type = typeof(T);
        var field = type.GetProperty(key);
        if (field == null)
        {
            Debug.LogError($"properity {key} doesn't exist in {type}");
            return default(T);
        }
        foreach (var pair in m_tableDic)
        {
            var val = field.GetValue(pair.Value);
            if (val.Equals(value))
            {
                if (result == null)
                {
                    return pair.Value;
                }
                else
                {
                    result.Add(pair.Value);
                }
            }
        }
        if (null != result && result.Count > 0)
        {
            return result[0];
        }
        return null;
    }

    #region interface IDataTable
    public int Count
    {
        get
        {
            return m_tableDic.Count();
        }
    }
    public IEnumerable<T> Values
    {
        get
        {
            return m_tableDic.Values;
        }
    }
    public IEnumerable<int> Keys
    {
        get
        {
            return m_tableDic.Keys;
        }
    }
    public T this[int index]
    {
        get
        {
            if (m_tableDic.ContainsKey(index))
                return m_tableDic[index];
            return null;
        }
    }
    public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
    {
        return m_tableDic.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return m_tableDic.GetEnumerator();
    }
    public bool ContainsKey(int id)
    {
        return m_tableDic.ContainsKey(id);
    }
    public T GetDataByKey(int key)
    {
        if (m_tableDic.ContainsKey(key))
            return m_tableDic[key];
        return null;
    }
    public T GetDataByKV(string key, int value)
    {
        return GetDataByKV(key, (object)value, null);
    }
    public T GetDataByKV(string key, int value, List<T> result)
    {
        return GetDataByKV(key, (object)value, result);
    }
    public T GetDataByKV<T1, T2>(string key1, T1 value1, string key2, T2 value2)
    {
        return GetDataByKV(key1, value1, key2, value2, null);
    }
    public T GetDataByKV<T1, T2>(string key1, T1 value1, string key2, T2 value2, List<T> result)
    {
        var type = m_tableDic.GetType();
        var field1 = type.GetProperty(key1);
        var field2 = type.GetProperty(key2);
        foreach (var pair in m_tableDic)
        {
            if (((T1)field1.GetValue(pair.Value)).Equals(value1) && ((T2)field2.GetValue(pair.Value)).Equals(value2))
            {
                if (null != result)
                {
                    result.Add(pair.Value);
                }
                else
                {
                    return pair.Value;
                }
            }
        }
        if (null != result && result.Count > 0)
        {
            return result[0];
        }
        return null;
    }
    public T GetDataByKV(string key, string value)
    {
        return GetDataByKV(key, (object)value, null);
    }
    public T GetDataByKV(string key, string value, List<T> result)
    {
        var type = m_tableDic.GetType();
        var field = type.GetField((string)key);
        foreach (var pair in m_tableDic)
        {
            if (field.GetValue(pair.Value).Equals(value))
            {
                result.Add(pair.Value);
            }
        }
        return null;
    }
    public T GetDataByKV(string key1, string value1, string key2, string value2)
    {
        return GetDataByKV<string, string>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, string value1, string key2, string value2, List<T> result)
    {
        return GetDataByKV<string, string>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, string value1, string key2, int value2)
    {
        return GetDataByKV<string, int>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, string value1, string key2, int value2, List<T> result)
    {
        return GetDataByKV<string, int>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, int value1, string key2, int value2)
    {
        return GetDataByKV<int, int>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, int value1, string key2, int value2, List<T> result)
    {
        return GetDataByKV<int, int>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, int value1, string key2, string value2)
    {
        return GetDataByKV<int, string>(key1, value1, key2, value2);
    }
    public T GetDataByKV(string key1, int value1, string key2, string value2, List<T> result)
    {
        return GetDataByKV<int, string>(key1, value1, key2, value2);
    }
    public List<T> GetDataArray()
    {
        return m_tableDic.Values.ToList();
    }
    public void Prepare()
    {
    }
    public void Reinit()
    {
        m_nullSet.Clear();
        m_tableDic.Clear();
    }
    #endregion
}
}