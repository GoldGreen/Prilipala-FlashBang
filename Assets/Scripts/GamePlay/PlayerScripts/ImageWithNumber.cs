using UnityEngine;
using UnityEngine.UI;

public class ImageWithNumber : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text numberText;

    private int number = 0;

    public Sprite Sprite
    {
        get => image.sprite;
        set => image.sprite = value;
    }

    public float FillAmount
    {
        get => image.fillAmount;
        set => image.fillAmount = value;
    }

    public bool ClockFill
    {
        get => image.fillClockwise;
        set => image.fillClockwise = value;
    }

    public int Number
    {
        get => number;
        set
        {
            number = value;
            numberText.text = value.ToString();
        }
    }

    public bool NumberVisible
    {
        get => numberText.gameObject.activeSelf;
        set => numberText.gameObject.SetActive(value);
    }
}