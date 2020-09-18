using UnityEngine;

public class BuilderEyer : Eyer, IHave<IdCode>, ILinkWithShower, IChangePhysics, ILinkWithTouchDetector
{
    public IdCode Item => IdCode.BuilderEyer;

    [SerializeField] private GameObject healthEffectPrefab;
    private ParticleSystem particle;

    [SerializeField] private Sprite restoredIcon;

    private ReloadingEntity<EquipData> restoreEntity = new ReloadingEntity<EquipData>(x => x.RestoreReloadingTime);
    private float restoredHealth;

    private IDisposableCollection subscribers = new Disposables();

    public EffectShower EffectShower { get; set; }
    public TouchDetector TouchDetector { get; set; }

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);
        restoredHealth = equipData.RestoredHealth;
        equipData.SetTo(restoreEntity);
    }

    private void Start()
    {
        EffectShower.AddOrUpdate(restoredIcon, EffectType.Reloadable, restoreEntity.ReloadingTime);
    }

    public override void Change(PlayerCharacterLogic character)
    {
        base.Change(character);

        TouchDetector.OnDoubleClick.Subscribe
        (
            () =>
            {
                if (restoreEntity.IsReloaded)
                {
                    restoreEntity.StartReload(this);
                    character.RestoreHealth(restoredHealth);

                    EffectShower.AddOrUpdate(restoredIcon, EffectType.Reloadable, restoreEntity.ReloadingTime);
                    particle.Play();
                }
            }
        ).AddTo(subscribers);
    }

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        var effect = Instantiate(healthEffectPrefab, playerTransform, true);
        effect.name = "HealthEffect";
        particle = effect.GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}