using UnityEngine;

public class ObjectWithAchievmentCode : MonoBehaviour, IHave<AchievmentCode>
{
    public AchievmentCode Item => achievmentCode;
    [SerializeField] private AchievmentCode achievmentCode;
}