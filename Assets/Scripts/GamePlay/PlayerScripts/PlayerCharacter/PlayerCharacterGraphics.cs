using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterGraphics : MonoBehaviour
{
    [SerializeField] private Image slidingImage;
    [SerializeField] private Text hpPercents;

    private float nextFillAmount;
    private float adding;

    private void FixedUpdate()
    {
        if (slidingImage.fillAmount > nextFillAmount && adding < 0
        || slidingImage.fillAmount < nextFillAmount && adding > 0)
        {
            slidingImage.fillAmount += adding;
        }
    }

    public void SetAnimatedData(float fill)
    {
        nextFillAmount = fill;
        hpPercents.text = Format(fill);
        adding = (nextFillAmount - slidingImage.fillAmount) * 0.05f;
    }

    public void SetData(float fill)
    {
        SetAnimatedData(fill);
        slidingImage.fillAmount = nextFillAmount;
    }

    private string Format(float value)
    {
        int digits = 1;
        if (value > 0.1f)
        {
            digits = 0;
        }

        return $"{(value * 100).FixeTo(digits)}%";
    }
}