using UnityEngine;

namespace SUG.Essentials.DI
{
    public static class Essentials
    {
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

        /// <summary>
        /// 场景服务清空
        /// </summary>
        public static void ClearSceneContainer()
        {
            ServiceRegistry.ClearScene();
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
