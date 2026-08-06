namespace SUG.Essentials
{
    using UnityEngine;

    /// <summary>
    /// Tag 是一个音频分类标识，用于在不同 Cue 配置中匹配交互规则，从而决定某种交互条件下播放哪一个声音。
    /// </summary>
    [CreateAssetMenu(fileName = "ObjectTagSO", menuName = "Essentials/Object/Ob_TagConfig")]
    public class ObjectTagSO : ScriptableObject
    {
        public string tagName;
        [TextArea] public string description;
    }
}