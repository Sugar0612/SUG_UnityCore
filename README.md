# SUG Essentials

> **Unity 轻量级依赖注入（DI）与基础服务框架** — 用几个 Attribute 就能完成 Service 注册、依赖注入与生命周期管理，快速搭建解耦、可维护的项目基础设施。

|  |  |
| --- | --- |
| 包名 | `com.sug.essentials`（位于 [`Assets/Essentials`](Assets/Essentials)） |
| 版本 | 1.0.1 |
| 最低 Unity | 2022.3 |
| 授权 | MIT |
| 作者 | Shiyi Tang (Sugar0612) |

---

## 目录 (Table of Contents)

- [这个项目解决什么问题](#这个项目解决什么问题)
- [特性](#特性)
- [安装](#安装)
- [快速上手](#快速上手)
- [同一个接口，多个实现](#同一个接口多个实现)
- [服务生命周期](#服务生命周期)
- [运行时实例化与手动解析](#运行时实例化与手动解析)
- [工作原理](#工作原理)
- [API 速查](#api-速查)
- [完整示例](#完整示例)
- [项目结构](#项目结构)
- [设计理念](#设计理念)
- [Roadmap](#roadmap)
- [License](#license)

---

## 这个项目解决什么问题

Unity 项目中常见的痛点：

- Manager 类满天飞，管理混乱
- Singleton 单例高耦合，难以替换与测试
- 场景服务的生命周期无人管理
- 模块之间依赖关系不明确
- 一个接口多个实现难以管理

Essentials 用一条简单的规则解决这些问题：

```
Interface + Attribute + Service Container + Dependency Injection
```

即：**用接口定义能力，用 Attribute 声明注册，由容器统一管理实例与生命周期，并在需要的地方自动注入。**

> 核心代码在 `Assets/Essentials`（即 UPM 包 `com.sug.essentials`），本仓库其余内容均为示例工程。

---

## 特性 (Features)

- **自动注册**：实现类打上 `[Service]` 即自动被扫描注册，无需手动添加到容器。
- **接口驱动**：`[Injectable]` 标记接口，`[Inject]` 注入字段，调用方只依赖接口、不依赖实现。
- **两种生命周期**：`Global`（全局常驻）/ `Scene`（随场景卸载释放）。
- **多实现支持**：同一接口可注册多个实现，用 `Id` 区分。
- **零配置接入**：编辑器自动在场景中创建 `DIBootstrap`，打开即用。
- **运行时注入**：动态 Instantiate 出来的对象也会被自动注入。

---

## 安装 (Installation)

### 环境要求

| 项目 | 要求 |
| --- | --- |
| Unity | 2022.3 或更高 |
| 平台 | 任意（纯 C# + UnityEngine API，无第三方依赖） |

### 方式一：Package Manager（推荐）

```
Window → Package Manager → ➕ → Add package from git URL...
```

输入：

```
https://github.com/Sugar0612/Essentials.git?path=Assets/Essentials#1.1.0
```

### 方式二：修改 manifest.json

打开 `Packages/manifest.json`，在 `dependencies` 中添加：

```json
{
  "dependencies": {
    "com.sug.essentials": "https://github.com/Sugar0612/Essentials.git?path=Assets/Essentials#1.1.0"
  }
}
```

> `path=Assets/Essentials` 指向包本体；`#1.1.0` 是版本 tag，去掉则使用最新提交。

### 安装之后

1. 等待 Unity 完成导入。打开任意场景，编辑器会**自动**创建一个 `[Essentials Bootstrap]` 对象（内含 `DIBootstrap`），它负责该场景的 Service 注册与依赖注入——**无需手动搭建**。
2. 本仓库自带示例场景，可直接打开查看效果：
   - `Assets/Scenes/SampleScene.unity`
   - `Assets/Samples/Essentials/0.1.0/Essentials Example/Bootstrap.unity`

---

## 快速上手 (Quick Start)

三步完成一个 Service 从定义到注入。

### 1. 定义接口 —— `[Injectable]`

```csharp
using SUG.Essentials;

[Injectable]
public interface IConfigService
{
    T GetConfig<T>();
}
```

> 只有标记了 `[Injectable]` 的接口才会进入 Essentials 的 Service 系统。

### 2. 实现服务 —— `[Service]`

```csharp
using SUG.Essentials;

[Service(Lifetime = ServiceLifetime.Global)]
public class ConfigManager : MonoBehaviour, IConfigService
{
    public T GetConfig<T>() => default;
}
```

> `[Service]` 实现类**必须是场景中的 MonoBehaviour 组件**（Essentials 通过扫描场景对象来注册服务）。`Lifetime` 缺省为 `Global`。

### 3. 注入 —— `[Inject]`

```csharp
using SUG.Essentials;

public class TestController : MonoBehaviour
{
    [Inject]
    private IConfigService _config;

    private void Start()
    {
        var data = _config.GetConfig<TestData>();
    }
}
```

Essentials 会在场景加载时自动完成 `IConfigService → ConfigManager` 的绑定。`[Inject]` 字段可以是 `private` 或 `public`。

完成。现在 `TestController` 不依赖 `ConfigManager` 的具体类型——替换实现、单元测试都更加容易。

---

## 同一个接口，多个实现

同一接口可以注册多个实现，通过 `[Service(Id = "...")]` 与 `[Inject("...")]` 配对使用。

```csharp
[Injectable]
public interface IAudioService
{
    void Play();
}
```

```csharp
[Service(Lifetime = ServiceLifetime.Global, Id = "SFX")]
public class SFXManager : MonoBehaviour, IAudioService { }

[Service(Lifetime = ServiceLifetime.Global, Id = "BGM")]
public class BGMManager : MonoBehaviour, IAudioService { }
```

```csharp
[Inject("SFX")] private IAudioService _sfx;
[Inject("BGM")] private IAudioService _bgm;
```

Essentials 用 `(ServiceType + Id)` 作为注册键：

- 不写 `Id` 时默认键为 `(ServiceType, "default")`；
- 注入时按同样的键查找，`[Inject]` 与 `EssentialsMethod.Resolve` 的 id 缺省均为 `"default"`。

---

## 服务生命周期 (Lifecycle)

```csharp
public enum ServiceLifetime
{
    Global, // 全局生命周期
    Scene,  // 场景局部生命周期
}
```

| 生命周期 | 说明 | 适用场景 |
| --- | --- | --- |
| `Global` | 全局唯一，场景切换不销毁 | AudioManager、ConfigManager、ResourceManager |
| `Scene` | 跟随场景，场景卸载后由容器自动释放 | 场景 UI Manager、当前场景数据、SceneManager |

- **Global**：把实现类放在常驻对象上（例如入口场景中 `DontDestroyOnLoad` 的对象），实例与注册在场景切换时持续有效。
- **Scene**：对象出现在场景中即自动注册；场景卸载时（`SceneManager.sceneUnloaded`）容器自动清空对应服务。

---

## 运行时实例化与手动解析

`DIBootstrap` 只负责场景中已存在的对象。运行时动态创建的对象，请使用 `SUG.Essentials.DI.EssentialsMethod`：

```csharp
using SUG.Essentials.DI;

// 实例化并自动注入（可选传入父节点）
var panel = EssentialsMethod.Instantiate(panelPrefab, transform);

// 手动解析服务（id 缺省为 "default"）
var audio = EssentialsMethod.Resolve<IAudioService>("SFX");
audio.Play();

// 手动清空场景服务（例如手动切换场景时）
EssentialsMethod.ClearSceneContainer();
```

---

## 工作原理 (How It Works)

一次场景加载的完整流程：

```
Editor：场景打开/保存时自动创建 [Essentials Bootstrap]（DIBootstrap）
        ↓
DIBootstrap.Awake()  （DefaultExecutionOrder = -32000，最先执行）
        ↓
ServiceScanner.ScanRegister(scene)
   ├─ 扫描全局对象上的 [Service] → 注册到 Global 容器
   └─ 扫描当前场景根对象下的 [Service] → 注册到 Scene 容器
        ↓
Injector.InjectScene(scene)
   └─ 遍历场景中带 [Inject] 字段的组件 → 按 (接口, id) 从容器解析并赋值
```

找不到对应服务时：字段注入会被跳过（保持原值），手动 `Resolve` 返回 `null`——请确保服务已正确注册。

---

## API 速查 (API Reference)

| API | 命名空间 | 作用 |
| --- | --- | --- |
| `[Injectable]` | `SUG.Essentials` | 标记接口，使其进入 Service 系统（仅限接口） |
| `[Service]` | `SUG.Essentials` | 标记 MonoBehaviour 实现类，声明生命周期与 Id（仅限类） |
| `[Inject]` / `[Inject("id")]` | `SUG.Essentials` | 标记需要自动注入的字段（仅限字段） |
| `ServiceLifetime` | `SUG.Essentials` | `Global` / `Scene` 生命周期枚举 |
| `DIBootstrap` | `SUG.Essentials` | 场景启动入口组件，由编辑器自动创建 |
| `EssentialsMethod.Instantiate<T>(prefab[, parent])` | `SUG.Essentials.DI` | 实例化对象并自动注入 |
| `EssentialsMethod.Resolve<T>(id = "default")` | `SUG.Essentials.DI` | 手动解析服务 |
| `EssentialsMethod.ClearSceneContainer()` | `SUG.Essentials.DI` | 清空 Scene 服务容器 |
| `ServiceKey` | `SUG.Essentials` | 注册键 `(ServiceType, Id)` |

> 提示：在自定义 asmdef 程序集中使用本库时，需要在 Assembly Definition References 中添加 `SUG.Essentials.Runtime`（该程序集 `autoReferenced`，`Assembly-CSharp` 可直接使用）。

---

## 完整示例 (Example)

### UI Service

```csharp
[Injectable]
public interface IUIService
{
    void OpenUI<T>();
}

[Service(Lifetime = ServiceLifetime.Global)]
public class UIManager : MonoBehaviour, IUIService
{
    public void OpenUI<T>() { /* ... */ }
}

public class LoginPanel : MonoBehaviour
{
    [Inject] private IUIService _ui;

    private void Start()
    {
        _ui.OpenUI<SettingPanel>();
    }
}
```

### File Service

```csharp
[Injectable]
public interface IFileService
{
    string ReadText(string path);
}

[Service(Lifetime = ServiceLifetime.Global)]
public class FileManager : MonoBehaviour, IFileService
{
    public string ReadText(string path) { /* ... */ }
}

public class ConfigLoader : MonoBehaviour
{
    [Inject] private IFileService _file;

    private void Start()
    {
        string json = _file.ReadText("config.json");
    }
}
```

---

## 项目结构 (Project Structure)

```
Assets/Essentials                  ← UPM 包本体（com.sug.essentials）
├── Runtime/
│   ├── Attributes/Inject/         [Injectable] [Service] [Inject] 特性
│   ├── DI/                        ServiceRegistry / ServiceScanner / Injector / ReflectionCache
│   ├── Bootstrap/DIBootstrap.cs   场景启动入口
│   └── EssentialsMethod.cs        运行时实例化与手动解析 API
├── Editor/
│   └── SceneBootstrapEditor.cs    自动创建 DIBootstrap 的编辑器工具
└── package.json                   UPM 包配置

Assets/Scenes/SampleScene.unity              工程示例场景
Assets/Samples/Essentials/0.1.0/             示例场景（Essentials Example）
```

---

## 设计理念 (Design Philosophy)

Essentials 不替代业务框架，只负责三件事：

```
基础服务管理 + 依赖关系管理 + 生命周期管理
```

业务逻辑仍由项目自身组织。推荐结构：

```
Game
 ├── UI / Gameplay / Config ...
Essentials
 ├── Service Container
 ├── Dependency Injection
 └── 常用基础服务
```

---

## Roadmap

- [ ] Service 生命周期释放（Dispose 回调）
- [ ] Async Service 初始化
- [ ] Service Editor 可视化工具
- [ ] Dependency Graph
- [ ] 更多 Unity 常用基础服务

---

## License

[MIT](LICENSE)
