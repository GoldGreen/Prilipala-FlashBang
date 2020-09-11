using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class InformationPanel<T> : Panel<T>
where T : BaseObjectData<T>
{
    [Header("Data object parametrs")]
    [SerializeField] private Text title;

    [SerializeField] private ElementWithIconAndText[] elements;

    [Header("Buttons")]
    [SerializeField] private Button increaseLevelBtn;

    [Header("Icons")]
    [SerializeField] private Sprite levelMoneyIcon;
    [SerializeField] private Sprite lvlIcon;

    [Header("ObjectLevelIcons")]
    [SerializeField] private Sprite noneIcon;
    [SerializeField] private Sprite bronzeIcon;
    [SerializeField] private Sprite silverIcon;
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite platinumIcon;

    protected abstract Sprite IncreaseMoneyIcon { get; }
    protected abstract void SetData(T dataBaseObject);

    private int addingIndex = 0;

    private Sprite GetSpriteByObjectLevel(ObjectLevel level)
    {
        switch (level)
        {
            case ObjectLevel.none:
                return noneIcon;
            case ObjectLevel.bronze:
                return bronzeIcon;
            case ObjectLevel.silver:
                return silverIcon;
            case ObjectLevel.gold:
                return goldIcon;
            case ObjectLevel.platinum:
                return platinumIcon;
            default:
                return null;
        }
    }

    protected void AddElement(Sprite sprite, object text)
    {
        var element = elements[addingIndex++];

        element.Icon.sprite = sprite;
        element.Text.text = text.ToString();
    }

    public sealed override void SetInfo(T dataBaseObject)
    {
        addingIndex = 0;

        elements.ForEach(x => x.Active = true);

        title.text = dataBaseObject.Name;

        increaseLevelBtn.onClick.RemoveAllListeners();

        if (dataBaseObject.Level < dataBaseObject.MaxLevel)
        {
            AddElement(levelMoneyIcon, dataBaseObject.IncreasingCost.Format());
            increaseLevelBtn.onClick.AddListener(() => dataBaseObject.PaidIncreasing());
        }
        else if (dataBaseObject.ObjectLevel < ObjectLevel.platinum)//level>=maxLevel
        {
            AddElement(IncreaseMoneyIcon, dataBaseObject.IncreasingObjectCost.Format());
            increaseLevelBtn.onClick.AddListener(() => dataBaseObject.PaidObjectIncreasing());
        }
        else//level>=maxLevel && objectLevel==Platinum
            AddElement(IncreaseMoneyIcon, "max");

        AddElement(lvlIcon, dataBaseObject.Level);
        AddElement(GetSpriteByObjectLevel(dataBaseObject.ObjectLevel), dataBaseObject.ObjectLevel);

        SetData(dataBaseObject);

        elements.Skip(addingIndex).ForEach(x => x.Active = false);
    }
}
