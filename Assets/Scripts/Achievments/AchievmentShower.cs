using UnityEngine;
using UnityEngine.UI;

public class AchievmentShower : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text description;
    [SerializeField] private Image achImage;

    public Sprite AchievmentIcon { set => achImage.sprite = value; }

    public void SetAchievmentData(Achievment achievment)
    {
        nameText.text = achievment.Name;
        description.text = $"{achievment.ProgressValue}/{achievment.MaxValue} ({(achievment.NormalProgressValue * 100).FixeTo(0)} %)";
    }
}