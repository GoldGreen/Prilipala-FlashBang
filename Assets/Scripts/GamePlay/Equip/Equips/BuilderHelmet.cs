using UnityEngine;

public class BuilderHelmet : Hat, IHave<IdCode>, IChangePhysics, IChange, ILinkWithTouchDetector, ILinkWithShower
{
    public IdCode Item => IdCode.BuilderHelmet;

    private ReloadingEntity<EquipData> shieldReloadingEntity;

    [SerializeField] private GameObject shieldPrefab;
    private GameObject shield;
    [SerializeField] private Sprite shieldIcon;

    private float shieldLivingTime;

    private IDisposableCollection subscribers = new Disposables();

    public TouchDetector TouchDetector { get; set; }
    public EffectShower EffectShower { get; set; }

    private void Awake()
    {
        shieldReloadingEntity = new ReloadingEntity<EquipData>(x => x.ShieldReloadingTime);
    }

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);

        equipData.SetTo(shieldReloadingEntity);

        shieldLivingTime = equipData.ShieldLivingTime;
    }

    private void Start()
    {
        shield = Instantiate(shieldPrefab, transform.parent);
        shield.name = "Shield";
        shield.gameObject.SetActive(false);

        EffectShower.AddOrUpdate(shieldIcon, EffectType.Reloadable, shieldReloadingEntity.ReloadingTime, this);
    }

    public override void Change(PlayerCharacterLogic character)
    {
        base.Change(character);

        TouchDetector.OnClick.Subscribe
        (
            () =>
            {
                if (shieldReloadingEntity.IsReloaded)
                {
                    character.BlockDamagingAt(shieldLivingTime);
                }
            }
        ).AddTo(subscribers);
    }

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        TouchDetector.OnClick.Subscribe
        (
            () =>
            {
                if (shieldReloadingEntity.IsReloaded)
                {
                    control.BlockAccelerateMultiplyAt(shieldLivingTime);
                }
            }
        ).AddTo(subscribers);
    }

    public void Change()
    {
        TouchDetector.OnClick.Subscribe
        (
            () =>
            {
                if (shieldReloadingEntity.IsReloaded)
                {
                    shieldReloadingEntity.StartReload(this);
                    ShowShieldAt(shieldLivingTime);
                    EffectShower.AddOrUpdate(shieldIcon, EffectType.Reloadable, shieldReloadingEntity.ReloadingTime, this);
                }
            }
        ).AddTo(subscribers);
    }

    private void ShowShieldAt(float time)
    {
        shield.SetActive(true);
        CoroutineT.Single(() => shield.SetActive(false), time).Start(this);
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
