using SUG.Essentials;
using UnityEditor;
using UnityEngine;

namespace SUG.Essentials.Editor
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text,"[Scene] can only be used on string fields.");
                return;
            }

            SceneAsset currentScene = null;

            if (!string.IsNullOrEmpty(property.stringValue))
                currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            EditorGUI.BeginProperty(position, label, property);

            SceneAsset newScene = (SceneAsset)EditorGUI.ObjectField(position, label, currentScene, typeof(SceneAsset), false);

            if (newScene != currentScene)
                property.stringValue = newScene == null ? string.Empty : AssetDatabase.GetAssetPath(newScene);

            EditorGUI.EndProperty();
        }
    }
}