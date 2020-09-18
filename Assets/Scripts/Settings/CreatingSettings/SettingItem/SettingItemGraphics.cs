using UnityEngine;
using UnityEngine.UI;

public class SettingItemGraphics : MonoBehaviour
{
    [SerializeField] private Sprite used;
    [SerializeField] private Sprite unUsed;
    [SerializeField] private Sprite unOpened;

    [SerializeField] private Image icon;

    public Image Icon => icon;
    public Sprite IconSprite { set => icon.sprite = value; }

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetData(bool isOpened, bool newSelection)
    {
        image.sprite = isOpened ? newSelection ? used : unUsed : unOpened;
    }

    public void PressItem()
    {
        image.color = Color.gray;
        icon.color = Color.gray;
    }

    public void UnpressItem()
    {
        image.color = Color.white;
        icon.color = Color.white;
    }
}
