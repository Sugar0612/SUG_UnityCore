using System;
using JetBrains.Annotations;
using SUG.Essentials.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SUG.Essentials
{
    /// <summary>
    /// UI特效基类，提供了基本的特效接口，所有UI特效都应该继承自这个类
    /// </summary>
    public abstract class EffectBase : MonoBehaviour
    {   
        // —— Runtime variable ——
        [Header("Dependent object.")]
        protected ControlBase _dependent;

        [SerializeField] private InteractionTrigger _playTrigger;
        [SerializeField] private InteractionTrigger _stopTrigger;
        [SerializeField] private InteractionTrigger _otherTrigger;

        // 控制Effect参数
        [Header("Effect常驻效果"), SerializeField]
        private bool alwaysActive = false;

        protected InteractionTrigger _currInterTrigger;
        protected ControlType _currControlType;

        // ===================
        // Life cycle
        // ===================
        protected virtual void Awake() 
        {
            if (_dependent == null)  _dependent = GetComponent<ControlBase>();
            if (_dependent == null) _dependent = GetComponentInParent<ControlBase>();
            if (_dependent == null) _dependent = GetComponentInChildren<ControlBase>();

            _dependent.onTrigger += OnTrigger;
        }

        private void OnEnable()
        {
            if (alwaysActive == false) return;
            Play();
        }

        private void FixedUpdate()
        {

        }

        // ===================
        // Initialized
        // ===================
        public virtual void OnTrigger(InteractionTrigger trigger, ControlType types)
        {
            _currInterTrigger = trigger;
            _currControlType = types;

            if ((trigger & _playTrigger) != 0) Play();
            if ((trigger & _stopTrigger) != 0) Stop();
            if ((trigger & _otherTrigger) != 0) Other();
        }

        // ===================
        // Virtual interface
        // ===================
        public virtual void Play() {}
        public virtual void Stop() {}
        public virtual void Other() {}
    }
}