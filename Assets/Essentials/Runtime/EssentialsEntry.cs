using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    internal static class EssentialsEntry
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Boot()
        {
            Essentials.Initialize();
        }
    }
}