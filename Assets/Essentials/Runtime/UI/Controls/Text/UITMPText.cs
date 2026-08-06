using SUG.Essentials;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TextDisplayMode
{
    Normal, // 正常显示
    Wbw, // Word by word
}

public class UITMPText : ControlBase
{
    private TMP_Text _tmpTx;

    private void Awake()
    {
        _tmpTx = transform.GetComponent<TMP_Text>();
    }

    #region 工具方法

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"> 文本内容 </param>
    /// <param name="mode"> 显示文字模式 </param>
    /// <param name="onComplate"> 完成时 </param>
    public void SetText(string content, TextDisplayMode mode = TextDisplayMode.Normal, Action onComplate = null)
    {
        _tmpTx.text = "";
        if (mode == TextDisplayMode.Normal) SetNromalText(content, onComplate);
        else if (mode == TextDisplayMode.Wbw) CharacterByCharacterDisplay(content, onComplate);
        else { }
    }

    /// <summary>
    /// 正常直接显示
    /// </summary>
    /// <param name="content"></param>
    /// <param name="callback"></param>
    private void SetNromalText(string content, Action callback = null)
    {
        _tmpTx.text = content;
        callback?.Invoke();
    }

    /// <summary>
    /// 逐字显示
    /// </summary>
    private void CharacterByCharacterDisplay(string content, Action onComplate = null)
    {
        StartCoroutine(WbwCoroutine(content, onComplate));
    }

    private IEnumerator WbwCoroutine(string content, Action onComplate = null)
    {
        foreach (var c in content)
        {
            _tmpTx.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        onComplate?.Invoke();
    }

    #endregion
}
