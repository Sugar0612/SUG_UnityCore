using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    public enum ServiceLifetime
    {
        Global, // 全局生命周期
        Scene, // 场景局部生命周期
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime lifetime { get; set; } // 该Manager的生命周期
        public string id { get; set; } // 为了解决单个Service注册多种Manager问题，所以需要id区分

        public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Global, string id = null)
        {
            this.lifetime = lifetime;
            this.id       = id;
        }
    }
}
