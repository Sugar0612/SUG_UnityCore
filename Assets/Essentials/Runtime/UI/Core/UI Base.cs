using DG.Tweening;
using PlasticGui;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUG.Essentials
{
    public class UIBase : MonoBehaviour
    {
        [Header("UI层级")]
        public UILevel uiLevel;

        [Header("世界空间UI")]
        public bool isWorldUI;

        private Vector3 _localPos = new Vector3();
        private Quaternion _localRot = new Quaternion();
        private Vector3 _localScl = new Vector3();

        public void Awake()
        {
            _localPos = transform.localPosition;
            _localRot = transform.localRotation;
            _localScl = transform.localScale;
        }

        public virtual void Init() 
        {
            string sc = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
#if UNITY_6000_0_OR_NEWER
            GameObject parent = isWorldUI ? FindAnyObjectByType<WorldCanvas>().gameObject : FindAnyObjectByType<ScreenCanvas>().gameObject;
#else
            GameObject parent = isWorldUI ? FindObjectOfType<WorldCanvas>().gameObject : FindObjectOfType<ScreenCanvas>().gameObject;
#endif
            transform.SetParent(parent.transform);
            transform.localPosition = _localPos;
            transform.localRotation = _localRot;
            transform.localScale = _localScl;
        }

        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Close() { Hide(); Destroy(gameObject); }
    }
}