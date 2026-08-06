using UnityEngine;

namespace SUG.Essentials
{
    /// <summary>
    /// 每个 Scene 的启动入口。
    /// 负责场景 Service 注册与依赖注入。
    /// </summary>
    [DefaultExecutionOrder(-32000)]
    public sealed class DIBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            ServiceScanner.ScanRegister(gameObject.scene);

            Injector.InjectScene(gameObject.scene);
        }
    }
}