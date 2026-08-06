# SUG Essentials

SUG Essentials 是一个面向 Unity 的轻量级 Runtime 服务基础框架。

它不是一个完整的游戏框架，而是提供一套通用的项目基础设施，用于解决 Unity 项目中常见的：

- Manager 管理混乱
- Singleton 高耦合
- 场景服务生命周期管理
- 模块之间依赖关系不明确
- 多实现 Service 管理困难

等问题。

Essentials 使用：

```
Interface + Attribute + Service Container + Dependency Injection
```

的方式管理项目中的基础服务。


---

# Features

## Service 自动注册

通过 Attribute 标记 Service：

```csharp
[Service(
    Lifetime = ServiceLifetime.Global
)]
public class AudioManager :
    MonoBehaviour,
    IAudioService
{
}
```

Essentials 会自动扫描并注册 Service。


---

## Interface 驱动依赖

通过接口进行依赖：

```csharp
[Inject]
private IAudioService _audio;
```

调用方不需要关心具体实现：

```
IAudioService
        |
        |
AudioManager
```


降低模块之间的耦合。


---

## 生命周期管理

Essentials 支持两种 Service 生命周期：

```csharp
public enum ServiceLifetime
{
    Global,
    Scene
}
```


### Global

全局服务：

```csharp
[Service(
    Lifetime = ServiceLifetime.Global
)]
```

特点：

- 场景切换不会销毁
- 全局唯一

适用于：

- AudioManager
- ConfigManager
- ResourceManager


---

### Scene

场景服务：

```csharp
[Service(
    Lifetime = ServiceLifetime.Scene
)]
```

特点：

- 跟随场景生命周期
- 场景卸载后释放

适用于：

- SceneManager
- Scene UI Manager
- 当前场景数据


---

## 支持多个 Service 实现

同一个 Interface 可以拥有多个实现。


例如：

```csharp
[Injectable]
public interface IAudioService
{
    void Play();
}
```


SFX：

```csharp
[Service(
    Lifetime = ServiceLifetime.Global,
    Id = "SFX"
)]
public class SFXManager :
    MonoBehaviour,
    IAudioService
{

}
```


BGM：

```csharp
[Service(
    Lifetime = ServiceLifetime.Global,
    Id = "BGM"
)]
public class BGMManager :
    MonoBehaviour,
    IAudioService
{

}
```


使用：

```csharp
[Inject("SFX")]
private IAudioService _sfx;


[Inject("BGM")]
private IAudioService _bgm;
```


Essentials 通过：

```
(ServiceType + Id)
```

区分不同 Service。


---

# Installation

## Unity Package Manager

打开：

```
Window
    -> Package Manager
    -> Add package from git URL
```


输入：

```
git@github.com:Sugar0612/Essentials.git
```


或者修改：

```
Packages/manifest.json
```


添加：

```json
{
  "dependencies": {
    "com.sug.essentials":
    "https://github.com/Sugar0612/Essentials.git"
  }
}
```


---

# Quick Start


## 1. 创建 Service Interface

首先使用 `[Injectable]` 标记接口：

```csharp
using SUG.Essentials;


[Injectable]
public interface IConfigService
{
    T GetConfig<T>();
}
```


只有标记 `[Injectable]` 的接口才会进入 Essentials Service 系统。


---

## 2. 创建 Service 实现


```csharp
using SUG.Essentials;


[Service(
    Lifetime = ServiceLifetime.Global
)]
public class ConfigManager :
    MonoBehaviour,
    IConfigService
{

    public T GetConfig<T>()
    {
        return default;
    }

}
```


---

## 3. 使用 Inject


在其他 MonoBehaviour 中：

```csharp
public class TestController : MonoBehaviour
{
    [Inject]
    private IConfigService _config;


    private void Start()
    {
        var data =
            _config.GetConfig<TestData>();
    }
}
```


Essentials 会自动完成：

```
IConfigService
        |
        |
ConfigManager
```

的绑定。


---

# Service Registration Rule


一个完整 Service 需要：

1. 使用 `[Service]` 标记实现类

2. 实现 `[Injectable]` 标记的接口


正确：

```csharp
[Injectable]
public interface ITestService
{

}


[Service]
public class TestManager :
    MonoBehaviour,
    ITestService
{

}
```


错误：

```csharp
public interface ITestService
{

}


[Service]
public class TestManager :
    MonoBehaviour,
    ITestService
{

}
```


因为接口没有加入 Service 系统。


---

# Example


## UI Service


Interface:

```csharp
[Injectable]
public interface IUIService
{
    void OpenUI<T>();
}
```


Implementation:

```csharp
[Service(
    Lifetime = ServiceLifetime.Global
)]
public class UIManager :
    MonoBehaviour,
    IUIService
{

}
```


Usage:

```csharp
public class LoginPanel : MonoBehaviour
{
    [Inject]
    private IUIService _ui;


    private void Start()
    {
        _ui.OpenUI<SettingPanel>();
    }
}
```


---

## File Service


Interface:

```csharp
[Injectable]
public interface IFileService
{
    string ReadText(string path);
}
```


Implementation:

```csharp
[Service(
    Lifetime = ServiceLifetime.Global
)]
public class FileManager :
    MonoBehaviour,
    IFileService
{

}
```


Usage:

```csharp
public class ConfigLoader : MonoBehaviour
{
    [Inject]
    private IFileService _file;


    private void Start()
    {
        string json =
            _file.ReadText(
                "config.json");
    }
}
```


---

# Design Philosophy


Essentials 不希望替代业务框架。

它只负责：

```
基础服务管理
        +
依赖关系管理
        +
生命周期管理
```


业务逻辑仍然由项目自身组织。


推荐结构：

```
Game
 |
 |-- UI
 |
 |-- Gameplay
 |
 |-- Config
 |
 |
Essentials
 |
 |-- Service Container
 |-- Dependency Injection
 |-- File Service
 |-- Resource Service
```


---

# Roadmap

- [ ] Service 生命周期释放
- [ ] Async Service 初始化
- [ ] Service Editor 可视化工具
- [ ] Dependency Graph
- [ ] 更多 Unity 常用基础服务
