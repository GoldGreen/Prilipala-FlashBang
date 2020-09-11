using UnityEngine;

public class Automat : GunAbstraction
{
    public override IdCode IdCode => IdCode.Automat;
    protected override string BulletName => "AutomatBullet";
}
