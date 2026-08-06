using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    internal static class ReflectionCache
    {
        private static readonly Dictionary<Type, Type[]> _interfaceCache = new();

        public static Type[] GetInterfaces(Type type)
        {
            if (_interfaceCache.TryGetValue(type, out var result))
                return result;

            result = type.GetInterfaces();

            _interfaceCache[type] = result;

            return result;
        }

        public static void Clear()
        {
            _interfaceCache.Clear();
        }
    }
}