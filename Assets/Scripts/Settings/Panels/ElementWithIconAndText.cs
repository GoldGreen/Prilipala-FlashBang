using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ElementWithIconAndText
{
    public Image Icon => icon;
    [SerializeField] private Image icon;

    public Text Text => text;
    [SerializeField] private Text text;

    public bool Active
    {
        set
        {
            icon.gameObject.SetActive(value);
            text.gameObject.SetActive(value);
        }
    }
}
