using System;
using UnityEngine;

public class LogModule
{
    public enum LogSet
    {
        OPEN_NORMAL_LOG = 0,
        OPEN_DEBUGINFO = 1,
        OPEN_SRDEBUG = 2,
    }

    public enum LogSystemName
    {
        UNIVERSAL_LOG = 0,
        AUDIO_SYSTEM = 1,
        NAVICATION_SYSTEM = 2,
        QINGGONG_SYSTEM = 3,
        RENDERING_SYSTEM = 4,
        ASSET_SYSTEM = 5,
    }


    public static bool openNormalLog = false;
    public static bool enableMemoryLog = false;

    public static void InitLogSet()
    {
#if !UNITY_EDITOR
        Tab_LogSet tab = TableManager.GetLogSetByID((int)LogSet.OPEN_NORMAL_LOG, 0);
        if (tab == null)
            return;
        openNormalLog = tab.Value > 0;
#else
        openNormalLog = false;
#endif

        RegisterCallBack();
    }

    public static void RegisterCallBack()
    {
      //  Application.logMessageReceived += LogCallBack;
    }

    public static void LogError(object log)
    {
        Debug.LogError(log);
    }

    public static  void LogError(string log)
    {
        Debug.LogError(log);
    }

    public static void LogError(object log, UnityEngine.Object context)
    {
        Debug.LogError(log, context);
    }

