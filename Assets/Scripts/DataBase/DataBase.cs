using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SharpExtensions;

public class DataBase : Singleton<DataBase>, IDataBase
{
    private const string FILE_MONEY = "Money";
    private const string FILE_SCORE = "Score";
    private const string FILE_TIMBER = "Timber";
    private const string FILE_PLUNGER = "Plunger";
    private const string FILE_MINE = "Mine";
    private const string FILE_NORMAL_BATUT = "NormalBatut";
    private const string FILE_POWERFULL_BATUT = "PowerfulBatut";
    private const string FILE_HAMMER = "Hammer";
    private const string FILE_CACTUS = "Cactus";
    private const string FILE_VILA = "Vila";
    private const string FILE_MUSHROOM = "Mushroom";
    private const string FILE_BLASTER = "Blaster";
    private const string FILE_LAZER = "Lazer";
    private const string FILE_PIRATE_HAT = "PirateHat";
    private const string FILE_SUMBRERO = "Submrero";
    private const string FILE_MOTO_HELMET = "MotoHelmet";
    private const string FILE_BUILDER_HELMET = "BuilderHelmet";
    private const string FILE_BOW = "Bow";
    private const string FILE_CROSS_BOW = "CrossBow";
    private const string FILE_BAZOOKA = "Bazooka";
    private const string FILE_MORTAL = "Mortal";
    private const string FILE_TRAP = "Trap";
    private const string FILE_SNIPER = "Sniper";
    private const string FILE_AUTOMAT = "Automat";
    private const string FILE_QUEUE_AUTOMAT = "QueueAutomat";
    private const string FILE_BUILDER_EYER = "BuilderEyeê";
    private const string FILE_PIRATE_EYER = "PirateEyer";
    private const string FILE_PIRATE_HEEL = "PirateHeel";
    private const string FILE_POWER_SOCKET = "PowerSocket";
    private const string FILE_PORTAL = "Portal";
    private const string FILE_HOOK = "Hook";

    public Money Money { get; }
    public Score Score { get; }
    public Character Character { get; }

    public IEnumerable<BaseObjectData> Equips => equips.Values;
    public IEnumerable<BaseObjectData> Interactives => interactives.Values;

    private readonly IDictionary<IdCode, InteractiveData> interactives = new Dictionary<IdCode, InteractiveData>();
    private readonly IDictionary<IdCode, EquipData> equips = new Dictionary<IdCode, EquipData>();

    public DataBase()
    {
        Character = new Character();

        Money = TryRead<Money>(FILE_MONEY, obj => new Money(obj));
        Score = TryRead<Score>(FILE_SCORE, obj => new Score(obj));

        Money.OnDataChanged.AddListener(x => Write(FILE_MONEY, x));
        Score.OnDataChanged.AddListener(x => Write(FILE_SCORE, x));

        AddInteractives();
        AddEquips();
    }

    public BaseObjectData Find(IdCode idCode)
    {
        if (idCode.IsOneOf(interactives.Keys))
            return interactives[idCode];

        if (idCode.IsOneOf(equips.Keys))
            return equips[idCode];

        return null;
    }

    public T Find<T>(IdCode idCode) where T : BaseObjectData
    {
        if (idCode.IsOneOf(interactives.Keys) && Equal<T, InteractiveData>())
            return interactives[idCode] as T;

        if (idCode.IsOneOf(equips.Keys) && Equal<T, EquipData>())
            return equips[idCode] as T;

        return null;
    }

    private T TryRead<T>(string fileName, Func<T, T> func)
    where T : class, new()
    => PlayerPrefs.HasKey(fileName) ? func(JsonUtility.FromJson<T>(PlayerPrefs.GetString(fileName))) : new T();

    private void Write<T>(string fileName, T element) => PlayerPrefs.SetString(fileName, JsonUtility.ToJson(element, true));

    private InteractiveData AddInteractive(string filename, IdCode idCode, string name,
    long openCost, int maxLevel, long increasingObjectCost, long increasingCost, long score, int tier)
    {
        var dynamicParatetrs = TryRead<DynamicParatetrs>(filename, obj => new DynamicParatetrs(obj));

        var interactiveData = new InteractiveData
        (
            dynamicParatetrs, name, openCost, maxLevel,
            increasingObjectCost, increasingCost,
            score, tier
        );

        interactiveData.OnDataChanged.Subscribe(x => Write(filename, x.DynamicParatetrs));
        interactives.Add(idCode, interactiveData);

        return interactiveData;
    }

    private EquipData AddEquip(string filename, IdCode idCode, string name,
    long openCost, int maxLevel, long increasingObjectCost, long increasingCost,
    Set set, TypeOfEquip typeOfEquip)
    {
        var dynamicParatetrs = TryRead<DynamicParatetrs>(filename, obj => new DynamicParatetrs(obj));

        var equipData = new EquipData
        (
            dynamicParatetrs, name,
            openCost, maxLevel, increasingObjectCost, increasingCost,
            set, typeOfEquip
        );

        equipData.OnDataChanged.Subscribe(x => Write(filename, x.DynamicParatetrs));
        equips.Add(idCode, equipData);

        return equipData;
    }

