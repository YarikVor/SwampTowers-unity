#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Runtime.CompilerServices;

public class LevelEditorWindow : EditorWindow
{
    public static readonly Color Blue = new Color(aczent, aczent, 1, 1);
    public static readonly Color Green = new Color(aczent, 1, aczent, 1);
    public static readonly Vector2 MinSize = new Vector2(900, 600);

    private const int _marginRight = 3;
    private const float _space = 16f;
    private const float aczent = 0.7f;
    private const float _sizeInfo = 256f;
    private const float _sizeVerticalGrid = 32f;

    private readonly Limiter<int> _countColumns = new Limiter<int>(n => n >= 1, 16);
    private readonly Limiter<Vector2> _cellSize
        = new Limiter<Vector2>(v => v.x >= 10 && v.y >= 10, new Vector2(20, 20));
    private readonly Limiter<int> _indexWave;
    private readonly Limiter<int> _indexOfSubWave;

    private Level _level;
    private SerializedObject _levelSerialized;

    private Vector2 _scrollPosition = Vector2.zero;
    private int _countRows = 0;

    public LevelEditorWindow()
    {
        _indexWave = new Limiter<int>(n => _level?.waves.IsValidIndex(n) ?? false, 0);
        _indexOfSubWave
            = new Limiter<int>(n => _level?.waves[_indexWave].subWaves.IsValidIndex(n) ?? false, 0);
    }

    public void Init(Level level)
    {
        _level = level;
        _levelSerialized = new SerializedObject(level);
    }

