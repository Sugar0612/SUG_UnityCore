using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private AssetReference _firstGameScene;

        [Inject] private ISceneService _sceneMgr;

        void Start()
        {
            //SceneManager.sceneLoaded += LoadFirstGameScene;
            if (_firstGameScene != null)
                _sceneMgr.LoadSceneAsync(_firstGameScene, false, LoadSceneMode.Additive);
        }
    }
}
