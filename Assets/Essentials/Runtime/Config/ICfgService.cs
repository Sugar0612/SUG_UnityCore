
using SUG.Essentials;
using UnityEngine;

[Injectable] public interface ICfgService 
{
    public T GetConfig<T>() where T : ScriptableObject;

    public bool HasConfig<T>() where T : ScriptableObject;
}