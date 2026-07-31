using System;
using System.Collections;
using System.IO;
using UnityEngine;
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
            if (useLoading && _loadingScene != null)
            {
                AsyncOperation loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_loadingScene, LoadSceneMode.Additive);
                yield return loading;
                SetActiveScene(_loadingScene);
            }

            // 如果不是Additive，那就卸载lastSc
            if (mode != LoadSceneMode.Additive)
            {
                AsyncOperation unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(lastSc);
                if (unload != null) yield return unload;
            }

            // 加载新场景并激活
            AsyncOperation load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            load.allowSceneActivation = false;

            while (load.progress < 0.9f)
            {
                yield return null;
            }

            // 准备切换场景清空场景DI容器
            ServiceRegistry.ClearScene();
            yield return new WaitForSeconds(1.0f);

            // 设置激活
            load.allowSceneActivation = true;

            while (!load.isDone)
            {
                yield return null;
            }

            SetActiveScene(sceneName);

            // 卸载【加载场景】
            if (useLoading && _loadingScene != null)
            {
                AsyncOperation unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_loadingScene);
                if (unload != null) yield return unload;
            }
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
