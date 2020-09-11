using UnityEngine;

public class PirateHeel : Heel, IHaveIdCode, IChangePhysics, ILinkWithShower
{
    public IdCode IdCode => IdCode.PirateHeel;

    private float secondJumpReloadingTime;
    [SerializeField] private Sprite secondJumpIcon;

    private IDisposableCollection subscribers = new Disposables();

    public EffectShower EffectShower { get; set; }

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);
        equipData.SetSecondJumpReloadingTo(out secondJumpReloadingTime);
    }

    private void Start()
    => EffectShower.AddOrUpdate(secondJumpIcon, EffectType.Reloadable, secondJumpReloadingTime);

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        control.WallGravityScale = 0;
        control.AllowSecondJump(secondJumpReloadingTime);
        control.OnTrySecondJump
        .Subscribe
        (
            _ => EffectShower.AddOrUpdate(secondJumpIcon, EffectType.Reloadable, secondJumpReloadingTime)
        ).AddTo(subscribers);
    }

    private void OnDestroy() => subscribers.Dispose();
}