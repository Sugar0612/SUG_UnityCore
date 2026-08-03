using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    [Service(lifetime = ServiceLifetime.Global)]
    public sealed class SceneManager : MonoBehaviour, ISceneService
    {
        private string _currScene;

        // Event SceneManager.sceneLoaded和SceneManager.sceneUnloaded的上层接口
        public event Action<UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded;
        public event Action<UnityEngine.SceneManagement.Scene> sceneUnloaded;

        // 当前场景
        public string currScene { get => _currScene; set => _currScene = value; }

        // 加载场景
        [Scene, SerializeField]
        private string _loadingScene;
        public string loadingScene { get => _loadingScene; set => _loadingScene = value; }

        private void Awake()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (s, l) => sceneLoaded?.Invoke(s,l);
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += (s) => sceneUnloaded?.Invoke(s);
        }

        /// <summary>
        /// 异步切换场景
        /// </summary>
        /// <param name="scene"> 新场景 </param>
        /// <param name="useLoading"> 是否显示【加载场景】？ </param>
        public void LoadSceneAsync(string scene, bool useLoading = false, LoadSceneMode mode = LoadSceneMode.Single)
        {
            StartCoroutine(LoadSceneDelayed(scene, useLoading, mode));
        }

        private IEnumerator LoadSceneDelayed(string sceneName, bool useLoading, LoadSceneMode mode)
        {
            // 获取当前场景
            Scene lastSc = GetActiveScene();

            // 加载场景界面显示
            AsyncOperationHandle<SceneInstance> loadingHandle = Addressables.LoadSceneAsync("Loading", LoadSceneMode.Additive);
            yield return loadingHandle;
            SetActiveScene(_loadingScene);
            //if (useLoading && _loadingScene != null)
            //{
            //    loadingHandle 
            //}

            // 如果不是Additive，那就卸载lastSc
            if (mode != LoadSceneMode.Additive)
            {
                AsyncOperation unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(lastSc);
                if (unload != null) yield return unload;
            }

            // 加载新场景并激活
            var load = Addressables.LoadSceneAsync("Start", LoadSceneMode.Additive, false);

            // 等待加载完成
            while (!load.IsDone)
            {
                yield return null;
            }

            // 准备切换场景清空场景DI容器
            ServiceRegistry.ClearScene();
            yield return new WaitForSeconds(1.0f);

            // 激活场景
            load.Result.ActivateAsync();

            while (!load.Result.Scene.isLoaded)
            {
                yield return null;
            }

            SetActiveScene(sceneName);


            var unloadd =
                    Addressables.UnloadSceneAsync(
                        loadingHandle
                    );

            yield return unloadd;
            // 卸载Loading
            //if (useLoading && _loadingScene != null)
            //{
            //    var unload =
            //        Addressables.UnloadSceneAsync(
            //            loadingHandle
            //        );

            //    yield return unload;
            //}
        }

        public void UnloadSceneAsync(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
        }

        public Scene GetActiveScene() => UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        public void SetActiveScene(string sceneName)
        {
            string sceneShortName = Path.GetFileNameWithoutExtension(sceneName);
            Scene sc = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneShortName);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(sc);
            currScene = sceneName;
        }
    }
}
