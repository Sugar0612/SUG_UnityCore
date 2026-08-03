#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


namespace SUG.Essentials.Editor
{
    [InitializeOnLoad]
    public static class EssentialsAutoInitializer
    {

        private const string Key =
            "Essentials.Initialized";


        static EssentialsAutoInitializer()
        {

            EditorApplication.delayCall += () =>
            {

                if (EditorPrefs.GetBool(Key))
                    return;


                EditorPrefs.SetBool(
                    Key,
                    true
                );


                EssentialsInitializerWindow.Open();

            };

        }

    }
}

#endif