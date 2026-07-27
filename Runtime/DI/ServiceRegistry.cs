using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    /// <summary>
    /// 作为注入依赖查找的Key值
    /// </summary>
    public readonly struct ServiceKey : IEquatable<ServiceKey>
    {
        /// <summary>
        /// id就是注册的时候Manger上面ServiceAttribute中你注册的Id
        /// 如果id为null，那么自动填充为 default
        /// </summary>
        public readonly string id;

        /// <summary>
        /// Interface为服务接口
        /// </summary>
        public readonly Type serviceType;

        public ServiceKey(Type serviceType, string id)
        {
            this.serviceType = serviceType;
            this.id = id ?? "default";
        }

        #region Interface
        public bool Equals(ServiceKey other)
        {
            return other.id == this.id 
                && other.serviceType == this.serviceType;
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.serviceType, this.id);
        }

        #endregion
    }

    internal static class ServiceRegistry
    {
        private static readonly Dictionary<ServiceKey, object> _globalServices = new();

        private static readonly Dictionary<ServiceKey, object> _sceneServices = new();

        public static void RegisterGlobal(ServiceKey serviceKey, object instance)
        {
            if (_globalServices.ContainsKey(serviceKey))
            {
                //Debug.LogWarning($"Global Service [{serviceKey.serviceType}]=[{serviceKey.id}] 已经注册。");
                return;
            }

            _globalServices.Add(serviceKey, instance);
        }

        public static void RegisterScene(ServiceKey serviceKey, object instance)
        {
            if (_sceneServices.ContainsKey(serviceKey))
            {
                //Debug.LogWarning($"Scene Service [{serviceKey.serviceType}]=[{serviceKey.id}] 已经注册。");
                return;
            }

            _sceneServices.Add(serviceKey, instance);
        }

        public static object Resolve(ServiceKey serviceKey)
        {
            if (_sceneServices.TryGetValue(serviceKey, out var scene))
                return scene;

            if (_globalServices.TryGetValue(serviceKey, out var global))
                return global;

            return null;
        }

        public static T Resolve<T>(ServiceKey serviceKey)
        {
            return (T)Resolve(serviceKey);
        }

        public static void ClearScene()
        {
            _sceneServices.Clear();
        }
    }
}