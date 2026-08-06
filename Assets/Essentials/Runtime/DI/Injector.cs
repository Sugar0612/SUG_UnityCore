using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    internal static class Injector
    {
        public static void InjectScene(Scene scene)
        {
            // Debug.Log($"=========== 注入 InjectScene {scene.name}");
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);

                foreach (var behaviour in behaviours)
                {
                    if (behaviour == null) continue;

                    // 没有 [Inject] 字段就直接跳过
                    if (!HasInjectField(behaviour.GetType())) continue;

                    Inject(behaviour);
                }
            }
        }

        // Toolkit
        private static bool HasInjectField(Type type)
        {
            var fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                    return true;
            }

            return false;
        }

        // Core

        // 注入
        public static void Inject(object target)
        {
            if (target == null) return;

            // 获取脚本中声明的成员字段
            var type = target.GetType();
            var fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(InjectAttribute))) continue;

                InjectAttribute attribte = field.GetCustomAttribute<InjectAttribute>();
                string id = attribte.id ?? "default";

                var service = ServiceRegistry.Resolve(new ServiceKey(field.FieldType, id));

                if (service == null) continue;

                field.SetValue(target, service);
            }
        }
    }
}