using System;
using UnityEngine;

[Serializable]
public class ObjectPresent : IHave<IdCode>, IHave<Sprite>
{
    Sprite IHave<Sprite>.Item => sprite;
    [SerializeField] private Sprite sprite;

    IdCode IHave<IdCode>.Item => idCode;
    [SerializeField] private IdCode idCode;
}