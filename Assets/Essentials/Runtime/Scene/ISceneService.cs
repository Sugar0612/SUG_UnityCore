using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    [Injectable] public interface ISceneService
    {

        public event Action<Scene, LoadSceneMode> sceneLoaded;

        public event Action<Scene> sceneUnloaded;

        //// 加载过渡场景资源引用
        //public AssetReference loadingScene { get; set; }    

        //// 当前场景实例
        //public SceneInstance currScene { get; set; }
        
        //// 上个场景实例
        //public SceneInstance lastSc { get; set; }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="scene"> 场景名 </param>
        /// <param name="useLoading"> 是否需要加载界面 </param>
        /// <param name="mode"> 场景加载模式 </param>
        public void LoadSceneAsync(AssetReference scene, bool useLoading = false, LoadSceneMode mode = LoadSceneMode.Single);

        /// <summary>
        /// 异步场景卸载
        /// </summary>
        /// <param name="scene"></param>
        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene);
    }
}
