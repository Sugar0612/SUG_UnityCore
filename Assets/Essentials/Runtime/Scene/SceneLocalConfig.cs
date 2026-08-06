using System.Collections.Generic;
using SUG.Essentials;
using UnityEngine;

namespace SUG.Essentials
{
    public class SceneLocalConfig : ScriptableObject
    {
        public List<UIBase> AutoOpenUITypes = new List<UIBase>();
    }

    [CreateAssetMenu(fileName = "StartSceneConfig", menuName = "Game/Scenes/StartSceneConfig")]
    public class StartSceneConfig : SceneLocalConfig
    {

    }

    [CreateAssetMenu(fileName = "TheorySceneConfig", menuName = "Game/Scenes/TheorySceneConfig")]
    public class TheorySceneConfig : SceneLocalConfig
    {

    }
}