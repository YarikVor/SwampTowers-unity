#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(TowerUpgradeInfo))]

public class TowerLevelPropertyDrawer : PropertyDrawer
{
    private const float _space = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int indent = EditorGUI.indentLevel;

        OnGUISave(position, property, label);

        EditorGUI.indentLevel = indent;
    }

    private void OnGUISave(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.indentLevel = 0;
        var firstLineRect = position;
        firstLineRect.height = EditorGUIUtility.singleLineHeight;
        DrawTowerLevel(firstLineRect, property);
    }

    private void DrawTowerLevel(Rect rect, SerializedProperty towerLevel)
    {
        var width = rect.width /= 4;
        
        PropertyField(towerLevel.FindPropertyRelative(nameof(TowerUpgradeInfo.tower)));
        
        rect.width /= 2;
        
        LabelField("Sell");
        PropertyField(towerLevel.FindPropertyRelative(nameof(TowerUpgradeInfo.salePrice)));
        
        LabelField("Buy");
        PropertyField(towerLevel.FindPropertyRelative(nameof(TowerUpgradeInfo.buyPrice)));
        
        LabelField("Build Time");
        PropertyField(towerLevel.FindPropertyRelative(nameof(TowerUpgradeInfo.buildTime)));
        
        void PropertyField(SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, GUIContent.none);
            rect.x += rect.width;
        }
        
        void LabelField(string str)
        {
            EditorGUI.LabelField(rect, str);
            rect.x += rect.width;
        }
    }
    
    
    
}



#endif
