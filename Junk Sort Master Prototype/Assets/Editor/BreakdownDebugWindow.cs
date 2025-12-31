#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class BreakdownDebugWindow : EditorWindow
{
    private Vector2 scroll;

    [MenuItem("Tools/Debug/Breakdown Debugger")]
    public static void ShowWindow()
    {
        GetWindow<BreakdownDebugWindow>("Breakdown Debugger");
    }

    private void OnGUI()
    {
        if (BreakdownManager.Instance == null || GameManager.Instance == null)
        {
            EditorGUILayout.HelpBox("Gameplay not running or missing references.", MessageType.Warning);
            return;
        }

        var breakdown = BreakdownManager.Instance;
        var lanes = GameManager.Instance.laneControllers;

        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("GLOBAL STATE", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Is Broken:", breakdown.IsAnyLaneBroken() ? "YES" : "NO");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("LANE STATES", EditorStyles.boldLabel);

        for (int i = 0; i < lanes.Length; i++)
        {
            var lane = lanes[i];
            bool isBroken = breakdown.IsLaneBroken(i);

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"Lane {i} ({lane.name})",
                isBroken ? EditorStyles.whiteLargeLabel : EditorStyles.label);

            EditorGUILayout.LabelField($"Penalties:  {lane.laneTracker.penaltyCount}");
            EditorGUILayout.LabelField($"Bonuses:    {lane.laneTracker.bonusCount}");
            EditorGUILayout.LabelField($"Broken:     {isBroken}");

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+ Penalty"))
                breakdown.AddPenalty(i);

            if (isBroken && GUILayout.Button("Repair"))
                breakdown.RepairLane(i);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }
}
#endif
