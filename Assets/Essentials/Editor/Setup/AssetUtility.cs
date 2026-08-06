using System.IO;
using UnityEditor;
using UnityEngine;

namespace SUG.Essentials.Editor
{
    internal static class AssetUtility
    {
        public static T CreateAssetIfNotExist<T>(string path)
            where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset != null)
                return asset;

            string directory = Path.GetDirectoryName(path);

            CreateFolderRecursive(directory);

            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, path);

            return asset;
        }

        private static void CreateFolderRecursive(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            string parent = Path.GetDirectoryName(path);
            string folder = Path.GetFileName(path);

            if (!AssetDatabase.IsValidFolder(parent))
            {
                CreateFolderRecursive(parent);
            }

            AssetDatabase.CreateFolder(parent, folder);
        }
    }
}