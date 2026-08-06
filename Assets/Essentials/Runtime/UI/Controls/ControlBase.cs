using UnityEngine;
using SUG.Essentials;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;

namespace SUG.Essentials
{
    public abstract class ControlBase : Interactable
    {
        [Header("Button type")]
        public ControlType type;

        public event Action<InteractionTrigger, ControlType> onTrigger;

        // =============
        // Life cycle
        // =============

        private void Start()
        {
            RaiseTrigger(InteractionTrigger.ObjectStart);
        }

        private void OnEnable()
        {
            onClickEnter += () => RaiseTrigger(InteractionTrigger.Selected);
            onHoverEnter += () => RaiseTrigger(InteractionTrigger.HoverEnter);
            onHoverExit += () => RaiseTrigger(InteractionTrigger.HoverExit);
        }

        private void OnDestroy()
        {
            RaiseTrigger(InteractionTrigger.ObjectStart);
        }

        // =============
        // Core
        // =============
        public void RaiseTrigger(InteractionTrigger trigger)
        {
            onTrigger?.Invoke(trigger, type);
        } 
    }

    [Flags]
    public enum InteractionTrigger
    {
        None = 0,
        HoverEnter = 1 << 0,  // 鼠标移动进入
        HoverExit = 1 << 1, // 鼠标移动退出
        DeSelect = 1 << 2, // 取消选中
        UnSelctable = 1 << 3, // 无法选中/选择失败
        Selected = 1 << 4, // 选中/选择成功
        ObjectStart = 1 << 5,
        ObjectDestroy = 1 << 6,
    }

    [Flags]
    public enum ControlType
    {
        None = 0, // 无
        Start = 1 << 0, // 开始类型
        Normal = 1 << 1, // 一般类型
        Theory = 1 << 2, // 理论模式
    }

    // For example:
    /*
        using SUG.Essentials;

        public class NormalButton : ControlBase
        {
            // ===================
            // Event
            // ===================
            protected override void OnHoverEnter()
            {
                RaiseTrigger(InteractionTrigger.HoverEnter, ControlType.Normal);
            }

            protected override void OnHoverExit()
            {
                RaiseTrigger(InteractionTrigger.HoverExit, ControlType.Normal);
            }

            protected override void OnClickEnter()
            {
                RaiseTrigger(InteractionTrigger.Selected, ControlType.Normal);
            }
        }

    */
}