    public static void LogException(Exception e)
    {
        Debug.LogException(e);
    }
    public static void LogWarning(object log)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogWarning(log);
    }

    public static void LogWarning(string log)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogWarning(log);
    }
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogWarning(message, context);
    }
    
    [System.Diagnostics.Conditional("LOG_ENABLE")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object log)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.Log(log);
    }
    
    [System.Diagnostics.Conditional("LOG_ENABLE")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(string log)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.Log(log);
    }
    
    [System.Diagnostics.Conditional("LOG_ENABLE")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object log, UnityEngine.Object context)
    {
        //return;
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.Log(log, context);
    }
     [System.Diagnostics.Conditional("LOG_ENABLE")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(LogSystemName logSystem, object log, UnityEngine.Object context)
    {
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.Log(GenerateLog(logSystem, log), context);
    }
     [System.Diagnostics.Conditional("LOG_ENABLE")]
     [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(LogSystemName logSystem, string format, params object[] args)
    {
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogFormat(GenerateLog(logSystem, String.Format(format, args)));
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }

    public static void LogErrorFormat(LogSystemName logSystem, string format, params object[] args)
    {
        Debug.LogError(GenerateLog(logSystem, String.Format(format, args)));
    }
    public static void LogWarningFormat(string format, params object[] args)
    {
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogWarningFormat(format, args);
    }
    public static void LogWarningFormat(LogSystemName logSystem, string format, params object[] args)
    {
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
     //   Debug.LogWarning(GenerateLog(logSystem, String.Format(format, args)));
    }
     [System.Diagnostics.Conditional("LOG_ENABLE")]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        /*if (!PlatformHelper.IsEnableDebugMode()
#if !UNITY_EDITOR
            || openNormalLog == false
#endif
            )
            return;*/
        Debug.LogFormat(format, args);
    }
    private static string GenerateLog(LogSystemName logSystem, object log)
    {
#if UNITY_EDITOR
        return String.Format("<color=green>{0}</color>------>{1}", logSystem, log);
#else
        return String.Format("{0}------>{1}", logSystem, log);
#endif
    }
  /*  public static void FileLog(string strLog)
    {
        if (!PlatformHelper.IsEnableDebugMode())
            return;

        string strTime = DateTime.Now.ToString("yyyy-MM-dd");
        string filePath = Application.persistentDataPath + "/Log/" + strTime + ".txt";
        Utils.CheckTargetPath(filePath);
        Utils.AppendStringToFile(filePath, strLog);
    }

    public static void CollisionMeshDataFileLog(string strLog)
    {
        if (!PlatformHelper.IsEnableDebugMode())
            return;

        string strTime = DateTime.Now.ToString("yyyy-MM-dd");
        string filePath = Application.persistentDataPath + "/Log/CollisionMeshData" + strTime + ".txt";
        Utils.CheckTargetPath(filePath);
        Utils.AppendStringToFile(filePath, strLog);
    }

    public static string ByteToString(byte[] byteData, int nStartIndex, int nCount)
    {
        if (!PlatformHelper.IsEnableDebugMode()) return "";

        string strResult = "";
        if (nStartIndex < 0 || nStartIndex >= byteData.Length)
        {
            return strResult;
        }

        for (int i = nStartIndex; i < nCount && i < byteData.Length; i++)
        {
            strResult += Convert.ToString(byteData[i]);
        }
        return strResult;
    }

    static public bool IsLogToScreen = false;

    static void LogCallBack(string condition, string stackTrace, LogType type)
    {
        if (!PlatformHelper.IsEnableDebugMode())
            return;
        if (IsLogToScreen)
        {
            DebugInfo.AddDebugLog(condition);
        }

        if (DebugInfo.EnableCollisionMeshDataLog)
        {
            if (condition.Contains("CollisionMeshData couldn't be created") ||
                condition.Contains("This Mesh Collider is attached to GameObject at path"))
            {
                CollisionMeshDataFileLog(string.Format("RunningScene:{0}\t{1}", GameManager.RunningScene, condition));
            }
        }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        string logType;
        string logReason; 
        string logStack;
        HandleLog(condition, stackTrace, type, out logType, out logReason, out logStack);
        string logStr = string.Empty;
        switch (type)
        {
            case LogType.Log:
            case LogType.Assert:
            case LogType.Warning:
            {
                logStr = string.Format("type:{0}：name:{1}\n", logType, logReason);
            }
            break;
            case LogType.Exception:
            case LogType.Error:
            {
                logStr = string.Format("type:{0}：name:{1}\n{2}\n", logType, logReason, logStack);
            }
            break;
        }
        FileLog(logStr);
#endif
    }

    private static List<string> m_conditionList = new List<string>();
    private static string[] sendFilter = new string[]
    {
        "on a NavMesh",
    };
    

    public static void HandleLog(string condition, string stackTrace, LogType logLevel, out string type, out string reason, out string stack)
    {
        type = "";
        reason = "";
        stack = "";

        if (!string.IsNullOrEmpty(condition))
        {
            try
            {
                if ((LogType.Exception == logLevel) && condition.Contains("Exception"))
                {
                    Match match = new Regex(@"^(?<errorType>\S+):\s*(?<errorMessage>.*)", RegexOptions.Singleline).Match(condition);
                    if (match.Success)
                    {
                        type = match.Groups["errorType"].Value.Trim();
                        reason = match.Groups["errorMessage"].Value.Trim();
                    }
                }
                else if ((LogType.Error == logLevel) && condition.StartsWith("Unhandled Exception:"))
                {
                    Match match = new Regex(@"^Unhandled\s+Exception:\s*(?<exceptionName>\S+):\s*(?<exceptionDetail>.*)", RegexOptions.Singleline).Match(condition);
                    if (match.Success)
                    {
                        string exceptionName = match.Groups["exceptionName"].Value.Trim();
                        string exceptionDetail = match.Groups["exceptionDetail"].Value.Trim();
                        int dotLocation = exceptionName.LastIndexOf(".");
                        if (dotLocation > 0 && dotLocation != exceptionName.Length)
                        {
                            type = exceptionName.Substring(dotLocation + 1);
                        }
                        else
                        {
                            type = exceptionName;
                        }
                        int stackLocation = exceptionDetail.IndexOf(" at ");
                        if (stackLocation > 0)
                        {
                            reason = exceptionDetail.Substring(0, stackLocation);
                            // substring after " at "
                            string callStacks = exceptionDetail.Substring(stackLocation + 3).Replace(" at ", "\n").Replace("in <filename unknown>:0", "").Replace("[0x00000]", "");
                            stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());
                        }
                        else
                        {
                            reason = exceptionDetail;
                        }

                        // for LuaScriptException
                        if (type.Equals("LuaScriptException") && exceptionDetail.Contains(".lua") && exceptionDetail.Contains("stack traceback:"))
                        {
                            stackLocation = exceptionDetail.IndexOf("stack traceback:");
                            if (stackLocation > 0)
                            {
                                reason = exceptionDetail.Substring(0, stackLocation);
                                // substring after "stack traceback:"
                                string callStacks = exceptionDetail.Substring(stackLocation + 16).Replace(" [", " \n[");
                                stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(reason))
            {
                reason = condition;
            }
        }

        if (string.IsNullOrEmpty(type))
        {
            type = string.Format("Unity{0}", logLevel.ToString());
        }

        if (string.IsNullOrEmpty(stackTrace))
        {
            stackTrace = StackTraceUtility.ExtractStackTrace();
        }

        if (string.IsNullOrEmpty(stackTrace))
        {
            stackTrace = "Empty";
        }
        else
        {
            try
            {
                string[] frames = stackTrace.Split('\n');
                if (frames != null && frames.Length > 0)
                {
                    StringBuilder trimFrameBuilder = new StringBuilder();
                    string frame = null;
                    int count = frames.Length;
                    for (int i = 0; i < count; i++)
                    {
                        frame = frames[i];
                        if (string.IsNullOrEmpty(frame) || string.IsNullOrEmpty(frame.Trim()))
                        {
                            continue;
                        }
                        frame = frame.Trim();
                        // System.Collections.Generic
                        if (frame.StartsWith("System.Collections.Generic.") || frame.StartsWith("ShimEnumerator"))
                        {
                            continue;
                        }
                        if (frame.StartsWith("Bugly"))
                        {
                            continue;
                        }
                        if (frame.Contains("..ctor"))
                        {
                            continue;
                        }
                        int start = frame.ToLower().IndexOf("(at");
                        int end = frame.ToLower().IndexOf("/assets/");
                        if (start > 0 && end > 0)
                        {
                            trimFrameBuilder.AppendFormat("{0}(at {1}", frame.Substring(0, start).Replace(":", "."), frame.Substring(end));
                        }
                        else
                        {
                            trimFrameBuilder.Append(frame.Replace(":", "."));
                        }
                        trimFrameBuilder.AppendLine();
                    }
                    stackTrace = trimFrameBuilder.ToString();
                }
            }
            catch
            {
            }
        }
        stack = stackTrace;
    }*/
}
