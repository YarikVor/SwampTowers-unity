#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyInfoHolder))]
public class EnemyInfoHolderEditor : Editor
{
    private EnemyInfoHolder _enemyInfoHolder;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _enemyInfoHolder = (EnemyInfoHolder)target;
        var enemyInfo = _enemyInfoHolder.EnemyInfo;

        OnInspectorGUI(enemyInfo);
    }

    public static void OnInspectorGUI(EnemyInfo enemyInfo)
    {
        List<string> errors = new();

        EditorGUILayout.LabelField(
            "Scale",
            EnemyHealth.HealthToScale(enemyInfo.RelativeHealth).ToString()
        );

        if (enemyInfo.enemyType == 0)
        {
            Error("Please select type");
        }
        else if (!enemyInfo.enemyType.IsValid())
        {
            Error("Need select type: Ground/Fly, Boss/Enemy");
        }

        if (enemyInfo.health > enemyInfo.maxHealth)
        {
            Error("Health > Max Health");
        }

        foreach (var text in errors)
            EditorGUILayout.HelpBox(text, MessageType.Error);

        void Error(string text)
        {
            errors.Add(text);
        }
    }
}
#endif