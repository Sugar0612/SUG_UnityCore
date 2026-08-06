#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SUG.Essentials.Editor
{
    /// <summary>
    /// 自动保证每个 Scene 都存在 SceneBootstrap。
    /// </summary>
    [InitializeOnLoad]
    internal static class SceneBootstrapEditor
    {
        static SceneBootstrapEditor()
        {
            EditorSceneManager.sceneSaving += OnSceneSaving;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneSaving(UnityEngine.SceneManagement.Scene scene, string path)
        {
            EnsureBootstrapExists();
        }

        private static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            EnsureBootstrapExists();
        }

        private static void EnsureBootstrapExists()
        {
            if (Object.FindFirstObjectByType<DIBootstrap>() != null)
                return;

            var go = new GameObject("[Essentials]");

            go.AddComponent<DIBootstrap>();

            Undo.RegisterCreatedObjectUndo(go, "Create SceneBootstrap");

            EditorSceneManager.MarkSceneDirty(go.scene);

            Debug.Log("[Essentials] Auto Create SceneBootstrap.");
        }
    }
}

#endif