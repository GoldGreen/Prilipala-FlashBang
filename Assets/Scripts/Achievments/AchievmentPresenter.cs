using System;
using UnityEngine;

[Serializable]
public class AchievmentPresenter : IHave<Sprite>, IHave<AchievmentCode>
{
    AchievmentCode IHave<AchievmentCode>.Item => code;
    Sprite IHave<Sprite>.Item => icon;

    [SerializeField] private Sprite icon;
    [SerializeField] private AchievmentCode code;
}
