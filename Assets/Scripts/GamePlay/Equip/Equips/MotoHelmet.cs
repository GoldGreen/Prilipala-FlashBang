using UnityEngine;

public class MotoHelmet : Hat, IHaveIdCode, IChangePhysics, ILinkWithShower
{
    public IdCode IdCode => IdCode.MotoHelmet;

    private float addedAcelerate;

    private IDisposableCollection subscribers = new Disposables();

    public EffectShower EffectShower { get; set; }

    public override void SetData(EquipData equipData)
    {
        base.SetData(equipData);
        equipData.SetAddedAccelerateTo(out addedAcelerate);
    }

    public void Change(Transform playerTransform, Rigidbody2D playerRigitBody, PlayerControl control)
    {
        control.Accelerate += addedAcelerate;
    }

    private void OnDestroy()
    {
        subscribers.Dispose();
    }
}
