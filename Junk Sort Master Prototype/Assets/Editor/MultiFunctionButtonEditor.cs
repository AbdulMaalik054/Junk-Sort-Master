using UnityEditor; // This namespace is required for editor scripts
using UnityEngine;

[CustomEditor(typeof(UIActionButton))]
public class MultiFunctionButtonEditor : Editor
{
    // SerializedProperty objects link the script variables to the Inspector display
    private SerializedProperty currentModeProp;
    private SerializedProperty typeAActionProp;
    private SerializedProperty typeBActionProp;

    void OnEnable()
    {
        // Initialize the SerializedProperties by finding their names in the target script
        currentModeProp = serializedObject.FindProperty("CurrentMode");
        typeAActionProp = serializedObject.FindProperty("action");
        typeBActionProp = serializedObject.FindProperty("difficulty");
    }

    // This method overrides how the Inspector draws the component
    public override void OnInspectorGUI()
    {
        // Always call this first thing
        serializedObject.Update();

        // Draw the default inspector properties that don't have [HideInInspector]
        DrawDefaultInspector();

        // Get a reference to the actual script instance
        UIActionButton myScript = (UIActionButton)target;

        // --- Custom Drawing Logic ---

        // Check which mode is currently selected in the dropdown
        switch (myScript.CurrentMode)
        {
            case UIActionButton.ButtonModes.UIButtonAction:
                // If Enum A is selected, draw ONLY the Type A options
                EditorGUILayout.PropertyField(typeAActionProp, new GUIContent("UIButtonAction"));
                break;

            case UIActionButton.ButtonModes.DifficultyButtons:
                // If Enum B is selected, draw ONLY the Type B options
                EditorGUILayout.PropertyField(typeBActionProp, new GUIContent("DifficultyButtons"));
                break;
        }

        // Apply changes back to the actual script instance
        serializedObject.ApplyModifiedProperties();
    }
}
