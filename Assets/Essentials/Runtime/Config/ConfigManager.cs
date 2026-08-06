using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    [Service(lifetime = ServiceLifetime.Scene)]
    public class ConfigManager : MonoBehaviour, ICfgService
    {
        [Header("当前场景所需的所有配置")]
        [SerializeField] private List<ScriptableObject> _configList = new();

        private Dictionary<Type, ScriptableObject> _configDict = new();

        // ====================
        // Lefe cycle
        // ====================
        private void Awake()
        {
            foreach (var config in _configList)
            {
                if (config == null) continue;

                Type t = config.GetType();
                if (!_configDict.ContainsKey(t)) _configDict.Add(t, config);
                else _configDict[t] = config;
            }
        }

        // ====================
        // Core
        // ====================
        public T GetConfig<T>() where T : ScriptableObject
        {
            ScriptableObject c = null;
            if (_configDict.ContainsKey(typeof(T)))
            {
                c = _configDict[typeof(T)];
                return c as T;
            }

            Type baseT = typeof(T);
            foreach (var t in _configDict)
            {
                if (t.Key.IsSubclassOf(baseT)) return t.Value as T;
            }
            return null;
        }

        public bool HasConfig<T>() where T : ScriptableObject
        {
            if (_configDict.ContainsKey(typeof(T))) return true;

            Type baseT = typeof(T);
            foreach (var t in _configDict)
            {
                if (t.Key.IsSubclassOf(baseT)) return true;
            }

            return false;
        }
    }
}