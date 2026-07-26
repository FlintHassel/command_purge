using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class FixBuildSettings
{
    static FixBuildSettings()
    {
        EditorApplication.delayCall += AutoAddScenes;
    }

    private static void AutoAddScenes()
    {
        string[] requiredScenes = new string[] 
        {
            "Assets/Scenes/House.unity",
            "Assets/Scenes/Ui Computer.unity",
            "Assets/Scenes/Computer.unity"
        };

        var editorBuildSettingsScenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        bool modified = false;

        foreach (string path in requiredScenes)
        {
            bool exists = false;
            foreach (var scene in editorBuildSettingsScenes)
            {
                if (scene.path == path)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                if (System.IO.File.Exists(path))
                {
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
                    modified = true;
                    Debug.Log($"[Antigravity] Auto-added missing scene to Build Settings: {path}");
                }
            }
        }

        if (modified)
        {
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("[Antigravity] Build Settings updated and saved successfully!");
        }
    }
}
