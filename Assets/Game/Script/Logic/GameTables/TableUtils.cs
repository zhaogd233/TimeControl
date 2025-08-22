#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.IO;

namespace Games.Table{

public class TableUtils
{
    private static bool m_OnlyReadPackage = true;
    public static void ChangeTableLoadSetting(bool bOnlyReadPakcage)
    {
        m_OnlyReadPackage = bOnlyReadPakcage;
    }
    public static int UseTableConfigId 
    { 
        get
        {
            return 0;
        }
    }
    public static void ChangeUseTableConfigId(int id)
    {

    }
    private static string GetWinPathTail()
    {
#if UNITY_EDITOR
        return "/Editor";
#endif
        return "";
    }

    private static string GetTablePathTail()
    {
        return "Table";
    }

    public static string ExecReadByPath(string tablePath) 
    {
        try
        {
            if (null == tablePath) 
            {
                System.Console.WriteLine("tablepath not null");
                return "";
            }

            if (tablePath.Length <= 0) 
            {
                System.Console.WriteLine("tablepath length is 0");
                return "";
            }

            if (!File.Exists(tablePath))
            {
                System.Console.WriteLine(tablePath + " file not exist");
                return "";
            }
            FileStream fs = new FileStream(tablePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            string retString = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            return retString;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.ToString());
            return "";
        }
    }

    public static string ReadFileString(string tableName)
    {
        string result = "";
        do
        {
            //优先处理项目的热更文件
            string resultPath = string.Format("{0}/ResData{1}/{2}/{3}.data", Application.persistentDataPath, GetWinPathTail(), GetTablePathTail(), tableName.ToLower());
            bool bExist = false == m_OnlyReadPackage && File.Exists(resultPath);
            /*if (bExist)
            {
                var ab = AssetBundle.LoadFromFile(resultPath);
                if (ab)
                {
                    string assetName = string.Format("Assets/Game/Bundle/{0}/{1}.asset", GetTablePathTail(), tableName);
                    TableManager.Load4Deserialize(tableName, ab.LoadAsset(assetName));
                    ab.Unload(false);
                }
                break;
            }*/

            resultPath = string.Format("{0}/ResData{1}/{2}/{3}.txt", Application.persistentDataPath, GetWinPathTail(), GetTablePathTail(), tableName);
            bExist = false == m_OnlyReadPackage && File.Exists(resultPath);
            if (bExist)
            {
                result = ExecReadByPath(resultPath);
                break;
            }
            //然后处理zeus的热更文件
            resultPath = Zeus.Core.FileSystem.OuterPackage.GetRealPath(string.Format("{0}/{1}.txt", GetTablePathTail(), tableName));
            bExist = false == m_OnlyReadPackage && File.Exists(resultPath);
            if (bExist) 
            {
                result = ExecReadByPath(resultPath);
                break;
            }
#if !UNITY_EDITOR //Editor下强制读文本，如果有测试读表ab的需求，这里单独处理下，防止引入其他可能引起运行时不稳定问题
            //序列化后的配置读取
            BundleType type = BundleType.TABLE;
            if(UseTableConfigId == 2)
                type = BundleType.TABLE2;
            else if(UseTableConfigId == 3)
                type = BundleType.TABLE3;
            var assetRef = AssetManager.LoadAssetSync(type, tableName);
            if(null != assetRef && null != assetRef.AssetObject)
            {
                TableManager.Load4Deserialize(tableName, assetRef.AssetObject);
                break;
            }
            else
            {
                LogModule.LogErrorFormat("{0} load failed, assetRef or assetObject is null", tableName);
            }
#else
            /*string assetTableName = string.Format("Assets/Game/Bundle/{0}/{1}.asset", GetTablePathTail(), tableName);
            var asset = AssetDatabase.LoadMainAssetAtPath(assetTableName);
            if (asset != null)
            {
                TableManager.Load4Deserialize(tableName, asset);
                break;
            }*/
#endif
            //如果zues加载没有加载到，这种情况一般出现在代码运行在编辑模式而非运行模式，直接按路径 读取文件
            resultPath = string.Format("{0}/Game/Bundle/{1}/{2}.txt", Application.dataPath, GetTablePathTail(), tableName);
            if (File.Exists(resultPath))
            {
                //LogModule.Log($"LoadTableFromFile {resultPath}");
                result = ExecReadByPath(resultPath);
                break;
            }
        } while (false);
        return result;
    }

