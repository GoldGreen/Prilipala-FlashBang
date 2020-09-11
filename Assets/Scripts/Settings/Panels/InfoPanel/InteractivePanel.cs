using UnityEngine;

public class InteractivePanel : InformationPanel<InteractiveData>
{
    [Header("InteractiveIcons")]
    [SerializeField] private Sprite scoreIcon;

    [SerializeField] private Sprite physDamageIcon;
    [SerializeField] private Sprite magicDamageIcon;
    [SerializeField] private Sprite electricDamageIcon;

    [SerializeField] private Sprite accelerateMultipplyIcon;

    [SerializeField] private Sprite blockedTimeIcon;

    [SerializeField] private Sprite interactiveMoneyIcon;
    protected override Sprite IncreaseMoneyIcon => interactiveMoneyIcon;

    protected override void SetData(InteractiveData interactiveData)
    {
        AddElement(scoreIcon, interactiveData.Score);

        if (interactiveData.DamageEntityExist)
            AddElement(GetSpriteByDamageType(interactiveData.DamageType), interactiveData.UregularDamage.FixeTo(1));

        if (interactiveData.AccelerateMultiplyingEntityExist)
            AddElement(accelerateMultipplyIcon, $"{interactiveData.AccelerateMultiply.FixeTo(1)} ({interactiveData.AccelerateMultiplyingTime.FixeTo(1)}s)");

        if (interactiveData.BlockedTimeEntityExist)
            AddElement(blockedTimeIcon, $"r - ({interactiveData.BlockedTime.FixeTo(1)}s)");
    }

    private Sprite GetSpriteByDamageType(DamageType type)
    {
        switch (type)
        {
            case DamageType.physical:
                return physDamageIcon;
            case DamageType.magic:
                return magicDamageIcon;
            case DamageType.electric:
                return electricDamageIcon;
        }

        return null;
    }
}