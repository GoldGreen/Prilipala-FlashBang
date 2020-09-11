using UnityEngine;
using UnityEngine.UI;

public class SettingItemGraphics : MonoBehaviour
{
    [SerializeField] private Sprite used;
    [SerializeField] private Sprite unUsed;
    [SerializeField] private Sprite unOpened;

    private Image image;

    public Image Icon { get; set; }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetData(bool isOpened, bool newSelection)
    {
        image.sprite = isOpened ? newSelection ? used : unUsed : unOpened;
    }

    public void Down()
    {
        image.color = Color.gray;

        if (Icon)
        {
            Icon.color = Color.gray;
        }
    }
    public void Up()
    {
        image.color = Color.white;

        if (Icon)
        {
            Icon.color = Color.white;
        }
    }
}
