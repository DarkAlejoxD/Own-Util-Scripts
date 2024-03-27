using UnityEditor;

namespace UtilsComplements
{
    #region Report
    //Last checked: March 2024
    //Last modification: March 2024

    //Commentaries:
    //  -   Probably will give some error if try to create a script from the current script templates.
    //  -   Last use was in Atka/SlowJam 2024
    #endregion

    /// <summary>
    /// Create Templates of scripts.
    /// </summary>
    public class ScriptFromTemplate_Base
    {
        private const string _pathToDarksTemplate = "Assets/Scripts/Complements/Editor/DarksTemplate.cs.txt";
        private const string _pathToTemplateTemplate = "Assets/Scripts/Complements/Editor/ScriptTemplateTemplate.cs.txt";
        private const string _pathToDialogueConditionTemplate = "Assets/Scripts/Complements/Editor/ConditionTemplate.cs.txt";
        private const string _pathToEndDialogueActionTemplate = "Assets/Scripts/Complements/Editor/ActionAfterEndNodeTemplate.cs.txt";


        [MenuItem(itemName: "Assets/Create/ScriptTemplates/C# Unity Darks Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_DarksTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToDarksTemplate, "newScript.cs");
        }

        [MenuItem(itemName: "Assets/Create/ScriptTemplates/C# New Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_MinigameTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToTemplateTemplate, "ScriptFromTemplate_name.cs");
        }

        //[MenuItem(itemName: "Assets/Create/C# Dialogue Condition Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_DialogueConditionTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToDialogueConditionTemplate, "DialogueCondition_name.cs");
        }

        //[MenuItem(itemName: "Assets/Create/C# End Dialogue Action Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_EndDialogueActionTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToEndDialogueActionTemplate, "EnNodeAction_name.cs");
        }
    }
}