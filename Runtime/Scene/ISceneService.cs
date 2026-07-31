using System;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    [Injectable] public interface ISceneService
    {
        [Scene] public string loadingScene { get; set; }    

        public event Action<Scene, LoadSceneMode> sceneLoaded;

        public event Action<Scene> sceneUnloaded;
        public string currScene { get; set; }
        public void LoadSceneAsync(string scene, bool useLoading = false, LoadSceneMode mode = LoadSceneMode.Single);
        public void UnloadSceneAsync(string scene);

        public Scene GetActiveScene();

        public void SetActiveScene(string sceneName); // 激活场景
    }
}
