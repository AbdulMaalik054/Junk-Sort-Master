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
        if (!Application.isPlaying ||
            BreakdownManager.Instance == null ||
            GameManager.Instance == null)
        {
            EditorGUILayout.HelpBox(
                "Gameplay not running or missing references.",
                MessageType.Warning
            );
            return;
        }

        var breakdown = BreakdownManager.Instance;
        var lanes = GameManager.Instance.laneControllers;

        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("GLOBAL STATE", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(
            "Any Lane Broken:",
            breakdown.IsAnyLaneBroken() ? "YES" : "NO"
        );

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("LANE STATES", EditorStyles.boldLabel);

        for (int i = 0; i < lanes.Length; i++)
        {
            var bulbController = lanes[i];
            if (bulbController == null) continue;

            var tracker = bulbController.GetComponentInParent<SortingLaneTracker>();
            if (tracker == null) continue;

            bool isBroken = breakdown.IsLaneBroken(i);
            int penalties = breakdown.GetPenaltyCount(i);

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                $"Lane {i} ({bulbController.name})",
                EditorStyles.boldLabel
            );

            EditorGUILayout.LabelField($"Penalties: {penalties}");
            EditorGUILayout.LabelField($"Bonuses:   {tracker.bonusCount}");
            EditorGUILayout.LabelField($"Streak:    {tracker.streakCount}");
            EditorGUILayout.LabelField($"Broken:    {isBroken}");

            DrawBulbs(bulbController);

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

    // ================= BULBS =================

    private void DrawBulbs(BulbIndicatorController bulbs)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("BULBS", EditorStyles.boldLabel);

        DrawBulbRow("Penalty (Red)", bulbs.penaltyBulbs);
        DrawBulbRow("Bonus (Green)", bulbs.bonusBulbs);

        DrawSingleBulb("Big Red", bulbs.BigRedBulb);
        DrawSingleBulb("Big Green", bulbs.BigGreenBulb);
    }

    private void DrawBulbRow(string label, BulbEmissionController[] bulbs)
    {
        if (bulbs == null || bulbs.Length == 0) return;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(120));

        foreach (var bulb in bulbs)
            DrawBulbIndicator(bulb != null && bulb.CurrentIntensity > 0f);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSingleBulb(string label, BulbEmissionController bulb)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(120));
        DrawBulbIndicator(bulb != null && bulb.CurrentIntensity > 0f);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBulbIndicator(bool isOn)
    {
        Color prev = GUI.color;
        GUI.color = isOn ? Color.green : Color.gray;
        GUILayout.Box(" ", GUILayout.Width(16), GUILayout.Height(16));
        GUI.color = prev;
    }
}
#endif
