//using UnityEditor;
//using UnityEditor.PackageManager;
//using UnityEditor.PackageManager.Requests;
//using UnityEngine;

//namespace SUG.Essentials.Editor
//{
//    /// <summary>
//    /// Essentials 自动安装依赖
//    /// </summary>
//    [InitializeOnLoad]
//    public static class DependencyInstaller
//    {
//        /// <summary>
//        /// VContainer Git 地址
//        /// </summary>
//        private const string VContainerGitUrl =
//            "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.18.0";

//        /// <summary>
//        /// Package 名称（用于检测）
//        /// </summary>
//        private const string VContainerPackageName =
//            "jp.hadashikick.vcontainer";

//        private static ListRequest _listRequest;
//        private static AddRequest _addRequest;

//        static DependencyInstaller()
//        {
//            EditorApplication.delayCall += CheckDependency;
//        }

//        /// <summary>
//        /// 检查是否安装 VContainer
//        /// </summary>
//        private static void CheckDependency()
//        {
//            _listRequest = Client.List(true);

//            EditorApplication.update += WaitList;
//        }

//        private static void WaitList()
//        {
//            if (!_listRequest.IsCompleted)
//                return;

//            EditorApplication.update -= WaitList;

//            if (_listRequest.Status == StatusCode.Failure)
//            {
//                Debug.LogError(_listRequest.Error.message);
//                return;
//            }

//            foreach (var package in _listRequest.Result)
//            {
//                if (package.name == VContainerPackageName)
//                {
//                    //Debug.Log("[Essentials] VContainer 已安装。");
//                    return;
//                }
//            }

//            InstallVContainer();
//        }

//        /// <summary>
//        /// 安装 VContainer
//        /// </summary>
//        private static void InstallVContainer()
//        {
//            bool install = EditorUtility.DisplayDialog(
//                "Essentials",
//                "VContainer is required.\n\nInstall it automatically?",
//                "Install",
//                "Cancel");

//            if (!install)
//                return;

//            Debug.Log("[Essentials] 正在安装 VContainer...");

//            _addRequest = Client.Add(VContainerGitUrl);

//            EditorApplication.update += WaitInstall;
//        }

//        private static void WaitInstall()
//        {
//            if (!_addRequest.IsCompleted)
//                return;

//            EditorApplication.update -= WaitInstall;

//            if (_addRequest.Status == StatusCode.Success)
//            {
//                Debug.Log("[Essentials] VContainer 安装成功！");
//            }
//            else
//            {
//                Debug.LogError(
//                    $"[Essentials] 安装失败：{_addRequest.Error.message}");
//            }
//        }
//    }
//}