using System;

namespace SUG.Essentials
{
    /// <summary>
    /// 标记需要由 Essentials 自动注入的字段。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute
    {
        /// <summary>
        /// id用来告诉IService，
        /// 这次的服务使用的是哪个Manager实现的接口
        /// </summary>
        public string id;

        public InjectAttribute(string id = null) => this.id = id;
    }
}