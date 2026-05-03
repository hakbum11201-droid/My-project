using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DevSnapshotTool
{
    private const string ProjectRoot = "Assets/_Project";
    private const string DocsFolder = "Assets/_Project/Docs";
    private const string SnapshotPath = "Assets/_Project/Docs/DevSnapshot_Expert.md";
    private const string GptContextPath = "Assets/_Project/Docs/GPT_CONTEXT.md";
    private const string DevStatePath = "Assets/_Project/Docs/DevState.md";
    private const string CachePath = "Library/DevSnapshotCache.json";

    private const int MaxDiffLines = 100;
    private const int MaxSerializedPropertiesPerComponent = 140;
    private const int MaxIssueCountPerSection = 80;

    private static readonly string[] TargetExtensions =
    {
        ".cs",
        ".unity",
        ".prefab",
        ".asset",
        ".controller",
        ".anim",
        ".png",
        ".jpg",
        ".jpeg",
        ".webp"
    };

    private static readonly string[] GeneratedOutputPaths =
    {
        NormalizePathStatic(SnapshotPath),
        NormalizePathStatic(GptContextPath)
    };

    private static readonly string[] ImportantObjectNameKeywords =
    {
        "Player",
        "EnemySpawner",
        "WaveManager",
        "GameTimer",
        "GameFlowManager",
        "HUDCanvas",
        "HUDPanel",
        "LevelUpCanvas",
        "LevelUpUI",
        "RelicSelectUI",
        "RelicSelectPanel",
        "StartMenuCanvas",
        "GameOverCanvas",
        "StageTileGenerator",
        "GroundGrid",
        "GroundTilemap",
        "EventSystem"
    };

    private static readonly string[] DebugNameKeywords =
    {
        "Debug",
        "DebugUI",
        "Test",
        "Temp",
        "Sample"
    };

    private static readonly string[] OptionalReferenceKeywords =
    {
        "icon",
        "sprite",
        "sourceImage",
        "material",
        "font",
        "audio",
        "sfx",
        "bgm",
        "tint",
        "preview"
    };

    [Serializable]
    private class SnapshotCache
    {
        public List<FileSnapshot> files = new List<FileSnapshot>();
    }

    [Serializable]
    private class FileSnapshot
    {
        public string path;
        public string hash;
        public string text;
        public long size;
    }

    private class DiagnosticIssue
    {
        public string severity;
        public string title;
        public string detail;
        public string suggestion;

        public DiagnosticIssue(string severity, string title, string detail, string suggestion)
        {
            this.severity = severity;
            this.title = title;
            this.detail = detail;
            this.suggestion = suggestion;
        }
    }

    private class SceneObjectInfo
    {
        public GameObject gameObject;
        public string path;
        public string name;
        public bool activeSelf;
        public string tag;
        public int layer;
        public List<string> componentNames = new List<string>();
        public List<Component> components = new List<Component>();
    }

    private class SerializedFieldLine
    {
        public string path;
        public string value;
        public int depth;
        public SerializedPropertyType type;
        public bool isNullObjectReference;
        public bool isOptionalReference;
    }

    [MenuItem("_Project/Dev Tools/Generate Dev Snapshot")]
    public static void GenerateDevSnapshot()
    {
        EnsureDocsFolder();

        SnapshotCache previousCache = LoadCache();
        Dictionary<string, FileSnapshot> previousMap = previousCache.files.ToDictionary(f => f.path, f => f);

        List<FileSnapshot> currentFiles = ScanProjectFiles();
        Dictionary<string, FileSnapshot> currentMap = currentFiles.ToDictionary(f => f.path, f => f);

        List<FileSnapshot> addedFiles = new List<FileSnapshot>();
        List<FileSnapshot> modifiedFiles = new List<FileSnapshot>();
        List<FileSnapshot> deletedFiles = new List<FileSnapshot>();

        foreach (FileSnapshot current in currentFiles)
        {
            if (!previousMap.TryGetValue(current.path, out FileSnapshot previous))
            {
                addedFiles.Add(current);
                continue;
            }

            if (previous.hash != current.hash)
            {
                modifiedFiles.Add(current);
            }
        }

        foreach (FileSnapshot previous in previousCache.files)
        {
            if (!currentMap.ContainsKey(previous.path))
            {
                deletedFiles.Add(previous);
            }
        }

        string report = BuildReport(
            previousMap,
            currentFiles,
            addedFiles,
            modifiedFiles,
            deletedFiles
        );

        File.WriteAllText(SnapshotPath, report, Encoding.UTF8);
        File.WriteAllText(GptContextPath, report, Encoding.UTF8);

        SaveCache(currentFiles);

        EditorGUIUtility.systemCopyBuffer = report;
        AssetDatabase.Refresh();

        Debug.Log($"Expert Dev Snapshot 생성 완료: {SnapshotPath}");
        Debug.Log($"GPT Context 생성 완료: {GptContextPath}");
        Debug.Log("리포트 전체가 클립보드에 복사되었습니다.");
    }

    [MenuItem("_Project/Dev Tools/Reset Dev Snapshot Baseline")]
    public static void ResetBaseline()
    {
        List<FileSnapshot> currentFiles = ScanProjectFiles();
        SaveCache(currentFiles);

        Debug.Log("Dev Snapshot 기준점이 현재 상태로 초기화되었습니다. 다음 Generate부터 변경분 기준으로 표시됩니다.");
    }

    private static string BuildReport(
        Dictionary<string, FileSnapshot> previousMap,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles)
    {
        List<SceneObjectInfo> sceneObjects = CollectSceneObjectInfos();
        List<DiagnosticIssue> issues = BuildDiagnostics(currentFiles, sceneObjects);

        StringBuilder sb = new StringBuilder();

        AppendHeader(sb);
        AppendUsageProtocol(sb);
        AppendHardRules(sb);
        AppendExecutiveSummary(sb, currentFiles, addedFiles, modifiedFiles, deletedFiles, sceneObjects, issues);
        AppendDevState(sb);
        AppendDiagnostics(sb, issues);
        AppendRecommendedNextSteps(sb, currentFiles, sceneObjects, issues);
        AppendArchitectureMap(sb, currentFiles, sceneObjects);
        AppendSceneHierarchy(sb, sceneObjects);
        AppendImportantSceneObjects(sb, sceneObjects);
        AppendUiStructure(sb, sceneObjects);
        AppendScriptableObjects(sb);
        AppendPrefabSummary(sb);
        AppendFileChangeSummary(sb, currentFiles, addedFiles, modifiedFiles, deletedFiles);
        AppendCodeChangePreview(sb, previousMap, addedFiles, modifiedFiles);
        AppendScriptsTree(sb, currentFiles);
        AppendCurrentScriptStructure(sb, currentFiles);
        AppendFullSourceCode(sb, currentFiles);

        return sb.ToString();
    }

    private static void AppendHeader(StringBuilder sb)
    {
        Scene activeScene = SceneManager.GetActiveScene();

        sb.AppendLine("# EXPERT GPT / DEV SNAPSHOT");
        sb.AppendLine();
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Unity Version: {Application.unityVersion}");
        sb.AppendLine($"Active Scene: {activeScene.name}");
        sb.AppendLine($"Scene Path: {activeScene.path}");
        sb.AppendLine($"Project Root: {ProjectRoot}");
        sb.AppendLine();
    }

    private static void AppendUsageProtocol(StringBuilder sb)
    {
        sb.AppendLine("## HOW GPT MUST USE THIS SNAPSHOT");
        sb.AppendLine();
        sb.AppendLine("1. 먼저 `Executive Summary`, `Diagnostics`, `Scene Hierarchy`, `UI Structure`, `Important Scene Objects`를 읽는다.");
        sb.AppendLine("2. 새 스크립트나 새 GameObject를 제안하기 전에 기존 구조로 해결 가능한지 판단한다.");
        sb.AppendLine("3. 코드 작성 전 반드시 수정 대상 파일과 이유를 먼저 말한다.");
        sb.AppendLine("4. 사용자가 현재 구조를 묻는 경우 이 Snapshot 내용을 기준으로 답한다.");
        sb.AppendLine("5. 이 Snapshot에 없는 오브젝트/스크립트/연결을 있다고 가정하지 않는다.");
        sb.AppendLine();
    }

    private static void AppendHardRules(StringBuilder sb)
    {
        sb.AppendLine("## HARD RULES");
        sb.AppendLine();
        sb.AppendLine("- 기존 HUD 구조 확인 전 새 HUD Canvas/Panel/Script 생성 금지.");
        sb.AppendLine("- DebugUI와 정식 UI 혼동 금지.");
        sb.AppendLine("- 새 기능 추가 전 기존 Manager/UI/Data 구조 재사용 우선.");
        sb.AppendLine("- Inspector 연결이 필요한 경우 연결 대상 필드명과 오브젝트명을 반드시 명시.");
        sb.AppendLine("- 전체 코드 제공 시 파일명/경로/전체 교체 여부를 반드시 명시.");
        sb.AppendLine("- 오류가 있으면 추측하지 말고 Console 로그/파일/Hierarchy 기준으로 판단.");
        sb.AppendLine("- 한 번에 여러 시스템을 동시에 수정하지 말 것.");
        sb.AppendLine("- 새 파일 추가는 반드시 `왜 기존 파일 수정으로 안 되는지` 설명한 뒤 제안.");
        sb.AppendLine();
    }

    private static void AppendExecutiveSummary(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        int scriptsCount = currentFiles.Count(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));
        int prefabsCount = currentFiles.Count(f => f.path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase));
        int assetsCount = currentFiles.Count(f => f.path.EndsWith(".asset", StringComparison.OrdinalIgnoreCase));

        int highCount = issues.Count(i => i.severity == "HIGH");
        int mediumCount = issues.Count(i => i.severity == "MEDIUM");
        int lowCount = issues.Count(i => i.severity == "LOW");

        sb.AppendLine("## Executive Summary");
        sb.AppendLine();
        sb.AppendLine($"- Scene Objects: {sceneObjects.Count}");
        sb.AppendLine($"- C# Scripts: {scriptsCount}");
        sb.AppendLine($"- Prefabs: {prefabsCount}");
        sb.AppendLine($"- Asset Files: {assetsCount}");
        sb.AppendLine($"- Changed Files: Added {addedFiles.Count}, Modified {modifiedFiles.Count}, Deleted {deletedFiles.Count}");
        sb.AppendLine($"- Diagnostics: HIGH {highCount}, MEDIUM {mediumCount}, LOW {lowCount}");
        sb.AppendLine();

        sb.AppendLine("### Key Existing Systems Detected");
        sb.AppendLine();

        AppendDetectedSystem(sb, currentFiles, sceneObjects, "HUD", "HUDCanvasUI.cs", "HUDCanvas");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Level Up", "LevelUpUI.cs", "LevelUpCanvas");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Relic Select", "RelicSelectUI.cs", "RelicSelectUI");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Relic Effects", "PlayerRelicEffects.cs", "Player");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Spawner", "EnemySpawner.cs", "EnemySpawner");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Wave", "WaveManager.cs", "WaveManager");
        AppendDetectedSystem(sb, currentFiles, sceneObjects, "Timer", "GameTimer.cs", "GameTimer");

        sb.AppendLine();
    }

    private static void AppendDetectedSystem(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        string label,
        string scriptFileName,
        string sceneObjectName)
    {
        bool hasScript = currentFiles.Any(f => Path.GetFileName(f.path).Equals(scriptFileName, StringComparison.OrdinalIgnoreCase));
        bool hasObject = sceneObjects.Any(o => o.name.Equals(sceneObjectName, StringComparison.OrdinalIgnoreCase));

        string status = hasScript && hasObject ? "OK" : hasScript ? "Script Only" : hasObject ? "Object Only" : "Missing";

        sb.AppendLine($"- {label}: {status} (Script: {scriptFileName}={hasScript}, Object: {sceneObjectName}={hasObject})");
    }

    private static void AppendDevState(StringBuilder sb)
    {
        sb.AppendLine("## DevState.md");
        sb.AppendLine();

        if (!File.Exists(DevStatePath))
        {
            sb.AppendLine("- DevState.md 없음");
            sb.AppendLine();
            return;
        }

        string text = File.ReadAllText(DevStatePath, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(text))
        {
            sb.AppendLine("- DevState.md 비어 있음");
            sb.AppendLine();
            return;
        }

        sb.AppendLine("```markdown");
        sb.AppendLine(text.Trim());
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendDiagnostics(StringBuilder sb, List<DiagnosticIssue> issues)
    {
        sb.AppendLine("## Diagnostics");
        sb.AppendLine();

        if (issues.Count == 0)
        {
            sb.AppendLine("- 감지된 주요 위험 없음");
            sb.AppendLine();
            return;
        }

        AppendDiagnosticsBySeverity(sb, issues, "HIGH");
        AppendDiagnosticsBySeverity(sb, issues, "MEDIUM");
        AppendDiagnosticsBySeverity(sb, issues, "LOW");
    }

    private static void AppendDiagnosticsBySeverity(StringBuilder sb, List<DiagnosticIssue> issues, string severity)
    {
        List<DiagnosticIssue> filtered = issues.Where(i => i.severity == severity).Take(MaxIssueCountPerSection).ToList();

        if (filtered.Count == 0)
            return;

        sb.AppendLine($"### {severity}");
        sb.AppendLine();

        for (int i = 0; i < filtered.Count; i++)
        {
            DiagnosticIssue issue = filtered[i];

            sb.AppendLine($"{i + 1}. **{issue.title}**");
            sb.AppendLine($"   - Detail: {issue.detail}");
            sb.AppendLine($"   - Suggestion: {issue.suggestion}");
        }

        sb.AppendLine();

        int total = issues.Count(i => i.severity == severity);

        if (total > filtered.Count)
        {
            sb.AppendLine($"- ... {severity} issue {total - filtered.Count}개 추가 생략");
            sb.AppendLine();
        }
    }

    private static void AppendRecommendedNextSteps(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        sb.AppendLine("## Recommended Next Steps");
        sb.AppendLine();

        List<string> recommendations = BuildRecommendations(currentFiles, sceneObjects, issues);

        if (recommendations.Count == 0)
        {
            sb.AppendLine("1. 현재 구조 기준으로 명확한 우선 위험 없음. 다음 기능은 DevState.md 기준으로 진행.");
            sb.AppendLine();
            return;
        }

        for (int i = 0; i < recommendations.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {recommendations[i]}");
        }

        sb.AppendLine();
    }

    private static List<string> BuildRecommendations(
        List<FileSnapshot> currentFiles,
        List<SceneObjectInfo> sceneObjects,
        List<DiagnosticIssue> issues)
    {
        List<string> result = new List<string>();

        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasHudCanvasUi = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicSelectUi = currentFiles.Any(f => f.path.EndsWith("RelicSelectUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicDebugUi = currentFiles.Any(f => f.path.EndsWith("RelicSelectDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasSmallHeal = currentFiles.Any(f => f.path.EndsWith("SmallHealPickup.cs", StringComparison.OrdinalIgnoreCase));
        bool hasEnemyHealth = currentFiles.Any(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));
        bool hasMeleeWeapon = currentFiles.Any(f => f.path.EndsWith("PlayerMeleeAutoAttack.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudCanvas && hasHudPanel && hasHudCanvasUi && hasRelicSelectUi)
        {
            result.Add("유물 보유 표시를 추가할 때는 새 HUD Canvas부터 만들지 말고, 기존 HUDCanvas/HUDPanel/HUDCanvasUI 구조를 먼저 검토한 뒤 통합 여부를 결정한다.");
        }

        if (hasSmallHeal && hasEnemyHealth)
        {
            FileSnapshot enemyHealth = currentFiles.FirstOrDefault(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));

            if (enemyHealth != null &&
                enemyHealth.text.Contains("DropExpGem") &&
                enemyHealth.text.Contains("DropSmallHeal") &&
                CountOccurrences(enemyHealth.text, "transform.position") >= 2)
            {
                result.Add("SmallHeal과 ExpGem이 같은 위치에 드랍될 가능성이 높다. 다음 작업은 EnemyHealth 드랍 위치 오프셋 분리로 잡는 것이 안전하다.");
            }
        }

        if (hasRelicDebugUi && hasRelicSelectUi)
        {
            result.Add("RelicSelectDebugUI와 RelicSelectUI가 공존한다. 신규 유물 UI 작업은 RelicSelectUI만 사용하고, DebugUI는 보류/삭제 후보로 분리한다.");
        }

        if (hasMeleeWeapon)
        {
            int weaponScriptCount = currentFiles.Count(f =>
                f.path.Contains("/Weapons/", StringComparison.OrdinalIgnoreCase) &&
                f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));

            if (weaponScriptCount <= 1)
            {
                result.Add("무기 스크립트가 근접 자동공격 1개 중심이다. 전투 단조로움 해결은 Brute 추가보다 MagicBolt 같은 두 번째 무기 구조를 곧 검토해야 한다.");
            }
        }

        if (issues.Any(i => i.severity == "HIGH"))
        {
            result.Insert(0, "HIGH 진단 항목이 있으므로 새 기능 추가 전에 연결 누락/중복/누락 스크립트부터 정리한다.");
        }

        if (result.Count == 0)
        {
            result.Add("다음 작업은 DevState.md의 우선순위를 따른다. 단, 새 구조 추가 전 기존 시스템 재사용 가능성을 먼저 판단한다.");
        }

        return result.Distinct().Take(10).ToList();
    }

    private static void AppendArchitectureMap(StringBuilder sb, List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Existing Systems To Reuse");
        sb.AppendLine();

        sb.AppendLine("### UI");
        sb.AppendLine("- HUD 작업: HUDCanvas, HUDPanel, HUDCanvasUI 우선 확인.");
        sb.AppendLine("- 레벨업 작업: LevelUpCanvas, LevelUpUI, UpgradeData 우선 활용.");
        sb.AppendLine("- 유물 선택 작업: RelicSelectUI, RelicData, PlayerRelicEffects 우선 활용.");
        sb.AppendLine("- DebugUI 파일은 신규 기능의 기준으로 삼지 말 것.");
        sb.AppendLine();

        sb.AppendLine("### Gameplay");
        sb.AppendLine("- 플레이어 체력/방어/회복: PlayerHealth.");
        sb.AppendLine("- 플레이어 이동/넉백: PlayerController.");
        sb.AppendLine("- 근접 자동공격/치명타: PlayerMeleeAutoAttack.");
        sb.AppendLine("- 경험치/레벨업: PlayerExp + LevelUpUI.");
        sb.AppendLine("- 적 체력/드랍/중간보스 사망 처리: EnemyHealth.");
        sb.AppendLine("- 적 스폰/중간보스 스폰: EnemySpawner + WaveManager.");
        sb.AppendLine();

        sb.AppendLine("### Data");
        sb.AppendLine("- 강화 데이터: UpgradeData ScriptableObject.");
        sb.AppendLine("- 유물 데이터: RelicData ScriptableObject.");
        sb.AppendLine();
    }

    private static void AppendSceneHierarchy(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Current Scene Hierarchy");
        sb.AppendLine();

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        sb.AppendLine("```text");

        foreach (GameObject root in roots)
        {
            AppendGameObjectTree(sb, root.transform, 0);
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendGameObjectTree(StringBuilder sb, Transform transform, int depth)
    {
        if (transform == null)
            return;

        string indent = new string(' ', depth * 2);
        GameObject go = transform.gameObject;
        string activeText = go.activeSelf ? "" : " (inactive)";
        string components = string.Join(", ", go.GetComponents<Component>()
            .Where(c => c != null)
            .Select(c => c.GetType().Name));

        sb.AppendLine($"{indent}- {go.name}{activeText} [{components}]");

        Component[] comps = go.GetComponents<Component>();

        foreach (Component component in comps)
        {
            if (component == null)
            {
                sb.AppendLine($"{indent}  ! Missing Script");
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            AppendGameObjectTree(sb, transform.GetChild(i), depth + 1);
        }
    }

    private static void AppendImportantSceneObjects(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## Important Scene Objects / Inspector Snapshot");
        sb.AppendLine();

        foreach (SceneObjectInfo info in sceneObjects)
        {
            bool importantByName = ImportantObjectNameKeywords.Any(k =>
                info.name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                info.path.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

            bool importantByComponent = info.componentNames.Any(c =>
                c.EndsWith("Manager", StringComparison.OrdinalIgnoreCase) ||
                c.EndsWith("UI", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Spawner", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Health", StringComparison.OrdinalIgnoreCase) ||
                c.Contains("Controller", StringComparison.OrdinalIgnoreCase));

            if (!importantByName && !importantByComponent)
                continue;

            sb.AppendLine($"### {info.path}");
            sb.AppendLine($"- Active: {info.activeSelf}");
            sb.AppendLine($"- Tag: {info.tag}");
            sb.AppendLine($"- Layer: {LayerMask.LayerToName(info.layer)}");
            sb.AppendLine($"- Components: {string.Join(", ", info.componentNames)}");
            sb.AppendLine();

            foreach (Component component in info.components)
            {
                if (component == null)
                    continue;

                Type type = component.GetType();

                if (type == typeof(Transform) || type == typeof(RectTransform))
                {
                    AppendTransformInfo(sb, component);
                    continue;
                }

                if (ShouldSkipVerboseBuiltInComponent(type))
                    continue;

                sb.AppendLine($"#### {type.Name}");
                AppendSerializedObjectFields(sb, component);
                sb.AppendLine();
            }
        }
    }

    private static void AppendUiStructure(StringBuilder sb, List<SceneObjectInfo> sceneObjects)
    {
        sb.AppendLine("## UI Structure Analysis");
        sb.AppendLine();

        List<SceneObjectInfo> uiObjects = sceneObjects
            .Where(o =>
                o.componentNames.Contains("Canvas") ||
                o.componentNames.Contains("Button") ||
                o.componentNames.Contains("TextMeshProUGUI") ||
                o.componentNames.Contains("Slider") ||
                o.componentNames.Contains("Image") ||
                o.name.Contains("Canvas") ||
                o.name.Contains("Panel") ||
                o.name.Contains("Text") ||
                o.name.Contains("Button"))
            .OrderBy(o => o.path)
            .ToList();

        if (uiObjects.Count == 0)
        {
            sb.AppendLine("- UI 오브젝트 없음");
            sb.AppendLine();
            return;
        }

        sb.AppendLine("```text");

        foreach (SceneObjectInfo info in uiObjects)
        {
            sb.AppendLine($"- {info.path} [{string.Join(", ", info.componentNames)}]");
        }

        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("### UI Warnings");
        sb.AppendLine();

        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasTimerText = sceneObjects.Any(o => o.name == "TimerText");
        bool hasRelicSelectPanel = sceneObjects.Any(o => o.name == "RelicSelectPanel");

        sb.AppendLine($"- HUDCanvas exists: {hasHudCanvas}");
        sb.AppendLine($"- HUDPanel exists: {hasHudPanel}");
        sb.AppendLine($"- TimerText exists: {hasTimerText}");
        sb.AppendLine($"- RelicSelectPanel exists: {hasRelicSelectPanel}");
        sb.AppendLine();

        if (hasHudCanvas && hasHudPanel)
        {
            sb.AppendLine("- HUD 관련 신규 작업은 기존 HUDCanvas/HUDPanel 구조를 우선 검토해야 함.");
        }

        if (hasRelicSelectPanel)
        {
            sb.AppendLine("- 유물 선택 UI는 Canvas 기반 구조가 이미 존재함. OnGUI DebugUI 기준으로 작업하지 말 것.");
        }

        sb.AppendLine();
    }

    private static void AppendScriptableObjects(StringBuilder sb)
    {
        sb.AppendLine("## ScriptableObjects Snapshot");
        sb.AppendLine();

        string folder = ProjectRoot + "/ScriptableObjects";

        if (!Directory.Exists(folder))
        {
            sb.AppendLine("- ScriptableObjects 폴더 없음");
            sb.AppendLine();
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folder });

        if (guids.Length == 0)
        {
            sb.AppendLine("- ScriptableObject 없음");
            sb.AppendLine();
            return;
        }

        foreach (string guid in guids.OrderBy(g => AssetDatabase.GUIDToAssetPath(g)))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset == null)
                continue;

            sb.AppendLine($"### {path}");
            sb.AppendLine($"- Type: {asset.GetType().Name}");
            AppendSerializedObjectFields(sb, asset);
            sb.AppendLine();
        }
    }

    private static void AppendPrefabSummary(StringBuilder sb)
    {
        sb.AppendLine("## Prefabs Snapshot");
        sb.AppendLine();

        string prefabFolder = ProjectRoot + "/Prefabs";

        if (!Directory.Exists(prefabFolder))
        {
            sb.AppendLine("- Prefabs 폴더 없음");
            sb.AppendLine();
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });

        if (guids.Length == 0)
        {
            sb.AppendLine("- Prefab 없음");
            sb.AppendLine();
            return;
        }

        foreach (string guid in guids.OrderBy(g => AssetDatabase.GUIDToAssetPath(g)))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            sb.AppendLine($"### {path}");
            sb.AppendLine("```text");
            AppendPrefabTree(sb, prefab.transform, 0);
            sb.AppendLine("```");
            sb.AppendLine();
        }
    }

    private static void AppendPrefabTree(StringBuilder sb, Transform transform, int depth)
    {
        if (transform == null)
            return;

        string indent = new string(' ', depth * 2);
        string components = string.Join(", ", transform.gameObject.GetComponents<Component>()
            .Where(c => c != null)
            .Select(c => c.GetType().Name));

        sb.AppendLine($"{indent}- {transform.gameObject.name} [{components}]");

        Component[] comps = transform.gameObject.GetComponents<Component>();

        foreach (Component comp in comps)
        {
            if (comp == null)
            {
                sb.AppendLine($"{indent}  ! Missing Script");
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            AppendPrefabTree(sb, transform.GetChild(i), depth + 1);
        }
    }

    private static void AppendFileChangeSummary(
        StringBuilder sb,
        List<FileSnapshot> currentFiles,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles,
        List<FileSnapshot> deletedFiles)
    {
        sb.AppendLine("## File Change Summary");
        sb.AppendLine();
        sb.AppendLine($"- Added: {addedFiles.Count}");
        sb.AppendLine($"- Modified: {modifiedFiles.Count}");
        sb.AppendLine($"- Deleted: {deletedFiles.Count}");
        sb.AppendLine($"- Total tracked files: {currentFiles.Count}");
        sb.AppendLine();

        if (addedFiles.Count == 0 && modifiedFiles.Count == 0 && deletedFiles.Count == 0)
        {
            sb.AppendLine("- 변경된 파일 없음");
            sb.AppendLine();
            return;
        }

        AppendFileList(sb, "Added", addedFiles);
        AppendFileList(sb, "Modified", modifiedFiles);
        AppendFileList(sb, "Deleted", deletedFiles);
    }

    private static void AppendCodeChangePreview(
        StringBuilder sb,
        Dictionary<string, FileSnapshot> previousMap,
        List<FileSnapshot> addedFiles,
        List<FileSnapshot> modifiedFiles)
    {
        sb.AppendLine("## Code Change Preview");
        sb.AppendLine();

        List<FileSnapshot> changedCs = addedFiles.Concat(modifiedFiles)
            .Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (changedCs.Count == 0)
        {
            sb.AppendLine("- 변경된 C# 코드 없음");
            sb.AppendLine();
            return;
        }

        foreach (FileSnapshot file in changedCs)
        {
            previousMap.TryGetValue(file.path, out FileSnapshot previous);

            sb.AppendLine($"### {file.path}");
            sb.AppendLine();

            AppendCodeStructure(sb, file.text);
            AppendSimpleDiff(sb, previous?.text ?? string.Empty, file.text);
            sb.AppendLine();
        }
    }

    private static void AppendScriptsTree(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Scripts Tree");
        sb.AppendLine();
        sb.AppendLine("```text");

        foreach (FileSnapshot file in currentFiles)
        {
            if (file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("- " + file.path);
            }
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendCurrentScriptStructure(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Current Script Structure");
        sb.AppendLine();

        foreach (FileSnapshot file in currentFiles)
        {
            if (!file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            sb.AppendLine($"### {file.path}");
            AppendCodeStructure(sb, file.text);
            sb.AppendLine();
        }
    }

    private static void AppendFullSourceCode(StringBuilder sb, List<FileSnapshot> currentFiles)
    {
        sb.AppendLine("## Full Source Code");
        sb.AppendLine();

        foreach (FileSnapshot file in currentFiles)
        {
            if (!file.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                continue;

            sb.AppendLine($"### FILE: {file.path}");
            sb.AppendLine();
            sb.AppendLine("```csharp");
            sb.AppendLine(file.text);
            sb.AppendLine("```");
            sb.AppendLine();
        }
    }

    private static List<DiagnosticIssue> BuildDiagnostics(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects)
    {
        List<DiagnosticIssue> issues = new List<DiagnosticIssue>();

        DetectMissingScripts(sceneObjects, issues);
        DetectDuplicateImportantObjects(sceneObjects, issues);
        DetectEmptyImportantObjects(sceneObjects, issues);
        DetectDebugAndOfficialCoexistence(currentFiles, sceneObjects, issues);
        DetectSerializedReferenceProblems(sceneObjects, issues);
        DetectScriptableObjectProblems(issues);
        DetectPrefabProblems(issues);
        DetectCodeSmells(currentFiles, issues);
        DetectKnownProjectRisks(currentFiles, sceneObjects, issues);

        return issues;
    }

    private static void DetectMissingScripts(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            if (info.components.Any(c => c == null))
            {
                issues.Add(new DiagnosticIssue(
                    "HIGH",
                    "Missing Script in Scene",
                    $"{info.path}에 Missing Script가 존재함.",
                    "해당 컴포넌트를 제거하거나 올바른 스크립트를 복구한다."
                ));
            }
        }
    }

    private static void DetectDuplicateImportantObjects(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        var groups = sceneObjects
            .GroupBy(o => o.name)
            .Where(g => g.Count() > 1)
            .OrderByDescending(g => g.Count());

        foreach (var group in groups)
        {
            string name = group.Key;

            bool important = ImportantObjectNameKeywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                             name.IndexOf("Grid", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("Canvas", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("Manager", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             name.IndexOf("UI", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!important)
                continue;

            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Duplicate Important GameObject Name",
                $"{name} 오브젝트가 {group.Count()}개 존재함: {string.Join(", ", group.Select(o => o.path))}",
                "실제로 필요한 중복인지 확인하고, 테스트용/빈 오브젝트는 삭제 또는 명확히 이름 변경한다."
            ));
        }
    }

    private static void DetectEmptyImportantObjects(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            bool onlyTransform = info.componentNames.All(c => c == "Transform" || c == "RectTransform");
            bool noChildren = info.gameObject.transform.childCount == 0;

            bool importantName = info.name.IndexOf("HUD", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("Grid", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("UI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 info.name.IndexOf("Manager", StringComparison.OrdinalIgnoreCase) >= 0;

            if (onlyTransform && noChildren && importantName)
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Empty Important-Looking GameObject",
                    $"{info.path}는 이름은 중요해 보이지만 Transform만 있고 자식도 없음.",
                    "실제 사용 중인지 확인하고, 필요 없으면 삭제하거나 테스트용 이름으로 변경한다."
                ));
            }
        }
    }

    private static void DetectDebugAndOfficialCoexistence(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        bool hasHudDebug = currentFiles.Any(f => f.path.EndsWith("HUDDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasHudOfficial = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicDebug = currentFiles.Any(f => f.path.EndsWith("RelicSelectDebugUI.cs", StringComparison.OrdinalIgnoreCase));
        bool hasRelicOfficial = currentFiles.Any(f => f.path.EndsWith("RelicSelectUI.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudDebug && hasHudOfficial)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "HUD DebugUI and Official HUD UI Coexist",
                "HUDDebugUI.cs와 HUDCanvasUI.cs가 동시에 존재함.",
                "신규 HUD 작업은 HUDCanvasUI 기준으로 진행하고, HUDDebugUI는 디버그/보류 후보로 분류한다."
            ));
        }

        if (hasRelicDebug && hasRelicOfficial)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Relic DebugUI and Official Relic UI Coexist",
                "RelicSelectDebugUI.cs와 RelicSelectUI.cs가 동시에 존재함.",
                "신규 유물 UI 작업은 RelicSelectUI 기준으로만 진행한다."
            ));
        }

        foreach (FileSnapshot file in currentFiles.Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)))
        {
            if (file.text.Contains("OnGUI("))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "OnGUI Usage Detected",
                    $"{file.path}에서 OnGUI 사용 감지.",
                    "OnGUI는 테스트/디버그용으로만 유지하고 출시형 UI는 Canvas 기반으로 진행한다."
                ));
            }
        }
    }

    private static void DetectSerializedReferenceProblems(List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        foreach (SceneObjectInfo info in sceneObjects)
        {
            foreach (Component component in info.components)
            {
                if (component == null)
                    continue;

                Type type = component.GetType();

                if (type == typeof(Transform) || type == typeof(RectTransform))
                    continue;

                if (ShouldSkipVerboseBuiltInComponent(type))
                    continue;

                List<SerializedFieldLine> fields = GetSerializedFieldLines(component);

                foreach (SerializedFieldLine field in fields)
                {
                    if (!field.isNullObjectReference)
                        continue;

                    if (field.isOptionalReference)
                        continue;

                    string severity = IsCriticalComponent(type.Name) ? "HIGH" : "MEDIUM";

                    issues.Add(new DiagnosticIssue(
                        severity,
                        "Possible Missing Inspector Reference",
                        $"{info.path} / {type.Name}.{field.path} = None",
                        "이 필드가 필수 연결인지 확인한다. 필수라면 Inspector에서 연결한다."
                    ));
                }
            }
        }
    }

    private static void DetectScriptableObjectProblems(List<DiagnosticIssue> issues)
    {
        string folder = ProjectRoot + "/ScriptableObjects";

        if (!Directory.Exists(folder))
            return;

        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset == null)
                continue;

            List<SerializedFieldLine> fields = GetSerializedFieldLines(asset);

            foreach (SerializedFieldLine field in fields)
            {
                if (!field.isNullObjectReference)
                    continue;

                if (field.isOptionalReference)
                    continue;

                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "ScriptableObject Null Reference",
                    $"{path} / {field.path} = None",
                    "아이콘 같은 선택 필드면 무시 가능. 필수 데이터라면 연결한다."
                ));
            }
        }
    }

    private static void DetectPrefabProblems(List<DiagnosticIssue> issues)
    {
        string prefabFolder = ProjectRoot + "/Prefabs";

        if (!Directory.Exists(prefabFolder))
            return;

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            List<GameObject> objects = new List<GameObject>();
            CollectGameObjects(prefab.transform, objects);

            foreach (GameObject go in objects)
            {
                Component[] comps = go.GetComponents<Component>();

                if (comps.Any(c => c == null))
                {
                    issues.Add(new DiagnosticIssue(
                        "HIGH",
                        "Missing Script in Prefab",
                        $"{path} / {GetRelativePrefabPath(prefab.transform, go.transform)}에 Missing Script 존재.",
                        "Prefab을 열어 Missing Script를 제거하거나 복구한다."
                    ));
                }
            }
        }
    }

    private static void DetectCodeSmells(List<FileSnapshot> currentFiles, List<DiagnosticIssue> issues)
    {
        foreach (FileSnapshot file in currentFiles.Where(f => f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)))
        {
            if (file.text.Contains("FindFirstObjectByType<"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "FindFirstObjectByType Usage",
                    $"{file.path}에서 FindFirstObjectByType 사용 감지.",
                    "초기 단계에서는 허용 가능. 호출 빈도가 높거나 확장되면 Inspector 참조 또는 이벤트 구조로 교체 검토."
                ));
            }

            if (file.text.Contains("Time.timeScale = 0f") && file.text.Contains("Time.timeScale = 1f"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Direct TimeScale Control",
                    $"{file.path}에서 Time.timeScale 직접 제어 감지.",
                    "여러 UI가 동시에 pause를 제어하면 충돌 가능. 나중에 PauseManager로 통합 검토."
                ));
            }

            if (file.text.Contains("Destroy(gameObject)") && file.path.Contains("/Enemy/"))
            {
                issues.Add(new DiagnosticIssue(
                    "LOW",
                    "Destroy Enemy Object",
                    $"{file.path}에서 Destroy(gameObject) 사용.",
                    "5분 MVP에서는 허용. 적 수가 많아지면 Object Pooling 검토."
                ));
            }
        }
    }

    private static void DetectKnownProjectRisks(List<FileSnapshot> currentFiles, List<SceneObjectInfo> sceneObjects, List<DiagnosticIssue> issues)
    {
        bool hasHudCanvas = sceneObjects.Any(o => o.name == "HUDCanvas");
        bool hasHudPanel = sceneObjects.Any(o => o.name == "HUDPanel");
        bool hasHudCanvasUi = currentFiles.Any(f => f.path.EndsWith("HUDCanvasUI.cs", StringComparison.OrdinalIgnoreCase));

        if (hasHudCanvas && hasHudPanel && hasHudCanvasUi)
        {
            bool hasRelicDisplayScript = currentFiles.Any(f => f.path.EndsWith("HUDRelicDisplay.cs", StringComparison.OrdinalIgnoreCase));
            bool hasRelicListPanel = sceneObjects.Any(o => o.name.IndexOf("RelicList", StringComparison.OrdinalIgnoreCase) >= 0);

            if (!hasRelicListPanel)
            {
                issues.Add(new DiagnosticIssue(
                    "MEDIUM",
                    "No Persistent Relic HUD Display",
                    "유물 선택 UI는 있으나 HUD에 보유 유물을 지속 표시하는 구조가 감지되지 않음.",
                    "새 UI를 만들기 전 HUDCanvasUI와 기존 HUDPanel 구조를 먼저 확인하고 통합 방식을 결정한다."
                ));
            }

            if (hasRelicDisplayScript && !hasRelicListPanel)
            {
                issues.Add(new DiagnosticIssue(
                    "MEDIUM",
                    "Relic Display Script Without Scene UI",
                    "HUDRelicDisplay.cs는 있으나 RelicList 계열 UI 오브젝트가 감지되지 않음.",
                    "스크립트만 추가된 상태인지 확인하고, 기존 HUD 구조와 연결 여부를 점검한다."
                ));
            }
        }

        FileSnapshot enemyHealth = currentFiles.FirstOrDefault(f => f.path.EndsWith("EnemyHealth.cs", StringComparison.OrdinalIgnoreCase));

        if (enemyHealth != null &&
            enemyHealth.text.Contains("DropExpGem") &&
            enemyHealth.text.Contains("DropSmallHeal") &&
            CountOccurrences(enemyHealth.text, "transform.position") >= 2)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Drop Items May Overlap",
                "EnemyHealth에서 ExpGem과 SmallHeal이 같은 transform.position으로 생성될 가능성이 있음.",
                "SmallHeal 드랍 위치를 약간 오프셋하여 ExpGem과 겹치지 않게 한다."
            ));
        }

        int weaponScripts = currentFiles.Count(f =>
            f.path.Contains("/Weapons/", StringComparison.OrdinalIgnoreCase) &&
            f.path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase));

        if (weaponScripts <= 1)
        {
            issues.Add(new DiagnosticIssue(
                "MEDIUM",
                "Only One Weapon Script Detected",
                "Weapons 폴더에 무기 스크립트가 1개 이하로 감지됨.",
                "근접 무기 단조로움 해소를 위해 두 번째 무기 구조를 계획한다. 단, HUD/드랍/웨이브 정리 후 진행."
            ));
        }
    }

    private static List<SceneObjectInfo> CollectSceneObjectInfos()
    {
        List<SceneObjectInfo> result = new List<SceneObjectInfo>();

        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] roots = activeScene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            CollectSceneObjectInfoRecursive(root.transform, result);
        }

        return result;
    }

    private static void CollectSceneObjectInfoRecursive(Transform transform, List<SceneObjectInfo> result)
    {
        if (transform == null)
            return;

        GameObject go = transform.gameObject;
        Component[] components = go.GetComponents<Component>();

        SceneObjectInfo info = new SceneObjectInfo
        {
            gameObject = go,
            path = GetGameObjectPath(go),
            name = go.name,
            activeSelf = go.activeSelf,
            tag = go.tag,
            layer = go.layer,
            components = components.ToList(),
            componentNames = components.Select(c => c == null ? "Missing Script" : c.GetType().Name).ToList()
        };

        result.Add(info);

        for (int i = 0; i < transform.childCount; i++)
        {
            CollectSceneObjectInfoRecursive(transform.GetChild(i), result);
        }
    }

    private static void AppendTransformInfo(StringBuilder sb, Component component)
    {
        if (component == null)
            return;

        Transform transform = component as Transform;

        if (transform == null)
            return;

        sb.AppendLine($"#### {component.GetType().Name}");
        sb.AppendLine("```text");
        sb.AppendLine($"Local Position: {transform.localPosition}");
        sb.AppendLine($"Local Rotation: {transform.localEulerAngles}");
        sb.AppendLine($"Local Scale: {transform.localScale}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendSerializedObjectFields(StringBuilder sb, UnityEngine.Object target)
    {
        List<SerializedFieldLine> fields = GetSerializedFieldLines(target);

        sb.AppendLine("```text");

        if (fields.Count == 0)
        {
            sb.AppendLine("No visible serialized fields");
        }
        else
        {
            int count = 0;

            foreach (SerializedFieldLine field in fields)
            {
                string indent = new string(' ', field.depth * 2);
                sb.AppendLine($"{indent}{field.path}: {field.value}");

                count++;

                if (count >= MaxSerializedPropertiesPerComponent)
                {
                    sb.AppendLine("# ... serialized fields truncated");
                    break;
                }
            }
        }

        sb.AppendLine("```");
    }

    private static List<SerializedFieldLine> GetSerializedFieldLines(UnityEngine.Object target)
    {
        List<SerializedFieldLine> result = new List<SerializedFieldLine>();

        if (target == null)
            return result;

        try
        {
            SerializedObject so = new SerializedObject(target);
            SerializedProperty iterator = so.GetIterator();

            bool enterChildren = true;
            int count = 0;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                    continue;

                if (iterator.propertyPath.StartsWith("m_"))
                    continue;

                if (iterator.depth > 4)
                    continue;

                string value = GetSerializedPropertyValue(iterator);

                if (string.IsNullOrEmpty(value))
                    continue;

                SerializedFieldLine line = new SerializedFieldLine
                {
                    path = iterator.propertyPath,
                    value = value,
                    depth = iterator.depth,
                    type = iterator.propertyType,
                    isNullObjectReference = iterator.propertyType == SerializedPropertyType.ObjectReference &&
                                            iterator.objectReferenceValue == null,
                    isOptionalReference = IsOptionalReferenceField(iterator.propertyPath)
                };

                result.Add(line);
                count++;

                if (count >= MaxSerializedPropertiesPerComponent)
                    break;
            }
        }
        catch (Exception ex)
        {
            result.Add(new SerializedFieldLine
            {
                path = "SerializationError",
                value = ex.Message,
                depth = 0,
                type = SerializedPropertyType.String,
                isNullObjectReference = false,
                isOptionalReference = false
            });
        }

        return result;
    }

    private static string GetSerializedPropertyValue(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue.ToString();

            case SerializedPropertyType.Boolean:
                return property.boolValue.ToString();

            case SerializedPropertyType.Float:
                return property.floatValue.ToString("0.###");

            case SerializedPropertyType.String:
                return string.IsNullOrEmpty(property.stringValue) ? "\"\"" : property.stringValue;

            case SerializedPropertyType.Color:
                return property.colorValue.ToString();

            case SerializedPropertyType.ObjectReference:
                if (property.objectReferenceValue == null)
                    return "None";

                return $"{property.objectReferenceValue.name} ({property.objectReferenceValue.GetType().Name})";

            case SerializedPropertyType.LayerMask:
                return property.intValue.ToString();

            case SerializedPropertyType.Enum:
                if (property.enumDisplayNames != null &&
                    property.enumValueIndex >= 0 &&
                    property.enumValueIndex < property.enumDisplayNames.Length)
                {
                    return property.enumDisplayNames[property.enumValueIndex];
                }

                return property.enumValueIndex.ToString();

            case SerializedPropertyType.Vector2:
                return property.vector2Value.ToString();

            case SerializedPropertyType.Vector3:
                return property.vector3Value.ToString();

            case SerializedPropertyType.Vector4:
                return property.vector4Value.ToString();

            case SerializedPropertyType.Rect:
                return property.rectValue.ToString();

            case SerializedPropertyType.ArraySize:
                return property.intValue.ToString();

            case SerializedPropertyType.Character:
                return property.intValue.ToString();

            case SerializedPropertyType.AnimationCurve:
                return "AnimationCurve";

            case SerializedPropertyType.Bounds:
                return property.boundsValue.ToString();

            case SerializedPropertyType.Quaternion:
                return property.quaternionValue.eulerAngles.ToString();

            case SerializedPropertyType.ExposedReference:
                return "ExposedReference";

            case SerializedPropertyType.FixedBufferSize:
                return property.fixedBufferSize.ToString();

            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue.ToString();

            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue.ToString();

            case SerializedPropertyType.RectInt:
                return property.rectIntValue.ToString();

            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue.ToString();

            case SerializedPropertyType.ManagedReference:
                return string.IsNullOrEmpty(property.managedReferenceFullTypename)
                    ? "ManagedReference"
                    : property.managedReferenceFullTypename;

            case SerializedPropertyType.Generic:
                if (property.isArray)
                    return $"Array/List Size: {property.arraySize}";

                return string.Empty;

            default:
                return property.propertyType.ToString();
        }
    }

    private static bool ShouldSkipVerboseBuiltInComponent(Type type)
    {
        if (type == typeof(Canvas) ||
            type.Name == "CanvasScaler" ||
            type.Name == "GraphicRaycaster" ||
            type.Name == "CanvasRenderer" ||
            type.Name == "SpriteRenderer" ||
            type.Name == "Image" ||
            type.Name == "Button" ||
            type.Name == "TextMeshProUGUI" ||
            type.Name == "Slider")
        {
            return false;
        }

        return false;
    }

    private static bool IsCriticalComponent(string componentName)
    {
        return componentName.Contains("Manager") ||
               componentName.Contains("Spawner") ||
               componentName.EndsWith("UI") ||
               componentName.Contains("Health") ||
               componentName.Contains("Controller");
    }

    private static bool IsOptionalReferenceField(string propertyPath)
    {
        string lower = propertyPath.ToLowerInvariant();

        foreach (string keyword in OptionalReferenceKeywords)
        {
            if (lower.Contains(keyword.ToLowerInvariant()))
                return true;
        }

        return false;
    }

    private static string GetGameObjectPath(GameObject go)
    {
        if (go == null)
            return "None";

        Stack<string> names = new Stack<string>();
        Transform current = go.transform;

        while (current != null)
        {
            names.Push(current.name);
            current = current.parent;
        }

        return string.Join("/", names);
    }

    private static string GetRelativePrefabPath(Transform root, Transform target)
    {
        if (root == null || target == null)
            return "Unknown";

        Stack<string> names = new Stack<string>();
        Transform current = target;

        while (current != null && current != root.parent)
        {
            names.Push(current.name);
            if (current == root)
                break;
            current = current.parent;
        }

        return string.Join("/", names);
    }

    private static void CollectGameObjects(Transform transform, List<GameObject> list)
    {
        if (transform == null)
            return;

        list.Add(transform.gameObject);

        for (int i = 0; i < transform.childCount; i++)
        {
            CollectGameObjects(transform.GetChild(i), list);
        }
    }

    private static void AppendFileList(StringBuilder sb, string title, List<FileSnapshot> files)
    {
        if (files.Count == 0)
            return;

        sb.AppendLine($"### {title}");
        sb.AppendLine();

        foreach (FileSnapshot file in files)
        {
            sb.AppendLine($"- {file.path}");
        }

        sb.AppendLine();
    }

    private static void AppendCodeStructure(StringBuilder sb, string text)
    {
        List<string> enums = Regex.Matches(text, @"\benum\s+([A-Za-z_][A-Za-z0-9_]*)")
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

        List<string> classes = Regex.Matches(text, @"\bclass\s+([A-Za-z_][A-Za-z0-9_]*)")
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

        List<string> methods = Regex.Matches(
                text,
                @"\b(public|private|protected|internal)\s+(static\s+)?[A-Za-z0-9_<>\[\],\s]+\s+([A-Za-z_][A-Za-z0-9_]*)\s*\("
            )
            .Cast<Match>()
            .Select(m => m.Groups[3].Value)
            .Where(name => name != "if" && name != "for" && name != "while" && name != "switch")
            .Distinct()
            .ToList();

        sb.AppendLine("```text");

        if (enums.Count > 0)
        {
            sb.AppendLine("Enums:");
            foreach (string enumName in enums)
            {
                sb.AppendLine($"- {enumName}");
            }
        }

        if (classes.Count > 0)
        {
            sb.AppendLine("Classes:");
            foreach (string className in classes)
            {
                sb.AppendLine($"- {className}");
            }
        }

        if (methods.Count > 0)
        {
            sb.AppendLine("Methods:");
            foreach (string methodName in methods)
            {
                sb.AppendLine($"- {methodName}()");
            }
        }

        if (enums.Count == 0 && classes.Count == 0 && methods.Count == 0)
        {
            sb.AppendLine("No structure detected");
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendSimpleDiff(StringBuilder sb, string oldText, string newText)
    {
        string[] oldLines = SplitLines(oldText);
        string[] newLines = SplitLines(newText);

        List<string> changes = new List<string>();

        int max = Math.Max(oldLines.Length, newLines.Length);

        for (int i = 0; i < max; i++)
        {
            string oldLine = i < oldLines.Length ? oldLines[i] : null;
            string newLine = i < newLines.Length ? newLines[i] : null;

            if (oldLine == newLine)
                continue;

            if (oldLine != null)
            {
                changes.Add($"- {oldLine}");
            }

            if (newLine != null)
            {
                changes.Add($"+ {newLine}");
            }

            if (changes.Count >= MaxDiffLines)
                break;
        }

        sb.AppendLine("Changed code preview:");
        sb.AppendLine("```diff");

        if (changes.Count == 0)
        {
            sb.AppendLine("# 코드 변경 미리보기 없음");
        }
        else
        {
            foreach (string line in changes)
            {
                sb.AppendLine(line);
            }

            if (changes.Count >= MaxDiffLines)
            {
                sb.AppendLine("# ... 변경 내용 일부만 표시됨");
            }
        }

        sb.AppendLine("```");
    }

    private static int CountOccurrences(string text, string pattern)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            return 0;

        int count = 0;
        int index = 0;

        while ((index = text.IndexOf(pattern, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += pattern.Length;
        }

        return count;
    }

    private static List<FileSnapshot> ScanProjectFiles()
    {
        List<FileSnapshot> result = new List<FileSnapshot>();

        if (!Directory.Exists(ProjectRoot))
        {
            Debug.LogWarning($"프로젝트 루트를 찾을 수 없습니다: {ProjectRoot}");
            return result;
        }

        string[] files = Directory.GetFiles(ProjectRoot, "*.*", SearchOption.AllDirectories);

        foreach (string rawPath in files)
        {
            string path = NormalizePath(rawPath);

            if (path.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                continue;

            if (GeneratedOutputPaths.Contains(path))
                continue;

            string extension = Path.GetExtension(path).ToLowerInvariant();

            if (!TargetExtensions.Contains(extension))
                continue;

            byte[] bytes = File.ReadAllBytes(path);

            FileSnapshot snapshot = new FileSnapshot
            {
                path = path,
                hash = GetSha256(bytes),
                size = bytes.LongLength,
                text = ShouldStoreText(extension) ? SafeReadText(path) : string.Empty
            };

            result.Add(snapshot);
        }

        return result.OrderBy(f => f.path).ToList();
    }

    private static bool ShouldStoreText(string extension)
    {
        return extension == ".cs";
    }

    private static string SafeReadText(string path)
    {
        try
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            return $"// Failed to read file: {path}\n// {ex.Message}";
        }
    }

    private static SnapshotCache LoadCache()
    {
        if (!File.Exists(CachePath))
        {
            return new SnapshotCache();
        }

        try
        {
            string json = File.ReadAllText(CachePath, Encoding.UTF8);
            SnapshotCache cache = JsonUtility.FromJson<SnapshotCache>(json);

            if (cache == null)
                return new SnapshotCache();

            if (cache.files == null)
                cache.files = new List<FileSnapshot>();

            return cache;
        }
        catch
        {
            return new SnapshotCache();
        }
    }

    private static void SaveCache(List<FileSnapshot> files)
    {
        SnapshotCache cache = new SnapshotCache
        {
            files = files
        };

        string json = JsonUtility.ToJson(cache, true);
        File.WriteAllText(CachePath, json, Encoding.UTF8);
    }

    private static void EnsureDocsFolder()
    {
        if (!Directory.Exists(DocsFolder))
        {
            Directory.CreateDirectory(DocsFolder);
        }
    }

    private static string[] SplitLines(string text)
    {
        if (string.IsNullOrEmpty(text))
            return Array.Empty<string>();

        return text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
    }

    private static string GetSha256(byte[] bytes)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    private static string NormalizePath(string path)
    {
        return path.Replace("\\", "/");
    }

    private static string NormalizePathStatic(string path)
    {
        return path.Replace("\\", "/");
    }
}