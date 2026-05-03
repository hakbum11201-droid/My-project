using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private SerializedProperty playerTarget;
    private SerializedProperty waveManager;
    private SerializedProperty enemySpawnEntries;
    private SerializedProperty spawnInterval;
    private SerializedProperty spawnRadius;
    private SerializedProperty maxEnemies;

    private ReorderableList enemyList;

    private void OnEnable()
    {
        playerTarget = serializedObject.FindProperty("playerTarget");
        waveManager = serializedObject.FindProperty("waveManager");
        enemySpawnEntries = serializedObject.FindProperty("enemySpawnEntries");
        spawnInterval = serializedObject.FindProperty("spawnInterval");
        spawnRadius = serializedObject.FindProperty("spawnRadius");
        maxEnemies = serializedObject.FindProperty("maxEnemies");

        enemyList = new ReorderableList(serializedObject, enemySpawnEntries, true, true, true, true);

        enemyList.drawHeaderCallback = DrawHeader;
        enemyList.drawElementCallback = DrawElement;
        enemyList.elementHeight = EditorGUIUtility.singleLineHeight + 8f;

        enemyList.onAddCallback = AddEnemyEntry;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawReferences();
        EditorGUILayout.Space(8f);

        DrawEnemySpawnTable();
        EditorGUILayout.Space(8f);

        DrawSpawnSettings();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawReferences()
    {
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(playerTarget);
        EditorGUILayout.PropertyField(waveManager);
    }

    private void DrawEnemySpawnTable()
    {
        EditorGUILayout.LabelField("Enemy Spawn Table", EditorStyles.boldLabel);

        enemyList.DoLayoutList();

        float totalWeight = GetTotalWeight();

        EditorGUILayout.HelpBox(
            $"Total Weight: {totalWeight:0.##}\n" +
            "Weight 값이 높을수록 더 자주 스폰됩니다. 빨간 박스 Enemy는 이 목록에서 빼면 더 이상 나오지 않습니다.",
            MessageType.Info
        );

        if (totalWeight <= 0f)
        {
            EditorGUILayout.HelpBox(
                "사용 가능한 몬스터가 없습니다. Prefab이 들어가 있고 Weight가 0보다 큰 항목이 최소 1개 필요합니다.",
                MessageType.Warning
            );
        }
    }

    private void DrawSpawnSettings()
    {
        EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(spawnInterval);
        EditorGUILayout.PropertyField(spawnRadius);
        EditorGUILayout.PropertyField(maxEnemies);
    }

    private void DrawHeader(Rect rect)
    {
        float x = rect.x;

        EditorGUI.LabelField(new Rect(x, rect.y, 45f, rect.height), "Use");
        x += 45f;

        EditorGUI.LabelField(new Rect(x, rect.y, 120f, rect.height), "Name");
        x += 120f;

        EditorGUI.LabelField(new Rect(x, rect.y, rect.width - 300f, rect.height), "Prefab");

        EditorGUI.LabelField(new Rect(rect.xMax - 130f, rect.y, 70f, rect.height), "Weight");
        EditorGUI.LabelField(new Rect(rect.xMax - 55f, rect.y, 55f, rect.height), "Rate");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(index);

        SerializedProperty enabled = element.FindPropertyRelative("enabled");
        SerializedProperty displayName = element.FindPropertyRelative("displayName");
        SerializedProperty enemyPrefab = element.FindPropertyRelative("enemyPrefab");
        SerializedProperty spawnWeight = element.FindPropertyRelative("spawnWeight");

        rect.y += 4f;
        rect.height = EditorGUIUtility.singleLineHeight;

        float x = rect.x;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, 35f, rect.height),
            enabled,
            GUIContent.none
        );
        x += 45f;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, 110f, rect.height),
            displayName,
            GUIContent.none
        );
        x += 120f;

        EditorGUI.PropertyField(
            new Rect(x, rect.y, rect.width - 310f, rect.height),
            enemyPrefab,
            GUIContent.none
        );

        EditorGUI.PropertyField(
            new Rect(rect.xMax - 130f, rect.y, 65f, rect.height),
            spawnWeight,
            GUIContent.none
        );

        float totalWeight = GetTotalWeight();
        float rate = 0f;

        if (enabled.boolValue && enemyPrefab.objectReferenceValue != null && spawnWeight.floatValue > 0f && totalWeight > 0f)
        {
            rate = spawnWeight.floatValue / totalWeight * 100f;
        }

        EditorGUI.LabelField(
            new Rect(rect.xMax - 55f, rect.y, 55f, rect.height),
            $"{rate:0.#}%"
        );
    }

    private void AddEnemyEntry(ReorderableList list)
    {
        enemySpawnEntries.arraySize++;

        SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(enemySpawnEntries.arraySize - 1);

        element.FindPropertyRelative("enabled").boolValue = true;
        element.FindPropertyRelative("displayName").stringValue = "New Enemy";
        element.FindPropertyRelative("enemyPrefab").objectReferenceValue = null;
        element.FindPropertyRelative("spawnWeight").floatValue = 1f;
    }

    private float GetTotalWeight()
    {
        float total = 0f;

        for (int i = 0; i < enemySpawnEntries.arraySize; i++)
        {
            SerializedProperty element = enemySpawnEntries.GetArrayElementAtIndex(i);

            bool enabled = element.FindPropertyRelative("enabled").boolValue;
            Object prefab = element.FindPropertyRelative("enemyPrefab").objectReferenceValue;
            float weight = element.FindPropertyRelative("spawnWeight").floatValue;

            if (!enabled)
                continue;

            if (prefab == null)
                continue;

            if (weight <= 0f)
                continue;

            total += weight;
        }

        return total;
    }
}