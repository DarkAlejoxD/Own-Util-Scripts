using UnityEditor;

namespace UtilsComplements
{
    public class ScriptFromTemplate
    {
        private const string _pathToDarksTemplate = "Assets/Scripts/Complements/Editor/DarksTemplate.cs.txt";
        private const string _pathToMinigameTemplate = "Assets/Scripts/Complements/Editor/MinigameTemplate.cs.txt";
        private const string _pathToDialogueConditionTemplate = "Assets/Scripts/Complements/Editor/ConditionTemplate.cs.txt";
        private const string _pathToEndDialogueActionTemplate = "Assets/Scripts/Complements/Editor/ActionAfterEndNodeTemplate.cs.txt";

        [MenuItem(itemName: "Assets/Create/C# Unity Darks Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_DarksTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToDarksTemplate, "newScript.cs");
        }

        [MenuItem(itemName: "Assets/Create/C# Minigame Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_MinigameTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToMinigameTemplate, "newScript.cs");
        }

        [MenuItem(itemName: "Assets/Create/C# Dialogue Condition Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_DialogueConditionTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToDialogueConditionTemplate, "DialogueCondition_name.cs");
        }

        [MenuItem(itemName: "Assets/Create/C# End Dialogue Action Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_EndDialogueActionTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_pathToEndDialogueActionTemplate, "EnNodeAction_name.cs");
        }
    }
}