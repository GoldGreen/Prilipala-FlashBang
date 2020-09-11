using UnityEngine;
using UnityEngine.UI;

public abstract class Panel<T> : MonoBehaviour
where T : BaseObjectData
{
    [SerializeField] private Image icon;
    public Sprite Icon
    {
        get => icon.sprite;
        set => icon.sprite = value;
    }

    public abstract void SetInfo(T linker);
}