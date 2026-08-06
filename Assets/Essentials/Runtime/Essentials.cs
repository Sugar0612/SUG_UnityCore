using UnityEngine;

namespace SUG.Essentials
{
    public static class Essentials
    {
        public static EssentialsSettingsSO Settings { get; private set; }

        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) 
                return;

            _initialized = true;

            // 加载Essentials总配置表
            Settings = Resources.Load<EssentialsSettingsSO>("Essentials/Bootstrap");
            if (Settings == null)
            {
                Debug.Log("Settings is invalid.");
            }
            else
            {
                Debug.Log("Setting is valid.");
            }
            // 启用规则：注册场景中所有的 IGlobalService 和 ILocalService
            // 场景预载更新场景服务容器。
            ServiceScanner.Initialize();

            // 弃用：现在不再使用SceneLoad来注册初始化！，现在改成了给各个场景添加一个SceneBootstrap来注入初始化。
            // 场景中所有的Mono注入
            //AutoInjector.Initialize();
        }

        #region Common
        // 场景实例化
        public static T Instantiate<T>(T prefab, Transform parent) where T : Object
        {
            var obj = Object.Instantiate(prefab, parent);

            Inject(obj);

            return obj;
        }

        public static T Instantiate<T>(T prefab) where T : Object
        {
            var obj = Object.Instantiate(prefab);

            Inject(obj);

            return obj;
        }

        #endregion

        #region DI

        private static void Inject(Object obj)
        {
            if (obj is GameObject go)
            {
                InjectGameObject(go);
                return;
            }

            if (obj is Component component)
            {
                Injector.Inject(component);
            }
        }

        private static void InjectGameObject(GameObject go)
        {
            var monos = go.GetComponentsInChildren<MonoBehaviour>(true);

            foreach (var mono in monos)
            {
                if (mono == null) continue;
                Injector.Inject(mono);
            }
        }

        public static T Resolve<T>(string id = "default")
        {
            var t = typeof(T);
            return ServiceRegistry.Resolve<T>(new ServiceKey(t, id));
        }

        #endregion
    }
}