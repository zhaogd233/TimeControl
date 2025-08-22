using System.Collections.Generic;
using System;

namespace Games.Table{

	public class TableMapDefine 
	{
		public static readonly Dictionary<Type, string> TableFileMap = new()
        {
            { typeof(Tab_Bullet), "Bullet" },
            { typeof(Tab_CoolDownTimeInfo), "CoolDownTimeInfo" },
            { typeof(Tab_CopyScene), "CopyScene" },
            { typeof(Tab_CopySceneRule), "CopySceneRule" },
            { typeof(Tab_LevelValue), "LevelValue" },
            { typeof(Tab_NpcAttr), "NpcAttr" },
            { typeof(Tab_PlayerAttrInitial), "PlayerAttrInitial" },
            { typeof(Tab_ProfessionConfigCom), "ProfessionConfig_Com" },
            { typeof(Tab_Relation), "Relation" },
            { typeof(Tab_RoleBaseAttr), "RoleBaseAttr" },
            { typeof(Tab_SceneClass), "SceneClass" },
            { typeof(Tab_SceneNpc), "SceneNpc" },
            { typeof(Tab_ScenePosDictionary), "ScenePosDictionary" },
            { typeof(Tab_SceneRule), "SceneRule" },
            { typeof(Tab_SkillValueFormula), "SkillValueFormula" },
            { typeof(Tab_SkillXmlBase), "SkillXmlBase" },
            { typeof(Tab_Teleport), "Teleport" },
            { typeof(Tab_ObjTypeConfig), "ObjTypeConfig" },
            { typeof(Tab_SceneResource), "SceneResource" },
        };
	}
}