    public static void OpenWindow(Level level)
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>($"{level.name} - Level Editor");
        window.minSize = MinSize;
        window.Init(level);
        window.Show();
    }

    private void OnGUI()
    {
        if (_level is not null && _levelSerialized is not null)
        {
            OnAvaibleLevelInGUI();
        }
        else
        {
            GUILayout.Label("Please select or create 'Level' (ScriptableObject)");
        }
    }

    private void OnAvaibleLevelInGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(position.width - _sizeInfo));

        EditorGUILayout.BeginHorizontal();
        DrawServices();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawTitle();
        EditorGUILayout.EndHorizontal();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.Width(_sizeVerticalGrid));
        DrawLeft();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.Width(position.width - _sizeInfo - _sizeVerticalGrid));
        DrawWave();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical(GUILayout.Width(_sizeInfo));

        DrawLevelInfo();
        EditorGUILayout.Separator();
        DrawWaveInfo();
        EditorGUILayout.Separator();
        DrawSubWaveInfo();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawLevelInfo()
    {
        GUILayout.Label("**LevelInfo**");
        var lifeProperty = _levelSerialized.FindProperty(nameof(Level.life));
        var moneyProperty = _levelSerialized.FindProperty(nameof(Level.startMoney));

        EditorGUILayout.PropertyField(lifeProperty);
        EditorGUILayout.PropertyField(moneyProperty);
    }

    private void DrawWaveInfo()
    {
        GUILayout.Label("**Wave info**");

        EditorGUILayout.BeginHorizontal();

        _indexWave.Value = EditorGUILayout.IntField("Wave index", _indexWave, GUILayout.Width(_sizeInfo - 80f));

        var wavesProperty = _levelSerialized.FindProperty(nameof(Level.waves));

        EditorGUILayout.LabelField($"/ {wavesProperty.arraySize - 1}", GUILayout.Width(28f));
        if (GUILayout.Button("<"))
        {
            _indexWave.Value--;
        }
        if (GUILayout.Button(">"))
        {
            _indexWave.Value++;
        }

        EditorGUILayout.EndHorizontal();

        var propertyWave = _levelSerialized.FindProperty(nameof(Level.waves));
        var wavesList = new SerializedList<Wave, Level>(
            propertyWave,
            (l, i) => l.waves[i],
            (w, l, i) => l.waves[i] = w
        );

        var pauseProperty = CurrentWave.FindPropertyRelative(nameof(Wave.pause));
        var spawnTimeProperty = CurrentWave.FindPropertyRelative(nameof(Wave.spawnTime));

        EditorGUILayout.PropertyField(pauseProperty);
        EditorGUILayout.PropertyField(spawnTimeProperty);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add wave"))
        {
            wavesList.Add();
            _indexWave.Value++;
        }

        if (GUILayout.Button("Delete current wave"))
        {
            wavesList.RemoveAt(_indexWave);
            _indexWave.Value--;
        }

        EditorGUILayout.EndHorizontal();

    }

    private void DrawServices()
    {
        _cellSize.Value = EditorGUILayout.Vector2Field("Row Size", _cellSize);

        _countColumns.Value = EditorGUILayout.IntField("Count columns", _countColumns);
    }

    private void DrawSubWaveInfo()
    {
        if (_level.waves[_indexWave].subWaves.IsEmpty())
        {
            return;
        }

        SerializedProperty subWave = CurrentSubWaves.GetArrayElementAtIndex(_indexOfSubWave);

        if (subWave == null)
        {
            return;
        }

        GUILayout.Label("**SubWave info**");

        var enemyInfo = _level.waves[_indexWave].subWaves[_indexOfSubWave].enemyInfo;

        EditorGUILayout.PropertyField(subWave);

        while (subWave.NextVisible(true))
        {
            subWave.isExpanded = true;
        }

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();

        DrawKeyboard();

        EditorGUILayout.EndHorizontal();

        GUILayout.Label("*Please save for update errors");

        EnemyInfoHolderEditor.OnInspectorGUI(enemyInfo);
    }

    public void DrawKeyboard()
    {
        var list = new SerializedList<SubWave, Level>(
            CurrentSubWaves,
            (l, i) => l.waves[_indexWave].subWaves[i],
            (s, l, i) => l.waves[_indexWave].subWaves[i] = s
        );

        if (GUILayout.Button("+||"))
        {
            list.Insert(0);
            _indexOfSubWave.Value = 0;
        }

        if (GUILayout.Button("||+"))
        {
            list.Add();
            _indexOfSubWave.Value = CurrentWave.arraySize - 1;
        }

        if (GUILayout.Button("+@"))
        {
            list.Insert(_indexOfSubWave);
        }

        if (GUILayout.Button("@+"))
        {
            list.Insert(_indexOfSubWave + 1);
            _indexOfSubWave.Value++;
        }

        if (GUILayout.Button("DEL"))
        {
            list.RemoveAt(_indexOfSubWave);
        }

        if (GUILayout.Button("<"))
        {
            list.Switch(_indexOfSubWave - 1, _indexOfSubWave);
            _indexOfSubWave.Value--;
        }

        if (GUILayout.Button(">"))
        {
            list.Switch(_indexOfSubWave, _indexOfSubWave + 1);
            _indexOfSubWave.Value++;
        }
    }

    int lastCount = 0;

    private void DrawLeft()
    {
        if (Event.current.type == EventType.Layout)
        {
            lastCount = _countRows;
        }

        EditorGUILayout.Space(_space);
        for (int i = 1; i <= lastCount; i++)
        {
            GUILayout.Label((i * _countColumns).ToString(), GUILayout.Height(_cellSize.Value.y));
        }

    }

    private void DrawTitle()
    {
        if (GUILayout.Button("Save"))
        {
            _levelSerialized.ApplyModifiedProperties();
        }
    }

    private void DrawWave()
    {
        if (_level.waves.Length == 0)
            return;

        var subWaves = CurrentSubWaves;

        DrawHorizontalGrid();
        DrawSelectedSubWave();
        _countRows = 1;

        EditorGUILayout.BeginHorizontal();

        for (int j = 0, size = 0; j < subWaves.arraySize; j++)
        {
            var subWave = subWaves.GetArrayElementAtIndex(j);

            var count = subWave.FindPropertyRelative(nameof(SubWave.count));

            var countValue = count.intValue;

            size += countValue;

            while (size > _countColumns)
            {
                _countRows++;
                var diff = size - _countColumns;
                var mod = countValue - diff;

                GUILayout.Box($"- {mod} >>", new GUIStyle(GUI.skin.box)
                {
                    margin = new RectOffset(3, 3, 2, 0)
                }, GUILayout.Height(20f), GUILayout.Width((mod + 2) * _cellSize.Value.x));
                countValue = diff;
                size -= _countColumns;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }


            var enemyType = (EnemyType)
                            (subWave
                             ?.FindPropertyRelative(nameof(SubWave.enemyInfo))
                             ?.FindPropertyRelative(nameof(EnemyInfo.enemyType))
                             ?.enumValueFlag ?? 0);

            if ((enemyType & EnemyType.Ground) != 0)
            {
                GUI.color = Green;
            }
            else if ((enemyType & EnemyType.Fly) != 0)
            {
                GUI.color = Blue;
            }
            else
            {
                GUI.color = Color.white;
            }


            var gameObject = (GameObject)subWave
                             .FindPropertyRelative(nameof(SubWave.monster))
                             .objectReferenceValue;


            var text = $"{count.intValue}{(((enemyType & EnemyType.Boss) != 0) ? "B" : "")} - {gameObject?.name}";
            var textSize = GUI.skin.button.CalcSize(new GUIContent(text));
            var realWidth = countValue * _cellSize.Value.x - _marginRight;

            var leftTextStyle = new GUIStyle(GUI.skin.button);
            leftTextStyle.alignment = TextAnchor.MiddleLeft;

            var styleOfButton = textSize.x >= realWidth ? leftTextStyle : GUI.skin.button;

            if (GUILayout.Button(text, styleOfButton, GUILayout.Width(realWidth)))
            {
                _indexOfSubWave.Value = j;
            }

            if (size == _countColumns)
            {
                _countRows++;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                size = 0;
            }
        }

        GUI.color = Color.white;

        if (GUILayout.Button("+", GUILayout.Width(_cellSize.Value.x - _marginRight)))
        {
            var list = new SerializedList<SubWave, Level>(
                CurrentSubWaves,
                (l, i) => l.waves[_indexWave].subWaves[i],
                (sw, l, i) => l.waves[_indexWave].subWaves[i] = sw
            );

            list.Add(new SubWave());
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSelectedSubWave()
    {
        if (_indexOfSubWave >= CurrentSubWaves.arraySize)
            return;

        var sum = Enumerable
                  .Range(0, _indexOfSubWave)
                  .Sum(i => CurrentSubWaves
                       .GetArrayElementAtIndex(i)
                       .FindPropertyRelative(nameof(SubWave.count))
                       .intValue
                      );

        var count = CurrentSubWaves
                    .GetArrayElementAtIndex(_indexOfSubWave)
                    .FindPropertyRelative(nameof(SubWave.count))
                    .intValue;

        var div = (sum + count) / _countColumns + 1;
        var mod = (sum + count) % _countColumns + 1;

        var rect = new Rect(mod * _cellSize.Value.x, div * 22f + 14f, _cellSize.Value.x, 1);

        var texture = EditorGUIUtility.whiteTexture;
        GUI.color = Color.white;

        GUI.DrawTexture(rect, texture);
    }

    private void DrawHorizontalGrid()
    {
        GUI.color = Color.white;
        var whiteTexture = EditorGUIUtility.whiteTexture;

        GUILayout.Space(_space);

        var lastRect = GUILayoutUtility.GetLastRect();
        var line = new Rect(lastRect.x + _marginRight, 0, 1, 15);

        var textRect = line;

        textRect.width = _cellSize.Value.x;


        for (int i = 0; i <= _countColumns; i++)
        {
            GUI.DrawTexture(line, whiteTexture);
            line.x += _cellSize.Value.x;
        }

        var longLine = line;
        longLine.height = 20 * _countRows;
        longLine.x -= _cellSize.Value.x;
        GUI.DrawTexture(longLine, whiteTexture);

        for (int i = 1; i <= _countColumns; i++)
        {
            GUI.Label(textRect, i.ToString());
            textRect.x += _cellSize.Value.x;
        }

        textRect.width = 40;
        GUI.Label(textRect, "Next");
    }

    private SerializedProperty CurrentWave =>
    _levelSerialized.FindProperty(nameof(Level.waves))
    .GetArrayElementAtIndex(_indexWave);

    private SerializedProperty CurrentSubWaves =>
    CurrentWave.FindPropertyRelative(nameof(Wave.subWaves));
}

#endif