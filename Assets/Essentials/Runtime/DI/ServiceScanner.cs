using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    /// <summary>
    /// 自动扫描并注册所有 Service。
    /// </summary>
    internal static class ServiceScanner
    {
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;

            // 首次扫描全局服务
            //ScanGlobal();
        }

        public static void ScanRegister(Scene scene)
        {
            // 全局IGlobalService注册
            ScanGlobal();

            // 局部场景ILocalService注册
            ScanScene(scene);
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            ServiceRegistry.ClearScene();
        }

        #region Scan

        /// <summary>
        /// 扫描整个场景
        /// </summary>
        private static void ScanScene(Scene scene)
        {
            // Debug.Log($"=========== Service 注册 ScanScene  {scene.name}");
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);

                foreach (var behaviour in behaviours)
                {
                    RegisterSceneService(behaviour);
                }
            }
        }

        /// <summary>
        /// 扫描所有全局对象
        /// </summary>
        private static void ScanGlobal()
        {
            var behaviours = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            foreach (var behaviour in behaviours)
            {
                RegisterGlobalService(behaviour);
            }
        }

        #endregion

        #region Register

        private static void RegisterSceneService(MonoBehaviour behaviour)
        {
            RegisterService(behaviour, true);
        }

        private static void RegisterGlobalService(MonoBehaviour behaviour)
        {
            RegisterService(behaviour, false);
        }

        /// <summary>
        /// 注册一个 Service
        /// </summary>
        private static void RegisterService(MonoBehaviour behaviour, bool isScene)
        {
            // 如果没有Service作为Attribute那么不会注册
            if (!HasServiceAttribute(behaviour.GetType())) return;

            var type = behaviour.GetType();

            // 获取attribute里面的参数
            var attribute = type.GetCustomAttribute<ServiceAttribute>();
            string id                = attribute.id;
            ServiceLifetime lifetime = attribute.lifetime;

            // 获取服务接口
            var interfaces = ReflectionCache.GetInterfaces(type);

            foreach (var i in interfaces)
            {
                // 跳过没有 Injectable 标记的 interface。
                if (!IsInjectInterface(i)) continue;

                if (isScene) 
                    ServiceRegistry.RegisterScene(new ServiceKey(i, id), behaviour);
                else 
                    ServiceRegistry.RegisterGlobal(new ServiceKey(i, id), behaviour);
            }

            //foreach (var i in interfaces)
            //{
            //    // Debug.Log($"  Interface : {i.Name}");
            //}
        }

        /// <summary>
        /// 是否为可注入的Interface
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsInjectInterface(Type type)
        {
            return Attribute.IsDefined(
                type,
                typeof(InjectableAttribute));
        }

        /// <summary>
        /// 该Type是否有[Service]作为Attribute？
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool HasServiceAttribute(Type type)
        {
            return Attribute.IsDefined(type, typeof(ServiceAttribute));
        }

        #endregion
    }
}