    public static UnityEngine.Object ReadTableObject(string tableName)
    {
        UnityEngine.Object result = null;
        do
        {
            //优先处理项目的热更文件
            string resultPath = string.Format("{0}/ResData{1}/{2}/{3}.data", Application.persistentDataPath, GetWinPathTail(), GetTablePathTail(), tableName.ToLower());
            bool bExist = false == m_OnlyReadPackage && File.Exists(resultPath);
            if (bExist)
            {
                var ab = AssetBundle.LoadFromFile(resultPath);
                if (ab)
                {
                    string assetName = string.Format("Assets/Game/Bundle/{0}/{1}.asset", GetTablePathTail(), tableName);
                    result = ab.LoadAsset(assetName);
                    ab.Unload(false);
                }
                break;
            }
#if !UNITY_EDITOR
            //序列化后的配置读取
            BundleType type = BundleType.TABLE;
            if(UseTableConfigId == 2)
                type = BundleType.TABLE2;
            else if(UseTableConfigId == 3)
                type = BundleType.TABLE3;
            var assetRef = AssetManager.LoadAssetSync(type, tableName);
            if(null != assetRef && null != assetRef.AssetObject)
            {
                result = assetRef.AssetObject;
                break;
            }
            else
            {
                LogModule.LogErrorFormat("{0} load failed, assetRef or assetObject is null", tableName);
            }
#else
            string assetTableName = string.Format("Assets/Game/Bundle/{0}/{1}.asset", GetTablePathTail(), tableName);
            result = AssetDatabase.LoadMainAssetAtPath(assetTableName);
#endif
            } while (false);
        return result;
    }

    public static int ParseInt(string strData)
    {
            //处理下日期一类的前面有*的数据，有*是因为Excel打开这类数字格式会转为科学计数法导致异常
            if (strData.StartsWith("*"))
            {
                strData = strData.Substring(1);
            }
        if (int.TryParse(strData, out var retValue))
        {
            return retValue;
        }

        return 0;
    }

    public static long ParseLong(string strData)
    {
        //处理下日期一类的前面有*的数据，有*是因为Excel打开这类数字格式会转为科学计数法导致异常
        if (strData.StartsWith("*"))
        {
            strData = strData.Substring(1);
        }
        if (long.TryParse(strData, out var retValue))
        {
            return retValue;
        }

        return 0;
    }

    public static float ParseFloat(string strData)
    {
        float retValue = 0;
        if (float.TryParse(strData, out retValue))
        {
            return retValue;
        }

        return 0;
    }

    public static bool ParseBool(string strData)
    {
        bool retValue = false;
        int tempValue = 0;
        if (bool.TryParse(strData, out retValue))
        {
            return retValue;
        }
        else if (int.TryParse(strData, out tempValue))
        {
            return 0 == tempValue ? false : true;
        }

        return false;
    }
	
	public static short ParseShort(string strData)
    {
        short retValue = 0;
        if (short.TryParse(strData, out retValue))
        {
            return retValue;
        }

        return 0;
	}
	
	public static byte ParseByte(string strData)
	{
		byte retValue = 0;
		if (byte.TryParse(strData, out retValue))
		{
			return retValue;
		}

		return 0;
	}

    public static uint ParseUInt(string strData)
    {
        //处理下日期一类的前面有*的数据，有*是因为Excel打开这类数字格式会转为科学计数法导致异常
        if (strData.StartsWith("*"))
        {
            strData = strData.Substring(1);
        }
        if (uint.TryParse(strData, out var retValue))
        {
            return retValue;
        }

        return 0;
    }

    public static ulong ParseULong(string strData)
    {
        //处理下日期一类的前面有*的数据，有*是因为Excel打开这类数字格式会转为科学计数法导致异常
        if (strData.StartsWith("*"))
        {
            strData = strData.Substring(1);
        }
        if (ulong.TryParse(strData, out var retValue))
        {
            return retValue;
        }

        return 0;
    }

    public static ushort ParseUShort(string strData)
    {
        ushort retValue = 0;
        if (ushort.TryParse(strData, out retValue))
        {
            return retValue;
        }

        return 0;
    }
 }
}