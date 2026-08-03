#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System;


namespace SUG.Essentials.Editor
{
    public class EssentialsInitializerWindow : EditorWindow
    {
        private static bool _initialized;

        private static readonly string InitKey =
            "Essentials.Initialized";


        private AddRequest _addRequest;


        private Vector2 _scroll;


        private DependencyInfo[] _dependencies;


        [MenuItem("Tools/Essentials/Initialization")]
        public static void Open()
        {
            var window =
                GetWindow<EssentialsInitializerWindow>(
                    "Essentials Initialization"
                );

            window.minSize =
                new Vector2(450, 350);

            window.Refresh();

            window.Show();
        }


        private void OnEnable()
        {
            Refresh();
        }


        private void Refresh()
        {
            _dependencies = new[]
            {
                new DependencyInfo(
                    "Addressables",
                    "com.unity.addressables",
                    CheckAddressables
                ),

                new DependencyInfo(
                    "DOTween",
                    "DOTween",
                    CheckDOTween
                )
            };
        }



        private void OnGUI()
        {

            GUILayout.Space(10);


            GUILayout.Label(
                "Essentials Initialization",
                EditorStyles.boldLabel
            );


            GUILayout.Space(5);


            EditorGUILayout.HelpBox(
                "Essentials requires the following dependencies.",
                MessageType.Info
            );


            GUILayout.Space(10);



            // Fix All

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button(
                "Fix All Dependencies",
                GUILayout.Height(35)))
            {
                FixAll();
            }

            GUI.backgroundColor = Color.white;



            GUILayout.Space(15);



            _scroll =
                EditorGUILayout.BeginScrollView(
                    _scroll
                );


            foreach (var dep in _dependencies)
            {
                DrawDependency(dep);
            }


            EditorGUILayout.EndScrollView();



            if (_addRequest != null)
            {
                if (_addRequest.IsCompleted)
                {
                    if (_addRequest.Status ==
                       StatusCode.Success)
                    {
                        Debug.Log(
                            "Dependency installed successfully."
                        );

                        _addRequest = null;

                        Refresh();

                        Repaint();
                    }
                    else
                    {
                        Debug.LogError(
                            _addRequest.Error.message
                        );

                        _addRequest = null;
                    }
                }
            }
        }



        private void DrawDependency(
            DependencyInfo dependency)
        {

            EditorGUILayout.BeginHorizontal(
                EditorStyles.helpBox
            );


            GUILayout.Label(
                dependency.Name,
                GUILayout.Width(150)
            );


            GUILayout.FlexibleSpace();



            if (dependency.IsInstalled())
            {

                GUI.color = Color.green;

                GUILayout.Label(
                    "✔ Installed",
                    GUILayout.Width(100)
                );

                GUI.color = Color.white;

            }
            else
            {

                if (GUILayout.Button(
                    "Install",
                    GUILayout.Width(100)))
                {
                    Install(dependency);
                }

            }


            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
        }




        private void FixAll()
        {
            foreach (var dep in _dependencies)
            {
                if (!dep.IsInstalled())
                {
                    Install(dep);
                }
            }
        }





        private void Install(
            DependencyInfo dependency)
        {

            if (dependency.Id ==
               "com.unity.addressables")
            {
                InstallPackage(
                    dependency.Id
                );
            }


            else if (dependency.Id ==
                    "DOTween")
            {
                Debug.LogWarning(
                    "DOTween installer not implemented."
                );
            }

        }



        private void InstallPackage(
            string packageId)
        {

            Debug.Log(
                $"Installing {packageId}"
            );


            _addRequest =
                Client.Add(packageId);

        }



        private static bool CheckAddressables()
        {

            var package =
                UnityEditor.PackageManager
                .PackageInfo.FindForAssetPath(
                    "Packages/com.unity.addressables"
                );


            return package != null;
        }




        private static bool CheckDOTween()
        {

            // DOTween一般在Assets目录

            var type =
                Type.GetType(
                    "DG.Tweening.DOTween, DOTween"
                );


            return type != null;
        }



        private class DependencyInfo
        {
            public string Name;

            public string Id;


            private Func<bool> checker;


            public DependencyInfo(
                string name,
                string id,
                Func<bool> checker)
            {
                Name = name;
                Id = id;
                this.checker = checker;
            }


            public bool IsInstalled()
            {
                return checker();
            }
        }
    }
}

#endif