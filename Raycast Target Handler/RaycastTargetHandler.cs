/*=================== Raycast Target Handler
It's me, Miko!

Tool to check state of raycast targets on UI Prefab.
Just put to object field prefab and check what You should enable.

0.1.0 Added Check All Targets In Prefabs  
0.1.1 Added Clear Console and Toogle to auto clear console 
 ===================*/
using UnityEditor;
using UnityEngine;

public class RaycastTargetHandler : EditorWindow
{
    private GameObject prefab;
    private RaycastTargetSearchType stateType = RaycastTargetSearchType.Enabled;
    private bool isClearConsoleOnAutoMode, isColorCodingFlipped;
    private string trueColor = "red";
    private string falseColor = "green";

    [MenuItem("Window/Miko Utils/Raycast Target Handler")]
    public static void ShowWindow() => GetWindow<RaycastTargetHandler>("Raycast Target Handler");
    
    private void OnGUI()
    {
        GUILayout.Label("Prefab", EditorStyles.boldLabel);
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), true);
        
        GUILayout.Label("Type", EditorStyles.boldLabel);
        stateType = (RaycastTargetSearchType)EditorGUILayout.EnumPopup(stateType);
        isClearConsoleOnAutoMode = EditorGUILayout.Toggle("Auto Clear Console On Search Start!", isClearConsoleOnAutoMode);
        isColorCodingFlipped = EditorGUILayout.Toggle("Is Color Coding Flipped?", isColorCodingFlipped);

        GUILayout.Space(5);
        
        if (GUILayout.Button("Search For All Raycast Targets"))
            SearchRaycastTargets();

        if (GUILayout.Button("Clear Console"))
            ClearConsole();
    }

    private void SearchRaycastTargets()
    {
        if(isClearConsoleOnAutoMode) ClearConsole();
        
        if (prefab == null)
        {
            Debug.LogError("<b>Prefab is null</b>. Please assign a prefab object to <b><color=red>Raycast Target Handler<color></b>.");
            return;
        }

        var uiObjects = prefab.GetComponentsInChildren<RectTransform>(true);

        foreach (var uiObject in uiObjects)
        {
            var raycastTarget = uiObject.GetComponent<UnityEngine.UI.Graphic>()?.raycastTarget;

            bool isRaycastTargetMatched = false;

            if (stateType == RaycastTargetSearchType.Enabled)
                isRaycastTargetMatched = (raycastTarget == true);
            else if (stateType == RaycastTargetSearchType.Disabled)
                isRaycastTargetMatched = (raycastTarget == false);
            else if (stateType == RaycastTargetSearchType.All)
            {
                if (raycastTarget == true)
                    Debug.Log(isColorCodingFlipped
                        ? $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={falseColor}>{raycastTarget}</color></b>"
                        : $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={trueColor}>{raycastTarget}</color></b>");
                else
                    Debug.Log(isColorCodingFlipped
                        ? $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={trueColor}>{raycastTarget}</color></b>"
                        : $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={falseColor}>{raycastTarget}</color></b>");
                continue;
            }

            if (isRaycastTargetMatched)
            {
                if(raycastTarget == true)
                    Debug.Log(isColorCodingFlipped
                        ? $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={falseColor}>{raycastTarget}</color></b>"
                        : $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={trueColor}>{raycastTarget}</color></b>");
                else
                    Debug.Log(isColorCodingFlipped
                        ? $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={trueColor}>{raycastTarget}</color></b>"
                        : $"UI Object: <b>{uiObject.name}</b> | Raycast Target: <b><color={falseColor}>{raycastTarget}</color></b>");
            }
        }
    }

    private void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}

public enum RaycastTargetSearchType
{
    Enabled,
    Disabled,
    All
}
