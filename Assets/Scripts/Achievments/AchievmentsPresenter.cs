using UnityEngine;

public class AchievmentsPresenter : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private AchievmentShower achievmentPrefab;

    [SerializeField] private AchievmentPresenter[] achievmentPresenters;

    private void Start()
    {
        achievmentPresenters.ForEach(x => InstansiateAchievment(x, x));
    }

    private void InstansiateAchievment(IHave<AchievmentCode> code, IHave<Sprite> sprite)
    {
        var achievment = Instantiate(achievmentPrefab, parent);
        var achievmentData = DB.Data.Find(code.Item);

        achievment.name = achievmentData.Name;
        achievment.SetAchievmentData(achievmentData);
        achievment.AchievmentIcon = sprite.Item;
    }
}