    private void AddInteractives()
    {
        #region Tier1

        AddInteractive(FILE_TIMBER, IdCode.Timber, "Timber",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 1)
        .SetDamage(20, DamageType.physical);

        AddInteractive(FILE_PLUNGER, IdCode.Plunger, "Plunger",
        openCost: 100, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 1)
        .SetDamage(15, DamageType.physical);

        AddInteractive(FILE_MINE, IdCode.Mine, "Mine",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 1)
        .SetDamage(60, DamageType.magic);

        AddInteractive(FILE_POWER_SOCKET, IdCode.PowerSocket, "PowerSocket",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 1)
        .SetDamage(13, DamageType.electric, 0.2f);

        #endregion

        #region Tier2

        AddInteractive(FILE_NORMAL_BATUT, IdCode.NormalBatut, "Batut",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 2);

        AddInteractive(FILE_POWERFULL_BATUT, IdCode.PowerfulBatut, "PowerfulBatut",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 2);

        AddInteractive(FILE_CACTUS, IdCode.Cactus, "Cactus",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 2)
        .SetBlockedTime(2.45f);

        AddInteractive(FILE_HAMMER, IdCode.Hammer, "Hammer",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 2)
        .SetDamage(30, DamageType.physical);

        AddInteractive(FILE_VILA, IdCode.Vila, "Vila",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 2)
        .SetDamage(25, DamageType.physical);

        #endregion

        #region Tier3

        AddInteractive(FILE_PORTAL, IdCode.Portal, "Portal",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 3);

        AddInteractive(FILE_MUSHROOM, IdCode.Mushroom, "Mushroom",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 3)
        .SetDamage(2, DamageType.magic, 0.2f, 35);

        AddInteractive(FILE_LAZER, IdCode.Lazer, "Lazer",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 3)
        .SetDamage(15, DamageType.electric, 0.15f)
        .SetAccelerateMultiply(0.5f, 2);

        AddInteractive(FILE_BLASTER, IdCode.Blaster, "Blaster",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 3)
        .SetDamage(20, DamageType.electric);

        #endregion

        #region Tier4

        AddInteractive(FILE_HOOK, IdCode.Hook, "Hook",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 4);

        AddInteractive(FILE_BOW, IdCode.Bow, "Bow",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 4)
        .SetDamage(80, DamageType.physical);

        AddInteractive(FILE_CROSS_BOW, IdCode.CrossBow, "CrossBow",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 4)
        .SetDamage(45, DamageType.physical);

        AddInteractive(FILE_BAZOOKA, IdCode.Bazooka, "Bazooka",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 4)
        .SetDamage(80, DamageType.magic);

        AddInteractive(FILE_MORTAL, IdCode.Mortal, "Mortal",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 4)
        .SetDamage(90, DamageType.magic);

        #endregion

        #region Tier5

        AddInteractive(FILE_AUTOMAT, IdCode.Automat, "Automat",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 5)
        .SetDamage(25, DamageType.physical, 0.2f)
        .SetAccelerateMultiply(0.95f, 1.0f);

        AddInteractive(FILE_QUEUE_AUTOMAT, IdCode.QueueAutomat, "QueueAutomat",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 5)
        .SetDamage(18, DamageType.physical, 0.2f)
        .SetAccelerateMultiply(0.95f, 1.0f);

        AddInteractive(FILE_SNIPER, IdCode.Sniper, "Sniper",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 5)
        .SetDamage(160, DamageType.physical)
        .SetAccelerateMultiply(0.7f, 2.0f);

        AddInteractive(FILE_TRAP, IdCode.Trap, "Trap",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        score: 100, tier: 5)
        .SetDamage(25, DamageType.physical, 0.2f);

        #endregion
    }

    public void AddEquips()
    {
        #region BuilderSet

        AddEquip(FILE_BUILDER_HELMET, IdCode.BuilderHelmet, "BuilderHelmet",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Builder, TypeOfEquip.helmet)
        .SetHealth(200)
        .SetArmor(4.0f, 0.0f, 2.0f)
        .SetShield(2.0f, 4.0f);

        AddEquip(FILE_BUILDER_EYER, IdCode.BuilderEyer, "BuilderEyer",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Builder, TypeOfEquip.eyer)
        .SetHealth(80)
        .SetRestoredHealth(30.0f, 4.0f);

        #endregion

        #region MexicoSet

        AddEquip(FILE_SUMBRERO, IdCode.Submrero, "Sumbrero",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Mexico, TypeOfEquip.helmet)
        .SetHealth(100)
        .SetArmor(2.0f, 1.5f, 1.2f)
        .SetDestroyTime(4.5f);

        #endregion

        #region MotoSet

        AddEquip(FILE_MOTO_HELMET, IdCode.MotoHelmet, "MotoHelmet",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Moto, TypeOfEquip.helmet)
        .SetHealth(140)
        .SetArmor(2.3f, 1.4f, 2.7f)
        .SetAccelerate(40.0f);

        #endregion

        #region PirateSet

        AddEquip(FILE_PIRATE_HAT, IdCode.PirateHat, "PirateHat",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Pirate, TypeOfEquip.helmet)
        .SetHealth(170)
        .SetArmor(1.0f, 3.2f, 0.6f);

        AddEquip(FILE_PIRATE_EYER, IdCode.PirateEyer, "PirateEyer",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Pirate, TypeOfEquip.eyer)
        .SetHealth(47)
        .SetWave(2.0f, 6.0f);

        AddEquip(FILE_PIRATE_HEEL, IdCode.PirateHeel, "PirateHeel",
        openCost: 0, maxLevel: 30, increasingObjectCost: 0, increasingCost: 0,
        Set.Pirate, TypeOfEquip.heel)
        .SetArmor(0.5f, 1.0f, 0.0f)
        .SetSecondJump(3.0f);

        #endregion
    }
}